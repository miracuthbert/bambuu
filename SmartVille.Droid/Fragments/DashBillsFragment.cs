using SupportFragment = Android.Support.V4.App.Fragment;

using Android.App;
using Android.OS;
using Android.Views;
using Android.Support.V7.Widget;

namespace SmartVille.Droid.Fragments
{
    public class DashBillsFragment : SupportFragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            RecyclerView recyclerView = inflater.Inflate(Resource.Layout.Dashboard, container, false) as RecyclerView;

            SetUpRecyclerView(recyclerView);

            return recyclerView;
        }

        private void SetUpRecyclerView(RecyclerView recyclerView)
        {
            
        }
    }
}