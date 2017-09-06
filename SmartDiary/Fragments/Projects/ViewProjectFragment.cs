using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using SmartDiary.Droid.ViewModel;
using Fragment = Android.Support.V4.App.Fragment;

namespace SmartDiary.Droid
{
    public class ViewProjectFragment : Fragment
    {
        private View view;
        private int selProjectId;
        private TextView txtProject;
        private TextView txtProjectDesc;
        private TextView txtDateStart;
        private TextView txtDateDeadline;
        private TextView txtEstBudget;
        private TextView txtActBudget;
        private TextView txtStatus;
        public Activity MyActivity;

        public override void OnCreate(Bundle savedInstanceState)
        {
            HasOptionsMenu = true;
            base.OnCreate(savedInstanceState);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            string result = null;
            DBHelper dbh;

            switch (item.ItemId)
            {
                case Resource.Id.menu_projectstatus:
                    dbh = new DBHelper();
                    string status = txtStatus.Text;                     //project status
                    string newstatus = null;

                    if (status.Equals("Completed"))
                    {
                        newstatus = "Pending";
                    }
                    if (status.Equals("Pending"))
                    {
                        newstatus = "Completed";
                    }

                    result = dbh.UpdateProjectStatus(selProjectId, newstatus);

                    if (result.Equals("ok"))
                    {
                        Toast.MakeText(view.Context, "Project status updated!", ToastLength.Short).Show();
                        populateAcitvity(this.selProjectId);
                    }
                    else
                    {
                        Toast.MakeText(view.Context, "Failed updating status!", ToastLength.Short).Show();
                    }
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            view = inflater.Inflate(Resource.Layout.ViewProject, container, false);

            //passed goal id
            MyActivity = this.Activity;
            selProjectId = Convert.ToInt32(MyActivity.Intent.Extras.Get("ProjectId"));

            txtProject = view.FindViewById<TextView>(Resource.Id.ViewProjectName);
            txtProjectDesc = view.FindViewById<TextView>(Resource.Id.ViewProjectDescription);
            txtDateStart = view.FindViewById<TextView>(Resource.Id.ViewProjectAdded);
            txtDateDeadline = view.FindViewById<TextView>(Resource.Id.ViewProjectDeadline);
            txtEstBudget = view.FindViewById<TextView>(Resource.Id.ViewProjectEstBudget);
            txtActBudget = view.FindViewById<TextView>(Resource.Id.ViewProjectActBudget);
            txtStatus = view.FindViewById<TextView>(Resource.Id.ViewProjectStatus);

            //populate activity
            populateAcitvity(selProjectId);

            return view;
        }

        //title
        public override string ToString()
        {
            return "Details:";
        }

        public override void OnResume()
        {
            base.OnResume();
            populateAcitvity(selProjectId);
        }

        //populate activity
        private async void populateAcitvity(int myproject)
        {
            try
            {
                DBHelper dbh = new DBHelper();
                string[] result = dbh.ReadProject(myproject);

                txtProject.Text = result[1];                            //project
                txtProjectDesc.Text = result[2];                        //project description
                txtDateStart.Text = result[3];                          //project start
                txtDateDeadline.Text = result[4];                       //project deadline
                txtEstBudget.Text = result[5];                          //project deadline
                txtStatus.Text = result[6];                             //project status
            }
            catch (Exception ex)
            {
                Toast.MakeText(view.Context, "Error: " + ex.Message, ToastLength.Long).Show();
            }
        }
    }
}