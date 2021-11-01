// *********************************************************************************
// Copyright @2021 Marcus Technical Services, Inc.
// <copyright
// file=CreateAccountViewModel.cs
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

namespace Com.MarcusTS.ModernAppDemo.ViewModels
{
   using System;
   using System.Threading.Tasks;
   using Com.MarcusTS.ModernAppDemo.Common.Interfaces;
   using Com.MarcusTS.PlatformIndependentShared.Common.Interfaces;
   using Com.MarcusTS.PlatformIndependentShared.Common.Utils;
   using Com.MarcusTS.PlatformIndependentShared.ViewModels;
   using Com.MarcusTS.ResponsiveTasks;
   using Com.MarcusTS.SharedUtils.Utils;
   using Com.MarcusTS.UI.XamForms.Common.Validations;
   using Com.MarcusTS.UI.XamForms.ViewModels;
   using SQLite;

   public interface ICreateAccountViewModel : IWizardViewModel_Forms, ICommonAccountProps,
                                              IHandleNullableDateTimeChangeTask
   { }

   public class CreateAccountViewModel : WizardViewModel_Forms, ICreateAccountViewModel
   {
      private string    _confirmPassword;
      private string    _firstName;
      private string    _lastName;
      private string    _password;
      private DateTime? _serviceStartDate;
      private string    _stateOfResidence;
      private string    _userName;

      public CreateAccountViewModel()
      {
         Title = "Create New Account";

         // IMPORTANT
         // Required to make the visual date time selector work.
         // Without this setting, the selector shows DateTime.Now, but stores null.
         ServiceStartDate = DateTime.Now.Date;
      }

      [CommonViewModelValidations_Forms.ValidatableTwoWayNonEmptyViewModelValidationAttribute( 4,
         IsPassword = ViewModelCustomAttribute_Static_PI.TRUE_BOOL,
         CanUnmaskPassword = ViewModelCustomAttribute_Static_PI.TRUE_BOOL,
         PlaceholderText = CommonViewModelValidations_Forms.CONFIRM_PASSWORD_PLACEHOLDER_TEXT,
         ValidatorType = typeof( ComparisonEntryValidatorBehavior_Forms ) )]
      public string ConfirmPassword
      {
         get => _confirmPassword;
         set
         {
            if ( SetProperty( ref _confirmPassword, value ) )


            {
               VerifyCommandCanExecute().FireAndFuhgetAboutIt();
            }
         }
      }

      [CommonViewModelValidations_Forms.ValidatableTwoWayNonEmptyViewModelValidationAttribute( 8,
         PlaceholderText = "State of Residence?", InputTypeStr = ValidationUtils_PI.STATE_INPUT_TYPE )]
      public string StateOfResidence
      {
         get => _stateOfResidence;
         set
         {
            if ( SetProperty( ref _stateOfResidence, value ) )


            {
               VerifyCommandCanExecute().FireAndFuhgetAboutIt();
            }
         }
      }

      // For storage only
      public string FirstAndLastName { get; set; }

      [CommonViewModelValidations_Forms.ValidatableTwoWayNonEmptyViewModelValidationAttribute( 0,
         PlaceholderText = "First Name", ExcludedChars = CommonViewModelValidations_Forms.FORBIDDEN_CHARS,
         IsInitialFocus = ViewModelCustomAttribute_Static_PI.TRUE_BOOL )]
      public string FirstName
      {
         get => _firstName;
         set
         {
            if ( SetProperty( ref _firstName, value ) )


            {
               RefreshFirstAndLastName().FireAndFuhgetAboutIt();
            }
         }
      }

      // For storage only
      [PrimaryKey, AutoIncrement,] 
      public int Id { get; set; }

      [CommonViewModelValidations_Forms.ValidatableTwoWayNonEmptyViewModelValidationAttribute( 1,
         PlaceholderText = "Last Name", ExcludedChars = CommonViewModelValidations_Forms.FORBIDDEN_CHARS )]
      public string LastName
      {
         get => _lastName;
         set
         {
            if ( SetProperty( ref _lastName, value ) )


            {
               RefreshFirstAndLastName().FireAndFuhgetAboutIt();
            }
         }
      }

      [CommonViewModelValidations_Forms.PasswordValidatableTwoWayNonEmptyViewModelValidationAttribute( 3 )]
      public string Password
      {
         get => _password;
         set
         {
            if ( SetProperty( ref _password, value ) )


            {
               VerifyCommandCanExecute().FireAndFuhgetAboutIt();
            }
         }
      }

      [CommonViewModelValidations_Forms.PhoneValidatableTwoWayNonEmptyViewModelValidationAttribute( 5 )]
      public string Phone { get; set; }

      [CommonViewModelValidations_Forms.DateTimeValidatableTwoWayNonEmptyViewModelValidationAttribute( 6,
         PlaceholderText = "Start Date" )]
      public DateTime? ServiceStartDate
      {
         get => _serviceStartDate;
         set
         {
            if ( SetProperty( ref _serviceStartDate, value ) )


            {
               VerifyCommandCanExecute().FireAndFuhgetAboutIt();
            }
         }
      }

      [CommonViewModelValidations_Forms.UserNameValidatableTwoWayNonEmptyValidationAttribute( 2 )]
      public string UserName
      {
         get => _userName;
         set
         {
            if ( SetProperty( ref _userName, value ) )


            {
               VerifyCommandCanExecute().FireAndFuhgetAboutIt();
            }
         }
      }

      public Task HandleNullableResultChangeTask( IResponsiveTaskParams paramDict )
      {
         var dateTime = paramDict.GetTypeSafeValue<DateTime>( 0 );
         {
            ServiceStartDate = dateTime;
         }

         return Task.CompletedTask;
      }

      private async Task RefreshFirstAndLastName()
      {
         FirstAndLastName = FirstName?.Trim() + " " + LastName?.Trim();
         await VerifyCommandCanExecute().WithoutChangingContext();
      }
   }
}