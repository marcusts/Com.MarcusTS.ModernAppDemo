#define MOCK_USER

// *********************************************************************************
// Copyright @2021 Marcus Technical Services, Inc.
// <copyright
// file=AppStateManager.cs
// company="Marcus Technical Services, Inc.">
// </copyright>
//
// MIT License
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NON-INFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// *********************************************************************************

namespace ModernAppDemo.Common.Navigation
{
   using System;
   using System.IO;
   using System.Linq;
   using System.Threading.Tasks;
   using Com.MarcusTS.ResponsiveTasks.XamFormsSupport.Common.Navigation;
   using Com.MarcusTS.ResponsiveTasks.XamFormsSupport.ViewModels;
   using Com.MarcusTS.ResponsiveTasks.XamFormsSupport.Views.Pages;
   using Com.MarcusTS.SharedForms.Common.Notifications;
   using Com.MarcusTS.SharedForms.Common.Utils;
   using Com.MarcusTS.SharedForms.ViewModels;
   using Com.MarcusTS.SharedUtils.Utils;
   using ModernAppDemo.Common.Interfaces;
   using ModernAppDemo.ViewModels;
   using SQLite;
   using Xamarin.Essentials;

   public interface IAppStateManager : IAppStateManagerBase
   {
   }

   public class AppStateManager : AppStateManagerBase, IAppStateManager
   {
      public const string ACCOUNTS_TITLE = "Accounts";
      public const string DASHBOARD_TITLE = "Dashboard";
      public const string LOGOUT_TITLE = "Log Out";
      public const string SETTINGS_TITLE = "Settings";
      private const string ACCOUNTS_APP_STATE = nameof(ACCOUNTS_APP_STATE);
      private const string CREATE_ACCOUNT_APP_STATE = nameof(CREATE_ACCOUNT_APP_STATE);
      private const string CREATION_SUCCESS_APP_STATE = nameof(CREATION_SUCCESS_APP_STATE);
      private const string DASHBOARD_APP_STATE = nameof(DASHBOARD_APP_STATE);
      private const string DATABASE_NAME = "users.db3";
      private const string LOGOUT_APP_STATE = nameof(LOGOUT_APP_STATE);
      private const string NO_APP_STATE = "";
      private const string SETTINGS_APP_STATE = nameof(SETTINGS_APP_STATE);
      private const string SIGN_IN_APP_STATE = nameof(SIGN_IN_APP_STATE);
      private static readonly string ERROR_TITLE = "ERROR".Expanded();
      private static readonly string OK_TEXT = "OK";

      private readonly SQLiteAsyncConnection _database;

      public AppStateManager(ICanShowProgressSpinner spinnerHost) : base(spinnerHost)
      {
         _database =
            new SQLiteAsyncConnection(Path.Combine(
               Environment.GetFolderPath(Environment.SpecialFolder
                  .LocalApplicationData),
               DATABASE_NAME));

         // No await possible
         MainThread.BeginInvokeOnMainThread(async () =>
         {
            await _database.CreateTableAsync<SavedAccountViewModel>().WithoutChangingContext();

#if MOCK_USER
            var fakeUser =
               new SavedAccountViewModel
               {
                  UserName = "TestUser1",
                  Password = "TestPassword1"
               };

            await _database.InsertAsync(fakeUser).WithoutChangingContext();
#endif
         });
      }

      public override string DefaultState => DASHBOARD_APP_STATE;
      public override string StartUpState => SIGN_IN_APP_STATE;

      public override (string, string)[] ToolbarItemNamesAndStates =>
         new[]
         {
            (DASHBOARD_TITLE, DASHBOARD_APP_STATE),
            (ACCOUNTS_TITLE, ACCOUNTS_APP_STATE),
            (SETTINGS_TITLE, SETTINGS_APP_STATE),
            (LOGOUT_TITLE, LOGOUT_APP_STATE)
         };

      protected override Task ProcessViewModelBeforeSettingAsCurrent<InterfaceT>(InterfaceT viewModel)
      {
         if (viewModel is IAccountsViewModel viewModelAsAccounts)
         {
            // NOTE Mandatory, especially with iOS
            MainThread.BeginInvokeOnMainThread(async () =>
               {
                  viewModelAsAccounts.Accounts =
                     await _database.Table<SavedAccountViewModel>().ToArrayAsync().WithoutChangingContext();
                  viewModelAsAccounts.Accounts =
                     viewModelAsAccounts.Accounts.OrderBy(a => a.FirstAndLastName).ToArray();
               });
         }

         return Task.CompletedTask;
      }

