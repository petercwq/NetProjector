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
        IMenuItem startMenuItem, stopMenuItem;

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

            //view = Window.DecorView.RootView;
            view = FindViewById(Resource.Id.fragmentContainer);
            view.DrawingCacheEnabled = true;

            this.ActionBar.NavigationMode = ActionBarNavigationMode.Tabs;

            ActionBar.SetTitle(Resource.String.Title);
            ActionBar.SetSubtitle(Resource.String.SubTitle);
            ActionBar.SetDisplayShowTitleEnabled(true);
            ActionBar.SetDisplayHomeAsUpEnabled(true);

            AddTab("", Resource.Drawable.ic_tab_config, new SampleTabFragment());
            AddTab("", Resource.Drawable.ic_tab_camera, new SampleTabFragment2());

            if (bundle != null)
                this.ActionBar.SelectTab(this.ActionBar.GetTabAt(bundle.GetInt("tab")));
        }

        private void Start()
        {
            if (server != null)
                return;
            server = new ProjectorServer(port, x =>
            {
                using (var b = Utils.ScreenShotByDraw(view.RootView))
                {
                    ShowToast("Refresh at " + DateTime.Now.ToString(), ToastLength.Short);
                    return Utils.BitmapToBytes(b, x == "png" ? Bitmap.CompressFormat.Png : Bitmap.CompressFormat.Jpeg).Result;
                }
            }).Start();

            ShowToast("Start successfully on: " + UrlAddress);
            startMenuItem.SetEnabled(false);
            stopMenuItem.SetEnabled(true);
        }

        private void Stop()
        {
            if (server != null)
                server.Stop();
            server = null;
            ShowToast("Stop successfully");
            startMenuItem.SetEnabled(true);
            stopMenuItem.SetEnabled(false);
        }

        protected override void OnDestroy()
        {
            Stop();
            base.OnDestroy();
        }

        private void ShowToast(string text, ToastLength length = ToastLength.Long)
        {
            RunOnUiThread(() => Toast.MakeText(this, text, length).Show());
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            outState.PutInt("tab", this.ActionBar.SelectedNavigationIndex);

            base.OnSaveInstanceState(outState);
        }

        /// <summary>
        /// Attach the menu to the menu button of the device for this activity
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            base.OnCreateOptionsMenu(menu);

            MenuInflater.Inflate(Resource.Menu.ActionBarMenu, menu);

            var shareMenuItem = menu.FindItem(Resource.Id.shareMenuItem);
            var shareActionProvider = (ShareActionProvider)shareMenuItem.ActionProvider;
            shareActionProvider.SetShareIntent(Utils.CreateShareTextIntent(UrlAddress));

            startMenuItem = menu.FindItem(Resource.Id.startmenuitem);
            stopMenuItem = menu.FindItem(Resource.Id.stopmenuitem);
            stopMenuItem.SetEnabled(false);
            return true;
        }

        /// <param name="item">The menu item that was selected.</param>
        /// <summary>
        /// This hook is called whenever an item in your options menu is selected.
        /// </summary>
        /// <returns>To be added.</returns>
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            base.OnOptionsItemSelected(item);

            switch (item.ItemId)
            {
                case Resource.Id.startmenuitem:
                    Start();
                    break;
                case Resource.Id.stopmenuitem:
                    Stop();
                    break;
                default:
                    break;
            }

            return true;
        }

        void AddTab(string tabText, int iconResourceId, Fragment view)
        {
            var tab = this.ActionBar.NewTab();
            tab.SetText(tabText);
            tab.SetIcon(iconResourceId);

            // must set event handler before adding tab
            tab.TabSelected += delegate(object sender, ActionBar.TabEventArgs e)
            {
                var fragment = this.FragmentManager.FindFragmentById(Resource.Id.fragmentContainer);
                if (fragment != null)
                    e.FragmentTransaction.Remove(fragment);
                e.FragmentTransaction.Add(Resource.Id.fragmentContainer, view);
            };
            tab.TabUnselected += delegate(object sender, ActionBar.TabEventArgs e)
            {
                e.FragmentTransaction.Remove(view);
            };

            this.ActionBar.AddTab(tab);
        }

        class SampleTabFragment : Fragment
        {
            public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
            {
                base.OnCreateView(inflater, container, savedInstanceState);

                var view = inflater.Inflate(Resource.Layout.Tab_config, container, false);
                var sampleTextView = view.FindViewById<TextView>(Resource.Id.statusTextView);
                sampleTextView.Text = "sample fragment text";

                return view;
            }
        }

        class SampleTabFragment2 : Fragment
        {
            public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
            {
                base.OnCreateView(inflater, container, savedInstanceState);

                var view = inflater.Inflate(Resource.Layout.Tab_camera, container, false);

                return view;
            }
        }
    }
}