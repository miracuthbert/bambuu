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
using Android.Support.V4.Widget;
using SupportFragment = Android.Support.V4.App.Fragment;
using SupportFragmentManager = Android.Support.V4.App.FragmentManager;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using SupportActionBar = Android.Support.V7.App.ActionBar;
using AlertDialog = Android.App.AlertDialog;
using Android.Support.V7.App;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V4.App;
using Java.Lang;
using Android.Util;
using SmartDiary.Droid.ViewModel;

namespace SmartDiary.Droid
{
    [Activity(Label = "@string/ApplicationName", Icon = "@drawable/icon", Theme = "@style/Theme.DesignDefault")]
    public class DashboardActivity : AppCompatActivity
    {
        private DrawerLayout mDrawerLayout;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.Dashboard);

            SupportToolbar toolBar = FindViewById<SupportToolbar>(Resource.Id.toolBar);
            SetSupportActionBar(toolBar);

            SupportActionBar ab = SupportActionBar;
            ab.SetHomeAsUpIndicator(Resource.Drawable.ic_action_menu);
            ab.SetDisplayHomeAsUpEnabled(true);

            mDrawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);

            NavigationView navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);

            if (navigationView != null)
            {
                SetUpDrawerContent(navigationView);
            }

            TabLayout tabs = FindViewById<TabLayout>(Resource.Id.tabs);

            ViewPager viewPager = FindViewById<ViewPager>(Resource.Id.viewpager);

            tabs.FillViewport = true;

            SetUpViewPager(viewPager);

            tabs.SetupWithViewPager(viewPager);

        }

        private void SetUpViewPager(ViewPager viewPager)
        {
            TabAdapter adapter = new TabAdapter(SupportFragmentManager);
            adapter.AddFragment(new ViewGoalsFragment(), "Goals");
            adapter.AddFragment(new ViewProjectsFragment(), "Projects");
            adapter.AddFragment(new ViewShoppingListsFragment(), "Shopping");
            adapter.AddFragment(new ViewBudgetsFragment(), "Budgets");

            viewPager.Adapter = adapter;
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
                    mDrawerLayout.OpenDrawer((int)GravityFlags.Left);
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
                            Snackbar.Make(mDrawerLayout, "All goals deleted!", Snackbar.LengthShort).Show();
                        }
                        else
                        {
                            Snackbar.Make(mDrawerLayout, result != null ? result : "Failed deleting goals!", Snackbar.LengthShort).SetAction("Ok", (v) => { }).Show();
                        }
                    });

                    mAlertDialog.SetButton("No", (s, ev) =>
                    {
                        Snackbar.Make(mDrawerLayout, "Action cancelled", Snackbar.LengthLong).SetAction("Ok", (v) => { }).Show();
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
                        Snackbar.Make(mDrawerLayout, "All projects deleted!", Snackbar.LengthLong).SetAction("Ok", (v) => { }).Show();
                    });

                    mAlertDialog.SetButton("No", (s, ev) =>
                    {
                        Snackbar.Make(mDrawerLayout, "Action cancelled", Snackbar.LengthLong).SetAction("Ok", (v) => { }).Show();
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
                        Snackbar.Make(mDrawerLayout, "All budgets deleted!", Snackbar.LengthLong).SetAction("Ok", (v) => { }).Show();
                    });

                    mAlertDialog.SetButton("No", (s, ev) =>
                    {
                        Snackbar.Make(mDrawerLayout, "Action cancelled", Snackbar.LengthLong).SetAction("Ok", (v) => { }).Show();
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
                        Snackbar.Make(mDrawerLayout, "All shopping lists deleted!", Snackbar.LengthLong).SetAction("Ok", (v) => { }).Show();
                    });

                    mAlertDialog.SetButton("No", (s, ev) =>
                    {
                        Snackbar.Make(mDrawerLayout, "Action cancelled", Snackbar.LengthLong).SetAction("Ok", (v) => { }).Show();
                    });

                    mAlertDialog.Show();

                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        private void SetUpDrawerContent(NavigationView navigationView)
        {
            navigationView.NavigationItemSelected += (object sender, NavigationView.NavigationItemSelectedEventArgs e) =>
            {
                e.MenuItem.SetChecked(true);

                Intent intent;

                switch (e.MenuItem.ItemId)
                {

                    case Resource.Id.menu_goPro:
                        intent = new Intent(this, typeof(DashboardProActivity));
                        StartActivity(intent);
                        break;

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
                        intent = new Intent(this, typeof(AddEditActivity));
                        intent.PutExtra("LoadView", Resource.Layout.AddBudget.ToString());
                        StartActivity(intent);
                        break;

                    case Resource.Id.menu_addBudgetItem:
                        intent = new Intent(this, typeof(AddEditActivity));
                        intent.PutExtra("LoadView", Resource.Layout.AddBudgetItem.ToString());
                        StartActivity(intent);
                        break;

                    case Resource.Id.menu_addShopList:
                        intent = new Intent(this, typeof(AddShoppingListActivity));
                        StartActivity(intent);
                        break;

                    case Resource.Id.menu_addShopItem:
                        intent = new Intent(this, typeof(AddShoppingItemActivity));
                        StartActivity(intent);
                        break;

                    case Resource.Id.menu_profile:
                        break;

                }

                mDrawerLayout.CloseDrawers();
            };
        }

        public class TabAdapter : FragmentPagerAdapter
        {

            public List<SupportFragment> Fragments { get; set; }
            public List<string> FragmentNames { get; set; }

            public TabAdapter(SupportFragmentManager sfm) : base(sfm)
            {
                Fragments = new List<SupportFragment>();
                FragmentNames = new List<string>();
            }

            public void AddFragment(SupportFragment fragment, string name)
            {
                Fragments.Add(fragment);
                FragmentNames.Add(name);
            }

            public override int Count
            {
                get
                {
                    return Fragments.Count;
                }
            }

            public override SupportFragment GetItem(int position)
            {
                return Fragments[position];
            }

            public override ICharSequence GetPageTitleFormatted(int position)
            {
                return new Java.Lang.String(FragmentNames[position]);
            }
        }
    }
}