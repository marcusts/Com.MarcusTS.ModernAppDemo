﻿// *********************************************************************************
// Copyright @2021 Marcus Technical Services, Inc.
// <copyright
// file=DashboardTitledTableViewHost.cs
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
   using Com.MarcusTS.SharedUtils.Utils;
   using Com.MarcusTS.UI.XamForms.Common.Interfaces;
   using Com.MarcusTS.UI.XamForms.Views.Subviews;
using System.Threading.Tasks;
   using Xamarin.Forms;

   /// <summary>
   /// We take responsibility for the spinner on load
   /// </summary>
   public interface IDashboardTitledFlexViewHost : ITitledViewHostBase_Forms
   { }

   public class DashboardTitledTableViewHost : TitledViewHostBase_Forms, IDashboardTitledFlexViewHost
   {
      public DashboardTitledTableViewHost( ICanShowProgressSpinner_Forms spinnerHost ) : base( spinnerHost )
      {
         // Our derived view handles the spinner
         PreserveSpinnerAfterRecreateUI = true;

         GetDerivedView                 = new DashboardView( spinnerHost );
         SetForceFullScreen( true ).FireAndFuhgetAboutIt();
      }

      protected override View GetDerivedView { get; }
   }
}