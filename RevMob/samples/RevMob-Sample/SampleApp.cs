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
using Com.Revmob;
using Com.Revmob.Ads;
using Com.Revmob.Ads.Banner;
using Com.Revmob.Ads.Fullscreen;
using Com.Revmob.Ads.Popup;
using Com.Revmob.Ads.Link;
using Com.Revmob.Internal;

//using Com.Revmob.Internal.RMLog;
//using Com.Revmob.RevMobAdsListener;
using Android.Locations;
using Java.Interop;

namespace RevMobSample
{
    [Activity(Label = "RevMob Publisher Sample App",
        MainLauncher = true,
        ConfigurationChanges = Android.Content.PM.ConfigChanges.KeyboardHidden | Android.Content.PM.ConfigChanges.Orientation,
        Theme = "@android:style/Theme.Black.NoTitleBar.Fullscreen")]			
    public class SampleApp : Activity
    {
        RevMob revmob;
        bool useUIThread = true;
        Activity currentActivity; // for anonymous classes
        RevMobFullscreen fullscreen;
        RevMobBanner banner;
        RevMobPopup popup;
        RevMobLink link;
        RevMobAdsListener revmobListener;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            revmobListener = new RevMobAdsListener(this);

            RMLog.D("onCreate");
            currentActivity = this;
            SetContentView(Resource.Layout.Main);
            revmob = RevMob.Start(currentActivity);
            // Deprecated:
            //revmob = RevMob.start(currentActivity, REVMOB_APPID);
            FillUserInfo();
            UpdateTestInfo();
        }

        protected override void OnResume()
        {
            base.OnResume();
            RMLog.D("onResume");
        }

        public virtual void UpdateTestInfo()
        {
            FindViewById<Button>(Resource.Id.buttonChangeActivity).Text = "Change to Activity 2";
            String activityStr = Class.SimpleName.Equals("SampleApp") ? "Activity 1" : "Activity 2";
            String threadStr = useUIThread == true ? "UIThread" : "Another thread";
            String testingModeStr;
            if (revmob.TestingMode == RevMobTestingMode.WithAds)
            {
                testingModeStr = "Test mode with ads";
            }
            else if (revmob.TestingMode == RevMobTestingMode.WithoutAds)
            {
                testingModeStr = "Test mode without ads";
            }
            else
            {
                testingModeStr = "Test mode disabled";
            }

            String parallaxModeStr;
            if (revmob.ParallaxMode == RevMobParallaxMode.Default)
            {
                parallaxModeStr = "Parallax mode enabled";
            }
            else
            {
                parallaxModeStr = "Parallax mode disabled";
            }

            FindViewById<TextView>(Resource.Id.textTestInfo).Text = activityStr + " / " + threadStr + " / " + testingModeStr + " / " + parallaxModeStr;
        }
        // Test functions
        [Export("changeActivity")]
        public virtual void ChangeActivity(View v)
        {
            Intent intent = new Intent(currentActivity, typeof(SampleApp2));
            StartActivity(intent);
        }

        [Export("openGLActivity")]
        public void OpenGLActivity(View v)
        {
            Intent intent = new Intent(currentActivity, typeof(SampleOpenGL));
            StartActivity(intent);
        }

        [Export("changeThread")]
        public void ChangeThread(View v)
        {
            useUIThread = !useUIThread;
            UpdateTestInfo();
        }

        [Export("testingModeWithAds")]
        public void TestingModeWithAds(View v)
        {
            revmob.TestingMode = RevMobTestingMode.WithAds;
            UpdateTestInfo();
        }

        [Export("testingModeWithoutAds")]
        public void TestingModeWithoutAds(View v)
        {
            revmob.TestingMode = RevMobTestingMode.WithoutAds;
            UpdateTestInfo();
        }

        [Export("testingModeDisable")]
        public void TestingModeDisable(View v)
        {
            revmob.TestingMode = RevMobTestingMode.Disabled;
            UpdateTestInfo();
        }

        [Export("parallaxModeDefault")]
        public void ParallaxModeDefault(View v)
        {
            revmob.ParallaxMode = RevMobParallaxMode.Default;
            UpdateTestInfo();
        }

        [Export("parallaxModeDisable")]
        public void ParallaxModeDisable(View v)
        {
            revmob.ParallaxMode = RevMobParallaxMode.Disabled;
            UpdateTestInfo();
        }

