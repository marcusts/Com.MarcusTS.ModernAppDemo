// *********************************************************************************
// Copyright @2021 Marcus Technical Services, Inc.
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

namespace ModernAppDemo.Views.Subviews
{
   using System.Threading.Tasks;
   using Com.MarcusTS.ResponsiveTasks.XamFormsSupport.Common.Utils;
using Com.MarcusTS.ResponsiveTasks.XamFormsSupport.ViewModels;
   using Com.MarcusTS.ResponsiveTasks.XamFormsSupport.Views.Subviews;
   using Com.MarcusTS.SharedForms.Common.Utils;
   using Com.MarcusTS.SharedForms.ViewModels;
   using Com.MarcusTS.SharedForms.Views.SubViews;
using Com.MarcusTS.SharedUtils.Utils;
   using Xamarin.Forms;

   public interface ILogInView : IFlexViewWithTasks
   {
   }

   public class LogInView : FlexViewWithTasks, ILogInView
   {
      protected override async Task AfterSourceViewsLoaded()
      {
         await base.AfterSourceViewsLoaded().WithoutChangingContext();
         // Add the save and new account buttons

         var nextTabIndex = MasterAnimatedStackLayoutAsView.SourceViews.Count;

         var loginButton =
            CreateButton(
                         "Log In", 
                         (BindingContext as IWizardViewModelWithTasks)?.NextCommand, 
                         nextTabIndex++, 
                         true,
                         useExtraTopSpace:true
                         );

         MasterAnimatedStackLayoutAsView.SourceViews.Add(loginButton as View);

         var createAccountButton =
            CreateButton(
                         "New User? Create Account", 
                         (BindingContext as IWizardViewModelWithTasks)?.CancelCommand,
                         // ReSharper disable once RedundantAssignment
                         nextTabIndex++, 
                         false, 
                         ThemeUtils_RTXFS.MAIN_STAGE_THEME_COLOR, 
                         fontAtributes: FontAttributes.Italic,
                         backColor:Color.Transparent
                         );

         MasterAnimatedStackLayoutAsView.SourceViews.Add(createAccountButton as View);
      }
   }
}