// *********************************************************************************
// Copyright @2022 Marcus Technical Services, Inc.
// <copyright
// file=RoundedContentViewRenderer.cs
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

using Com.MarcusTS.ModernAppDemo.Android;
using Com.MarcusTS.UI.XamForms.Views.Controls;
using Xamarin.Forms;
using Canvas = Android.Graphics.Canvas;
using Color = Xamarin.Forms.Color;
using ColorExtensions = Xamarin.Forms.Platform.Android.ColorExtensions;
using Context = Android.Content.Context;
using DeviceDisplay = Xamarin.Essentials.DeviceDisplay;
using Paint = Android.Graphics.Paint;
using Path = Android.Graphics.Path;
using RectF = Android.Graphics.RectF;
using View = Android.Views.View;
using ViewRenderer = Xamarin.Forms.Platform.Android.ViewRenderer;

[assembly: ExportRenderer( typeof( RoundedContentView ), typeof( RoundedContentViewRenderer ) )]

namespace Com.MarcusTS.ModernAppDemo.Android
{
   using System;

   public class RoundedContentViewRenderer : ViewRenderer
   {
      public RoundedContentViewRenderer( Context context ) : base( context )
      { }

      protected override bool DrawChild( Canvas canvas, View child, long drawingTime )
      {
         if ( Element == null )
         {
            return false;
         }

         var rcv = (RoundedContentView)Element;
         SetClipChildren( true );

         rcv.Padding = new Thickness( 0, 0, 0, 0 );
         var radius = (int)rcv.CornerRadius;
         if ( rcv.Circle )
         {
            radius = Math.Min( Width, Height ) / 2;
         }

         radius *= 2;

         try
         {
            var path = new Path();
            path.AddRoundRect( new RectF( 0, 0, Width, Height ),
               new float[] { radius, radius, radius, radius, radius, radius, radius, radius, },
               Path.Direction.Ccw );
            if ( rcv.HasShadow )
            {
               var shadowPath = new Path();
               shadowPath.AddRoundRect( new RectF( 5, 5, Width, Height ),
                  new float[] { radius, radius, radius, radius, radius, radius, radius, radius, },
                  Path.Direction.Ccw );
               var paint = new Paint();
               paint.AntiAlias   = true;
               paint.StrokeWidth = 5;
               paint.SetStyle( Paint.Style.Stroke );
               paint.Color = ColorExtensions.ToAndroid( Color.FromRgba( 0, 0, 0, 0.3 ) );
               canvas.DrawPath( shadowPath, paint );
            }

            canvas.Save();
            canvas.ClipPath( path );
            canvas.DrawColor( ColorExtensions.ToAndroid( rcv.FillColor ) );
            var result = base.DrawChild( canvas, child, drawingTime );

            canvas.Restore();
            if ( rcv.BorderWidth > 0 )
            {
               var paint = new Paint();
               paint.AntiAlias = true;

               // !!! Must adjust for the Android screen density
               // paint.StrokeWidth = (float)rcv.BorderWidth;
               paint.StrokeWidth = (float)( rcv.BorderWidth * DeviceDisplay.MainDisplayInfo.Density );

               paint.SetStyle( Paint.Style.Stroke );
               paint.Color = ColorExtensions.ToAndroid( rcv.BorderColor );
               canvas.DrawPath( path, paint );
            }

            path.Dispose();
            return result;
         }
         catch ( Exception ex )
         {
            Console.Write( ex.Message );
         }

         return base.DrawChild( canvas, child, drawingTime );
      }
   }
}