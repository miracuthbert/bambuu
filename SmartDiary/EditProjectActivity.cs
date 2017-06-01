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
    [Activity(Label = "Edit Project", Icon = "@drawable/storelogo", Theme = "@style/MyTheme")]
    public class EditProjectActivity : AppCompatActivity
    {
        private int selProjectId;
        EditText project;
        EditText projectDesc;
        TextView projectStart;
        TextView projectDeadline;
        TextView projectStartReset;
        TextView projectDeadlineReset;
        EditText projectBudget;
        Spinner projectStatus;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.EditProject);

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
            projectStatus = FindViewById<Spinner>(Resource.Id.spinProjectStatus);

            //Get passed goal id
            selProjectId = Convert.ToInt32(Intent.Extras.Get("ProjectId").ToString());

            //populate activity
            populateAcitvity(selProjectId);

            //on click
            projectStart.Click += ProjectStart_Click;
            projectDeadline.Click += ProjectDeadline_Click;

            //reset on click
            projectStartReset.Click += delegate
            {
                resetProjectStart(selProjectId);
            };

            projectDeadlineReset.Click += delegate
            {
                resetProjectDeadline(selProjectId);
            };

        }

        //populate activity
        public void populateAcitvity(int id)
        {
            try
            {
                DBHelper dbh = new DBHelper();
                string[] values = dbh.ReadProject(id);
                project.Text = values[1];                                   //project
                projectDesc.Text = values[2];                               //project description
                projectStart.Text = values[3];                              //project start
                projectDeadline.Text = values[4];                           //project deadline
                projectBudget.Text = values[5];                             //project budget
                if (values[6].Equals("Completed"))
                {
                    projectStatus.SetSelection(2);
                }
                else if (values[6].Equals("Postponed"))
                {
                    projectStatus.SetSelection(1);
                }
                else
                {
                    projectStatus.SetSelection(0);
                }
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, "Error: " + ex, ToastLength.Long).Show();
            }
        }

        //reset goal start
        public void resetProjectStart(int id)
        {
            DBHelper dbh = new DBHelper();
            string[] values = dbh.ReadGoal(id);
            projectStart.Text = values[3];
        }

        //reset goal start
        public void resetProjectDeadline(int id)
        {
            DBHelper dbh = new DBHelper();
            string[] values = dbh.ReadGoal(id);
            projectDeadline.Text = values[4];
        }

        //on project deadline click
        private void ProjectDeadline_Click(object sender, EventArgs e)
        {
            DatePickerFragment dpf = new DatePickerFragment(DateTime.Parse(projectDeadline.Text));

            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
            {
                projectDeadline.Text = time.ToShortDateString();
            });
            frag.Show(FragmentManager, DatePickerFragment.TAG);
        }

        //on project start click
        private void ProjectStart_Click(object sender, EventArgs e)
        {
            DatePickerFragment dpf = new DatePickerFragment(DateTime.Parse(projectStart.Text));

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
                    ProjectUpdate();
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
        public void ProjectUpdate()
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
                        int stat = projectStatus.SelectedItemPosition;
                        string mprojectStatus = "Pending";

                        if(stat == 0)
                        {
                            mprojectStatus = "Pending";
                        }
                        if (stat == 1)
                        {
                            mprojectStatus = "Postponed";
                        }
                        if (stat == 2)
                        {
                            mprojectStatus = "Completed";
                        }

                        string result = dbh.UpdateProject(selProjectId, mproject, mprojectDesc, mprojectStart, mprojectDeadline, mprojectBudget, mprojectStatus);

                        if (result.Equals("ok"))
                        {
                            Toast.MakeText(this, "Project updated!", ToastLength.Short).Show();
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