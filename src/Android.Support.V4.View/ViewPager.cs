using System;
namespace Android.Support.V4.View
{
	public partial class ViewPager
	{
		public partial class PageScrolledEventArgs
		{
			[Obsolete ("Use Position property instead")]
			public int P0 {
				get { return Position; }
			}
			[Obsolete ("Use PositionOffset property instead")]
			public float P1 {
				get { return PositionOffset; }
			}
			[Obsolete ("Use PositionOffsetPixels property instead")]
			public int P2 {
				get { return PositionOffsetPixels; }
			}
		}
		public partial class PageScrollStateChangedEventArgs
		{
			[Obsolete ("Use State property instead")]
			public int P0 {
				get { return State; }
			}
		}
		public partial class PageSelectedEventArgs
		{
			[Obsolete ("Use Position property instead")]
			public int P0 {
				get { return Position; }
			}
		}
	}
}

