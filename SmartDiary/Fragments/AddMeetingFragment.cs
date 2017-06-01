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

namespace SmartDiary.Droid.Fragments
{
    public class AddMeetingFragment : Fragment
    {
        private View view;
        private TextInputLayout Bugdet;
        private TextInputLayout BudgetDetails;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            view = inflater.Inflate(Resource.Layout.AddMeeting, container, false);

            //Budget = view.FindViewById<TextInputLayout>(Resource.Id.txtInputLayoutGroupName);
            //BudgetDetails = view.FindViewById<TextInputLayout>(Resource.Id.txtInputLayoutGroupLocation);

            return view;
        }
    }
}