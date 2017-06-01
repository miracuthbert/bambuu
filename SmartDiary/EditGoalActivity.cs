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
using SmartDiary.Droid.Models;
using Android.Database;

namespace SmartDiary.Droid
{
    [Activity(Label = "Edit Goal", Icon = "@drawable/storelogo", Theme = "@style/MyTheme")]
    public class EditGoalActivity : AppCompatActivity
    {
        private int selGoalId;
        EditText txtGoal;
        EditText txtGoalDesc;
        TextView dteGoalStart;
        TextView dteGoalStartReset;
        TextView dteGoalDeadline;
        TextView dteGoalDeadlineReset;
        Spinner spinStatus;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.EditGoal);

            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetHomeButtonEnabled(true);

            //Get passed goal id
            selGoalId = Convert.ToInt32(Intent.Extras.Get("GoalId").ToString());

            //Get view elements
            txtGoal = FindViewById<EditText>(Resource.Id.txtGoal);
            txtGoalDesc = FindViewById<EditText>(Resource.Id.txtGoalDesc);
            dteGoalStart = FindViewById<TextView>(Resource.Id.dteGoalStarts);
            dteGoalStartReset = FindViewById<TextView>(Resource.Id.dteGoalStartsReset);
            dteGoalDeadline = FindViewById<TextView>(Resource.Id.dteGoalDeadline);
            dteGoalDeadlineReset = FindViewById<TextView>(Resource.Id.dteGoalDeadlineReset);
            spinStatus = FindViewById<Spinner>(Resource.Id.spinGoalStatus);

            //populate activity
            populateActivity(selGoalId);

            //on click event
            dteGoalStart.Click += DteGoalStart_Click;
            dteGoalDeadline.Click += DteGoalDeadline_Click;

            dteGoalStartReset.Click += delegate
            {        //Reset goal start
                resetGoalStart(selGoalId);
            };


            dteGoalDeadlineReset.Click += delegate
            {        //Reset goal deadline
                resetGoalDeadline(selGoalId);
            };
        }

        //populate activity
        public void populateActivity(int id)
        {
            DBHelper dbh = new DBHelper();
            string[] values = dbh.ReadGoal(id);
            txtGoal.Text = values[1];
            txtGoalDesc.Text = values[2];
            dteGoalStart.Text = values[3];
            dteGoalDeadline.Text = values[4];
            
            if (values[5].Equals("Pending"))
            {
                spinStatus.SetSelection(0);
            }
            else if(values[5].Equals("Completed"))
            {
                spinStatus.SetSelection(1);
            }
        }

        //reset goal start
        public void resetGoalStart(int id)
        {
            DBHelper dbh = new DBHelper();
            string[] values = dbh.ReadGoal(id);
            dteGoalStart.Text = values[3];
        }

        //reset goal start
        public void resetGoalDeadline(int id)
        {
            DBHelper dbh = new DBHelper();
            string[] values = dbh.ReadGoal(id);
            dteGoalDeadline.Text = values[4];
        }

        //picker goal deadline
        private void DteGoalDeadline_Click(object sender, EventArgs e)
        {
            DatePickerFragment dpf = new DatePickerFragment(DateTime.Parse(dteGoalDeadline.Text));

            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
            {
                dteGoalDeadline.Text = time.ToShortDateString();
            });
            frag.Show(FragmentManager, DatePickerFragment.TAG);
        }

        //picker goal start
        private void DteGoalStart_Click(object sender, EventArgs e)
        {
            DatePickerFragment dpf = new DatePickerFragment(DateTime.Parse(dteGoalStart.Text));

            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
            {
                dteGoalStart.Text = time.ToShortDateString();
            });
            frag.Show(FragmentManager, DatePickerFragment.TAG);
        }

        //Override On create menu
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.add_cancel_menu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        //Override On Options Selected
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.menu_new_add:
                    GoalUpdate();
                    return true;

                case Resource.Id.menu_new_cancel:
                    Finish();
                    return true;

                case Android.Resource.Id.Home:
                    Finish();
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        //Create Goal 
        private void GoalUpdate()
        {
            try
            {
                txtGoal = FindViewById<EditText>(Resource.Id.txtGoal);
                txtGoalDesc = FindViewById<EditText>(Resource.Id.txtGoalDesc);

                if (txtGoal.Text.Equals("") || txtGoalDesc.Text.Equals("") || dteGoalDeadline.Text.Equals(""))
                {
                    Toast.MakeText(this, "Fill in all fields!", ToastLength.Long).Show();
                    return;
                }
                else
                {
                    if (DateTime.Parse(dteGoalDeadline.Text) <= DateTime.Parse(dteGoalStart.Text))
                    {
                        Toast.MakeText(this, "Goal deadline should be greater than goal start!", ToastLength.Long).Show();
                        return;
                    }
                    if (DateTime.Parse(dteGoalDeadline.Text) <= DateTime.Today)
                    {
                        Toast.MakeText(this, "Goal deadline should be greater than current date!", ToastLength.Long).Show();
                        return;
                    }
                    else
                    {
                        DBHelper dbh = new DBHelper();

                        string goal = DatabaseUtils.SqlEscapeString(txtGoal.Text);
                        string goalDesc = DatabaseUtils.SqlEscapeString(txtGoalDesc.Text);
                        string goalStart = dteGoalStart.Text;
                        string goalDeadline = dteGoalDeadline.Text;
                        int stat = spinStatus.SelectedItemPosition;
                        string goalStatus = "Pending";

                        if (stat == 1)
                        {
                            goalStatus = "Completed";
                        }
                        else
                        {
                            goalStatus = "Pending";
                        }


                        string result = dbh.UpdateGoal(selGoalId, goal, goalDesc, goalStart, goalDeadline, goalStatus);

                        if (result.Equals("ok"))
                        {
                            Toast.MakeText(this, "Goal updated!", ToastLength.Short).Show();
                            Finish();
                        }
                        else
                        {
                            Toast.MakeText(this, result, ToastLength.Short).Show();
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