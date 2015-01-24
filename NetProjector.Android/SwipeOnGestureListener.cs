using System;
using Android.Content;
using Android.Views;
using Android.Widget;

namespace NetProjector.Android
{
    class SwipeOnGestureListener : GestureDetector.SimpleOnGestureListener
    {
        public event EventHandler Next;
        public event EventHandler Previous;

        private readonly int _minDistance, _thresholdVelocity;

        public SwipeOnGestureListener(int minDistance, int thresholdVelocity)
        {
            _minDistance = minDistance;
            _thresholdVelocity = thresholdVelocity;
        }

        public override bool OnFling(MotionEvent e1, MotionEvent e2, float velocityX, float velocityY)
        {
            try
            {
                // right to left swipe
                if (e1.GetX() - e2.GetX() > _minDistance && Math.Abs(velocityX) > _thresholdVelocity)
                {
                    if (Next != null)
                    {
                        Next(this, new EventArgs());
                    }
                    return true;
                }
                else if (e2.GetX() - e1.GetX() > _minDistance && Math.Abs(velocityX) > _thresholdVelocity)
                {
                    if (Previous != null)
                    {
                        Previous(this, new EventArgs());
                    }
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