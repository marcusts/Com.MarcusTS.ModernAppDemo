// *********************************************************************************
// Copyright @2021 Marcus Technical Services, Inc.
// <copyright
// file=CreateAccountTitledFlexViewHost.cs
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

namespace ModernAppDemo.Views.Subviews
{
   using System.Threading.Tasks;
   using Com.MarcusTS.ResponsiveTasks.XamFormsSupport.Common.Utils;
   using Com.MarcusTS.ResponsiveTasks.XamFormsSupport.ViewModels;
   using Com.MarcusTS.ResponsiveTasks.XamFormsSupport.Views.Subviews;
   using Com.MarcusTS.SharedForms.Common.Utils;
   using Com.MarcusTS.SharedUtils.Utils;
   using Xamarin.Forms;

   public interface ICreateAccountTitledFlexViewHost : ITitledFlexViewHost
   {
   }

   public class CreateAccountTitledFlexViewHost : TitledFlexViewHost, ICreateAccountTitledFlexViewHost
   {
      private IFlexViewWithTasksBase _derivedFlexViewHost;

      protected override IFlexViewWithTasksBase DerivedFlexViewHost =>
         _derivedFlexViewHost ??= 
            new CreateAccountView
            {
               // HACK Hard-coded; Tends to over-react logarythmically
               HeightRequest = ScaleUtils.CURRENT_DEVICE_HEIGHT - 220.0.AdjustForOsAndDevice()
            };

      protected override Task AfterHandlingPostBindingTask()
      {
         // IMPORTANT Setting some animation speeds here because they *must* follow the base class's HandlePostBindingTask
         AnimatedBaseLayout.FlowSettings.TranslateBoundsMilliseconds =
            2 * FlowableConst.DEFAULT_TRANSLATE_BOUNDS_MILLISECONDS;
         AnimatedBaseLayout.FlowSettings.FadeInMilliseconds =
            2 * FlowableConst.DEFAULT_FADE_IN_MILLISECONDS;
         AnimatedBaseLayout.AnimateInDelayMilliseconds = 2 * FlowableConst.MILLISECOND_DELAY_BETWEEN_ANIMATION_TRANSITIONS;

         // Add the save and cancel buttons
         if (FlexViewHostAsView.BindingContext.IsNotNullOrDefault() &&
             FlexViewHostAsView.BindingContext is IWizardViewModelWithTasks bindingContextAsWizard &&
             AnimatedBaseLayout.IsNotNullOrDefault())
         {
            var nextTabIndex = AnimatedBaseLayout.SourceViews.Count;

            var saveButton =
               FlowableUtils.CreateFlowableControlButton(
                  "Create Account",
                  bindingContextAsWizard.NextCommand,
                  bindingContextAsWizard,
                  nextTabIndex++,
                  true,
                  extraTopSpace: FlowableConst.DEFAULT_EXTRA_TOP_MARGIN);

            AnimatedBaseLayout.SourceViews.Add(saveButton as View);

            var cancelButton =
               FlowableUtils.CreateFlowableControlButton(
                  "CANCEL".Expanded(),
                  bindingContextAsWizard.CancelCommand,
                  bindingContextAsWizard,
                  nextTabIndex++,
                  false,
                  Color.Red,
                  null,
                  backColor: Color.Transparent);

            AnimatedBaseLayout.SourceViews.Add(cancelButton as View);
         }

         return Task.CompletedTask;
      }
   }
}