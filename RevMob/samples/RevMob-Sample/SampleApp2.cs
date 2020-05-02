using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace RevMobSample
{
    [Activity(Label = "RevMob Publisher Sample App")]			
    public class SampleApp2 : SampleApp
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
        }

        public override void UpdateTestInfo()
        {
            base.UpdateTestInfo();
            FindViewById<Button>(Resource.Id.buttonChangeActivity).Text = "Change to Activity 1";

        }

        public override void ChangeActivity(View v)
        {
            Intent intent = new Intent(this, typeof(SampleApp));
            this.StartActivity(intent);
        }
    }
}

