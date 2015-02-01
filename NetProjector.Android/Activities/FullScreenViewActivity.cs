using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using NetProjector.Android.Adaptors;

namespace NetProjector.Android.Activities
{
    [Activity(Label = "FullScreenViewActivity", Theme = "@android:style/Theme.Holo.NoActionBar")]
    public class FullScreenViewActivity : Activity
    {
        AdapterViewFlipper mViewFlipper;
        ViewAdapter adapter;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Create your application here

            var mContext = this;

            SetContentView(Resource.Layout.Fullscreen_view);

            mViewFlipper = (AdapterViewFlipper)FindViewById(Resource.Id.adpater_view_flipper);

            var listener = new SwipeOnGestureListener(AppConstant.SWIPE_MIN_DISTANCE, AppConstant.SWIPE_THRESHOLD_VELOCITY);
            var detector = new GestureDetector(listener);

            mViewFlipper.Touch += (sender, e) =>
            {
                detector.OnTouchEvent(e.Event);
            };

            listener.Next += (sender, e) =>
            {
                mViewFlipper.SetInAnimation(mContext, Resource.Animation.left_in_obj);
                mViewFlipper.SetOutAnimation(mContext, Resource.Animation.left_out_obj);
                mViewFlipper.ShowNext();
            };

            listener.Previous += (sender, e) =>
            {
                mViewFlipper.SetInAnimation(mContext, Resource.Animation.right_in_obj);
                mViewFlipper.SetOutAnimation(mContext, Resource.Animation.right_out_obj);
                mViewFlipper.ShowPrevious();
            };

            int position = Intent.GetIntExtra("position", 0);
            var imagePaths = Utils.GetFileInfos(AppConstant.PHOTO_ALBUM, AppConstant.FILE_EXTN).Select(x => x.FullName);

            adapter = new ViewAdapter(() => imagePaths.Count(), (pos, oldView) =>
            {
                var viewLayout = oldView != null ? oldView : this.LayoutInflater.Inflate(Resource.Layout.Fullscreen_image, mViewFlipper, false);

                var imgDisplay = (TouchImageView)viewLayout.FindViewById(Resource.Id.imgDisplay);
                imgDisplay.SetScaleType(ImageView.ScaleType.CenterInside);

                if (viewLayout != oldView)
                {
                    // close button click event
                    var btnClose = viewLayout.FindViewById(Resource.Id.btnClose);
                    btnClose.Click += (sender, e) => this.Finish();
                }

                //BitmapFactory.Options options = new BitmapFactory.Options();
                //options.InPreferredConfig = Bitmap.Config.Argb8888;
                //Bitmap bitmap = BitmapFactory.DecodeFile(imagePaths.ElementAt(pos), options);
                Task.Run<Bitmap>(() => Utils.LoadAndResizeBitmap(imagePaths.ElementAt(pos), Resources.DisplayMetrics.WidthPixels, Resources.DisplayMetrics.HeightPixels)).ContinueWith(x => imgDisplay.SetImageBitmap(x.Result), TaskScheduler.FromCurrentSynchronizationContext());

                return viewLayout;
            });

            mViewFlipper.Adapter = adapter;

            // displaying selected image first
            mViewFlipper.SetSelection(position);
        }
    }
}