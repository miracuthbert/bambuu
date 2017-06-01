using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using Android.Support.V7.App;
using Android.Support.V4.Widget;
using System.Collections.Generic;
using Android.Support.V4.View;
using FragmentManager = Android.Support.V4.App.FragmentManager;
using Android.Support.Design.Widget;
using AlertDialog = Android.App.AlertDialog;
using SmartDiary.Droid.ViewModel;
using Java.Lang;
using SmartDiary.Droid.mServices;

namespace SmartDiary.Droid
{
    [Activity(Label = "@string/ApplicationName", Icon = "@drawable/app_icon", Theme = "@style/MainTheme")]
    public class MainActivity : AppCompatActivity
    {
        private RelativeLayout main_layout;
        private MyActionBarDrawerToggle mDrawerToggle;
        private DrawerLayout mDrawerLayout;
        private NavigationView mLeftDrawer;
        private ViewPager mViewPager;
        private SlidingTabScrollView mScrollView;
        private FragmentManager mFragManager;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            main_layout = FindViewById<RelativeLayout>(Resource.Id.main_layout);

            var mToolbar = FindViewById<SupportToolbar>(Resource.Id.toolbar);

            mDrawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);

            mLeftDrawer = FindViewById<NavigationView>(Resource.Id.left_drawer);
            mLeftDrawer.NavigationItemSelected += (sender, e) =>
            {
                e.MenuItem.SetChecked(true);

                Intent intent;                  //new intent

                switch (e.MenuItem.ItemId)
                {

                    case Resource.Id.menu_addGoal:
                        intent = new Intent(this, typeof(AddGoalActivity));
                        StartActivity(intent);
                        break;

                    case Resource.Id.menu_addGoalTask:
                        intent = new Intent(this, typeof(AddGoalTaskActivity));
                        StartActivity(intent);
                        break;

                    case Resource.Id.menu_addProject:
                        intent = new Intent(this, typeof(AddProjectActivity));
                        StartActivity(intent);
                        break;

                    case Resource.Id.menu_addProjectTask:
                        intent = new Intent(this, typeof(AddProjectTaskActivity));
                        StartActivity(intent);
                        break;

                    case Resource.Id.menu_addBudget:
                        Toast.MakeText(this, "Go: " + e.MenuItem.TitleFormatted, ToastLength.Short).Show();
                        break;

                    case Resource.Id.menu_addBudgetItem:
                        Toast.MakeText(this, "Go: " + e.MenuItem.TitleFormatted, ToastLength.Short).Show();
                        break;

                    case Resource.Id.menu_addShopList:
                        intent = new Intent(this, typeof(AddShoppingListActivity));
                        StartActivity(intent);
                        break;

                    case Resource.Id.menu_addShopItem:
                        intent = new Intent(this, typeof(AddShoppingItemActivity));
                        StartActivity(intent);
                        break;
                }

                mDrawerLayout.CloseDrawers();
            };

            SetSupportActionBar(mToolbar);

            mDrawerToggle = new MyActionBarDrawerToggle(
                this,                                   //host activity
                mDrawerLayout,                          //drawer layout
                Resource.String.openedDrawer,           //Opened message
                Resource.String.closedDrawer            //Close message
            );


            mDrawerLayout.SetDrawerListener(mDrawerToggle);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetHomeButtonEnabled(true);
            SupportActionBar.SetDisplayShowTitleEnabled(true);
            mDrawerToggle.SyncState();


            //if (bundle != null)
            //{

            //}
            //else
            //{
            //    SupportActionBar.SetTitle(Resource.String.closedDrawer);
            //}

            //FragmentTransaction transaction = FragmentManager.BeginTransaction();
            //SlidingTabsFragment fragment = new SlidingTabsFragment();
            //transaction.Replace(Resource.Id.main_content_fragment, fragment);
            //transaction.Commit();

