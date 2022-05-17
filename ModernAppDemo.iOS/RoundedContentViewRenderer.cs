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

using Com.MarcusTS.ModernAppDemo.iOS;
using Com.MarcusTS.UI.XamForms.Views.Controls;
using Xamarin.Forms;

[assembly: ExportRenderer( typeof( RoundedContentView ), typeof( RoundedContentViewRenderer ) )]

namespace Com.MarcusTS.ModernAppDemo.iOS
{
   using System;
   using System.Diagnostics;
   using System.Linq;
   using System.Reflection;
   using Com.MarcusTS.SharedUtils.Utils;
   using Com.MarcusTS.UI.XamForms.Common.Utils;
   using CoreGraphics;
   using UIKit;
   using Xamarin.Forms;
   using Xamarin.Forms.Platform.iOS;

   public class RoundedContentViewRenderer : ViewRenderer
   {
      private static readonly PropertyInfo[] _staticPropInfos;

      static RoundedContentViewRenderer()
      {
         _staticPropInfos = SharedUtils.Utils.Extensions.GetAllPropInfos<IRoundedContentView>();
      }

      protected override void OnElementChanged( ElementChangedEventArgs<View> e )
      {
         base.OnElementChanged( e );

         if ( Element == null )
         {
            return;
         }

         Element.PropertyChanged += ( sender, args ) =>
                                    {
                                       try
                                       {
                                          if ( NativeView.IsNotNullOrDefault() &&
                                               _staticPropInfos.Any( pi => pi.Name.IsSameAs( args.PropertyName ) ) )
                                          {
                                             ThreadHelper_Forms.ConsiderBeginInvokeActionOnMainThread( () =>
                                             {
                                                try
                                                {
                                                   if ( Element.IsNotNullOrDefault() )
                                                   {
                                                      NativeView.SetNeedsDisplay();
                                                      NativeView.SetNeedsLayout();
                                                   }
                                                }
                                                catch ( ObjectDisposedException )
                                                {
                                                   // Bury this error
                                                }
                                             } );
                                          }
                                       }
                                       catch ( Exception exp )
                                       {
                                          Debug.WriteLine( "Warning : " + exp.Message );
                                       }
                                    };
      }

      public override void Draw( CGRect rect )
      {
         base.Draw( rect );

         LayoutIfNeeded();

         var rcv = (RoundedContentView)Element;
         if ( rcv == null )
         {
            return;
         }

         ClipsToBounds         = true;
         Layer.BackgroundColor = rcv.FillColor.ToCGColor();
         Layer.MasksToBounds   = true;
         Layer.CornerRadius    = (nfloat)rcv.CornerRadius;
         if ( rcv.HasShadow )
         {
            Layer.ShadowRadius  = 3.0f;
            Layer.ShadowColor   = UIColor.Gray.CGColor;
            Layer.ShadowOffset  = new CGSize( 1, 1 );
            Layer.ShadowOpacity = 0.60f;

            //Layer.ShadowPath = UIBezierPath.FromRect(Layer.ContentsRect).CGPath;
            Layer.MasksToBounds = false;
         }

         if ( rcv.Circle )
         {
            Layer.CornerRadius = (int)( Math.Min( Element.Width, Element.Height ) / 2 );
         }

         Layer.BorderWidth = 0;

         if ( ( rcv.BorderWidth > 0 ) && ( rcv.BorderColor.A > 0.0 ) )
         {
            // Layer.BorderWidth = rcv.BorderWidth;
            Layer.BorderWidth = new nfloat( rcv.BorderWidth );
            Layer.BorderColor =
               new UIColor(
                  (nfloat)rcv.BorderColor.R,
                  (nfloat)rcv.BorderColor.G,
                  (nfloat)rcv.BorderColor.B,
                  (nfloat)rcv.BorderColor.A ).CGColor;
         }
      }
   }
}