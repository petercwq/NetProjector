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
        private const ushort port = 8080;
        View view;
        IStop server;

        public string UrlAddress
        {
            get
            {
                return "http://" + NetworkUtils.GetIPAddressFromDns().FirstOrDefault().ToString() + ":" + port;
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
            var statusTextView = FindViewById<TextView>(Resource.Id.statusTextView);
            var startButton = FindViewById<Button>(Resource.Id.startButton);
            var stopButton = FindViewById<Button>(Resource.Id.stopButton);
            var shareButton = FindViewById<Button>(Resource.Id.shareButton);

            this.Title = UrlAddress;
            stopButton.Enabled = false;
            shareButton.Enabled = true;

            //NetworkUtils.GetPublicIPAsync().ContinueWith(t =>
            //{
            //    if (t.Status == TaskStatus.RanToCompletion)
            //        RunOnUiThread(() => this.Title = t.Result.Trim());
            //});

            // add event handlers
            startButton.Click += (sender, e) =>
            {
                server = new ProjectorServer(port, x =>
                {
                    using (var b = Utils.ScreenShotByDraw(view.RootView))
                    {
                        RunOnUiThread(() => statusTextView.Text = "Get at " + DateTime.Now.ToString());

                        return Utils.BitmapToBytes(b, x == "png" ? Bitmap.CompressFormat.Png : Bitmap.CompressFormat.Jpeg).Result;
                    }
                }).Start();

                if (server != null)
                {
                    stopButton.Enabled = true;
                    startButton.Enabled = false;
                }
            };

            stopButton.Click += (sender, e) =>
            {
                Stop();
                startButton.Enabled = true;
                stopButton.Enabled = false;
            };

            shareButton.Click += (sender, e) =>
            {
                this.ShareTextTo(UrlAddress);
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