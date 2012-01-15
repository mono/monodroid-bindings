using System;

namespace Android.GoogleMaps {

	partial class MyLocationOverlay {

		public bool RunOnFirstFix (Action action)
		{
			return RunOnFirstFix (new Java.Lang.Runnable (action));
		}
	}
}

