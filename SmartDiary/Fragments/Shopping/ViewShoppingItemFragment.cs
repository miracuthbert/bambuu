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
using SmartDiary.Droid.ViewModel;

namespace SmartDiary.Droid
{
    public class ViewShoppingItemFragment : Fragment
    {
        private View view;
        private int selItemId;
        TextView itemID;
        TextView item;
        TextView itemQty;
        TextView itemMse;
        TextView itemStatus;


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            view = inflater.Inflate(Resource.Layout.ShoppingItem, container, false);

            Activity MyActivity = Activity;
            selItemId = Convert.ToInt32(MyActivity.Intent.Extras.Get("PassedId").ToString());

            itemID = view.FindViewById<TextView>(Resource.Id.ItemID);
            item = view.FindViewById<TextView>(Resource.Id.ItemName);
            itemQty = view.FindViewById<TextView>(Resource.Id.ItemQty);
            itemMse = view.FindViewById<TextView>(Resource.Id.ItemMse);
            itemStatus = view.FindViewById<TextView>(Resource.Id.ItemStatus);

            //populate view
            populateActivity(selItemId);

            return view;
        }

        /// <summary>
        /// Populates Activity with Shopping Item.
        /// </summary>
        /// <param name="selItemId"></param>
        private void populateActivity(int selItemId)
        {
            try
            {
                DBHelper dbh = new DBHelper();
                string[] result = dbh.ReadShoppingItem(selItemId);

                itemID.Text = result[0];
                item.Text = result[1];
                itemQty.Text = result[3];
                itemMse.Text = result[4];
                itemStatus.Text = result[7];

            }
            catch (Exception ex)
            {
                Toast.MakeText(view.Context, "Error: " + ex.Message, ToastLength.Long).Show();
            }
        }
    }
}