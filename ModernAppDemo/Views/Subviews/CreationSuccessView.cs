// *********************************************************************************
// Copyright @2021 Marcus Technical Services, Inc.
// <copyright
// file=CreationSuccessView.cs
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
   using Com.MarcusTS.ModernAppDemo.Common.Images;
   using Com.MarcusTS.PlatformIndependentShared.Common.Utils;
   using Com.MarcusTS.SharedUtils.Controls;
   using Com.MarcusTS.SharedUtils.Utils;
   using Com.MarcusTS.UI.XamForms.Common.Interfaces;
   using Com.MarcusTS.UI.XamForms.Common.Utils;
   using Com.MarcusTS.UI.XamForms.ViewModels;
   using Com.MarcusTS.UI.XamForms.Views.Subviews;
   using Xamarin.Forms;

   public interface ICreationSuccessView : IScrollableFlexView_Forms
   { }

   public class CreationSuccessView : ScrollableFlexView_Forms, ICreationSuccessView
   {
      private static readonly double CHECK_MARK_WIDTH_HEIGHT = 75.0.AdjustForOsAndDevice();

      protected override async Task BeforeSourceViewsAssigned( BetterObservableCollection<View> retViews )
      {
         await base.BeforeSourceViewsAssigned( retViews ).WithoutChangingContext();

         // Add the check mark, message and sign on views

         var nextTabIndex = retViews.Count;

         var checkMark = UIUtils_Forms.GetImage(
            ImageUtils.MODERN_APP_DEMO_IMAGE_PRE_PATH + "green_check_mark.jpg",
            CHECK_MARK_WIDTH_HEIGHT, CHECK_MARK_WIDTH_HEIGHT, getFromResources: true,
            resourceClass: typeof( ImageUtils ) );

         checkMark.Margin = 20.0.AdjustForOsAndDevice();

         retViews.Add( checkMark );

         var messLabel =
            UIUtils_Forms.GetSimpleLabel( "Account Successfully Created", fontNamedSize: NamedSize.Small );
         messLabel.Margin = new Thickness( 0, 25.0.AdjustForOsAndDevice() );

         retViews.Add( messLabel );

         if ( BindingContext is IWizardViewModel_Forms bindingContextAsWizardViewModel )
         {
            var logInButton =
               FlowableUtils_Forms.CreateFlowableControlButton(
                  "Log In?",
                  bindingContextAsWizardViewModel.NextCommand,
                  bindingContextAsWizardViewModel,
                  nextTabIndex,
                  false );

            retViews.Add( logInButton as View );
         }
      }

      public CreationSuccessView( ICanShowProgressSpinner_Forms spinnerHost ) : base( spinnerHost )
      { }
   }
}