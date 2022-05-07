// *********************************************************************************
// Copyright @2022 Marcus Technical Services, Inc.
// <copyright
// file=LogInTitledFlexViewHost.cs
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

// #define FAKE_UI

namespace Com.MarcusTS.ModernAppDemo.Views.Subviews
{
   using Com.MarcusTS.PlatformIndependentShared.Common.Utils;
   using Com.MarcusTS.UI.XamForms.Common.Interfaces;
   using Com.MarcusTS.UI.XamForms.Common.Utils;
   using Com.MarcusTS.UI.XamForms.Views.Subviews;
   using Xamarin.Forms;

   public interface ILogInTitledFlexViewHost : ITitledViewHostBase_Forms
   { }

   public class LogInTitledFlexViewHost : TitledViewHostBase_Forms, ILogInTitledFlexViewHost
   {
      public LogInTitledFlexViewHost( ICanShowProgressSpinner_Forms spinnerHost ) : base( spinnerHost )
      {
         Margin = new Thickness(
            ( ScaleUtils_PI.CURRENT_DEVICE_WIDTH - UIConst_PI.DEFAULT_ENTRY_WIDTH -
              ( 2 * UIConst_PI.MARGIN_SPACING_DOUBLE_FACTOR ) ) / 2, 0 );

         GetDerivedView =

#if FAKE_UI
            new Label { Text = "Hello World!", BackgroundColor = Color.Yellow };
#else
            new LogInView( spinnerHost )
            {
               Margin = UIConst_Forms.DEFAULT_STACK_LAYOUT_MARGIN,
            };
#endif
      }

      protected override View GetDerivedView { get; }
   }
}