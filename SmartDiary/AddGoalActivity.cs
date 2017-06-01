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
using Android.Database;

namespace SmartDiary.Droid
{
    [Activity(Label = "Add Goal", Icon = "@drawable/storelogo", Theme = "@style/MyTheme")]
    public class AddGoalActivity : AppCompatActivity
    {
        EditText txtGoal;
        EditText txtGoalDesc;
        TextView dteGoalStart;
        TextView dteGoalStartReset;
        TextView dteGoalDeadline;
        TextView dteGoalDeadlineReset;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.AddGoal);

            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetHomeButtonEnabled(true);

            dteGoalStart = FindViewById<TextView>(Resource.Id.dteGoalStarts);
            dteGoalStart.Text = DateTime.Now.ToShortDateString();
            dteGoalStart.Click += DteGoalStart_Click;

            dteGoalStartReset = FindViewById<TextView>(Resource.Id.dteGoalStartsReset);
            dteGoalStartReset.Click += delegate
            {        //Reset goal start
                dteGoalStart.Text = DateTime.Now.ToShortDateString();
            };

            dteGoalDeadline = FindViewById<TextView>(Resource.Id.dteGoalDeadline);
            dteGoalDeadline.Text = DateTime.Now.ToShortDateString();
            dteGoalDeadline.Click += DteGoalDeadline_Click;

            dteGoalDeadlineReset = FindViewById<TextView>(Resource.Id.dteGoalDeadlineReset);
            dteGoalDeadlineReset.Click += delegate
            {        //Reset goal deadline
                dteGoalDeadline.Text = DateTime.Now.ToShortDateString();
            };

        }

        //picker goal deadline
        private void DteGoalDeadline_Click(object sender, EventArgs e)
        {
            DatePickerFragment dpf = new DatePickerFragment(DateTime.Now);

            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
            {
                dteGoalDeadline.Text = time.ToShortDateString();
            });
            frag.Show(FragmentManager, DatePickerFragment.TAG);
        }

        //picker goal start
        private void DteGoalStart_Click(object sender, EventArgs e)
        {
            DatePickerFragment dpf = new DatePickerFragment(DateTime.Now);

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
                    GoalCreate();
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
        private void GoalCreate()
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

                        string result = dbh.CreateGoal(goal, goalDesc, goalStart, goalDeadline);

                        if (result.Equals("ok"))
                        {
                            Toast.MakeText(this, "Goal added!", ToastLength.Short).Show();
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