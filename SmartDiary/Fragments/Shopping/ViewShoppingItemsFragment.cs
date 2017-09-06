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
using SmartDiary.Droid.Models;
using SmartDiary.Droid.Views;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;

namespace SmartDiary.Droid
{
    public class ViewShoppingItemsFragment : Fragment
    {
        private View view;
        private ListView mListItems;
        private ShoppingItemsAdapter adapter;
        private JavaList<ShoppingItems> sItems;
        private int listId;
        private int selShopItem;
        private string selItemStatus;
        public Activity mActivity;


        public override void OnCreate(Bundle savedInstanceState)
        {
            HasOptionsMenu = true;
            base.OnCreate(savedInstanceState);
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            menu.SetGroupVisible(Resource.Id.grp_edit_shop_list, false);
            menu.SetGroupVisible(Resource.Id.grp_shop_list_items, true);
            base.OnCreateOptionsMenu(menu, inflater);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            DBHelper dbh;

            switch(item.ItemId)
            {
                case Resource.Id.menu_sitem_del_all:
                    dbh = new DBHelper();
                    AlertDialog.Builder builder = new AlertDialog.Builder(view.Context);
                    AlertDialog alert = builder.Create();
                    alert.SetTitle("Delete list items");
                    alert.SetIcon(Android.Resource.Drawable.IcDialogAlert);
                    alert.SetMessage("Are you sure?");

                    alert.SetButton2("Yes", (s, ev) =>
                    {   //yes
                        string dresult = dbh.DeleteShoppingListItems(listId);
                        if (dresult.Equals("ok"))
                        {
                            Toast.MakeText(view.Context, "Items deleted!", ToastLength.Short).Show();
                            populateItemsList(view, listId);
                        }
                        else
                        {
                            Toast.MakeText(view.Context, "Deletion failed!", ToastLength.Short).Show();
                        }
                    });
                    alert.SetButton("No", (s, ev) =>
                    {   //no

                    });
                    alert.Show();
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }

        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            view =  inflater.Inflate(Resource.Layout.ViewShoppingItems, container, false);

            Activity MyActivity = this.Activity;
            listId = Convert.ToInt32(MyActivity.Intent.Extras.Get("ListId"));

            mListItems = view.FindViewById<ListView>(Resource.Id.listTrackShoppingItems);

            //click
            mListItems.ItemClick += MListItems_ItemClick;

            //context menu
            mListItems.ContextMenuCreated += MListItems_ContextMenuCreated;

            //context menu click
            //mListItems.ContextClick += MListItems_ContextClick;

            //populate list
            populateItemsList(view, listId);

            return view;
        }

        //context menu click
        //private void MListItems_ContextClick(object sender, View.ContextClickEventArgs e)
        //{
        //    int id = Convert.ToInt32(e.V.FindViewById<TextView>(Resource.Id.ItemID).Text);
        //}

        //context menu created
        private void MListItems_ContextMenuCreated(object sender, View.CreateContextMenuEventArgs e)
        {
            e.Menu.SetHeaderTitle("Shopping item options:");
            MenuInflater inflater = new MenuInflater(mListItems.Context);
            inflater.Inflate(Resource.Menu.shop_item_popup, e.Menu);
        }

        //context item click
        public override bool OnContextItemSelected(IMenuItem item)
        {
            string selItem = selShopItem.ToString();
            DBHelper dbh;
            Intent intent;

            switch (item.ItemId)
            {
                case Resource.Id.pop_shop_item_view:  //view item
                    intent = new Intent(view.Context, typeof(ViewActivity));
                    intent.PutExtra("PassedId", listId);
                    intent.PutExtra("LoadView", Resource.Layout.ViewShoppingItem.ToString());
                    StartActivity(intent);
                    return true;

                case Resource.Id.pop_shop_list_edit:  //edit item
                    intent = new Intent(view.Context, typeof(AddEditActivity));
                    intent.PutExtra("PassedId", listId);
                    intent.PutExtra("LoadView", Resource.Layout.AddShoppingItem.ToString());
                    StartActivity(intent);
                    return true;

                case Resource.Id.pop_shop_item_delete:  //delete item
                    dbh = new DBHelper();
                    AlertDialog.Builder builder = new AlertDialog.Builder(view.Context);
                    AlertDialog alert = builder.Create();
                    alert.SetTitle("Delete item");
                    alert.SetIcon(Android.Resource.Drawable.IcDialogAlert);
                    alert.SetMessage("Are you sure?");

                    alert.SetButton2("Yes", (s, ev) =>
                    {   //yes
                        string dresult = dbh.DeleteShoppingItem(selShopItem);
                        if (dresult.Equals("ok"))
                        {
                            Toast.MakeText(view.Context, "Item deleted!", ToastLength.Short).Show();
                            populateItemsList(view, listId);
                        }
                        else
                        {
                            Toast.MakeText(view.Context, "Deletion failed!", ToastLength.Short).Show();
                        }
                    });
                    alert.SetButton("No", (s, ev) =>
                    {   //no

                    });
                    alert.Show();
                    return true;

                case Resource.Id.pop_shop_item_status:  //update item status
                    dbh = new DBHelper();
                    string status = "Pending";

                    if(selItemStatus.Equals("Pending"))
                    {
                        status = "Done";
                    }
                    else
                    {
                        status = "Pending";
                    }

                    string sresult = dbh.UpdateShoppingItemStatus(selShopItem, status);
                    if (sresult.Equals("ok"))
                    {
                        //Toast.MakeText(view.Context, "" + selShopItem, ToastLength.Short).Show();
                        populateItemsList(view, listId);
                    }
                    else
                    {
                        Toast.MakeText(view.Context, "Failed updating item status!", ToastLength.Short).Show();
                    }
                    return true;

                default:
                    base.OnContextItemSelected(item);
                    return true;
            }
        }

        //list item click
        private void MListItems_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            selShopItem = Convert.ToInt32(sItems[e.Position].Id);
            selItemStatus = sItems[e.Position].ItemStatus;

            //Intent intent = new Intent(view.Context, typeof(ViewGoalActivity));
            //intent.PutExtra("ShopItemId", selShopItem);
            //StartActivity(intent);
            mListItems.ShowContextMenu();
        }

        //title
        public override string ToString()
        {
            return "Items";
        }

        public override void OnResume()
        {
            base.OnResume();
            populateItemsList(view, listId);
        }

        //populate list
        private async void populateItemsList(View view, int sId)
        {
            try
            {
                sItems = ShoppingCollection.GetShoppingItems(sId);
                adapter = new ShoppingItemsAdapter(view.Context, sItems);
                mListItems.Adapter = adapter;
            }
            catch (Exception ex)
            {
                Toast.MakeText(view.Context, "" + ex.Message, ToastLength.Long).Show();
            }
        }
    }
}