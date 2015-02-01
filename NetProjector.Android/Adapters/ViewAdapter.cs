using System;
using Android.Views;
using Android.Widget;

namespace NetProjector.Android.Adaptors
{
    class ViewAdapter : BaseAdapter
    {
        //private Context _context;
        //private IEnumerable<string> _filePaths;
        //private readonly Dictionary<string, View> _imageViews;
        private readonly Func<int, View, View> _viewUpdater;
        private readonly Func<int> _getCount;

        public ViewAdapter(Func<int> getCount, Func<int, View, View> viewUpdater)
        {
            //this._context = context;
            //this._filePaths = filePaths;
            _viewUpdater = viewUpdater;
            //_imageViews = new Dictionary<string, View>();
            _getCount = getCount;
        }

        public override bool HasStableIds
        {
            get { return true; }
        }

        public override int Count
        {
            get { return _getCount(); }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            //throw new NotImplementedException();
            return null;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = /*convertView ?? */_viewUpdater(position, convertView);
            return view;
        }
    }
}