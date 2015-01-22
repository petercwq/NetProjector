
using Android.App;
using Android.OS;
using Android.Views;

namespace NetProjector.Android
{
    class NoteTabFragment : Fragment
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(Resource.Layout.Tab_note, container, false);

            return view;
        }
    }
}