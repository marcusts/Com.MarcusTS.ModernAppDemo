// *********************************************************************************
// Copyright @2022 Marcus Technical Services, Inc.
// <copyright
// file=LogInView.cs
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
   using System.Threading.Tasks;
   using Com.MarcusTS.PlatformIndependentShared.Common.Utils;
   using Com.MarcusTS.SharedUtils.Controls;
   using Com.MarcusTS.SharedUtils.Utils;
   using Com.MarcusTS.UI.XamForms.Common.Interfaces;
   using Com.MarcusTS.UI.XamForms.Common.Utils;
   using Com.MarcusTS.UI.XamForms.ViewModels;
   using Com.MarcusTS.UI.XamForms.Views.Controls;
   using Com.MarcusTS.UI.XamForms.Views.Subviews;
   using Xamarin.Forms;

   public interface ILogInView : IScrollableFlexView_Forms
   { }

   public class LogInView : ScrollableFlexView_Forms, ILogInView
   {
      protected override async Task BeforeSourceViewsAssigned( BetterObservableCollection<View> retViews )
      {
         await base.BeforeSourceViewsAssigned( retViews ).WithoutChangingContext();

         if ( AnimatableLayout is IFlowableCollectionCanvas_Forms animatableLayoutAsFlowableCollectionCanvas )
         {
            await animatableLayoutAsFlowableCollectionCanvas.SetScrollBottomMargin( 0 ).WithoutChangingContext();
         }

         // Add the save and new account buttons
         if ( BindingContext is IWizardViewModel_Forms bindingContextAsWizardViewModel )
         {
            await AddLoginAndCancelButtons().ConsiderBeginInvokeTaskOnMainThread();
         }

         // -----------------------------------------------------------------------------------------------
         // P R I V A T E   M E T H O D S
         // -----------------------------------------------------------------------------------------------
         Task AddLoginAndCancelButtons()
         {
            var nextTabIndex = retViews.Count;

            var loginButton =
               FlowableUtils_Forms.CreateFlowableControlButton(
                  "Log In",
                  bindingContextAsWizardViewModel.NextCommand,
                  bindingContextAsWizardViewModel,
                  nextTabIndex++,
                  true,
                  extraTopSpace: FlowableConst_PI.DEFAULT_EXTRA_TOP_MARGIN );

            retViews.Add( loginButton as View );

            var createAccountButton =
               FlowableUtils_Forms.CreateFlowableControlButton(
                  "New User? Create Account",
                  bindingContextAsWizardViewModel.CancelCommand,
                  bindingContextAsWizardViewModel,
                  // ReSharper disable once RedundantAssignment
                  nextTabIndex++,
                  false,
                  ThemeUtils_Forms.MAIN_STAGE_THEME_COLOR,
                  fontAttributes: FontAttributes.Italic,
                  backColor: Color.Transparent
               );

            retViews.Add( createAccountButton as View );

            return Task.CompletedTask;
         }

         // -----------------------------------------------------------------------------------------------
      }

      public LogInView( ICanShowProgressSpinner_Forms spinnerHost ) : base( spinnerHost )
      { }
   }
}