// *********************************************************************************
// Copyright @2021 Marcus Technical Services, Inc.
// <copyright
// file=App.cs
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

namespace Com.MarcusTS.ModernAppDemo.Views.App
{
   using Com.MarcusTS.PlatformIndependentShared.Common.Interfaces;
   using Com.MarcusTS.PlatformIndependentShared.Common.Utils;
   using Com.MarcusTS.UI.XamForms.Common.Interfaces;
   using Com.MarcusTS.UI.XamForms.Common.Navigation;
   using Com.MarcusTS.UI.XamForms.Views.App;
   using Com.MarcusTS.UI.XamForms.Views.Presenters;
   using Com.MarcusTS.UI.XamForms.Views.Subviews;
   using ModernAppDemo.Common.Images;
   using ModernAppDemo.Common.Navigation;
   using ModernAppDemo.Views.Presenters;

   public interface IApp : IAppBase_Forms
   { }

   public sealed class App : AppBaseForms, IApp
   {
      /// <summary>
      /// iOS crash:
      /// https://developercommunity.visualstudio.com/t/trying-to-preview-forms-for-ios-fails-with-invalid/1052336
      /// </summary>
      public App()
      { }

      protected override IMasterViewPresenterBase_Forms GetMasterPresenter(
         ICanShowProgressSpinner_Forms spinnerHost)
      {
         return new MasterViewPresenter(spinnerHost);
      }

      protected override IAppStateManagerBase_Forms GetAppStateManager(ICanShowProgressSpinner_Forms spinnerHost)
      {
         return new AppStateManager(spinnerHost);
      }

      protected override IToolbar_PI GetMasterToolbar()
      {
         return
            new Toolbar_Forms
            {
               ImageResourcePath = ImageUtils.MODERN_APP_DEMO_IMAGE_PRE_PATH,
               ImageResourceType = typeof(ImageUtils),
               MarginAndSpacing  = UIConst_PI.DEFAULT_STACK_LAYOUT_SPACING,
            };
      }
   }
}