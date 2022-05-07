// *********************************************************************************
// Copyright @2022 Marcus Technical Services, Inc.
// <copyright
// file=MasterViewPresenter.cs
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

namespace Com.MarcusTS.ModernAppDemo.Views.Presenters
{
   using System.Threading.Tasks;
   using Com.MarcusTS.ModernAppDemo.ViewModels;
   using Com.MarcusTS.ModernAppDemo.Views.Subviews;
   using Com.MarcusTS.SharedUtils.Utils;
   using Com.MarcusTS.UI.XamForms.Common.Interfaces;
   using Com.MarcusTS.UI.XamForms.Views.Presenters;

   public interface IMasterViewPresenter : IMasterViewPresenterBase_Forms
   { }

   public class MasterViewPresenter : MasterViewPresenterBase_Forms, IMasterViewPresenter
   {
      public MasterViewPresenter( ICanShowProgressSpinner_Forms spinnerHost ) : base( spinnerHost )
      { }

      protected override async Task RespondToViewModelChange( object newModule )
      {
         if ( newModule is IDashboardViewModel )
         {
            await ChangeContentView<IDashboardTitledFlexViewHost, DashboardTitledTableViewHost>( newModule )
              .WithoutChangingContext();
         }
         else if ( newModule is ISettingsViewModel )
         {
            await ChangeContentView<ISettingsTitledFlexViewHost, SettingsTitledFlexViewHost>( newModule )
              .WithoutChangingContext();
         }
         else if ( newModule is ILogInViewModel )
         {
            await ChangeContentView<ILogInTitledFlexViewHost, LogInTitledFlexViewHost>( newModule )
              .WithoutChangingContext();
         }
         else if ( newModule is ICreateAccountViewModel )
         {
            await ChangeContentView<ICreateAccountTitledViewHost, CreateAccountTitledViewHost>( newModule )
              .WithoutChangingContext();
         }
         else if ( newModule is ICreationSuccessViewModel )
         {
            await ChangeContentView<ICreationSuccessTitledFlexViewHost, CreationSuccessTitledFlexViewHost>( newModule )
              .WithoutChangingContext();
         }
      }
   }
}