using Com.MarcusTS.SharedForms.Views.Controls;
using ModernAppDemo.iOS;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(CustomEntry), typeof(CustomEntryRenderer))]
namespace ModernAppDemo.iOS
{
   using UIKit;
   using Xamarin.Forms;
   using Xamarin.Forms.Platform.iOS;

   public class CustomEntryRenderer : EntryRenderer
   {
      protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
      {
         base.OnElementChanged(e);

         if (Control != null)
         {
            Control.BorderStyle = UITextBorderStyle.None;
            Control.Layer.CornerRadius = 0;
            // Control.TextColor = UIColor.White;
            Control.BackgroundColor = UIColor.Clear;
         }
      }
   }
}