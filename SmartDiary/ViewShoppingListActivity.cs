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
using Android.Support.V7.App;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using Android.Support.V4.View;
using SmartDiary.Droid.ViewModel;
using SupportFragment = Android.Support.V4.App.Fragment;

namespace SmartDiary.Droid
{
    [Activity(Label = "View List", Icon = "@drawable/storelogo", Theme = "@style/MyTheme")]
    public class ViewShoppingListActivity : AppCompatActivity
    {
        private int mShopFrame;
        private int selListId;
        private SupportFragment mCurrentFragment;
        private ViewShoppingListFragment mShopListFrag;
        private ViewShoppingItemsFragment mListItemsFrag;
        private ViewShoppingItemFragment mListItemFrag;
        private Stack<SupportFragment> mStackFrag;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.ShoppingMaster);

            //Get passed list id
            selListId = Convert.ToInt32(Intent.Extras.Get("ListId").ToString());

            var mToolbar = FindViewById<SupportToolbar>(Resource.Id.toolbar);

            SetSupportActionBar(mToolbar);

            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetHomeButtonEnabled(true);

            mShopFrame = Resource.Id.shoplist_frame;

            mShopListFrag = new ViewShoppingListFragment();
            mListItemsFrag = new ViewShoppingItemsFragment();
            mListItemFrag = new ViewShoppingItemFragment();

            mStackFrag = new Stack<SupportFragment>();

            var trans = SupportFragmentManager.BeginTransaction();

            trans.Add(mShopFrame, mListItemsFrag, "List Items");
            trans.Hide(mListItemsFrag);

            trans.Add(mShopFrame, mShopListFrag, "List Details");

            trans.Commit();

            mCurrentFragment = mShopListFrag;
        }

        public override void OnBackPressed()
        {
            if (SupportFragmentManager.BackStackEntryCount > 0)
            {
                SupportFragmentManager.PopBackStack();
                mCurrentFragment = mStackFrag.Pop();
            }
            else
            {
                base.OnBackPressed();
            }
        }

        private void ShowFragment(SupportFragment fragment)
        {
            var trans = SupportFragmentManager.BeginTransaction();

            trans.Hide(mCurrentFragment);
            trans.Show(fragment);
            trans.AddToBackStack(null);
            trans.Commit();

            mStackFrag.Push(mCurrentFragment);
            mCurrentFragment = fragment;
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.view_shop_list, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            Intent intent;
            DBHelper dbh;

            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    if (SupportFragmentManager.BackStackEntryCount > 0)
                    {
                        SupportFragmentManager.PopBackStack();
                        mCurrentFragment = mStackFrag.Pop();
                    }
                    else
                    {
                        Finish();
                    }
                    return true;

                case Resource.Id.menu_slist_edit:
                    intent = new Intent(this, typeof(EditShoppingListActivity));
                    intent.PutExtra("ListId", selListId.ToString());
                    StartActivity(intent);
                    return true;

                case Resource.Id.menu_slist_pend:
                    dbh = new DBHelper();
                    dbh.UpdateShoppingListStatus(selListId, "Pending");
                    return true;

                case Resource.Id.menu_slist_poned:
                    dbh = new DBHelper();
                    dbh.UpdateShoppingListStatus(selListId, "Postponed");
                    return true;

                case Resource.Id.menu_slist_done:
                    dbh = new DBHelper();
                    dbh.UpdateShoppingListStatus(selListId, "Completed");
                    return true;

                case Resource.Id.menu_slist_items:
                    if (SupportFragmentManager.BackStackEntryCount > 0)
                    {
                        SupportFragmentManager.PopBackStack();
                        mCurrentFragment = mStackFrag.Pop();
                    }
                    else
                    {
                        ShowFragment(mListItemsFrag);
                    }
                    return true;

                case Resource.Id.menu_sitem_add:
                    intent = new Intent(this, typeof(NewShoppingItemActivity));
                    intent.PutExtra("ListId", selListId.ToString());
                    StartActivity(intent);
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }

        }
    }
}