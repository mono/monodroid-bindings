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
using Java.IO;
using Android.Util;


namespace JmDNSSample
{
    [Activity(Label = "JmDNS-Sample", MainLauncher = true)]
    public class MainActivity : Activity
    {
        private Android.Net.Wifi.WifiManager.MulticastLock _lock;
        private Android.OS.Handler _handler = new Android.OS.Handler();
        private string _type = "_workstation._tcp.local.";
        private ServiceListener _listener = null;
        private ServiceInfo _serviceInfo;
        private JmDNS _jmdns = null;
        public JmDNS JmDNS
        {
            get { return _jmdns; }
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);
            Button button = FindViewById<Button>(Resource.Id.button);
            button.Click += delegate
            {

            };

            _handler.PostDelayed(delegate()
            {
                Setup();
            }, 1000);      
        }

        private void Setup()
        {
            Android.Net.Wifi.WifiManager wifi = GetSystemService(Android.Content.Context.WifiService) as Android.Net.Wifi.WifiManager;

            _lock = wifi.CreateMulticastLock("mylockthereturn");
            _lock.SetReferenceCounted(true);
            _lock.Acquire();
            try 
            {
                _jmdns = JmDNS.Create();
                _listener = new ServiceListener(this);
                _jmdns.AddServiceListener(_type, _listener);
                _serviceInfo = ServiceInfo.Create("_test._tcp.local.", "AndroidTest", 0, "plain test service from android");
                _jmdns.RegisterService(_serviceInfo);
            }
            catch (IOException e)
            {
                e.PrintStackTrace();
                return;
            }
        }


        public void NotifyUser(string msg)
        {
            _handler.PostDelayed(delegate()
            {
                TextView t = FindViewById<TextView>(Resource.Id.text);
                t.Text = msg + "\n=== " + t.Text;
            }, 1);
        }

        protected override void OnStart()
        {
            base.OnStart();
            //new Thread(){public void run() {setUp();}}.start();
        }

        protected override void OnStop()
        {
            if (_jmdns != null)
            {
                if (_listener != null)
                {
                    _jmdns.RemoveServiceListener(_type, _listener);
                    _listener = null;
                }
                _jmdns.UnregisterAllServices();
                try
                {
                    _jmdns.Close();
                }
                catch (IOException e)
                {
                    // TODO Auto-generated catch block
                    e.PrintStackTrace();
                }
                _jmdns = null;
            }
            //repo.stop();
            //s.stop();

            _lock.Release();
            base.OnStop();
        }
    }


    public class ServiceListener : Java.Lang.Object, IServiceListener
    {
        private static string TAG = "ServiceListener";

        MainActivity _mainActivity = null;
        public ServiceListener(MainActivity mainActivity)
        {
            _mainActivity = mainActivity;
        }

        public void ServiceAdded(ServiceEvent ev)
        {
            Log.Info(TAG, "ServiceAdded: " + ev.Name);

            // Required to force serviceResolved to be called again (after the first search)
            _mainActivity.JmDNS.RequestServiceInfo(ev.Type, ev.Name, 1);
        }

        public void ServiceRemoved(ServiceEvent ev)
        {
            Log.Info(TAG, "ServiceRemoved: " + ev.Name);

            _mainActivity.NotifyUser("Service removed: " + ev.Name);
        }

        public void ServiceResolved(ServiceEvent ev)
        {
            Log.Info(TAG, "ServiceResolved: " + ev.Name);

            String additions = "";
            if (ev.Info.GetInetAddresses() != null && ev.Info.GetInetAddresses().Length > 0)
            {
                additions = ev.Info.GetInetAddresses()[0].HostAddress;
            }

            _mainActivity.NotifyUser("Service resolved: " + ev.Info.QualifiedName + " port:" + ev.Info.Port + additions);

        }
    }
}


