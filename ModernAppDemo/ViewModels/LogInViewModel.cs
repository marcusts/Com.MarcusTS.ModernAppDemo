// *********************************************************************************
// Copyright @2021 Marcus Technical Services, Inc.
// <copyright
// file=LogInViewModel.cs
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

namespace ModernAppDemo.ViewModels
{
   using Com.MarcusTS.ResponsiveTasks.XamFormsSupport.Common.Utils;
using Com.MarcusTS.ResponsiveTasks.XamFormsSupport.ViewModels;
   using Com.MarcusTS.SharedForms.Common.Behaviors;
   using Com.MarcusTS.SharedForms.Common.Interfaces;
   using Com.MarcusTS.SharedForms.ViewModels;
   using Common.Interfaces;
using ModernAppDemo.Common.Utils;
   using Xamarin.Forms;
using static ModernAppDemo.Common.Utils.CommonViewModelValidations;

   public interface ILogInViewModel : IWizardViewModelWithTasks, ICanLogIn
   {
   }

   public class LogInViewModel : WizardViewModelWithTasks, ILogInViewModel
   {
      private string _password;
      private string _userName;

      public LogInViewModel()
      {
         Title = "Sign In";
      }

      [TwoWayNonEmptyViewModelValidationAttribute(0, PlaceholderText = CommonViewModelValidations.USER_NAME_PLACEHOLDER_TEXT)]
      public string UserName
      {
         get => _userName;
         set
         {
            if (SetProperty(ref _userName, value))
            {
               VerifyCommandCanExecute();
            }
         }
      }
      
      [TwoWayNonEmptyViewModelValidationAttribute(1, PlaceholderText = CommonViewModelValidations.PASSWORD_PLACEHOLDER_TEXT, IsPassword = ViewModelValidationAttribute_Static.TRUE_BOOL, CanUnmaskPassword = ViewModelValidationAttribute_Static.TRUE_BOOL)]
      public string Password
      {
         get => _password;
         set
         {
            if (SetProperty(ref _password, value))
            {
               VerifyCommandCanExecute();
            }
         }
      }
   }
}