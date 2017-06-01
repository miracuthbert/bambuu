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
using Toolbar = Android.Support.V7.Widget.Toolbar;
using SmartDiary.Droid.ViewModel;
using SupportFragment = Android.Support.V4.App.Fragment;
using SmartDiary.Droid.Models;
using AlertDialog = Android.App.AlertDialog;

namespace SmartDiary.Droid
{
    [Activity(Label = "Project", Icon = "@drawable/storelogo", Theme = "@style/MyTheme")]
    public class ViewProjectActivity : AppCompatActivity
    {
        private int mProjectFrame;
        private int selProjectId;
        private JavaList<Projects> mProjects;
        private SupportFragment mCurrentFragment;
        private ViewProjectFragment mProjectFrag;
        private ViewProjectTasksFragment mProjectTasksFrag;
        private Stack<SupportFragment> mStackFrag;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.ProjectMaster);

            //Get passed goal id
            selProjectId = Convert.ToInt32(Intent.Extras.Get("ProjectId").ToString());

            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetHomeButtonEnabled(true);

            mProjectFrame = Resource.Id.project_frame;

            mProjectFrag = new ViewProjectFragment();
            mProjectTasksFrag = new ViewProjectTasksFragment();

            mStackFrag = new Stack<SupportFragment>();

            var trans = SupportFragmentManager.BeginTransaction();

            trans.Add(mProjectFrame, mProjectTasksFrag, "Project Tasks");
            trans.Hide(mProjectTasksFrag);

            trans.Add(mProjectFrame, mProjectFrag, "Project Details");

            trans.Commit();

            mCurrentFragment = mProjectFrag;

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

        //on create options menu
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.view_project, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            string result = null;
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

                case Resource.Id.menu_projectedit:
                    intent = new Intent(this, typeof(EditProjectActivity));
                    intent.PutExtra("ProjectId", selProjectId.ToString());
                    StartActivity(intent);
                    return true;

                case Resource.Id.menu_projectdel:
                    dbh = new DBHelper();
                    AlertDialog.Builder builder = new AlertDialog.Builder(this);
                    AlertDialog alert = builder.Create();
                    alert.SetTitle("Delete project");
                    alert.SetIcon(Android.Resource.Drawable.IcDialogAlert);
                    alert.SetMessage("Are you sure?");

                    alert.SetButton2("Yes", (s, ev) =>
                    {   //yes
                        string dresult = dbh.DeleteProject(selProjectId);
                        if (dresult.Equals("ok"))
                        {
                            Toast.MakeText(this, "Project deleted!", ToastLength.Short).Show();
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

                case Resource.Id.menu_project_tasks:
                    if (SupportFragmentManager.BackStackEntryCount > 0)
                    {
                        SupportFragmentManager.PopBackStack();
                        mCurrentFragment = mStackFrag.Pop();
                    }
                    else
                    {
                        ShowFragment(mProjectTasksFrag);
                    }
                    return true;

                case Resource.Id.menu_prj_task_add:
                    intent = new Intent(this, typeof(NewProjectTaskActivity));
                    intent.PutExtra("ProjectId", selProjectId.ToString());
                    StartActivity(intent);
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

    }
}