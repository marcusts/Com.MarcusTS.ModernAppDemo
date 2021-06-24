// *********************************************************************************
// Copyright @2021 Marcus Technical Services, Inc.
// <copyright
// file=MasterViewPresenter.cs
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
// *********************************************************************************6

namespace ModernAppDemo.Views.Presenters
{
   using System.Threading.Tasks;
   using Com.MarcusTS.ResponsiveTasks.XamFormsSupport.Views.Pages;
   using Com.MarcusTS.ResponsiveTasks.XamFormsSupport.Views.Presenters;
   using Com.MarcusTS.SharedUtils.Utils;
   using Subviews;
   using ViewModels;
   using Xamarin.Essentials;

   public interface IMasterViewPresenter : IMasterViewPresenterBase
   {
   }

   public class MasterViewPresenter : MasterViewPresenterWithTasks, IMasterViewPresenter
   {
      public MasterViewPresenter(ICanShowProgressSpinner spinnerHost) : base(spinnerHost)
      {
      }

      protected override async Task RespondToViewModelChange(object newModule)
      {

         if (newModule is IDashboardViewModel)
         {
            await ChangeContentView<IDashboardTitledFlexViewHost, DashboardTitledFlexViewHost>(newModule).WithoutChangingContext();
         }
         else if (newModule is ISettingsViewModel)
         {
            await ChangeContentView<ISettingsTitledFlexViewHost, SettingsTitledFlexViewHost>(newModule).WithoutChangingContext();
         }
         else if (newModule is IAccountsViewModel)
         {
            await ChangeContentView<IAccountsTitledFlexViewHost, AccountsTitledFlexViewHost>(newModule).WithoutChangingContext();
         }
         else if (newModule is ILogInViewModel)
         {
            await ChangeContentView<ILogInTitledFlexViewHost, LogInTitledFlexViewHost>(newModule).WithoutChangingContext();
         }
         else if (newModule is ICreateAccountViewModel)
         {
            await ChangeContentView<ICreateAccountTitledFlexViewHost, CreateAccountTitledFlexViewHost>(newModule).WithoutChangingContext();
         }
         else if (newModule is ICreationSuccessViewModel)
         {
            await ChangeContentView<ICreationSuccessTitledFlexViewHost, CreationSuccessTitledFlexViewHost>(newModule).WithoutChangingContext();
         }
      }
   }
}