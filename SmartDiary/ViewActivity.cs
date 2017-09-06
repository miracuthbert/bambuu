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
using SupportActionBar = Android.Support.V7.App.ActionBar;
using SupportFragment = Android.Support.V4.App.Fragment;
using SmartDiary.Droid.Fragments;

namespace SmartDiary.Droid
{
    [Activity(Label = "@string/ApplicationName", Icon = "@drawable/icon", Theme = "@style/Theme.DesignDefault")]
    public class ViewActivity : AppCompatActivity
    {
        private int mFrameLayout;
        private int passedId;
        private TextView mFragTitle;
        private SupportFragment mCurrentFragment;
        private ViewShoppingItemFragment mViewShoppingItemFrag;
        private Stack<SupportFragment> mStackFrag;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.AddEdit);

            //catch passed layout
            string mLoadView = Intent.Extras.Get("LoadView").ToString();

            //catch passed if
            passedId = Convert.ToInt32(Intent.Extras.Get("PassedId").ToString());

            SupportToolbar toolBar = FindViewById<SupportToolbar>(Resource.Id.toolBar);
            SetSupportActionBar(toolBar);

            SupportActionBar ab = SupportActionBar;
            ab.SetDisplayHomeAsUpEnabled(true);
            ab.SetHomeButtonEnabled(true);
            ab.SetDisplayShowTitleEnabled(true);
            ab.SetTitle(Resource.String.viewActivityTitle);

            //Get Fragment Title Holder
            mFragTitle = FindViewById<TextView>(Resource.Id.textFragmentTitle);

            //Get Fragment Holder
            mFrameLayout = Resource.Id.fragmentContainer;

            //instantiate fragments
            mViewShoppingItemFrag = new ViewShoppingItemFragment();

            mStackFrag = new Stack<SupportFragment>();

            LoadView(mLoadView);
        }

        //on create options menu
        //public override bool OnCreateOptionsMenu(IMenu menu)
        //{
        //    MenuInflater.Inflate(Resource.Menu.add_cancel_menu, menu);
        //    return base.OnCreateOptionsMenu(menu);
        //}

        //on options menu item click
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    //    if (SupportFragmentManager.BackStackEntryCount > 0)
                    //    {
                    //        SupportFragmentManager.PopBackStack();
                    //        mCurrentFragment = mStackFrag.Pop();
                    //    }
                    //    else
                    //    {
                    Finish();
                    //}
                    return true;

                case Resource.Id.menu_new_cancel:
                    Finish();
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        /// <summary>
        /// Method to load fragment
        /// </summary>
        /// <param name="loadView"></param>
        void LoadView(string loadView)
        {
            int mLoadView = Convert.ToInt32(loadView);

            var trans = SupportFragmentManager.BeginTransaction();

            switch (mLoadView)
            {
                case Resource.Layout.ViewShoppingItem:
                    mFragTitle.Text = "Shopping Item";
                    trans.Add(mFrameLayout, mViewShoppingItemFrag, "Shopping Item");
                    mCurrentFragment = mViewShoppingItemFrag;
                    trans.Commit();
                    break;
            }
        }

        /// <summary>
        /// On back pressed
        /// </summary>
        public override void OnBackPressed()
        {
            //if (SupportFragmentManager.BackStackEntryCount > 0)
            //{
            //    SupportFragmentManager.PopBackStack();
            //    mCurrentFragment = mStackFrag.Pop();
            //}
            //else
            //{
                base.OnBackPressed();
            //}
        }

        /// <summary>
        /// Method to handle fragment switch
        /// </summary>
        /// <param name="fragment"></param>
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
    }
}