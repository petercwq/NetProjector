using System.IO;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Views;

namespace NetProjector.Android
{
    static class Utils
    {
        public static Bitmap ScreenShotByDraw(View view)
        {
            var b = Bitmap.CreateBitmap(view.Width, view.Height, Bitmap.Config.Argb8888);
            Canvas canvas = new Canvas(b);
            view.RootView.Draw(canvas);
            canvas.Dispose();
            return b;
        }

        public static Bitmap ScreenShotByCache(View view)
        {
            return view.GetDrawingCache(true);
        }

        public async static Task<byte[]> BitmapToBytes(Bitmap bmp, int quality = 70)
        {
            return await BitmapToBytes(bmp, Bitmap.CompressFormat.Jpeg, quality);
        }

        public async static Task<byte[]> BitmapToBytes(Bitmap bmp, Bitmap.CompressFormat format, int quality = 70)
        {
            if (bmp != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    if (await bmp.CompressAsync(format, quality, ms))
                        return ms.ToArray();
                }
            }
            return null;
        }

        public async static Task<bool> WriteBitmapTo(Stream st, Bitmap bmp, Bitmap.CompressFormat format, int quality = 70)
        {
            return await bmp.CompressAsync(format, quality, st);
        }

        public static void ShareTextTo(this Context context, string text)
        {
            Intent sendIntent = new Intent();
            sendIntent.SetAction(Intent.ActionSend);
            sendIntent.PutExtra(Intent.ExtraText, text);
            sendIntent.SetType("text/plain");
            context.StartActivity(Intent.CreateChooser(sendIntent, "Send NetProjector link to:"));
        }
    }
}