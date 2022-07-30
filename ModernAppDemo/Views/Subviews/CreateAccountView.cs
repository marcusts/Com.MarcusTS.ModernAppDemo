// *********************************************************************************
// Copyright @2022 Marcus Technical Services, Inc.
// <copyright
// file=CreateAccountView.cs
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

namespace Com.MarcusTS.ModernAppDemo.Views.Subviews
{
   using System.Linq;
   using System.Threading.Tasks;
   using Com.MarcusTS.PlatformIndependentShared.ViewModels;
   using Com.MarcusTS.SharedUtils.Controls;
   using Com.MarcusTS.SharedUtils.Utils;
   using Com.MarcusTS.UI.XamForms.Common.Interfaces;
   using Com.MarcusTS.UI.XamForms.Common.Validations;
   using Com.MarcusTS.UI.XamForms.Views.Controls;
   using Com.MarcusTS.UI.XamForms.Views.Subviews;
   using Xamarin.Forms;

   public interface ICreateAccountView : IScrollableFlexView_Forms
   { }

   public class CreateAccountView : ScrollableFlexView_Forms, ICreateAccountView
   {
      protected override async Task BeforeSourceViewsAssigned( BetterObservableCollection<View> retViews )
      {
         await base.BeforeSourceViewsAssigned( retViews ).AndReturnToCallingContext();

         // Set the comparison validator

         // 1. Get the binding context as a view with a list of behaviors
         if ( BindingContext is IHaveValidationViewModelHelper
            bindingContextAsHavingValidationViewModelHelper )
         {
            var allBehaviors = bindingContextAsHavingValidationViewModelHelper.ValidationHelper
                                                                              .GetBehaviors();

            // 2. Find the comparison validator
            var comparisonBehavior =
               allBehaviors.FirstOrDefault( b => b is IComparisonEntryValidatorBehavior_Forms ) as
                  IComparisonEntryValidatorBehavior_Forms;

            // 3. Use the retViews as entries (which they are); find the password view (and entry).
            var passwordView =
               retViews.FirstOrDefault( sv =>
                                           sv is IValidatableEntry_Forms viewAsValidatableEntry      &&
                                           viewAsValidatableEntry.EditableEntry.IsNotNullOrDefault() &&
                                           viewAsValidatableEntry.EditableEntry.Behaviors.Any( b =>
                                              b is IPasswordEntryValidationBehavior_Forms ) ) as
                  IValidatableEntry_Forms;

            // 4. Assign the password entry to the comparison validator
            var passwordEntry = passwordView?.EditableEntry;

            if ( passwordEntry.IsNotNullOrDefault() && comparisonBehavior.IsNotNullOrDefault() )
            {
               // 3. Link the password and comparison behaviors
               // ReSharper disable once PossibleNullReferenceException
               comparisonBehavior.CompareEntry = passwordEntry;
            }
         }
      }

      public CreateAccountView( ICanShowProgressSpinner_Forms spinnerHost ) : base( spinnerHost )
      { }
   }
}