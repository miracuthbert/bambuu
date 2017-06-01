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
    [Activity(Label = "Add Project", Icon = "@drawable/storelogo", Theme ="@style/MyTheme")]
    public class AddProjectActivity : AppCompatActivity
    {
        private EditText project;
        private EditText projectDesc;
        private TextView projectStart;
        private TextView projectDeadline;
        private TextView projectStartReset;
        private TextView projectDeadlineReset;
        private EditText projectBudget;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.AddProject);

            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetHomeButtonEnabled(true);

            project = FindViewById<EditText>(Resource.Id.txtProject);
            projectDesc = FindViewById<EditText>(Resource.Id.txtProjectDescription);
            projectStart = FindViewById<TextView>(Resource.Id.dteProjectStarts);
            projectDeadline = FindViewById<TextView>(Resource.Id.dteProjectDeadline);
            projectStartReset = FindViewById<TextView>(Resource.Id.dteProjectStartsReset);
            projectDeadlineReset = FindViewById<TextView>(Resource.Id.dteProjectDeadlineReset);
            projectBudget = FindViewById<EditText>(Resource.Id.txtProjectBudget);

            projectStart.Text = DateTime.Now.ToShortDateString();
            projectDeadline.Text = DateTime.Now.ToShortDateString();

            //on click
            projectStart.Click += ProjectStart_Click;
            projectDeadline.Click += ProjectDeadline_Click;

            //reset on click
            projectStartReset.Click += delegate
            {
                projectStart.Text = DateTime.Now.ToShortDateString();
            };

            projectDeadlineReset.Click += delegate
            {
                projectDeadline.Text = DateTime.Now.ToShortDateString();
            };

        }

        //on project deadline click
        private void ProjectDeadline_Click(object sender, EventArgs e)
        {
            DatePickerFragment dpf = new DatePickerFragment(DateTime.Now);

            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
            {
                projectDeadline.Text = time.ToShortDateString();
            });
            frag.Show(FragmentManager, DatePickerFragment.TAG);
        }

        //on project start click
        private void ProjectStart_Click(object sender, EventArgs e)
        {
            DatePickerFragment dpf = new DatePickerFragment(DateTime.Now);

            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
            {
                projectStart.Text = time.ToShortDateString();
            });
            frag.Show(FragmentManager, DatePickerFragment.TAG);
        }

        //overide on create menu
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.add_cancel_menu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        //on menu opion item select
        public override bool OnOptionsItemSelected(IMenuItem item)
        {

            switch (item.ItemId)
            {
                case Resource.Id.menu_new_add:
                    ProjectCreate();
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

        //Create Project
        public void ProjectCreate()
        {
            try
            {
                if (project.Text.Equals("") || projectDesc.Text.Equals("") || projectDeadline.Text.Equals(""))
                {
                    Toast.MakeText(this, "Fill in all fields!", ToastLength.Long).Show();
                    return;
                }
                else
                {
                    if (DateTime.Parse(projectDeadline.Text) <= DateTime.Parse(projectStart.Text))
                    {
                        Toast.MakeText(this, "Project deadline should be greater than project start date!", ToastLength.Long).Show();
                        return;
                    }
                    if (DateTime.Parse(projectDeadline.Text) <= DateTime.Today)
                    {
                        Toast.MakeText(this, "Project deadline should be greater than current date!", ToastLength.Long).Show();
                        return;
                    }
                    else
                    {
                        DBHelper dbh = new DBHelper();

                        string mproject = DatabaseUtils.SqlEscapeString(project.Text);
                        string mprojectDesc = DatabaseUtils.SqlEscapeString(projectDesc.Text);
                        string mprojectStart = projectStart.Text;
                        string mprojectDeadline = projectDeadline.Text;
                        decimal mprojectBudget = Convert.ToDecimal(projectBudget.Text);

                        string result = dbh.CreateProject(mproject, mprojectDesc, mprojectStart, mprojectDeadline, mprojectBudget);

                        if (result.Equals("ok"))
                        {
                            Toast.MakeText(this, "Project added!", ToastLength.Short).Show();
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
                Toast.MakeText(this, "" + ex.Message, ToastLength.Long).Show();
            }

        }
    }
}