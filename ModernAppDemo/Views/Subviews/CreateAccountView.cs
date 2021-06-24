// *********************************************************************************
// Copyright @2021 Marcus Technical Services, Inc.
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

using Com.MarcusTS.ResponsiveTasks.XamFormsSupport.Common.Behaviors;
using Com.MarcusTS.ResponsiveTasks.XamFormsSupport.Views.Controls;

namespace ModernAppDemo.Views.Subviews
{
   using System.Linq;
   using System.Threading.Tasks;
   using Com.MarcusTS.ResponsiveTasks.XamFormsSupport.ViewModels;
   using Com.MarcusTS.ResponsiveTasks.XamFormsSupport.Views.Subviews;
   using Com.MarcusTS.SharedForms.Common.Behaviors;
   using Com.MarcusTS.SharedForms.Common.Utils;
   using Com.MarcusTS.SharedForms.Views.Controls;
   using Com.MarcusTS.SharedUtils.Utils;
   using Xamarin.Forms;

   public interface ICreateAccountView : IFlexViewWithTasks
   {
   }

   public class CreateAccountView : FlexViewWithTasks, ICreateAccountView
   {
      protected override Task AfterSourceViewsLoaded()
      {
         // Set the comparison validator
         
         // 1. Get the binding context as a view with a list of behaviors
         if (BindingContext is IHaveValidationViewModelHelperWithTasks bindingContextAsHavingValidationViewModelHelperWithTasks)
         {
            var allBehaviors = bindingContextAsHavingValidationViewModelHelperWithTasks.ValidationHelperWithTasks.GetBehaviors();

            // 2. Find the comparison validator
            var comparisonBehavior =
               allBehaviors.FirstOrDefault(b => b is IComparisonEntryValidatorBehaviorWithTasks) as
                   IComparisonEntryValidatorBehaviorWithTasks;

            // 3. Use the MasterAnimatedStackLayoutAsView/s Children as entries (which they are); find the password view (and entry).
            var passwordView  =
               MasterAnimatedStackLayoutAsView.SourceViews.FirstOrDefault(sv => sv is IValidatableEntryWithTasks viewAsValidatableEntryWithTasks &&
                   viewAsValidatableEntryWithTasks.EditableEntry.IsNotNullOrDefault() &&
                   viewAsValidatableEntryWithTasks.EditableEntry.Behaviors.Any(b => b is IPasswordEntryValidationBehaviorWithTasks)) as IValidatableEntryWithTasks;

            // 4. Assign the password entry to the comparison validator
            var passwordEntry = passwordView?.EditableEntry;

            if (passwordEntry.IsNotNullOrDefault() && comparisonBehavior.IsNotNullOrDefault())
            {
               // 3. Link the password and comparison behaviors
               // ReSharper disable once PossibleNullReferenceException
               comparisonBehavior.CompareEntry = passwordEntry;
            }
         }

         // Add the save and cancel buttons

         var nextTabIndex = MasterAnimatedStackLayoutAsView.SourceViews.Count;
         
         var saveButton =
            CreateButton(
                         "Create Account", 
                         (BindingContext as IWizardViewModelWithTasks)?.NextCommand, 
                         nextTabIndex++, 
                         true,
                         useExtraTopSpace:true);

         MasterAnimatedStackLayoutAsView.SourceViews.Add(saveButton as View);

         var cancelButton =
            CreateButton(
                         "CANCEL".Expanded(), 
                         (BindingContext as IWizardViewModelWithTasks)?.CancelCommand, 
                         // ReSharper disable once RedundantAssignment
                         nextTabIndex++,
                         false,
                         Color.Red, 
                         null, 
                         backColor:Color.Transparent);

         MasterAnimatedStackLayoutAsView.SourceViews.Add(cancelButton as View);

         return base.AfterSourceViewsLoaded();
      }
   }
}