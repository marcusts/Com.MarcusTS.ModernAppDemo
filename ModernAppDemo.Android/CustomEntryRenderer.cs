// *********************************************************************************
// Copyright @2022 Marcus Technical Services, Inc.
// <copyright
// file=CustomEntryRenderer.cs
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

using Com.MarcusTS.UI.XamForms.Views.Controls;
using ModernAppDemo.Android;
using Xamarin.Forms;
using Context = Android.Content.Context;

// HACK Crashes when we add Com.MarcusTS.
[assembly: ExportRenderer( typeof( CustomEntry ), typeof( CustomEntryRenderer ) )]

namespace ModernAppDemo.Android
{
   using Xamarin.Forms;
   using Xamarin.Forms.Platform.Android;

   internal class CustomEntryRenderer : EntryRenderer
   {
      public CustomEntryRenderer( Context context ) : base( context )
      { }

      protected override void OnElementChanged( ElementChangedEventArgs<Entry> e )
      {
         base.OnElementChanged( e );

         Control?.SetBackground( null );
      }
   }
}