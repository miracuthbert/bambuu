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
using SmartDiary.Droid.ViewModel;
using Android.Support.V7.App;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using SmartDiary.Droid.Models;
using SmartDiary.Droid.Views;
using Android.Database;

namespace SmartDiary.Droid
{
    [Activity(Label = "Add Item", Icon = "@drawable/storelogo", Theme = "@style/MyTheme")]
    public class AddShoppingItemActivity : AppCompatActivity
    {
        private ArrayAdapter adapter;
        private JavaList<ShoppingLists> allLists;
        private Spinner itemList;
        private EditText itemName;
        private EditText itemQty;
        private EditText itemMse;
        private EditText itemExpPrice;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Get layout for this activity
            SetContentView(Resource.Layout.AddShoppingItem);

            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetHomeButtonEnabled(true);

            itemList = FindViewById<Spinner>(Resource.Id.spinItemList);
            itemName = FindViewById<EditText>(Resource.Id.txtShopItemName);
            itemQty = FindViewById<EditText>(Resource.Id.txtShopItemQty);
            itemMse = FindViewById<EditText>(Resource.Id.txtShopItemMeasure);
            itemExpPrice = FindViewById<EditText>(Resource.Id.itemExpPrice);

            allLists = ShoppingCollection.SpinnerLists();
            adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, allLists);
            itemList.Adapter = adapter;

        }


        //Override On create menu
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.add_cancel_item_menu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        //Override On Options Selected
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.menu_task_add:
                    CreateItem();
                    return true;

                case Resource.Id.menu_task_cancel:
                    Finish();
                    return true;

                case Android.Resource.Id.Home:
                    Finish();
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);

            }
        }

        //save item
        private void CreateItem()
        {
            try
            {
                if (itemList.SelectedItemPosition <= -1)
                {
                    Toast.MakeText(this, "Select a shopping list!", ToastLength.Long).Show();
                    return;
                }
                if (itemName.Text.Equals("") || itemQty.Text.Equals("") || itemMse.Text.Equals("") || itemExpPrice.Text.Equals(""))
                {
                    Toast.MakeText(this, "Fill in all fields!", ToastLength.Long).Show();
                }
                else
                {
                    allLists = ShoppingCollection.GetShoppingLists(); 
                    DBHelper dbh = new DBHelper();

                    string name = DatabaseUtils.SqlEscapeString(itemName.Text);
                    int list = Convert.ToInt32(allLists[itemList.SelectedItemPosition].Id);
                    decimal qty = Convert.ToDecimal(itemQty.Text);
                    string measure = DatabaseUtils.SqlEscapeString(itemMse.Text);
                    decimal expPrice = Convert.ToDecimal(itemExpPrice.Text);

                    string result = dbh.CreateShoppingItem(name, list, qty, measure, expPrice);

                    if (result.Equals("ok"))
                    {
                        Toast.MakeText(this, "Item added!", ToastLength.Short).Show();
                        Finish();
                    }
                    else
                    {
                        Toast.MakeText(this, result, ToastLength.Short).Show();
                    }
                }

            }
            catch (Exception ex)
            {
                Toast.MakeText(this, "Error:\n" + ex.Message, ToastLength.Long);
            }
        }
    }
}