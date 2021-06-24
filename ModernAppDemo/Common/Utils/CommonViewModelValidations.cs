// *********************************************************************************
// Copyright @2021 Marcus Technical Services, Inc.
// <copyright
// file=CommonViewModelValidations.cs
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

namespace ModernAppDemo.Common.Utils
{
   using System.Runtime.CompilerServices;
   using Com.MarcusTS.ResponsiveTasks.XamFormsSupport.Common.Behaviors;
   using Com.MarcusTS.ResponsiveTasks.XamFormsSupport.Common.Utils;
   using Com.MarcusTS.SharedForms.Common.Behaviors;
   using Com.MarcusTS.SharedForms.ViewModels;
   using Xamarin.Forms;

/// <summary></summary>
   public static class CommonViewModelValidations
   {
      public const int    MUST_BE_NON_EMPTY_MIN_LENGTH      = 1;
      public const string PASSWORD_PLACEHOLDER_TEXT         = "Password";
      public const int    PHONE_EDIT_LEN                    = 14;
      public const string PHONE_PLACEHOLDER_TEXT            = "Phone";
      public const string USER_NAME_PLACEHOLDER_TEXT        = "User Name";
      public const string CONFIRM_PASSWORD_PLACEHOLDER_TEXT = "Confirm " + PASSWORD_PLACEHOLDER_TEXT;
      public const string FORBIDDEN_CHARS                   = "!@#$%^&";
      public const string PHONE_NUMBER_FORMAT               = "{0:(###) ###-####}";
      public const string PHONE_NUMBER_MASK                 = "(XXX) XXX-XXXX";

      public class NonEmptyViewModelValidationAttribute : ViewModelValidationAttribute
      {
         public NonEmptyViewModelValidationAttribute(int displayOrder, [CallerMemberName] string propName = "")
            : base(displayOrder: displayOrder, viewModelPropertyName: propName)
         {
            MinLength = MUST_BE_NON_EMPTY_MIN_LENGTH;
            PlaceholderTextColorHex = ThemeUtils_RTXFS.MAIN_STAGE_THEME_COLOR_HEX;
         }
      }

      public class OneWayNonEmptyViewModelValidationAttribute : NonEmptyViewModelValidationAttribute
      {
         public OneWayNonEmptyViewModelValidationAttribute(int displayOrder, [CallerMemberName] string propName = "")
            : base(displayOrder: displayOrder, propName)
         {
            BoundMode = BindingMode.OneWay;
         }
      }

      public class TwoWayNonEmptyViewModelValidationAttribute : NonEmptyViewModelValidationAttribute
      {
         public TwoWayNonEmptyViewModelValidationAttribute(int displayOrder, [CallerMemberName] string propName = "")
            : base(displayOrder: displayOrder, propName)
         {
            BoundMode = BindingMode.TwoWay;
         }
      }

      public class ValidatableTwoWayNonEmptyViewModelValidationAttribute : TwoWayNonEmptyViewModelValidationAttribute
        {
          public ValidatableTwoWayNonEmptyViewModelValidationAttribute(int displayOrder, [CallerMemberName] string propName = "")
              : base(displayOrder: displayOrder, propName)
          {
              ShowValidationErrors = ViewModelValidationAttribute_Static.TRUE_BOOL;
          }
      }

      public class PasswordValidatableTwoWayNonEmptyViewModelValidationAttribute : ValidatableTwoWayNonEmptyViewModelValidationAttribute
        {
         public PasswordValidatableTwoWayNonEmptyViewModelValidationAttribute(int displayOrder, [CallerMemberName] string propName = "")
            : base(displayOrder, propName)
         {
            CanUnmaskPassword = ViewModelValidationAttribute_Static.TRUE_BOOL;
            IsPassword        = ViewModelValidationAttribute_Static.TRUE_BOOL;
            MinLength = PasswordEntryValidationBehaviorWithTasks.DEFAULT_MIN_CHARACTER_COUNT;
            MaxLength = PasswordEntryValidationBehaviorWithTasks.DEFAULT_MAX_CHARACTER_COUNT;
            PlaceholderText   = PASSWORD_PLACEHOLDER_TEXT;
            ValidatorType     = typeof(PasswordEntryValidationBehaviorWithTasks);
         }
      }

      public class PhoneValidatableTwoWayNonEmptyViewModelValidationAttribute : ValidatableTwoWayNonEmptyViewModelValidationAttribute
      {
          public PhoneValidatableTwoWayNonEmptyViewModelValidationAttribute(int displayOrder, [CallerMemberName] string propName = "")
              : base(displayOrder, propName)
          {
              PlaceholderText = PHONE_PLACEHOLDER_TEXT;
              MinLength= PHONE_EDIT_LEN;
              MaxLength = PHONE_EDIT_LEN;
              Mask = PHONE_NUMBER_MASK;
              StringFormat = PHONE_NUMBER_FORMAT;
              BoundMode = BindingMode.TwoWay;
              ValidatorType = typeof(PhoneEntryValidatorBehaviorWithTasks);
          }
      }

      public class UserNameValidatableTwoWayNonEmptyValidationAttribute : ValidatableTwoWayNonEmptyViewModelValidationAttribute
        {
         public UserNameValidatableTwoWayNonEmptyValidationAttribute(int displayOrder, [CallerMemberName] string propName = "")
            : base(displayOrder, propName)
         {
            PlaceholderText = USER_NAME_PLACEHOLDER_TEXT;
         }
      }

      public class DateTimeValidatableTwoWayNonEmptyViewModelValidationAttribute :
              ValidatableTwoWayNonEmptyViewModelValidationAttribute
      {
          public DateTimeValidatableTwoWayNonEmptyViewModelValidationAttribute(int displayOrder,
              [CallerMemberName] string propName = "")
              : base(displayOrder, propName)
          {
              InputType = InputTypes.NullableDateTimeInput;
          }
        }
    }
}
