using System;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using NetProjector.Core;

namespace NetProjector.Android
{
    /// <summary>
    /// Main activity of the application.
    /// </summary>
    [Activity(Label = "NetProjector", MainLauncher = true)]
    public class MainActivity : Activity
    {
        View view;
        IStop server;

        public string UrlAddress
        {
            get
            {
                return "http://" + NetworkUtils.GetIPAddressFromDns().FirstOrDefault().ToString();
            }
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            view = Window.DecorView;
            view.RootView.DrawingCacheEnabled = true;

            // initialize controls
            var serverEditText = FindViewById<EditText>(Resource.Id.serverEditText);
            var statusTextView = FindViewById<TextView>(Resource.Id.statusTextView);

            var responseView = FindViewById<TextView>(Resource.Id.responseView);
            var connectButton = FindViewById<Button>(Resource.Id.connectButton);
            var disconnectButton = FindViewById<Button>(Resource.Id.disconnectButton);
            var queryButton = FindViewById<Button>(Resource.Id.myButton);

            serverEditText.Text = UrlAddress;
            serverEditText.Enabled = false;
            disconnectButton.Enabled = false;
            queryButton.Enabled = false;

            //NetworkUtils.GetPublicIPAsync().ContinueWith(t =>
            //{
            //    if (t.Status == TaskStatus.RanToCompletion)
            //        RunOnUiThread(() => this.Title = t.Result.Trim());
            //});

            // add event handlers
            connectButton.Click += (sender, e) =>
            {
                server = new ProjectorServer(8080, x =>
                {
                    using (var b = Utils.ScreenShotByDraw(view.RootView))
                    {
                        RunOnUiThread(() => statusTextView.Text = "Get at " + DateTime.Now.ToString());

                        return Utils.BitmapToBytes(b, x == "png" ? Bitmap.CompressFormat.Png : Bitmap.CompressFormat.Jpeg).Result;
                    }
                }).Start();

                if (server != null)
                {
                    disconnectButton.Enabled = true;
                    connectButton.Enabled = false;
                }
            };

            disconnectButton.Click += (sender, e) =>
            {
                Stop();
                connectButton.Enabled = true;
                disconnectButton.Enabled = false;
            };

            queryButton.Click += (sender, e) =>
            {

            };
        }

        private Task<T> RunAsync<T>(Func<T> func)
        {
            return Task.Factory.StartNew(func);
        }

        private void ShowToast(string text, ToastLength length = ToastLength.Long)
        {
            RunOnUiThread(() => Toast.MakeText(this, text, length).Show());
        }

        private void Stop()
        {
            if (server != null)
                server.Stop();
            server = null;
        }

        protected override void OnDestroy()
        {
            Stop();
            base.OnDestroy();
        }
    }
}