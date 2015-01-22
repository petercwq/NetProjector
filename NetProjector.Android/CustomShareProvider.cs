using System;
using Android.Content;
using Android.Graphics.Drawables;
using Android.Views;
using Android.Widget;

namespace NetProjector.Android
{
    public class CustomShareProvider : ShareActionProvider
    {
        private readonly Context mContext;

        public CustomShareProvider(Context context)
            : base(context)
        {
            mContext = context;
        }

        public override View OnCreateActionView()
        {
            View chooserView = base.OnCreateActionView();

            var icon = mContext.Resources.GetDrawable(Resource.Drawable.ic_action_start);

            var clazz = chooserView.GetType();

            //reflect all of this shit so that I can change the icon
            try
            {
                var method = clazz.GetMethod("SetExpandActivityOverflowButtonDrawable", new Type[] { typeof(Drawable) });
                method.Invoke(chooserView, new object[] { icon });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return chooserView;
        }
    }
}