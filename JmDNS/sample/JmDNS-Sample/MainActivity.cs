using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

// Only thing I can import.
//using Javax.Jmdns.Impl ??
using Javax.Jmdns;
using Javax.Jmdns.Impl;
// What the sample imports.
//import javax.jmdns.JmDNS;
//import Javax.Jmdns.ServiceEvent;
//import Javax.Jmdns.ServiceListener;


namespace JmDNSSample
{
    [Activity(Label = "JmDNS-Sample", MainLauncher = true)]
    public class MainActivity : Activity
    {
        int count = 1;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.myButton);
			
            button.Click += delegate
            {
                button.Text = string.Format("{0} clicks!", count++);
            };




            String bonjourServiceType = "_http._tcp.local.";
            bonjourService = JmDNS.create();
            bonjourService.addServiceListener(bonjourServiceType, bonjourServiceListener);
            // Not Android.Content.PM.ServiceInfo
            ServiceInfo[] serviceInfos = bonjourService.list(bonjourServiceType);
            foreach (ServiceInfo info in serviceInfos)
            {
                Console.WriteLine("## resolve service " + info.Name  + " : " + info.URL);
            }
            bonjourService.close();
        }
    }
}


