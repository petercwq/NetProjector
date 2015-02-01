using System.Collections.Generic;
using Android.OS;

namespace NetProjector.Android
{
    public class AppConstant
    {
        // Number of columns of Grid View
        public static readonly int NUM_OF_COLUMNS = 3;

        // Gridview image padding
        public static readonly int GRID_PADDING = 8; // in dp

        // SD card image directory
        public static readonly string PHOTO_ALBUM = Environment.GetExternalStoragePublicDirectory(Environment.DirectoryDcim).AbsolutePath;
        // supported file formats
        public static readonly List<string> FILE_EXTN = new List<string> { "*.jpg", "*.jpeg", "*.png", "*.JPG", "*.JPEG" };

        public static readonly int SWIPE_MIN_DISTANCE = 80;
        public static readonly int SWIPE_THRESHOLD_VELOCITY = 100;
    }
}