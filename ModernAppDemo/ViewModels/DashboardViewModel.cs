// *********************************************************************************
// Copyright @2022 Marcus Technical Services, Inc.
// <copyright
// file=DashboardViewModel.cs
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

#define MOCK_DASHBOARD_DATA

namespace Com.MarcusTS.ModernAppDemo.ViewModels
{
   using System;
   using System.Collections.Generic;
   using System.Threading.Tasks;
   using Bogus;
   using Com.MarcusTS.ModernAppDemo.Common.Navigation;
   using Com.MarcusTS.PlatformIndependentShared.Common.Utils;
   using Com.MarcusTS.SharedUtils.Utils;
   using Com.MarcusTS.UI.XamForms.Common.Utils;
   using Com.MarcusTS.UI.XamForms.ViewModels;

   public interface IDashboardViewModel : IServiceListWizardViewModelBase_Forms<IDashboardTableRowViewModel>
   { }

   public class DashboardViewModel :
      ServiceListWizardViewModelBase_Forms<IDashboardTableRowViewModel>, IDashboardViewModel
   {
      private const           int    MAX_DAYS_AFTER                  = 30;
      private const           int    MAX_DAYS_BEFORE                 = 100;
      private const           int    MAX_MOCK_ENTRIES                = 50;
      private const           int    MAX_WORDS                       = 7;
      private const           int    MIN_MOCK_ENTRIES                = 10;
      private const           int    MIN_WORDS                       = 3;
      private const           int    MOCK_SERVICE_DELAY_MILLISECONDS = 25;
      private static readonly Faker  _FAKER                          = new Faker();
      private static readonly Random _random                         = Extensions.CreateRandom;

      public DashboardViewModel()
      {
         Title = AppStateManager.DASHBOARD_TITLE;
      }

      protected override async Task<IList<IDashboardTableRowViewModel>> LoadServiceData()
      {
#if MOCK_DASHBOARD_DATA
         IList<IDashboardTableRowViewModel> retData = new List<IDashboardTableRowViewModel>();

         var isFirst = true;

         for ( var count = 0; count < _random.Next( MIN_MOCK_ENTRIES, MAX_MOCK_ENTRIES + 1 ); count++ )
         {
            IDashboardTableRowViewModel newTableRow = new DashboardTableRowViewModel();

            newTableRow.Description = 
               isFirst 
               ?
               "" 
               :
               string.Join( UIUtils_PI.SPACE_CHAR.ToString(),
               _FAKER.Lorem.Words( _random.Next( MIN_WORDS, MAX_WORDS ) ) );
            newTableRow.Kind =
               TableAttributeViewManager_Forms.DISPLAY_KINDS.RandomString( false, _random );

            var isCompleted = Extensions.RandomBool( _random );

            newTableRow.WhenCompleted =
               isCompleted
                  ? DateTime.Now.RandomDateTimeFromHere( MAX_DAYS_BEFORE, 0 )
                  : default;

            // Must be on or before when due
            var newWhenDue =
               isCompleted
                  ? newTableRow.WhenCompleted.RandomDateTimeFromHere( MAX_DAYS_BEFORE, 0, _random )

                  // Shorter range on both ends gives us more future items
                  : DateTime.Now.RandomDateTimeFromHere( MAX_DAYS_AFTER, MAX_DAYS_AFTER, _random );

            newTableRow.WhenDue = newWhenDue;

            newTableRow.IsArchived = isCompleted && Extensions.RandomBool( _random );

            newTableRow.SetRowState();

            retData.Add( newTableRow );

            await Task.Delay( MOCK_SERVICE_DELAY_MILLISECONDS ).WithoutChangingContext();

            isFirst = false;
         }

         return retData;
#else
         throw new System.NotImplementedException();
#endif
      }

#if MOCK_DASHBOARD_DATA
#endif
   }
}