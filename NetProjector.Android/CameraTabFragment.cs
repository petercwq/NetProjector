using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;

namespace NetProjector.Android
{
    class CameraTabFragment : Fragment, Animation.IAnimationListener
    {
        private static readonly int SWIPE_MIN_DISTANCE = 120;
        private static readonly int SWIPE_THRESHOLD_VELOCITY = 200;
        private GestureDetector detector;
        private ViewFlipper mViewFlipper;
        private Context mContext;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            //return base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(Resource.Layout.Tab_camera, container, false);

            mContext = this.Activity;
            mViewFlipper = (ViewFlipper)view.FindViewById(Resource.Id.view_flipper);

            detector = new GestureDetector(new SwipeGestureDetector(mContext, mViewFlipper, this));

            mViewFlipper.Touch += (sender, e) =>
            {
                detector.OnTouchEvent(e.Event);
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

        #region IAnimationListener Members

        public void OnAnimationEnd(Animation animation)
        {
            //animation started event
        }

        public void OnAnimationRepeat(Animation animation)
        {
        }

        public void OnAnimationStart(Animation animation)
        {
            //TODO animation stopped event
        }

        #endregion


        class SwipeGestureDetector : GestureDetector.SimpleOnGestureListener
        {
            Context mContext;
            Animation.IAnimationListener mAnimationListener;
            ViewFlipper mViewFlipper;

            public SwipeGestureDetector(Context context, ViewFlipper flipper, Animation.IAnimationListener animationListener)
            {
                this.mContext = context;
                this.mAnimationListener = animationListener;
                this.mViewFlipper = flipper;
            }

            public override bool OnFling(MotionEvent e1, MotionEvent e2, float velocityX, float velocityY)
            {
                try
                {
                    // right to left swipe
                    if (e1.GetX() - e2.GetX() > SWIPE_MIN_DISTANCE && Math.Abs(velocityX) > SWIPE_THRESHOLD_VELOCITY)
                    {
                        mViewFlipper.SetInAnimation(mContext, Resource.Animation.left_in);
                        mViewFlipper.SetOutAnimation(mContext, Resource.Animation.left_out);
                        // controlling animation
                        mViewFlipper.InAnimation.SetAnimationListener(mAnimationListener);
                        mViewFlipper.ShowNext();
                        return true;
                    }
                    else if (e2.GetX() - e1.GetX() > SWIPE_MIN_DISTANCE && Math.Abs(velocityX) > SWIPE_THRESHOLD_VELOCITY)
                    {
                        mViewFlipper.SetInAnimation(mContext, Resource.Animation.right_in);
                        mViewFlipper.SetOutAnimation(mContext, Resource.Animation.right_out);
                        // controlling animation
                        mViewFlipper.InAnimation.SetAnimationListener(mAnimationListener);
                        mViewFlipper.ShowPrevious();
                        return true;
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                return false;
            }
        }
    }
}