using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;

namespace NetProjector.Android.Fragments
{
    class CameraTabFragment : Fragment
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.Tab_camera, container, false);

            var mContext = this.Activity;
            var mViewFlipper = (ViewFlipper)view.FindViewById(Resource.Id.view_flipper);

            var listener = new SwipeOnGestureListener(AppConstant.SWIPE_MIN_DISTANCE, AppConstant.SWIPE_THRESHOLD_VELOCITY);
            var detector = new GestureDetector(listener);

            mViewFlipper.Touch += (sender, e) =>
            {
                detector.OnTouchEvent(e.Event);
            };

            listener.Next += (sender, e) =>
            {
                mViewFlipper.SetInAnimation(mContext, Resource.Animation.left_in);
                mViewFlipper.SetOutAnimation(mContext, Resource.Animation.left_out);
                mViewFlipper.ShowNext();
            };

            listener.Previous += (sender, e) =>
            {
                mViewFlipper.SetInAnimation(mContext, Resource.Animation.right_in);
                mViewFlipper.SetOutAnimation(mContext, Resource.Animation.right_out);
                mViewFlipper.ShowPrevious();
            };

            view.FindViewById(Resource.Id.play).Click += (sender, e) =>
                {
                    //sets auto flipping
                    mViewFlipper.AutoStart = true;
                    mViewFlipper.SetFlipInterval(4000);
                    mViewFlipper.StartFlipping();
                };

            view.FindViewById(Resource.Id.stop).Click += (sender, e) =>
                {
                    //stop auto flipping 
                    mViewFlipper.StopFlipping();
                };
            return view;
        }
    }
}