using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.Net;
using Android.Provider;
using Android.Views;

namespace NetProjector
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

        public async static Task<bool> WriteBitmapTo(System.IO.Stream st, Bitmap bmp, Bitmap.CompressFormat format, int quality = 70)
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

        public static Intent CreateShareTextIntent(string text)
        {
            Intent sendIntent = new Intent();
            sendIntent.SetAction(Intent.ActionSend);
            sendIntent.SetType("text/plain");
            sendIntent.PutExtra(Intent.ExtraText, text);
            return sendIntent;
        }

        public static Intent CreateSharePictureIntent(Uri uri)
        {
            var sendPictureIntent = new Intent(Intent.ActionSend);
            sendPictureIntent.SetType("image/*");
            sendPictureIntent.PutExtra(Intent.ExtraStream, uri);
            return sendPictureIntent;
        }

        public static Bitmap LoadAndResizeBitmap(string fileName, int width, int height)
        {
            // First we get the the dimensions of the file on disk
            BitmapFactory.Options options = new BitmapFactory.Options { InJustDecodeBounds = true };
            BitmapFactory.DecodeFile(fileName, options);

            //ExifInterface exif = new ExifInterface(fileName);
            //var orientation = exif.GetAttributeInt(ExifInterface.TagOrientation, 1);

            //if (orientation == )
            //{
            //    Matrix matrix = new Matrix();
            //    matrix.PostRotate(90);
            //}

            // Next we calculate the ratio that we need to resize the image by
            // in order to fit the requested dimensions.
            int outHeight = options.OutHeight;
            int outWidth = options.OutWidth;
            int inSampleSize = 1;

            if (outHeight > height || outWidth > width)
            {
                inSampleSize = outWidth > outHeight
                                   ? outHeight / height
                                   : outWidth / width;
            }

            // Now we will load the image and have BitmapFactory resize it for us.
            options.InSampleSize = inSampleSize;
            options.InJustDecodeBounds = false;
            Bitmap resizedBitmap = BitmapFactory.DecodeFile(fileName, options);

            return resizedBitmap;
        }

        //public static Bitmap decodeFile(string filePath, int WIDTH, int HIGHT) {
        //    try {

        //        File f = new File(filePath);

        //        BitmapFactory.Options o = new BitmapFactory.Options();
        //        o.inJustDecodeBounds = true;
        //        BitmapFactory.decodeStream(new FileInputStream(f), null, o);

        //        final int REQUIRED_WIDTH = WIDTH;
        //        final int REQUIRED_HIGHT = HIGHT;
        //        int scale = 1;
        //        while (o.outWidth / scale / 2 >= REQUIRED_WIDTH
        //                && o.outHeight / scale / 2 >= REQUIRED_HIGHT)
        //            scale *= 2;

        //        BitmapFactory.Options o2 = new BitmapFactory.Options();
        //        o2.inSampleSize = scale;
        //        return BitmapFactory.decodeStream(new FileInputStream(f), null, o2);
        //    } catch (FileNotFoundException e) {
        //        e.printStackTrace();
        //    }
        //    return null;
        //}

        private static bool IsThereAnAppToTakePictures(this Context context)
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            IList<ResolveInfo> availableActivities = context.PackageManager.QueryIntentActivities(intent, PackageInfoFlags.MatchDefaultOnly);
            return availableActivities != null && availableActivities.Count > 0;
        }

        /// <summary>
        ///  Reading file paths from SDCard
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<FileInfo> GetFileInfos(string path, IEnumerable<string> patterns)
        {

            var directory = new DirectoryInfo(path);

            // check for directory
            if (directory.Exists)
            {
                // getting list of file paths
                var listFiles = patterns.SelectMany(p => directory.EnumerateFiles(p, SearchOption.AllDirectories)).Distinct();

                return listFiles;
            }
            else
            {
                return new FileInfo[] { };
            }
        }

        public static int GetScreenWidthInPix()
        {
            return Application.Context.Resources.DisplayMetrics.WidthPixels;
        }

        public static int GetScreenWidthInDip()
        {
            var dm = Application.Context.Resources.DisplayMetrics;
            return ConvertPixelsToDip(dm.WidthPixels);
        }

        public static int ConvertPixelsToDip(float pixelValue)
        {
            var dp = (int)((pixelValue) / Application.Context.Resources.DisplayMetrics.Density);
            return dp;
        }
    }
}