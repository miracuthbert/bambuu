using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Lang;
using SmartDiary.Droid.Models;

namespace SmartDiary.Droid.ViewModel
{
    class ShoppingAdapter : BaseAdapter
    {
        private Context context;
        private JavaList<ShoppingLists> lists;
        private LayoutInflater inflater;
        private ShoppingLists list;

        public ShoppingAdapter(Context context, JavaList<ShoppingLists> lists)
        {
            this.context = context;
            this.lists = lists;
        }

        public override int Count
        {
            get
            {
                return lists.Size();
            }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return lists.Get(position);
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            if (inflater == null)
            {
                inflater = (LayoutInflater)context.GetSystemService(Context.LayoutInflaterService);
            }

            if (convertView == null)
            {
                convertView = inflater.Inflate(Resource.Layout.ShoppingListItem, parent, false);
            }

            TextView myListId = convertView.FindViewById<TextView>(Resource.Id.ListItemID);
            TextView myList = convertView.FindViewById<TextView>(Resource.Id.ListItemName);
            TextView myShopDate = convertView.FindViewById<TextView>(Resource.Id.ListItemDate);
            TextView myListBudget = convertView.FindViewById<TextView>(Resource.Id.ListItemBudget);
            TextView myListStatus = convertView.FindViewById<TextView>(Resource.Id.ListItemStatus);
            TextView myListNotify = convertView.FindViewById<TextView>(Resource.Id.ListItemNotification);

            list = lists[position];

            myListId.Text = list.Id.ToString();
            myList.Text = list.ListTitle;
            myShopDate.Text = list.ShoppingDate;
            myListBudget.Text = list.ExpectedBudget.ToString();
            myListStatus.Text = list.ListStatus;

            int days = Convert.ToInt32(DateTime.Parse(list.ShoppingDate).Subtract(DateTime.Today).TotalDays);
            myListNotify.Text = days + " days left to shopping.";

            if (list.ListStatus.Equals("Pending"))
            {
                //item background
                convertView.SetBackgroundColor(Android.Graphics.Color.White);
                //list title color
                myList.SetTextColor(Android.Graphics.Color.SlateGray);
                //list status color
                myListNotify.SetTextColor(Android.Graphics.Color.White);
                //myListStatus.SetTextColor(Android.Graphics.Color.Rgb(183, 28, 28));
                //list notification visibility
                myListNotify.Visibility = ViewStates.Visible;

                if (days < 0)
                {
                    //myListStatus.SetBackgroundColor(Android.Graphics.Color.Rgb(236, 64, 122));
                    //myListNotify.SetBackgroundColor(Android.Graphics.Color.Rgb(183, 28, 28));
                    
                    //item background
                    convertView.SetBackgroundColor(Android.Graphics.Color.Maroon);
                    //list title color
                    myList.SetTextColor(Android.Graphics.Color.White);
                    //list status color
                    myListStatus.SetTextColor(Android.Graphics.Color.White);
                    //list notification color
                    myListNotify.SetTextColor(Android.Graphics.Color.White);
                    //list notification text
                    myListNotify.Text = "Shopping is " + (days * -1) + " past due date. Do something!";
                }
                else if (days == 0)
                {
                    //convertView.SetBackgroundColor(Android.Graphics.Color.Rgb(240, 98, 146));

                    //list notification backgroundcolor
                    myListNotify.SetBackgroundColor(Android.Graphics.Color.Rgb(255, 82, 82));
                    //list notification text
                    myListNotify.Text = "Today is shopping day. Go shopping!";
                }
                else if (days > 0 && days <= 14)
                {
                    //convertView.SetBackgroundColor(Android.Graphics.Color.Rgb(244, 143, 177));

                    //list notification backgroundcolor
                    myListNotify.SetBackgroundColor(Android.Graphics.Color.Rgb(239, 154, 154));
                    //list notification text
                    myListNotify.Text = days + " days left to shopping.";
                }
                else if (days > 14)
                {
                    //list notification text
                    myListNotify.Text = days + " days left to shopping.";
                }
            }
            if (list.ListStatus.Equals("Postponed"))
            {
                convertView.SetBackgroundColor(Android.Graphics.Color.Rgb(130, 177, 255));
                //list title color
                myList.SetTextColor(Android.Graphics.Color.White);
                //list date visibility
                myShopDate.Visibility = ViewStates.Gone;
                //list notification visibility
                myListNotify.Visibility = ViewStates.Gone;
            }

            if (list.ListStatus.Equals("Completed"))
            {
                convertView.SetBackgroundColor(Android.Graphics.Color.Rgb(52, 148, 219));
                //list title color
                myList.SetTextColor(Android.Graphics.Color.White);
                //list status text
                myShopDate.Text = "Done on " + list.ShoppingDate;
                //list status color
                myListStatus.SetTextColor(Android.Graphics.Color.White);
                //list notification visibility
                myListNotify.Visibility = ViewStates.Gone;
            }

            return convertView;
        }
    }
}