        [Export("printEnvironmentInformation")]
        public void PrintEnvironmentInformation(View v)
        {
            if (useUIThread)
            {
                revmob.PrintEnvironmentInformation(currentActivity);
            }
            else
            {
                RunOnAnotherThread(new Java.Lang.Runnable(delegate
                {
                    revmob.PrintEnvironmentInformation(currentActivity);
                }));
            }
        }

        [Export("closeApplication")]
        public void CloseApplication(View v)
        {
            SetResult(0);
            Finish();

            //System.Exit(0);
        }
        // Fullscreen
        [Export("showFullscreen")]
        public void ShowFullscreen(View v)
        {
            if (useUIThread)
            {
                revmob.ShowFullscreen(currentActivity);
            }
            else
            {
                RunOnAnotherThread(new Java.Lang.Runnable(delegate
                {
                    revmob.ShowFullscreen(currentActivity);
                }));
            }
        }

        [Export("loadFullscreen")]
        public void LoadFullscreen(View v)
        {
            if (useUIThread)
            {
                fullscreen = revmob.CreateFullscreen(currentActivity, revmobListener);
            }
            else
            {
                RunOnAnotherThread(new Java.Lang.Runnable(delegate
                {
                    fullscreen = revmob.CreateFullscreen(currentActivity, revmobListener);
                }));
            }
        }

        [Export("showLoadedFullscreen")]
        public void ShowLoadedFullscreen(View v)
        {
            if (useUIThread)
            {
                if (fullscreen != null)
                {
                    fullscreen.Show();
                }
            }
            else
            {
                RunOnAnotherThread(new Java.Lang.Runnable(delegate
                {
                    if (fullscreen != null)
                    {
                        fullscreen.Show();
                    }
                }));
            }
        }
        // Banner
        void ShowBanner()
        {
            banner = revmob.CreateBanner(currentActivity, revmobListener);
            RunOnUiThread(delegate
            {
               
                
                ViewGroup view = FindViewById<ViewGroup>(Resource.Id.banner);
                view.AddView(banner);
            }
            );
        }

        [Export("showBanner")]
        public void ShowBanner(View v)
        {
            if (useUIThread)
            {
                ShowBanner();
            }
            else
            {
                RunOnAnotherThread(new Java.Lang.Runnable(delegate
                {
                   
                    
                    ShowBanner();
                })
                );
            }
        }

        [Export("hideBanner")]
        public void HideBanner(View v)
        {
            if (useUIThread)
            {
                if (banner != null)
                {
                    banner.Hide();
                }
            }
            else
            {
                RunOnAnotherThread(new Java.Lang.Runnable(delegate
                {
                    if (banner != null)
                    {
                        banner.Hide();
                    }
                }));
            }
        }

        [Export("showBannerCustomSize")]
        public void ShowBannerCustomSize(View v)
        {
            RevMobBanner banner = revmob.CreateBanner(currentActivity);
            ViewGroup view = FindViewById<ViewGroup>(Resource.Id.bannerCustomSize);
            view.RemoveAllViews();
            view.AddView(banner);
        }

        [Export("showAbsoluteBannerOnTop")]
        public void ShowAbsoluteBannerOnTop(View v)
        {
            revmob.ShowBanner(currentActivity, (int)GravityFlags.Top, null, revmobListener);
        }

        [Export("showAbsoluteBannerOnBottom")]
        public void ShowAbsoluteBannerOnBottom(View v)
        {
            revmob.ShowBanner(currentActivity, (int)GravityFlags.Bottom, null, revmobListener);
        }

        [Export("hideAbsoluteBanner")]
        public void HideAbsoluteBanner(View v)
        {
            revmob.HideBanner(currentActivity);
        }
        // Link
        [Export("openAdLink")]
        public void OpenAdLink(View v)
        {
            if (useUIThread)
            {
                revmob.OpenAdLink(currentActivity, revmobListener);
            }
            else
            {
                RunOnAnotherThread(new Java.Lang.Runnable(delegate
                {
                    revmob.OpenAdLink(currentActivity, revmobListener);
                }));
            }
        }

        [Export("loadAdLink")]
        public void LoadAdLink(View v)
        {
            if (useUIThread)
            {
                link = revmob.CreateAdLink(currentActivity, revmobListener);
            }
            else
            {
                RunOnAnotherThread(new Java.Lang.Runnable(delegate
                {
                    link = revmob.CreateAdLink(currentActivity, revmobListener);
                }));
            }
        }

