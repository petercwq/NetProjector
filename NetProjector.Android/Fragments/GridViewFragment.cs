using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Widget;
using NetProjector.Android.Activities;
using NetProjector.Android.Adaptors;

namespace NetProjector.Android.Fragments
{
    [Activity(Label = "GridViewActivity")]
    public class GridViewFragment : Fragment
    {
        private IEnumerable<string> imagePaths;
        private ViewAdapter adapter;
        private GridView gridView;
        private int columnWidth;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.Grid_view, container, false);
            var activity = this.Activity;

            gridView = (GridView)view.FindViewById(Resource.Id.grid_view);

            // Initilizing Grid View
            InitilizeGridLayout();

            // loading all image paths from SD card
            imagePaths = Utils.GetFileInfos(AppConstant.PHOTO_ALBUM, AppConstant.FILE_EXTN).Select(x => x.FullName);

            // Gridview adapter
            adapter = new ViewAdapter(() => imagePaths.Count(), pos =>
            {
                var imageWidth = columnWidth;
                var imageView = new ImageView(activity);
                var image = Utils.LoadAndResizeBitmap(imagePaths.ElementAt(pos), imageWidth, imageWidth);

                imageView.SetScaleType(ImageView.ScaleType.CenterCrop);
                imageView.LayoutParameters = new GridView.LayoutParams(imageWidth, imageWidth);
                imageView.SetImageBitmap(image);

                // image view click listener
                imageView.Click += (sender, e) =>
                {
                    // on selecting grid view image
                    // launch full screen activity
                    Intent i = new Intent(activity, typeof(FullScreenViewActivity));
                    i.PutExtra("position", pos);
                    StartActivity(i);
                };

                return imageView;
            });

            // setting grid view adapter
            gridView.Adapter = adapter;

            return view;
        }

        //protected override void OnCreate(Bundle bundle)
        //{
        //    base.OnCreate(bundle);

        //    // Create your application here
        //    SetContentView(Resource.Layout.Grid_view);

        //    gridView = (GridView)FindViewById(Resource.Id.grid_view);

        //    // Initilizing Grid View
        //    InitilizeGridLayout();

        //    // loading all image paths from SD card
        //    imagePaths = Utils.GetFileInfos(AppConstant.PHOTO_ALBUM, AppConstant.FILE_EXTN).Select(x => x.FullName);

        //    // Gridview adapter
        //    adapter = new ViewAdapter(() => imagePaths.Count(), pos =>
        //    {
        //        var imageWidth = columnWidth;
        //        var imageView = new ImageView(this);
        //        var image = Utils.LoadAndResizeBitmap(imagePaths.ElementAt(pos), imageWidth, imageWidth);

        //        imageView.SetScaleType(ImageView.ScaleType.CenterCrop);
        //        imageView.LayoutParameters = new GridView.LayoutParams(imageWidth, imageWidth);
        //        imageView.SetImageBitmap(image);

        //        // image view click listener
        //        imageView.Click += (sender, e) =>
        //        {
        //            // on selecting grid view image
        //            // launch full screen activity
        //            Intent i = new Intent(this, typeof(FullScreenViewActivity));
        //            i.PutExtra("position", pos);
        //            StartActivity(i);
        //        };

        //        return imageView;
        //    });

        //    // setting grid view adapter
        //    gridView.Adapter = adapter;
        //}

        private void InitilizeGridLayout()
        {
            float padding = TypedValue.ApplyDimension(ComplexUnitType.Dip, AppConstant.GRID_PADDING, this.Resources.DisplayMetrics);

            columnWidth = (int)((Utils.GetScreenWidthInPix() - ((AppConstant.NUM_OF_COLUMNS + 1) * padding)) / AppConstant.NUM_OF_COLUMNS);

            gridView.SetNumColumns(AppConstant.NUM_OF_COLUMNS);
            gridView.SetColumnWidth(columnWidth);
            gridView.StretchMode = StretchMode.NoStretch;
            gridView.SetPadding((int)padding, (int)padding, (int)padding, (int)padding);
            gridView.SetHorizontalSpacing((int)padding);
            gridView.SetVerticalSpacing((int)padding);
        }
    }
}