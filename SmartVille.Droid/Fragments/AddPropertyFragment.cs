using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Fragment = Android.Support.V4.App.Fragment;
using Android.Support.Design.Widget;

namespace SmartVille.Droid.Fragments
{
    public class AddPropertyFragment : Fragment
    {
        private View view;
        private TextInputLayout PropertyName;


        public override void OnCreate(Bundle savedInstanceState)
        {
            HasOptionsMenu = true;
            Activity.SetTitle(Resource.String.add_property);
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            view = inflater.Inflate(Resource.Layout.AddProperty, container, false);

            return view;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {

            switch (item.ItemId)
            {
                case Resource.Id.menu_action_add:
                    postAddProperty();
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        private void postAddProperty()
        {
            
        }
    }
}