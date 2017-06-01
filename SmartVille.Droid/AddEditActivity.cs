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
using SmartVille.Droid.Fragments;

namespace SmartVille.Droid
{
    [Activity(Label = "@string/ApplicationName", Icon = "@drawable/icon", Theme = "@style/Theme.DesignDefault")]
    public class AddEditActivity : AppCompatActivity
    {
        private int mFrameLayout;
        private SupportFragment mCurrentFragment;
        private AddGroupFragment mAddGroupFrag;
        private AddPropertyFragment mAddPropertyFrag;
        private AddTenantFragment mAddTenantFrag;
        private Stack<SupportFragment> mStackFrag;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.AddEdit);

            string mLoadView = Intent.Extras.Get("LoadView").ToString();

            SupportToolbar toolBar = FindViewById<SupportToolbar>(Resource.Id.toolBar);
            SetSupportActionBar(toolBar);

            SupportActionBar ab = SupportActionBar;
            ab.SetDisplayHomeAsUpEnabled(true);
            ab.SetHomeButtonEnabled(true);
            ab.SetDisplayShowTitleEnabled(false);

            //Get Fragment Holder
            mFrameLayout = Resource.Id.fragmentContainer;

            //instantiate fragments
            mAddGroupFrag = new AddGroupFragment();
            mAddPropertyFrag = new AddPropertyFragment();
            mAddTenantFrag = new AddTenantFragment();

            mStackFrag = new Stack<SupportFragment>();

            LoadView(mLoadView);
        }

        //on create options menu
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.add_edit_menu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        //on options menu item click
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
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

                case Resource.Id.menu_action_cancel:
                    Finish();
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        //Method to load fragment
        void LoadView(string loadView)
        {
            int mLoadView = Convert.ToInt32(loadView);

            var trans = SupportFragmentManager.BeginTransaction();

            switch (mLoadView)
            {
                case Resource.Layout.AddGroup:
                    trans.Add(mFrameLayout, mAddGroupFrag, "Add Group");
                    mCurrentFragment = mAddGroupFrag;
                    trans.Commit();
                    break;

                case Resource.Layout.AddProperty:
                    trans.Add(mFrameLayout, mAddPropertyFrag, "Add Property");
                    mCurrentFragment = mAddPropertyFrag;
                    trans.Commit();
                    break;

                case Resource.Layout.AddTenant:
                    trans.Add(mFrameLayout, mAddTenantFrag, "Add Tenant");
                    mCurrentFragment = mAddTenantFrag;
                    trans.Commit();
                    break;
            }
        }

        //On back pressed

        //public override void OnBackPressed()
        //{
        //    if (SupportFragmentManager.BackStackEntryCount > 0)
        //    {
        //        SupportFragmentManager.PopBackStack();
        //        mCurrentFragment = mStackFrag.Pop();
        //    }
        //    else
        //    {
        //        base.OnBackPressed();
        //    }
        //}

        //Method to handle fragment switch
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