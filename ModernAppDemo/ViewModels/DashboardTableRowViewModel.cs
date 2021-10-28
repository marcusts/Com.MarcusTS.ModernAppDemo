// *********************************************************************************
// Copyright @2021 Marcus Technical Services, Inc.
// <copyright
// file=DashboardTableRowViewModel.cs
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

namespace Com.MarcusTS.ModernAppDemo.ViewModels
{
   using System;
   using Com.MarcusTS.PlatformIndependentShared.ViewModels;
   using Com.MarcusTS.SharedUtils.Utils;
   using Com.MarcusTS.UI.XamForms.Common.Utils;

   public interface IDashboardTableRowViewModel : IPropertyChangedBase_PI
   {
      string    Kind          { get; set; }
      string    Description   { get; set; }
      bool      IsArchived    { get; set; }
      DateTime? WhenCompleted { get; set; }
      DateTime? WhenDue       { get; set; }
      string    RowState      { get; set; }
      void      SetRowState();
   }

   public class DashboardTableRowViewModel : PropertyChangedBase_PI, IDashboardTableRowViewModel
   {
      public const string    OVERDUE_ROW_STATE   = nameof( OVERDUE_ROW_STATE );
      public const string    ON_TIME_ROW_STATE   = nameof( ON_TIME_ROW_STATE );
      public const string    COMPLETED_ROW_STATE = nameof( COMPLETED_ROW_STATE );

      public string[] ALL_ROW_STATES = new[] { OVERDUE_ROW_STATE, ON_TIME_ROW_STATE, COMPLETED_ROW_STATE };

      private      DateTime? _whenCompleted;

      private DateTime? _whenDue;

      // VISIBLE PROPERTIES
      // Widths are flex/scaled, as with Excel (not fixed)

      [ViewModelTableColumnAttribute_PI( 0, TableAttributeViewManager_Forms.ITEM_DISPLAY_KIND, nameof( Kind ),
         columnWidth: 2.0, 
         isFlexWidth: ViewModelCustomAttribute_Static_PI.TRUE_BOOL, 
         defaultSortOrder: 2, 
         canSort: ViewModelCustomAttribute_Static_PI.TRUE_BOOL )]
      public string Kind { get; set; }

      [ViewModelTableColumnAttribute_PI( 1, TableAttributeViewManager_Forms.READ_ONLY_TEXT, nameof( Description ),
         columnWidth: 4.5,
         isFlexWidth: ViewModelCustomAttribute_Static_PI.TRUE_BOOL, 
         canSort: ViewModelCustomAttribute_Static_PI.TRUE_BOOL )]
      public string Description { get; set; }

      [ViewModelTableColumnAttribute_PI( 2, TableAttributeViewManager_Forms.PLAIN_ENGLISH_DATE, "Due", columnWidth: 2.0,
         isFlexWidth: ViewModelCustomAttribute_Static_PI.TRUE_BOOL, 
         defaultSortOrder: 1, 
         defaultSortDescending: ViewModelCustomAttribute_Static_PI.TRUE_BOOL, 
         canSort: ViewModelCustomAttribute_Static_PI.TRUE_BOOL )]
      public DateTime? WhenDue
      {
         get => _whenDue;
         set
         {
            _whenDue = value;
            SetRowState();
         }
      }

      [ViewModelTableColumnAttribute_PI( 3, TableAttributeViewManager_Forms.PLAIN_ENGLISH_DATE, "Completed",
         columnWidth: 2.0,
         isFlexWidth: ViewModelCustomAttribute_Static_PI.TRUE_BOOL, 
         defaultSortOrder: 0, 
         canSort: ViewModelCustomAttribute_Static_PI.TRUE_BOOL )]
      public DateTime? WhenCompleted
      {
         get => _whenCompleted;
         set
         {
            _whenCompleted = value;
            SetRowState();
         }
      }

      [ViewModelTableColumnAttribute_PI( 4, TableAttributeViewManager_Forms.COMPLETED_TOGGLE, "Archived",
         columnWidth: 2.0,
         isFlexWidth: ViewModelCustomAttribute_Static_PI.TRUE_BOOL, 
         canSort: ViewModelCustomAttribute_Static_PI.TRUE_BOOL )]
      public bool IsArchived { get; set; }

      public string RowState { get; set; }

      public void SetRowState()
      {
         RowState =
            WhenCompleted.GetValueOrDefault().IsNotEmpty()
               ? COMPLETED_ROW_STATE
               : WhenDue < DateTime.Now
                  ? OVERDUE_ROW_STATE
                  : ON_TIME_ROW_STATE;
      }
   }
}