            mScrollView = FindViewById<SlidingTabScrollView>(Resource.Id.sliding_tabs);
            mViewPager = FindViewById<ViewPager>(Resource.Id.viewpager);
            mViewPager.Adapter = new MainPageAdapter(SupportFragmentManager);
            mScrollView.ViewPager = mViewPager;

        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.home, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {

            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            AlertDialog mAlertDialog = builder.Create();

            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    mDrawerToggle.OnOptionsItemSelected(item);
                    return true;
                
                //delete goals
                case Resource.Id.menu_delete_goals:
                    mAlertDialog.SetTitle("Delete all goals");
                    mAlertDialog.SetIcon(Resource.Drawable.ic_bin);
                    mAlertDialog.SetMessage("Are you sure?");

                    //buttons
                    mAlertDialog.SetButton2("Yes", (s, ev) =>
                    {
                        DBHelper dbh = new DBHelper();

                        string result = dbh.DeleteAllGoals();

                        if (result.Equals("ok"))
                        {
                            Snackbar.Make(main_layout, "All goals deleted!", Snackbar.LengthShort).Show();
                        }
                        else
                        {
                            Snackbar.Make(main_layout, "Failed deleting goals!", Snackbar.LengthShort).SetAction("Ok", (v) => { }).Show();
                        }
                    });

                    mAlertDialog.SetButton("No", (s, ev) =>
                    {
                        Snackbar.Make(main_layout, "Action cancelled", Snackbar.LengthLong).SetAction("Ok", (v) => { }).Show();
                    });

                    mAlertDialog.Show();

                    return true;

                //delete projects
                case Resource.Id.menu_delete_projects:
                    mAlertDialog.SetTitle("Delete all projects");
                    mAlertDialog.SetIcon(Resource.Drawable.ic_bin);
                    mAlertDialog.SetMessage("Are you sure?");

                    //buttons
                    mAlertDialog.SetButton2("Yes", (s, ev) =>
                    {
                        Snackbar.Make(main_layout, "All projects deleted!", Snackbar.LengthLong).SetAction("Ok", (v) => { }).Show();
                    });

                    mAlertDialog.SetButton("No", (s, ev) =>
                    {
                        Snackbar.Make(main_layout, "Action cancelled", Snackbar.LengthLong).SetAction("Ok", (v) => { }).Show();
                    });

                    mAlertDialog.Show();

                    return true;

                //delete budgets
                case Resource.Id.menu_delete_budgets:
                    mAlertDialog.SetTitle("Delete all budgets");
                    mAlertDialog.SetIcon(Resource.Drawable.ic_bin);
                    mAlertDialog.SetMessage("Are you sure?");

                    //buttons
                    mAlertDialog.SetButton2("Yes", (s, ev) =>
                    {
                        Snackbar.Make(main_layout, "All budgets deleted!", Snackbar.LengthLong).SetAction("Ok", (v) => { }).Show();
                    });

                    mAlertDialog.SetButton("No", (s, ev) =>
                    {
                        Snackbar.Make(main_layout, "Action cancelled", Snackbar.LengthLong).SetAction("Ok", (v) => { }).Show();
                    });

                    mAlertDialog.Show();

                    return true;

                //delete shopping lists
                case Resource.Id.menu_delete_shop_lists:
                    mAlertDialog.SetTitle("Delete all shopping lists");
                    mAlertDialog.SetIcon(Resource.Drawable.ic_bin);
                    mAlertDialog.SetMessage("Are you sure?");

                    //buttons
                    mAlertDialog.SetButton2("Yes", (s, ev) =>
                    {
                        Snackbar.Make(main_layout, "All shopping lists deleted!", Snackbar.LengthLong).SetAction("Ok", (v) => { }).Show();
                    });

                    mAlertDialog.SetButton("No", (s, ev) =>
                    {
                        Snackbar.Make(main_layout, "Action cancelled", Snackbar.LengthLong).SetAction("Ok", (v) => { }).Show();
                    });

                    mAlertDialog.Show();

                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }
    }

    public class MainPageAdapter : Android.Support.V4.App.FragmentPagerAdapter
    {
        private List<Android.Support.V4.App.Fragment> mFragmentHolder;

        public MainPageAdapter(Android.Support.V4.App.FragmentManager fragManager) : base(fragManager)
        {
            mFragmentHolder = new List<Android.Support.V4.App.Fragment>();
            mFragmentHolder.Add(new ViewGoalsFragment());
            mFragmentHolder.Add(new ViewProjectsFragment());
            mFragmentHolder.Add(new ViewShoppingListsFragment());
            mFragmentHolder.Add(new ViewBudgetsFragment());
        }

        public override int Count
        {
            get
            {
               return mFragmentHolder.Count;
            }
        }

        public override Android.Support.V4.App.Fragment GetItem(int position)
        {
            return mFragmentHolder[position];
        }
    }
}

