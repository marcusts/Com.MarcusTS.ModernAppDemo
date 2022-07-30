// *********************************************************************************
// Copyright @2022 Marcus Technical Services, Inc.
// <copyright
// file=DashboardView.cs
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
   using System.Collections.Generic;
   using System.Linq;
   using System.Threading.Tasks;
   using Com.MarcusTS.ModernAppDemo.ViewModels;
   using Com.MarcusTS.PlatformIndependentShared.Common.Utils;
   using Com.MarcusTS.PlatformIndependentShared.ViewModels;
   using Com.MarcusTS.SharedUtils.Utils;
   using Com.MarcusTS.UI.XamForms.Common.Converters;
using Com.MarcusTS.UI.XamForms.Common.Interfaces;
   using Com.MarcusTS.UI.XamForms.Common.Utils;
   using Com.MarcusTS.UI.XamForms.Views.Subviews;
   using Xamarin.Forms;

   public interface IDashboardView : IScrollableTable_Forms
   { }

   public class DashboardView : ScrollableTable_Forms<DashboardTableRowViewModel>, IDashboardView
   {
      private static readonly double              ROW_MARGIN                        = 0.0.AdjustForOsAndDevice();
      private static readonly double              ROW_PADDING                       = 3.0.AdjustForOsAndDevice();
      private static readonly double              DEFAULT_HORIZONTAL_LINE_THICKNESS = 1.5.AdjustForOsAndDevice();
      private readonly        IThreadSafeAccessor IsInitializing                    = new ThreadSafeAccessor( 0 );

      public DashboardView( ICanShowProgressSpinner_Forms spinnerHost ) : base( spinnerHost )
      {
         IsInitializing.SetTrue();
         HorizontalGridlineGirth = DEFAULT_HORIZONTAL_LINE_THICKNESS;
         HorizontalGridlineColor = UIConst_Forms.HEADER_AND_KEY_LINE_COLOR;
         HorizontalGridlineMargin =
            new Thickness( 0, DEFAULT_HORIZONTAL_LINE_THICKNESS, 0, DEFAULT_HORIZONTAL_LINE_THICKNESS );
         HasHorizontalGridlines = true;
         SetFlowableChildSpacing( 0 );
         EndOfConstruction();
      }

      private void EndOfConstruction()
      {
         IsInitializing.SetFalse();
         EndInitialization().FireAndFuhgetAboutIt();
      }

      protected override async Task EndInitialization()
      {
         if ( IsInitializing.IsTrue() )
         {
            return;
         }

         // ELSE
         await base.EndInitialization().AndReturnToCallingContext();
      }


      /// <summary>
      /// Allows style set-up and override
      /// The child is the DashboardTableRowViewModel
      /// The row frame holds the grid that contains the rowCellViews
      /// Each rowCellView is a view for a view model property
      /// </summary>
      protected override Task AfterRowFrameCreated( ShapeView_Forms rowFrame, object rowViewModel,
                                                    List<View>      rowCellViews,
                                                    List<IViewModelTableColumnAttribute_PI>
                                                       rowCellAttributes )
      {
         // In case it didn't get created
         rowFrame.Margin  = ROW_MARGIN;
         rowFrame.Padding = ROW_PADDING;

         rowFrame.SetUpBinding
         (
            FillColorProperty,
            nameof( IDashboardTableRowViewModel.RowState ),
            BindingMode.OneWay,
            new RowStateToBackgroundColorConverter(), source: rowViewModel
         );

         var completedViewAttribute =
            rowCellAttributes.FirstOrDefault(
               rca => rca.ViewModelPropertyName.IsSameAs( nameof( IDashboardTableRowViewModel.WhenCompleted ) ) );
         if ( completedViewAttribute.IsNotNullOrDefault() )
         {
            var completedCellIdx = rowCellAttributes.IndexOf( completedViewAttribute );

            if ( ( completedCellIdx >= 0 ) && ( rowCellViews.Count > completedCellIdx ) )
            {
               // Hide when completed unless the item is completed
               var completedView = rowCellViews[ completedCellIdx ];

               completedView.SetUpBinding( IsVisibleProperty, nameof( IDashboardTableRowViewModel.RowState ),
                  BindingMode.OneWay,
                  new RowStateToWhenCompletedVisibilityConverter(), source: rowViewModel
               );
            }
         }

         return base.AfterRowFrameCreated( rowFrame, rowViewModel, rowCellViews, rowCellAttributes );
      }

      private class RowStateToBackgroundColorConverter : OneWayConverter<string, Color>
      {
         protected override Color Convert( string rowState, object parameter )
         {
            switch ( rowState )
            {
               case DashboardTableRowViewModel.OVERDUE_ROW_STATE:
                  return Color.FromRgb( 255, 240, 240 );

               case DashboardTableRowViewModel.COMPLETED_ROW_STATE:
                  return Color.FromRgb( 240, 240, 240 );

               default:
                  // case DashboardTableRowViewModel.ON_TIME_ROW_STATE:
                  return Color.FromRgb( 240, 255, 240 );
            }
         }
      }

      private class RowStateToWhenCompletedVisibilityConverter : OneWayConverter<string, bool>
      {
         protected override bool Convert( string rowState, object parameter )
         {
            return rowState.IsSameAs( DashboardTableRowViewModel.COMPLETED_ROW_STATE );
         }
      }
   }
}