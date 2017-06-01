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
    public class NewGoalTaskActivity : AppCompatActivity
    {
        private Spinner taskGoal;
        private EditText txtTask;
        private EditText txtTaskDesc;
        private TextView dteTaskStart;
        private TextView dteTaskStartReset;
        private TextView dteTaskDeadline;
        private TextView dteTaskDeadlineReset;
        private int selGoalId;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.AddGoalTasks);

            //Get passed goal id
            selGoalId = Convert.ToInt32(Intent.Extras.Get("GoalId").ToString());

            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetHomeButtonEnabled(true);

            txtTask = FindViewById<EditText>(Resource.Id.txtTask);
            txtTaskDesc = FindViewById<EditText>(Resource.Id.txtTaskDetails);
            taskGoal = FindViewById<Spinner>(Resource.Id.spinTaskGoal);

            FindViewById<LinearLayout>(Resource.Id.task_spinner_wrapper).Visibility = ViewStates.Gone;

            //task start
            dteTaskStart = FindViewById<TextView>(Resource.Id.AddGTStarts);
            dteTaskStart.Text = DateTime.Now.ToShortDateString();
            dteTaskStart.Click += DteTaskStart_Click;

            //task start reset
            dteTaskStartReset = FindViewById<TextView>(Resource.Id.AddGTStartsReset);
            dteTaskStartReset.Click += delegate
            {        //Reset goal start
                dteTaskStart.Text = DateTime.Now.ToShortDateString();
            };

            //task deadline
            dteTaskDeadline = FindViewById<TextView>(Resource.Id.AddGTDeadline);
            dteTaskDeadline.Text = DateTime.Now.ToShortDateString();
            dteTaskDeadline.Click += DteTaskDeadline_Click;

            //task deadline reset
            dteTaskDeadlineReset = FindViewById<TextView>(Resource.Id.AddGTDeadlineReset);
            dteTaskDeadlineReset.Click += delegate
            {        //Reset goal deadline
                dteTaskDeadline.Text = DateTime.Now.ToShortDateString();
            };

        }
        //picker task deadline
        private void DteTaskDeadline_Click(object sender, EventArgs e)
        {
            DatePickerFragment dpf = new DatePickerFragment(DateTime.Now);

            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
            {
                dteTaskDeadline.Text = time.ToShortDateString();
            });
            frag.Show(FragmentManager, DatePickerFragment.TAG);
        }

        //picker task start
        private void DteTaskStart_Click(object sender, EventArgs e)
        {
            DatePickerFragment dpf = new DatePickerFragment(DateTime.Now);

            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
            {
                dteTaskStart.Text = time.ToShortDateString();
            });
            frag.Show(FragmentManager, DatePickerFragment.TAG);
        }

        //Override On create menu
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.add_cancel_item_menu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        //Override On Options Selected
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

                //if (taskGoal.SelectedItemPosition < 0 )
                //{
                //    Toast.MakeText(this, "Select a goal!", ToastLength.Long).Show();
                //    return;
                //}
                if (txtTask.Text.Equals("") || txtTaskDesc.Text.Equals("") || dteTaskStart.Text.Equals("") || dteTaskDeadline.Text.Equals(""))
                {
                    Toast.MakeText(this, "Fill in all fields!", ToastLength.Long).Show();
                }
                else
                {
                    if (DateTime.Parse(dteTaskDeadline.Text) <= DateTime.Parse(dteTaskStart.Text))
                    {
                        Toast.MakeText(this, "Task deadline should be greater than task start!", ToastLength.Long).Show();
                        return;
                    }
                    else if (DateTime.Parse(dteTaskDeadline.Text) <= DateTime.Today)
                    {
                        Toast.MakeText(this, "Task deadline should be greater than current date!", ToastLength.Long).Show();
                    }
                    else
                    {
                        int goal = selGoalId;
                        string task = DatabaseUtils.SqlEscapeString(txtTask.Text);
                        string taskDesc = DatabaseUtils.SqlEscapeString(txtTaskDesc.Text);
                        string taskStart = dteTaskStart.Text;
                        string taskDeadline = dteTaskDeadline.Text;

                        string result = dbh.CreateGoalTask(task, goal, taskDesc, taskStart, taskDeadline);

                        if (result.Equals("ok"))
                        {
                            Toast.MakeText(this, "Goal task added!", ToastLength.Short).Show();
                            Finish();
                        }
                        else
                        {
                            Toast.MakeText(this, "Failed adding goal task\n. Error info:" + result, ToastLength.Short).Show();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Toast.MakeText(this, "Error:\n" + ex.Message, ToastLength.Long);
            }
        }

    }
}