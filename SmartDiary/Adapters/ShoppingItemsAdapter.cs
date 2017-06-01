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
    class ShoppingItemsAdapter : BaseAdapter
    {
        private Context context;
        private JavaList<ShoppingItems> lists;
        private LayoutInflater inflater;
        private ShoppingItems list;

        public ShoppingItemsAdapter(Context context, JavaList<ShoppingItems> lists)
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
                convertView = inflater.Inflate(Resource.Layout.ShoppingItem, parent, false);
            }

            TextView itemID = convertView.FindViewById<TextView>(Resource.Id.ItemID);
            TextView item = convertView.FindViewById<TextView>(Resource.Id.ItemName);
            TextView itemQty = convertView.FindViewById<TextView>(Resource.Id.ItemQty);
            TextView itemMse = convertView.FindViewById<TextView>(Resource.Id.ItemMse);
            TextView itemStatus = convertView.FindViewById<TextView>(Resource.Id.ItemStatus);

            list = lists[position];

            itemID.Text = list.Id.ToString();
            item.Text = list.Item;
            itemQty.Text = list.ItemQuantity.ToString();
            itemMse.Text = list.ItemMeasure.ToString();
            itemStatus.Text = list.ItemStatus;

            if(itemStatus.Equals("Pending"))
            {
                itemStatus.SetTextColor(Android.Graphics.Color.Rgb(183, 28, 28));
            }

            return convertView;

        }
    }
}