        [Export("openLoadedAdLink")]
        public void OpenLoadedAdLink(View v)
        {
            if (useUIThread)
            {
                if (link != null)
                {
                    link.Open();
                }
            }
            else
            {
                RunOnAnotherThread(new Java.Lang.Runnable(delegate
                {
                    if (link != null)
                    {
                        link.Open();
                    }
                }));
            }
        }
        // Popup
        [Export("showPopup")]
        public void ShowPopup(View v)
        {
            if (useUIThread)
            {
                revmob.ShowPopup(currentActivity);
            }
            else
            {
                RunOnAnotherThread(new Java.Lang.Runnable(delegate
                {
                    revmob.ShowPopup(currentActivity);
                }));
            }
        }

        [Export("loadPopup")]
        public void LoadPopup(View v)
        {
            if (useUIThread)
            {
                popup = revmob.CreatePopup(currentActivity, revmobListener);
            }
            else
            {
                RunOnAnotherThread(new Java.Lang.Runnable(delegate
                {
                    popup = revmob.CreatePopup(currentActivity, revmobListener);
                }));
            }
        }

        [Export("showLoadedPopup")]
        public void ShowLoadedPopup(View v)
        {
            if (useUIThread)
            {
                if (popup != null)
                {
                    popup.Show();
                }
            }
            else
            {
                RunOnAnotherThread(new Java.Lang.Runnable(delegate
                {
                    if (popup != null)
                    {
                        popup.Show();
                    }
                }));
            }
        }
        // Auxiliar methods
        private void RunOnAnotherThread(Java.Lang.Runnable action)
        {
            new Java.Lang.Thread(action).Start();
        }

        public void ToastOnUiThread(String message)
        {
            RunOnUiThread(delegate
            {

                Toast.MakeText(currentActivity, message, ToastLength.Short).Show();

            });
        }

        void FillUserInfo()
        {
            revmob.UserGender = RevMobUserGender.Female;
            revmob.UserAgeRangeMin = 18;
            revmob.UserAgeRangeMax = 25;
            revmob.UserBirthday = new Java.Util.GregorianCalendar(1990, 11, 12);
            revmob.UserPage = "twitter.com/revmob";
            List<String> interests = new List<String>();
            interests.Add("mobile");
            interests.Add("Android");
            interests.Add("apps");
            revmob.UserInterests = interests;

            try
            {
                LocationManager locationManager = (LocationManager)ApplicationContext.GetSystemService(Context.LocationService);
                Location gpsLocation = locationManager.GetLastKnownLocation(LocationManager.GpsProvider);
                Location netLocation = locationManager.GetLastKnownLocation(LocationManager.NetworkProvider);

                if (gpsLocation != null)
                {
                    revmob.SetUserLocation(gpsLocation.Latitude, gpsLocation.Longitude, gpsLocation.Accuracy);
                }
                else if (netLocation != null)
                {
                    revmob.SetUserLocation(netLocation.Latitude, netLocation.Longitude, netLocation.Accuracy);
                }
                else
                {
                    RMLog.D("No location data available");
                }       
            }
            catch (Exception e)
            {
                RMLog.D("Unable to get the location data");
            }
        }
        // Debug logs
        protected override void OnStart()
        {
            base.OnStart();
            RMLog.D("onStart");
        }

        protected override void OnPause()
        {
            base.OnPause();
            RMLog.D("onPause");
        }

        protected override void OnStop()
        {
            base.OnStop();
            RMLog.D("onStop");
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            RMLog.D("onDestroy");
        }
    }

    public class RevMobAdsListener : Java.Lang.Object, IRevMobAdsListener
    {
        private SampleApp _sampleApp = null;

        public RevMobAdsListener(SampleApp sampleApp)
        {
            _sampleApp = sampleApp;
        }

        public void OnRevMobAdClicked()
        {
            _sampleApp.ToastOnUiThread("Ad clicked.");
        }

        public void OnRevMobAdDismiss()
        {
            _sampleApp.ToastOnUiThread("Ad dismissed.");
        }

        public void OnRevMobAdDisplayed()
        {
            _sampleApp.ToastOnUiThread("Ad displayed.");
        }

        public void OnRevMobAdNotReceived(string message)
        {
            _sampleApp.ToastOnUiThread("RevMob ad not received.");
        }

        public void OnRevMobAdReceived()
        {
            _sampleApp.ToastOnUiThread("RevMob ad received.");
        }
    }
}

