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
using Android.Support.Design.Widget;
using SmartDiary.Droid.Models;
using SupportFragment = Android.Support.V4.App.Fragment;
using AlertDialog = Android.App.AlertDialog;

namespace SmartDiary.Droid
{
    [Activity(Label = "Goal", Icon = "@drawable/storelogo", Theme = "@style/MyTheme")]
    public class ViewGoalActivity : AppCompatActivity
    {
        private int mGoalFrame;
        private int selGoalId;
        private JavaList<Goals> mGoals;
        private SupportFragment mCurrentFragment;
        private ViewGoalFragment mGoalFrag;
        private ViewGoalTasksFragment mGoalTasksFrag;
        private Stack<SupportFragment> mStackFrag;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.GoalMaster);

            //Get passed goal id
            selGoalId = Convert.ToInt32(Intent.Extras.Get("GoalId").ToString());

            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetHomeButtonEnabled(true);

            mGoalFrame = Resource.Id.goal_frame;

            mGoalFrag = new ViewGoalFragment();
            mGoalTasksFrag = new ViewGoalTasksFragment();

            mStackFrag = new Stack<SupportFragment>();

            var trans = SupportFragmentManager.BeginTransaction();

            trans.Add(mGoalFrame, mGoalTasksFrag, "Goal Tasks");
            trans.Hide(mGoalTasksFrag);

            trans.Add(mGoalFrame, mGoalFrag, "Goal Details");

            trans.Commit();

            mCurrentFragment = mGoalFrag;

        }

        //on navigated back
        //protected override void OnRestart()
        //{
        //    populateAcitvity(selGoalId);
        //    base.OnRestart();
        //}

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

        //on create options menu
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.view_goal, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        //on options menu item click
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            DBHelper dbh;
            Intent intent;

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

                case Resource.Id.menu_goal_tasks:
                    if (SupportFragmentManager.BackStackEntryCount > 0)
                    {
                        SupportFragmentManager.PopBackStack();
                        mCurrentFragment = mStackFrag.Pop();
                    }
                    else
                    {
                        ShowFragment(mGoalTasksFrag);
                    }
                    return true;

                case Resource.Id.menu_goaledit:     //goal edit
                    intent = new Intent(this, typeof(EditGoalActivity));
                    intent.PutExtra("GoalId", selGoalId.ToString());
                    StartActivity(intent);
                    return true;

                case Resource.Id.menu_goaldel:
                    dbh = new DBHelper();
                    AlertDialog.Builder builder = new AlertDialog.Builder(this);
                    AlertDialog alert = builder.Create();
                    alert.SetTitle("Delete goal");
                    alert.SetIcon(Android.Resource.Drawable.IcDialogAlert);
                    alert.SetMessage("Are you sure?");

                    alert.SetButton2("Yes", (s, ev) =>
                    {   //yes
                        string dresult = dbh.DeleteGoal(selGoalId);
                        if (dresult.Equals("ok"))
                        {
                            Toast.MakeText(this, "Goal deleted!", ToastLength.Short).Show();
                            Finish();
                        }
                        else
                        {
                            Toast.MakeText(this, "Deletion failed!", ToastLength.Short).Show();
                        }
                    });
                    alert.SetButton("No", (s, ev) =>
                    {   //no

                    });
                    alert.Show();
                    return true;

                case Resource.Id.menu_goal_task_add:    //add new goal task
                    intent = new Intent(this, typeof(NewGoalTaskActivity));
                    intent.PutExtra("GoalId", selGoalId);
                    StartActivity(intent);
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }
    }
}