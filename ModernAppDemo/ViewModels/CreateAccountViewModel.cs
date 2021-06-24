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

using Com.MarcusTS.SharedUtils.Utils;
using System.Threading.Tasks;

namespace ModernAppDemo.ViewModels
{
    using Com.MarcusTS.ResponsiveTasks.XamFormsSupport.ViewModels;
    using Com.MarcusTS.SharedForms.Common.Behaviors;
    using Com.MarcusTS.SharedForms.Common.Interfaces;
    using Com.MarcusTS.SharedForms.ViewModels;
    using Common.Interfaces;
    using Common.Utils;
    using SQLite;
    using System;
    using Xamarin.Forms;

    public interface ICreateAccountViewModel : IWizardViewModelWithTasks, ICommonAccountProps,
       IHandleNullableDateTimeChanges
    {
    }

    public class CreateAccountViewModel : WizardViewModelWithTasks, ICreateAccountViewModel
    {
        private string _confirmPassword;
        private string _firstName;
        private bool _isTheSkyBlue;
        private string _lastName;
        private string _password;
        private DateTime? _serviceStartDate;
        private string _stateOfResidence;
        private string _userName;

        public CreateAccountViewModel()
        {
            Title = "Create New Account";

            // IMPORTANT
            // Required to make the visual date time selector work.
            // Without this setting, the selector shows DateTime.Now, but stores null.
            ServiceStartDate = DateTime.Now.Date;
        }

      [CommonViewModelValidations.ValidatableTwoWayNonEmptyViewModelValidationAttribute(displayOrder: 4,
                                    IsPassword = ViewModelValidationAttribute_Static.TRUE_BOOL,
                                    CanUnmaskPassword = ViewModelValidationAttribute_Static.TRUE_BOOL,
                                    PlaceholderText = CommonViewModelValidations.CONFIRM_PASSWORD_PLACEHOLDER_TEXT,
                                    ValidatorType = typeof(ComparisonEntryValidatorBehavior))]
      public string ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                if (SetProperty(ref _confirmPassword, value))
                {
                    // WARNING Invalid TPL root.
                    VerifyCommandCanExecute().FireAndForget();
                }
            }
        }

        // For storage only
        public string FirstAndLastName { get; set; }

      [CommonViewModelValidations.ValidatableTwoWayNonEmptyViewModelValidationAttribute(displayOrder: 0, PlaceholderText = "First Name", ExcludedChars = CommonViewModelValidations.FORBIDDEN_CHARS)]
      public string FirstName
        {
            get => _firstName;
            set
            {
                if (SetProperty(ref _firstName, value))
                {
                    // WARNING Invalid TPL root.
                    RefreshFirstAndLastName().FireAndForget();
                }
            }
        }

        // For storage only
        [PrimaryKey] [AutoIncrement] public int Id { get; set; }

      [CommonViewModelValidations.ValidatableTwoWayNonEmptyViewModelValidationAttribute(displayOrder: 7, PlaceholderText = "Is the Sky Blue?",
                         InputType = InputTypes.CheckBoxInput)]
      public bool IsTheSkyBlue
        {
            get => _isTheSkyBlue;
            set
            {
                if (SetProperty(ref _isTheSkyBlue, value))
                {
                    // WARNING Invalid TPL root.
                    VerifyCommandCanExecute().FireAndForget();
                }
            }
        }

      [CommonViewModelValidations.ValidatableTwoWayNonEmptyViewModelValidationAttribute(displayOrder: 0, PlaceholderText = "Last Name", ExcludedChars = CommonViewModelValidations.FORBIDDEN_CHARS)]
      public string LastName
        {
            get => _lastName;
            set
            {
                if (SetProperty(ref _lastName, value))
                {
                    // WARNING Invalid TPL root.
                    RefreshFirstAndLastName().FireAndForget();
                }
            }
        }

      [CommonViewModelValidations.PasswordValidatableTwoWayNonEmptyViewModelValidation(3)]
      public string Password
        {
            get => _password;
            set
            {
                if (SetProperty(ref _password, value))
                {
                    // WARNING Invalid TPL root.
                    VerifyCommandCanExecute().FireAndForget();
                }
            }
        }

      [CommonViewModelValidations.PhoneValidatableTwoWayNonEmptyViewModelValidationAttribute(displayOrder: 5)]
      public string Phone { get; set; }

        [CommonViewModelValidations.DateTimeValidatableTwoWayNonEmptyViewModelValidationAttribute(displayOrder: 6, PlaceholderText = "Start Date")]
        public DateTime? ServiceStartDate
        {
            get => _serviceStartDate;
            set
            {
                if (SetProperty(ref _serviceStartDate, value))
                {
                    // WARNING Invalid TPL root.
                    VerifyCommandCanExecute().FireAndForget();
                }
            }
        }

      [CommonViewModelValidations.ValidatableTwoWayNonEmptyViewModelValidationAttribute(displayOrder: 8, PlaceholderText = "State of Residence?", InputType = InputTypes.StateInput)]
      public string StateOfResidence
        {
            get => _stateOfResidence;
            set
            {
                if (SetProperty(ref _stateOfResidence, value))
                {
                    // WARNING Invalid TPL root.
                    VerifyCommandCanExecute().FireAndForget();
                }
            }
        }

      [CommonViewModelValidations.UserNameValidatableTwoWayNonEmptyValidation(2)]
      public string UserName
        {
            get => _userName;
            set
            {
                if (SetProperty(ref _userName, value))
                {
                    // WARNING Invalid TPL root.
                    VerifyCommandCanExecute().FireAndForget();
                }
            }
        }

        public void HandleNullableResultChanged(DateTime? dateTime)
        {
            ServiceStartDate = dateTime;
        }

        private async Task RefreshFirstAndLastName()
        {
            FirstAndLastName = FirstName?.Trim() + " " + LastName?.Trim();
            await VerifyCommandCanExecute().WithoutChangingContext();
        }
    }
}
