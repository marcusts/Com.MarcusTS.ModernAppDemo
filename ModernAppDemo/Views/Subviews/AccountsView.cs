// *********************************************************************************
// Copyright @2021 Marcus Technical Services, Inc.
// <copyright
// file=AccountsView.cs
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
   using Com.MarcusTS.ResponsiveTasks.XamFormsSupport.Views.Subviews;
   using Com.MarcusTS.SharedForms.Common.Utils;
   using Com.MarcusTS.SharedUtils.Controls;
   using Com.MarcusTS.SharedUtils.Utils;
   using ModernAppDemo.ViewModels;
   using Xamarin.Forms;

   public interface IAccountsView : IFlexViewWithTasks_FlowLayout
   {
   }

   public class AccountsView : FlexViewWithTasks_FlowLayout, IAccountsView
   {
      private static readonly double SPACER_HEIGHT = 2.0.AdjustForOsAndDevice();

      public AccountsView()
      {
         SetForceFullScreen(true).FireAndForget();
      }

      protected override async Task BeforeSourceViewsAssigned(BetterObservableCollection<View> retViews)
      {
         await base.BeforeSourceViewsAssigned(retViews).WithoutChangingContext();

         retViews.Clear();

         if (!(BindingContext is IAccountsViewModel bindingContextAsAccounts) ||
             bindingContextAsAccounts.Accounts.IsAnEmptyList())
         {
            return;
         }

         foreach (var account in bindingContextAsAccounts.Accounts)
         {
            var stackLayout = FormsUtils.GetExpandingStackLayout();
            stackLayout.VerticalOptions = LayoutOptions.Start;

            var firstLastNameLabel = FormsUtils.GetSimpleLabel(account.FirstAndLastName, ThemeUtils.DARK_THEME_COLOR,
               fontNamedSize: NamedSize.Medium);
            stackLayout.Children.Add(firstLastNameLabel);
            var userNameLabel = FormsUtils.GetSimpleLabel(account.UserName, Color.Black,
               fontAttributes: FontAttributes.Italic,
               fontNamedSize: NamedSize.Small);
            stackLayout.Children.Add(userNameLabel);
            var line = FormsUtils.GetSpacer(SPACER_HEIGHT);
            line.BackgroundColor = ThemeUtils.LIGHT_THEME_COLOR;
            stackLayout.Children.Add(line);

            retViews.Add(stackLayout);
         }
      }
   }
}