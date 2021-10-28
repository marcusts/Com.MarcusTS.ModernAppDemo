// *********************************************************************************
// Copyright @2021 Marcus Technical Services, Inc.
// <copyright
// file=SettingsViewModel.cs
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
   using Com.MarcusTS.ModernAppDemo.Common.Navigation;
   using Com.MarcusTS.PlatformIndependentShared.Common.Utils;
   using Com.MarcusTS.SharedUtils.Utils;
   using Com.MarcusTS.UI.XamForms.Common.Validations;
   using Com.MarcusTS.UI.XamForms.ViewModels;

   public interface ISettingsViewModel : IWizardViewModel_Forms
   { }

   public class SettingsViewModel : WizardViewModel_Forms, ISettingsViewModel
   {
      private bool _isTheSkyBlue;

      public SettingsViewModel()
      {
         Title = AppStateManager.SETTINGS_TITLE;
      }

      [CommonViewModelValidations_Forms.ValidatableTwoWayNonEmptyViewModelValidationAttribute( 7,
         PlaceholderText = "Is the Sky Blue?", InputTypeStr = ValidationUtils_PI.CHECK_BOX_INPUT_TYPE )]
      public bool IsTheSkyBlue
      {
         get => _isTheSkyBlue;
         set
         {
            if ( SetProperty( ref _isTheSkyBlue, value ) )
            {
               VerifyCommandCanExecute().FireAndFuhgetAboutIt();
            }
         }
      }
   }
}