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

namespace SmartDiary.Droid
{
    public class ViewBudgetsFragment : Fragment
    {
        private View view;
        private ListView mListBudgets;

        public override void OnCreate(Bundle savedInstanceState)
        {
            HasOptionsMenu = true;
            base.OnCreate(savedInstanceState);
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            menu.SetGroupVisible(Resource.Id.grp_budgets_frag, true);
            base.OnCreateOptionsMenu(menu, inflater);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            view = inflater.Inflate(Resource.Layout.ViewBudgets, container, false);
            mListBudgets = view.FindViewById<ListView>(Resource.Id.listAllBudgets);

            return view;
        }

        public override string ToString()
        {
            return "Budgets";
        }
    }
}