      protected override async Task RespondToAppStateChange(string newState, bool andRebuildToolbars = false)
      {
         switch (newState)
         {
            case DASHBOARD_APP_STATE:
               await ChangeToolbarViewModelState<IDashboardViewModel, DashboardViewModel>(newState)
                  .WithoutChangingContext();
               break;

            case ACCOUNTS_APP_STATE:
               await ChangeToolbarViewModelState<IAccountsViewModel, AccountsViewModel>(newState)
                  .WithoutChangingContext();
               break;

            case SETTINGS_APP_STATE:
               await ChangeToolbarViewModelState<ISettingsViewModel, SettingsViewModel>(newState)
                  .WithoutChangingContext();
               break;

            case SIGN_IN_APP_STATE:
               await RequestLogin().WithoutChangingContext();
               break;

            case CREATE_ACCOUNT_APP_STATE:
               await ChangeLoginViewModelState<ICreateAccountViewModel, CreateAccountViewModel
                  >(CREATION_SUCCESS_APP_STATE,
                     SIGN_IN_APP_STATE,
                     ServiceDateIsValidAndUserCanBeSaved)
                  .WithoutChangingContext();
               break;

            case CREATION_SUCCESS_APP_STATE:
               await ChangeLoginViewModelState<ICreationSuccessViewModel, CreationSuccessViewModel
               >(SIGN_IN_APP_STATE, NO_APP_STATE).WithoutChangingContext();
               break;

            case LOGOUT_APP_STATE:
               // TODO - Log out physically -- ??
               await RequestLogin().WithoutChangingContext();
               break;
         }

         // PRIVATE METHODS
         async Task RequestLogin()
         {
            await ChangeLoginViewModelState<ILogInViewModel, LogInViewModel>(DASHBOARD_APP_STATE,
               CREATE_ACCOUNT_APP_STATE,
               UserExists).WithoutChangingContext();
         }
      }

      private async Task<bool> ServiceDateIsValidAndUserCanBeSaved(IWizardViewModelWithTasks viewModel)
      {
         if (!(viewModel is CreateAccountViewModel viewModelAsNewAccount))
         {
            return false;
         }

         // If the service start date is in the future, issue an error
         if (viewModelAsNewAccount.ServiceStartDate.GetValueOrDefault().Date > DateTime.Now.AddDays(30).Date)
         {
            await DialogFactory.ShowYesNoDialog(ERROR_TITLE,
               "The service start date must be within 30 days.  Please try again.",
               OK_TEXT,
               "").WithoutChangingContext();
            await viewModel.SetOutcome(Outcomes.TryAgain).WithoutChangingContext();
            return false;
         }

         // If the service start date is in the future, issue an error
         if (viewModelAsNewAccount.ServiceStartDate.GetValueOrDefault().Date < DateTime.Now.Date)
         {
            await DialogFactory.ShowYesNoDialog(ERROR_TITLE,
               "Service can only start date on or after today.  Please try again.",
               OK_TEXT,
               "").WithoutChangingContext();
            await viewModel.SetOutcome(Outcomes.TryAgain).WithoutChangingContext();
            return false;
         }

         // Save the record

         // Copy common properties to the smaller database type
         var accountToSave = new SavedAccountViewModel();
         accountToSave.CopySettablePropertyValuesFrom((ICommonAccountProps) viewModel);

         var savedId = await _database.InsertAsync(accountToSave).WithoutChangingContext();

         if (savedId < 0)
         {
            await DialogFactory.ShowYesNoDialog(ERROR_TITLE, "The user could not be saved.  Please try again.", OK_TEXT,
               "").WithoutChangingContext();
            await viewModel.SetOutcome(Outcomes.TryAgain).WithoutChangingContext();
            return false;
         }

         // ELSE success
         await viewModel.SetOutcome(Outcomes.Next).WithoutChangingContext();
         return true;
      }

      private async Task<bool> UserExists(IWizardViewModelWithTasks viewModel)
      {
         if (!(viewModel is ICanLogIn viewModelAsCanLogIn) || viewModelAsCanLogIn.UserName.IsEmpty() ||
             viewModelAsCanLogIn.Password.IsEmpty())
         {
            return false;
         }

         // If the account doesn't exist under this user name, show a modal dialog error.
         var foundUser = await _database.Table<SavedAccountViewModel>()
            .FirstOrDefaultAsync(vm => vm.UserName == viewModelAsCanLogIn.UserName)
            .WithoutChangingContext();

         if (foundUser == default)
         {
            await DialogFactory.ShowYesNoDialog(ERROR_TITLE, "The user name does not exist.  Please try again.",
               OK_TEXT,
               "").WithoutChangingContext();
            await viewModel.SetOutcome(Outcomes.TryAgain).WithoutChangingContext();
            return false;
         }

         // If the account exists and the password is incorrect, show a modal dialog error.
         if (foundUser.Password.IsDifferentThan(viewModelAsCanLogIn.Password))
         {
            await DialogFactory.ShowYesNoDialog(ERROR_TITLE, "Incorrect password.  Please try again.", OK_TEXT,
               "").WithoutChangingContext();
            await viewModel.SetOutcome(Outcomes.TryAgain).WithoutChangingContext();
            return false;
         }

         // ELSE the user exists and the password is correct. Show a modal dialog and then move on to Accounts.
         //await DialogFactory.ShowYesNoDialog(SUCCESS_TITLE, "You are now logged in.", OK_TEXT,
         //                                    "").WithoutChangingContext();
         await viewModel.SetOutcome(Outcomes.Next).WithoutChangingContext();

         return true;
      }
   }
}