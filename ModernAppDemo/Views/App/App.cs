﻿// *********************************************************************************
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

namespace ModernAppDemo.Views.App
{
   using Com.MarcusTS.ResponsiveTasks.XamFormsSupport.Common.Navigation;
   using Com.MarcusTS.ResponsiveTasks.XamFormsSupport.Views.App;
   using Com.MarcusTS.ResponsiveTasks.XamFormsSupport.Views.Pages;
   using Com.MarcusTS.ResponsiveTasks.XamFormsSupport.Views.Presenters;
   using Com.MarcusTS.ResponsiveTasks.XamFormsSupport.Views.Subviews;
   using Com.MarcusTS.SharedForms.Common.Utils;
   using Common.Images;
   using Common.Navigation;
   using Presenters;

   public interface IApp : IAppBase
   {
   }

   public sealed class App : AppBase, IAppBase
   {
      protected override IMasterViewPresenterBase GetMasterPresenter(ICanShowProgressSpinner spinnerHost)
      {
         return new MasterViewPresenter(spinnerHost);
      }

      protected override IAppStateManagerBase GetAppStateManager(ICanShowProgressSpinner spinnerHost)
      {
         return new AppStateManager(spinnerHost);
      }

      protected override IToolbar GetMasterToolbar()
      {
         return new Toolbar { ImageResourcePath = ImageUtils.MODERN_APP_DEMO_IMAGE_PRE_PATH, ImageResourceType = typeof(ImageUtils), MarginAndSpacing = 10.0.AdjustForOsAndDevice()};
      }
   }
}