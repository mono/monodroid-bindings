using System;

namespace Android.Support.V4.Widget {

	partial class DrawerLayout {

		partial class DrawerClosedEventArgs {

			public global::Android.Views.View P0 {
				get {return DrawerView;}
			}
		}

		partial class DrawerOpenedEventArgs {

			public global::Android.Views.View P0 {
				get {return DrawerView;}
			}
		}

		partial class DrawerSlideEventArgs {

			public global::Android.Views.View P0 {
				get {return DrawerView;}
			}

			public float P1 {
				get {return slideOffset;}
			}
		}

		partial class DrawerStateChangedEventArgs {

			public int P0 {
				get {return NewState;}
			}
		}
	}
}

