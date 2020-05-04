using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Tapit.Adview;

namespace Sample
{
	[Activity (Label = "Sample", MainLauncher = true)]
	public class Activity1 : Activity
	{
		int count = 1;
		public static String BANNER_ZONE_ID = "7979";
		public static String VIDEO_ZONE_ID = "7981";
		public static String MED_RECT_ZONE_ID = "7982";
		public static String INTRS_ZONE_ID = "7983";
		public static String ADPROMPT_ZONE_ID = "7984";

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);


			LinearLayout linearLayout = FindViewById<LinearLayout> (Resource.Id.linearLayout);
			AdView adView = new AdView(this, BANNER_ZONE_ID); 
			adView.LayoutParameters = (new ViewGroup.LayoutParams(ViewGroup.LayoutParams.WrapContent,ViewGroup.LayoutParams.FillParent));
			linearLayout.AddView(adView);

			// Get our button from the layout resource,
			// and attach an event to it

		}
	}
}


