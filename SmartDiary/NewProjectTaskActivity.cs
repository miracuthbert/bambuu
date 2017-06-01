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
using SmartDiary.Droid.Views;
using SmartDiary.Droid.Models;
using Android.Database;

namespace SmartDiary.Droid
{
    [Activity(Label = "Add Task", Icon = "@drawable/storelogo", Theme = "@style/MyTheme")]
    public class NewProjectTaskActivity : AppCompatActivity
    {
        private Spinner project;
        private EditText task;
        private EditText taskDetails;
        private TextView taskStarts;
        private TextView taskDeadline;
        private TextView taskStartsReset;
        private TextView taskDeadlineReset;
        private EditText taskBudget;
        private int selProjectId;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.AddProjectTasks);

            //Get passed goal id
            selProjectId = Convert.ToInt32(Intent.Extras.Get("ProjectId").ToString());

            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetHomeButtonEnabled(true);

            project = FindViewById<Spinner>(Resource.Id.spinTaskProject);
            task = FindViewById<EditText>(Resource.Id.txtProjectTask);
            taskDetails = FindViewById<EditText>(Resource.Id.txtProjectTaskDetails);
            taskStarts = FindViewById<TextView>(Resource.Id.txtProjectTaskStarts);
            taskDeadline = FindViewById<TextView>(Resource.Id.txtProjectTaskDeadline);
            taskStartsReset = FindViewById<TextView>(Resource.Id.txtProjectTaskStartsReset);
            taskDeadlineReset = FindViewById<TextView>(Resource.Id.txtProjectTaskDeadlineReset);
            taskBudget = FindViewById<EditText>(Resource.Id.txtProjectTaskBudget);

            FindViewById<LinearLayout>(Resource.Id.task_spinner_wrapper).Visibility = ViewStates.Gone;

            //task start
            taskStarts.Text = DateTime.Now.ToShortDateString();
            taskStarts.Click += TaskStarts_Click;

            //reset task start
            taskStartsReset.Click += delegate
            {
                taskStarts.Text = DateTime.Now.ToShortDateString();
            };


            //task deadline
            taskDeadline.Text = DateTime.Now.ToShortDateString();
            taskDeadline.Click += TaskDeadline_Click;

            //reset task deadline
            taskDeadlineReset.Click += delegate
            {
                taskDeadline.Text = DateTime.Now.ToShortDateString();
            };
        }
        //task deadline click
        private void TaskDeadline_Click(object sender, EventArgs e)
        {
            DatePickerFragment dpf = new DatePickerFragment(DateTime.Now);

            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
            {
                taskDeadline.Text = time.ToShortDateString();
            });
            frag.Show(FragmentManager, DatePickerFragment.TAG);
        }

        //task start click
        private void TaskStarts_Click(object sender, EventArgs e)
        {
            DatePickerFragment dpf = new DatePickerFragment(DateTime.Now);

            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
            {
                taskStarts.Text = time.ToShortDateString();
            });
            frag.Show(FragmentManager, DatePickerFragment.TAG);
        }

        //create menu override
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.add_cancel_item_menu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        //menu item select override
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.menu_task_add:
                    TaskCreate();
                    return true;

                case Resource.Id.menu_task_cancel:
                    Finish();
                    return true;

                case Android.Resource.Id.Home:
                    Finish();
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);

            }
        }

        //Create Task 
        private void TaskCreate()
        {
            try
            {
                DBHelper dbh = new DBHelper();
                if (task.Text.Equals("") || taskDetails.Text.Equals("") || taskStarts.Text.Equals("") || taskDeadline.Text.Equals(""))
                {
                    Toast.MakeText(this, "Fill in all fields!", ToastLength.Long).Show();
                }
                else
                {
                    if (DateTime.Parse(taskDeadline.Text) <= DateTime.Parse(taskStarts.Text))
                    {
                        Toast.MakeText(this, "Task deadline should be greater than task start!", ToastLength.Long).Show();
                        return;
                    }
                    else if (DateTime.Parse(taskDeadline.Text) <= DateTime.Today)
                    {
                        Toast.MakeText(this, "Task deadline should be greater than current date!", ToastLength.Long).Show();
                    }
                    else
                    {
                        int mproject = selProjectId;
                        string mtask = DatabaseUtils.SqlEscapeString(task.Text);
                        string mtaskDesc = DatabaseUtils.SqlEscapeString(taskDetails.Text);
                        string mtaskStart = taskStarts.Text;
                        string mtaskDeadline = taskDeadline.Text;
                        decimal mtaskBudget = Convert.ToDecimal(taskBudget.Text);

                        string result = dbh.CreateProjectTask(mtask, mproject, mtaskDesc, mtaskStart, mtaskDeadline, mtaskBudget);

                        if (result.Equals("ok"))
                        {
                            Toast.MakeText(this, "Project task added!", ToastLength.Short).Show();
                            Finish();
                        }
                        else
                        {
                            Toast.MakeText(this, "Failed adding project task\n. Error info:" + result, ToastLength.Short).Show();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Toast.MakeText(this, "Error: " + ex.Message, ToastLength.Long);
            }
        }

    }
}