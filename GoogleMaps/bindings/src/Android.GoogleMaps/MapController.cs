using System;

namespace Android.GoogleMaps {

	partial class MapController {
		
		public void AnimateTo (Android.GoogleMaps.GeoPoint point, Action action)
		{
			AnimateTo (point, new Java.Lang.Runnable (action));
		}
	}
}

