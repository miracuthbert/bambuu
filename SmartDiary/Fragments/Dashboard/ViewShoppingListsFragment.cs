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
using SmartDiary.Droid.Models;
using SmartDiary.Droid.Views;
using SmartDiary.Droid.ViewModel;
using Android.Support.Design.Widget;

namespace SmartDiary.Droid
{
    public class ViewShoppingListsFragment : Fragment
    {
        private View view;
        private ListView mShopList;
        private ShoppingAdapter adapter;
        private JavaList<ShoppingLists> lists;
        private int selListId;

        public override void OnCreate(Bundle savedInstanceState)
        {
            HasOptionsMenu = true;
            base.OnCreate(savedInstanceState);
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            menu.SetGroupVisible(Resource.Id.grp_goals_frag, false);
            menu.SetGroupVisible(Resource.Id.grp_projects_frag, false);
            menu.SetGroupVisible(Resource.Id.grp_shopping_frag, true);
            base.OnCreateOptionsMenu(menu, inflater);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            view = inflater.Inflate(Resource.Layout.ViewShoppingLists, container, false);

            FloatingActionButton fab = Activity.FindViewById<FloatingActionButton>(Resource.Id.fab);

            fab.Click += Fab_Click;

            mShopList = view.FindViewById<ListView>(Resource.Id.listShopLists);

            //populate list
            populateShoppingList(view);

            //click
            mShopList.ItemClick += MShopList_ItemClick;

            //context menu
            mShopList.ContextMenuCreated += MShopList_ContextMenuCreated;
            mShopList.FastScrollEnabled = true;

            return view;
        }

        private void Fab_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(view.Context, typeof(AddShoppingListActivity));
            StartActivity(intent);
        }

        public override void OnPrepareOptionsMenu(IMenu menu)
        {
            //menu.FindItem(Resource.Id.menu_delete_goals).SetVisible(false);
            //menu.FindItem(Resource.Id.menu_delete_projects).SetVisible(false);
            menu.FindItem(Resource.Id.menu_delete_shop_lists).SetVisible(true);
            base.OnPrepareOptionsMenu(menu);
        }

        public override void OnViewStateRestored(Bundle savedInstanceState)
        {
            populateShoppingList(view);
            base.OnViewStateRestored(savedInstanceState);
        }

        private void MShopList_ContextMenuCreated(object sender, View.CreateContextMenuEventArgs e)
        {
            e.Menu.SetHeaderTitle("Shopping list options:");
            MenuInflater inflater = new MenuInflater(mShopList.Context);
            inflater.Inflate(Resource.Menu.shop_list_popup, e.Menu);
        }

        private void MShopList_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            try
            {
                this.selListId = Convert.ToInt32(lists[e.Position].Id);

                Intent intent = new Intent(view.Context, typeof(ViewShoppingListActivity));
                intent.PutExtra("ListId", lists[e.Position].Id);
                StartActivity(intent);
            }
            catch (Exception ex)
            {
                Toast.MakeText(view.Context, "" + ex.Message, ToastLength.Long).Show();
            }

            //mShopList.ShowContextMenu();
        }

        public override bool OnContextItemSelected(IMenuItem item)
        {
            string listId = selListId.ToString();
            DBHelper dbh;
            Intent intent;

            switch (item.ItemId)
            {
                case Resource.Id.pop_shop_list_view:
                    intent = new Intent(view.Context, typeof(ViewShoppingListActivity));
                    intent.PutExtra("ListId", listId);
                    StartActivity(intent);
                    return true;
                case Resource.Id.pop_shop_list_edit:
                    //intent = new Intent(view.Context, typeof(EditGoalActivity));
                    //intent.PutExtra("ListId", listId.ToString());
                    //StartActivity(intent);
                    return true;
                case Resource.Id.pop_shop_list_delete:
                    dbh = new DBHelper();
                    AlertDialog.Builder builder = new AlertDialog.Builder(view.Context);
                    AlertDialog alert = builder.Create();
                    alert.SetTitle("Delete list");
                    alert.SetIcon(Android.Resource.Drawable.IcDialogAlert);
                    alert.SetMessage("Are you sure?");

                    alert.SetButton2("Yes", (s, ev) =>
                    {   //yes
                        string result = dbh.DeleteShoppingList(selListId);
                        if (result.Equals("ok"))
                        {
                            Toast.MakeText(view.Context, "List deleted!", ToastLength.Short).Show();
                            populateShoppingList(view);
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
                case Resource.Id.pop_shop_list_status:
                    Toast.MakeText(view.Context, "Clicked: " + "Update list status", ToastLength.Short).Show();
                    return true;
                default:
                    base.OnContextItemSelected(item);
                    return true;
            }
        }

        public override string ToString()
        {
            return "Shopping Lists";
        }

        public override void OnResume()
        {
            base.OnResume();
            populateShoppingList(view);
        }

        //populate list
        private async void populateShoppingList(View view)
        {
            lists = ShoppingCollection.GetShoppingLists();
            adapter = new ShoppingAdapter(view.Context, lists);
            mShopList.Adapter = adapter;
        }
    }
}