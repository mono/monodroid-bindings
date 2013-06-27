using System;
namespace Android.Support.V4.Content
{
	public partial class Loader
	{
		public partial class LoadCompleteEventArgs
		{
			[Obsolete ("Use Loader property instead")]
			public Loader P0 {
				get { return Loader; }
			}
			[Obsolete ("Use Data property instead")]
			public Java.Lang.Object P1 {
				get { return Data; }
			}
		}
	}
}

