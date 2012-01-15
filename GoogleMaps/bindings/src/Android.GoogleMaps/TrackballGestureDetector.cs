using System;

namespace Android.GoogleMaps {

	partial class TrackballGestureDetector {

		public void RegisterLongPressCallback (Action action)
		{
			RegisterLongPressCallback (new Java.Lang.Runnable (action));
		}
	}
}

