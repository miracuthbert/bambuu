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
    public class AddGroupFragment : Fragment
    {
        private View view;
        private TextInputLayout GroupName;
        private TextInputLayout GroupLocation;

        public override void OnCreate(Bundle savedInstanceState)
        {
            HasOptionsMenu = true;
            Activity.SetTitle(Resource.String.add_group);

            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            view = inflater.Inflate(Resource.Layout.AddGroup, container, false);

            GroupName = view.FindViewById<TextInputLayout>(Resource.Id.txtInputLayoutGroupName);
            GroupLocation = view.FindViewById<TextInputLayout>(Resource.Id.txtInputLayoutGroupLocation);

            return view;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {

            switch (item.ItemId)
            {
                case Resource.Id.menu_action_add:
                    postAddGroup();
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        private void postAddGroup()
        {
            string name = GroupName.EditText.Text;
            string loc = GroupLocation.EditText.Text;

            if (name.Equals("")) { GroupName.Error = "group name required!"; }

            if (loc.Equals("")) { GroupLocation.Error = "group location required!"; }

            if (name != "" || loc != "")
            {
                Toast.MakeText(view.Context, name + " - " + loc, ToastLength.Short);
                Activity.Finish();
            }

        }
    }
}