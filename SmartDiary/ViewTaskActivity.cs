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
using AlertDialog = Android.App.AlertDialog;

namespace SmartDiary.Droid
{
    [Activity(Label = "Task", Icon = "@drawable/storelogo", Theme = "@style/MyTheme")]
    public class ViewTaskActivity : AppCompatActivity
    {
        private TextView mTaskId;
        private TextView mTask;
        private TextView mTaskDetails;
        private TextView mTaskStart;
        private TextView mTaskDeadline;
        private TextView mTaskExpCost;
        private TextView mTaskActCost;
        private TextView mTaskStatus;
        private TextView mTaskNotify;
        private int taskId;
        private string taskType;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.ViewTask);

            //get passed id
            taskId = Convert.ToInt32(Intent.Extras.Get("TaskId").ToString());

            //get passed id
            taskType = Intent.Extras.Get("TaskType").ToString();

            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetHomeButtonEnabled(true);

            mTaskId = FindViewById<TextView>(Resource.Id.TaskID);
            mTask = FindViewById<TextView>(Resource.Id.TaskName);
            mTaskDetails = FindViewById<TextView>(Resource.Id.TaskDetails);
            mTaskStart = FindViewById<TextView>(Resource.Id.TaskStarts);
            mTaskDeadline = FindViewById<TextView>(Resource.Id.TaskDeadline);
            mTaskExpCost = FindViewById<TextView>(Resource.Id.TaskExpCost);
            mTaskActCost = FindViewById<TextView>(Resource.Id.TaskActCost);
            mTaskStatus = FindViewById<TextView>(Resource.Id.TaskStatus);
            mTaskNotify = FindViewById<TextView>(Resource.Id.TaskNotification);

            //populate activity
            populateView(taskId, taskType);

        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.view_task, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            DBHelper dbh;
            string status = "Pending";

            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    Finish();
                    return true;

                case Resource.Id.menu_task_done:
                    dbh = new DBHelper();
                    status = "Completed";

                    string sresult = dbh.UpdateGoalTaskStatus(taskId, status);
                    if (sresult.Equals("ok"))
                    {
                        Toast.MakeText(this, "Task status: " + status, ToastLength.Short).Show();
                        mTaskStatus.Text = status;
                    }
                    else
                    {
                        Toast.MakeText(this, "Failed updating task status!", ToastLength.Short).Show();
                    }
                    return true;

                case Resource.Id.menu_task_undo:
                    dbh = new DBHelper();
                    status = "Pending";

                    string uresult = dbh.UpdateGoalTaskStatus(taskId, status);
                    if (uresult.Equals("ok"))
                    {
                        Toast.MakeText(this, "Task status: " + status, ToastLength.Short).Show();
                        mTaskStatus.Text = status;
                    }
                    else
                    {
                        Toast.MakeText(this, "Failed updating task status!", ToastLength.Short).Show();
                    }
                    return true;

                case Resource.Id.menu_task_del:
                    dbh = new DBHelper();
                    AlertDialog.Builder builder = new AlertDialog.Builder(this);
                    AlertDialog alert = builder.Create();
                    alert.SetTitle("Delete task");
                    alert.SetIcon(Android.Resource.Drawable.IcDialogAlert);
                    alert.SetMessage("Are you sure?");

                    alert.SetButton2("Yes", (s, ev) =>
                    {   //yes
                        string dresult = dbh.DeleteGoalTask(taskId);
                        if (dresult.Equals("ok"))
                        {
                            Toast.MakeText(this, "Task deleted!", ToastLength.Short).Show();
                            Finish();
                        }
                        else
                        {
                            Toast.MakeText(this, "Task deletion failed!", ToastLength.Short).Show();
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

        //populate activity
        private void populateView(int tId, string type)
        {
            if (tId > 0)
            {
                if (type.Equals("Goal"))
                {
                    //if true try
                    try
                    {
                        DBHelper dbh = new DBHelper();
                        string[] result = dbh.ReadGoalTask(tId);

                        mTaskId.Text = result[0];
                        mTask.Text = result[1];                             //task
                        mTaskDetails.Text = result[2];                      //task description
                        mTaskStart.Text = result[3];                        //task start
                        mTaskDeadline.Text = result[4];                     //task deadline
                        mTaskStatus.Text = result[5];                       //task status

                        FindViewById<LinearLayout>(Resource.Id.TaskCostWrap).Visibility = ViewStates.Gone;
                        FindViewById<LinearLayout>(Resource.Id.TaskCostWrap2).Visibility = ViewStates.Gone;
                    }
                    catch (Exception ex)
                    {
                        Toast.MakeText(this, "Error: " + ex.Message, ToastLength.Long).Show();
                    } 
                }
                else if(type.Equals("Project"))
                {
                    //if true try
                    try
                    {
                        DBHelper dbh = new DBHelper();
                        string[] result = dbh.ReadProjectTask(tId);

                        mTaskId.Text = result[0];
                        mTask.Text = result[1];                             //task
                        mTaskDetails.Text = result[2];                      //task description
                        mTaskStart.Text = result[3];                        //task start
                        mTaskDeadline.Text = result[4];                     //task deadline
                        mTaskStatus.Text = result[5];                       //task status
                    }
                    catch (Exception ex)
                    {
                        Toast.MakeText(this, "Error: " + ex.Message, ToastLength.Long).Show();
                    }
                }
            }

        }
    }
}