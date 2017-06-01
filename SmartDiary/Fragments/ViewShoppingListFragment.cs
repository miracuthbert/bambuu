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
using SmartDiary.Droid.ViewModel;
using Fragment = Android.Support.V4.App.Fragment;

namespace SmartDiary.Droid
{
    public class ViewShoppingListFragment : Fragment
    {
        private View view;
        private TextView myListId;
        private TextView myList;
        private TextView myListDesc;
        private TextView myShopDate;
        private TextView myListBudget;
        private TextView myListActBudget;
        private TextView myListStatus;
        private TextView myListNotify;
        private int selList;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {

            // Use this to return your custom view for this Fragment
            view = inflater.Inflate(Resource.Layout.ViewShoppingList, container, false);

            Activity MyActivity = this.Activity;
            selList = Convert.ToInt32(MyActivity.Intent.Extras.Get("ListId"));

            myListId = view.FindViewById<TextView>(Resource.Id.ListItemID);
            myList = view.FindViewById<TextView>(Resource.Id.ListItemName);
            myListDesc = view.FindViewById<TextView>(Resource.Id.ListItemDetails);
            myShopDate = view.FindViewById<TextView>(Resource.Id.ListItemDate);
            myListBudget = view.FindViewById<TextView>(Resource.Id.ListItemBudget);
            myListActBudget = view.FindViewById<TextView>(Resource.Id.ListItemActBudget);
            myListStatus = view.FindViewById<TextView>(Resource.Id.ListItemStatus);
            myListNotify = view.FindViewById<TextView>(Resource.Id.ListItemNotification);

            //populate activity
            populateActivity(selList);

            return view;
        }

        //title
        public override string ToString()
        {
            return "Details:";
        }

        public override void OnResume()
        {
            base.OnResume();
            populateActivity(selList);
        }

        //populate activity
        private void populateActivity(int id)
        {
            try
            {
                DBHelper dbh = new DBHelper();
                string[] values = dbh.ReadShoppingList(id);

                myListId.Text = values[0];
                myList.Text = values[1];
                myListDesc.Text = values[2];
                myShopDate.Text = values[3];
                myListBudget.Text = values[4];
                myListActBudget.Text = values[5];
                myListStatus.Text = values[6];

                if (values[6].Equals("Pending"))    //show days left if status is "Pending"
                {
                    int days = Convert.ToInt32(DateTime.Parse(values[3]).Subtract(DateTime.Today).TotalDays);

                    myListNotify.Text = days + " days left to shopping.";
                }
            }
            catch (Exception ex)
            {
                Toast.MakeText(view.Context, "" + ex.Message, ToastLength.Long).Show();
            }
        }
    }
}