using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using Java.Net;
using Java.IO;
using Org.Json;

using Shutterbug;
using Shutterbug.Cache;

namespace ShutterbugSample
{
    [Activity(Label = "Shutterbug-Sample", MainLauncher = true)]
    public class ShutterbugActivity : Activity
    {
        private ListView       mListView;
        private DemoAdapter    mAdapter;
        private ProgressDialog mProgressDialog;
        private List<String>   mUrls   = new List<String>();
        public List<String> Urls
        {
            get { return mUrls; }
        }

        private List<String>   mTitles = new List<String>();
        public List<String> Titles
        {
            get { return mTitles; }
        }



        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.activity_shutterbug);

            mListView = FindViewById<ListView>(Resource.Id.list);
            mAdapter = new DemoAdapter(this);
            mListView.Adapter = mAdapter;

            Button b = FindViewById<Button>(Resource.Id.clear_cache_button);
            b.Click += delegate
            {
                ImageCache.GetSharedImageCache(this).Clear();
                mAdapter.NotifyDataSetChanged();
            };

            loadGalleryContents();
        }

        private class DemoAdapter : BaseAdapter
        {
            private ShutterbugActivity _shutterbugActivity;


            public DemoAdapter(ShutterbugActivity shutterbugActivity)
            {
                _shutterbugActivity = shutterbugActivity;
            }

            public override int Count
            {
                get
                {
                    return _shutterbugActivity.Urls.Count;
                }
            }

            public override Java.Lang.Object GetItem(int position)
            {
                return position;
            }

            public override long GetItemId(int position)
            {
                return position;
            }

            public override View GetView(int position, View convertView, ViewGroup parent)
            {

                View view = convertView;
                if (view == null) {
                    view = _shutterbugActivity.LayoutInflater.Inflate(Resource.Layout.shutterbug_demo_row, null);
                }

                TextView text = view.FindViewById<TextView>(Resource.Id.text);
                text.Text = "#" + position + ": " + _shutterbugActivity.Titles[position];

                FetchableImageView image = view.FindViewById<FetchableImageView>(Resource.Id.image);
                image.SetImage(_shutterbugActivity.Urls[position]);

                return view;
            }
        }

        private void loadGalleryContents() {
            mProgressDialog = ProgressDialog.Show(this, String.Empty, GetString(Resource.String.loading));

            new LoadAsyncTask(this).Execute();

        }

        public class LoadAsyncTask : AsyncTask
        {
            private ShutterbugActivity _shutterbugActivity;


            public LoadAsyncTask(ShutterbugActivity shutterbugActivity)
            {
                _shutterbugActivity = shutterbugActivity;
            }

            protected override Java.Lang.Object DoInBackground(params Java.Lang.Object[] @params)
            {
                try
                {
                    URL url = new URL("http://imgur.com/gallery/top/all.json");
                    HttpURLConnection urlConnection = (HttpURLConnection) url.OpenConnection();
                    urlConnection.SetRequestProperty("User-Agent", "");
                    //System.IO.Stream inputStream = new System.IO.Stream(urlConnection.InputStream);
                    // InputStream inputStream =
                    string file = new Java.Util.Scanner(urlConnection.InputStream).UseDelimiter("\\A").Next();
                    JSONObject result = new JSONObject(file);
                    if (result.Has("data")) {
                        JSONArray data = result.GetJSONArray("data");
                        _shutterbugActivity.Urls.Clear();
                        _shutterbugActivity.Titles.Clear();
                        for (int i = 0; i < data.Length(); i++) {
                            JSONObject dataObject = data.GetJSONObject(i);
                            _shutterbugActivity.Urls.Add("http://api.imgur.com/" + dataObject.GetString("hash") + "s" + dataObject.GetString("ext"));
                            _shutterbugActivity.Titles.Add(dataObject.GetString("title"));
                        }
                    }
                } catch (MalformedURLException e) {
                    e.PrintStackTrace();
                } catch (IOException e) {
                    e.PrintStackTrace();
                } catch (JSONException e) {
                    e.PrintStackTrace();
                }
                return null;
            }

            protected override void OnPostExecute(Java.Lang.Object result)
            {
                base.OnPostExecute(result);

                _shutterbugActivity.mAdapter.NotifyDataSetChanged();
                _shutterbugActivity.mProgressDialog.Dismiss();
            }

            /*
            new AsyncTask<Void, Void, Void>() {

                @Override
                protected Void doInBackground(Void... params) {

                }

                @Override
                protected void onPostExecute(Void result) {
                    super.onPostExecute(result);
                }

            }.execute();
            */
        }
    }
}


