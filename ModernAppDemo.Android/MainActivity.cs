// *********************************************************************************
// Copyright @2021 Marcus Technical Services, Inc.
// <copyright
// file=MainActivity.cs
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


// HACK Crashes when we add Com.MarcusTS.
namespace ModernAppDemo.Android
{
   using Acr.UserDialogs;
using Com.MarcusTS.ModernAppDemo.Android;
   using Com.MarcusTS.ModernAppDemo.Views.App;
   using Com.MarcusTS.UI.XamForms.Views.Controls;
   using global::Android.App;
   using global::Android.Content.PM;
   using global::Android.OS;
   using global::Android.Runtime;
   using Xamarin.Forms;
   using Xamarin.Forms.Platform.Android;
   
   [Activity(Label = "ModernAppDemo", Icon = "@mipmap/icon", Theme = "@style/MainTheme", 
      ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode |
         ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
   public class MainActivity : FormsAppCompatActivity
   {
      protected override void OnCreate(Bundle savedInstanceState)
      {
         base.OnCreate(savedInstanceState);

         Forms.SetFlags("UseLegacyRenderers");
         RoundedContentView.Init();
         Xamarin.Essentials.Platform.Init(this, savedInstanceState);
         Forms.Init(this, savedInstanceState);
         UserDialogs.Init(this);
         LoadApplication(new App());
      }

      public override void OnRequestPermissionsResult(int requestCode, string[] permissions,
         [GeneratedEnum] Permission[]                     grantResults)
      {
         Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

         base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
      }
   }
}