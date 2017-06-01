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
using Java.Lang;
using SmartDiary.Droid.Models;

namespace SmartDiary.Droid.ViewModel
{
    class ProjectsAdapter : BaseAdapter
    {
        private Context context;
        private JavaList<Projects> projects;
        private LayoutInflater inflater;
        private Projects project;

        public ProjectsAdapter(Context context, JavaList<Projects> projects)
        {
            this.context = context;
            this.projects = projects;
        }

        public override int Count
        {
            get
            {
                return projects.Size();
            }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return projects.Get(position);
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            if (inflater == null)
            {
                inflater = (LayoutInflater)context.GetSystemService(Context.LayoutInflaterService);
            }

            if (convertView == null)
            {
                convertView = inflater.Inflate(Resource.Layout.ProjectItem, parent, false);
            }

            TextView myProjectId = convertView.FindViewById<TextView>(Resource.Id.ProjectID);
            TextView myProject = convertView.FindViewById<TextView>(Resource.Id.ProjectName);
            TextView myProjectDeadline = convertView.FindViewById<TextView>(Resource.Id.ProjectDeadline);
            TextView myProjectBudget = convertView.FindViewById<TextView>(Resource.Id.ProjectBudget);
            TextView myProjectStatus = convertView.FindViewById<TextView>(Resource.Id.ProjectStatus);
            TextView myProjectNotify = convertView.FindViewById<TextView>(Resource.Id.ProjectNotification);

            project = projects[position];

            myProjectId.Text = project.Id.ToString();
            myProject.Text = project.Project;
            myProjectDeadline.Text = project.ProjectDeadline;
            myProjectBudget.Text = project.ProjectBudget.ToString();
            myProjectStatus.Text = project.ProjectStatus;

            int days = Convert.ToInt32(DateTime.Parse(project.ProjectDeadline).Subtract(DateTime.Today).TotalDays);

            myProjectNotify.Text = days + " days left to project deadline.";

            if (project.ProjectStatus.Equals("Pending"))
            {
                myProjectStatus.SetTextColor(Android.Graphics.Color.Rgb(183, 28, 28));
                if (days < 0)
                {
                    //convertView.SetBackgroundColor(Android.Graphics.Color.Rgb(236, 64, 122));
                    myProjectNotify.SetTextColor(Android.Graphics.Color.White);
                    myProjectNotify.SetBackgroundColor(Android.Graphics.Color.Rgb(183, 28, 28));
                    myProjectNotify.Text = "Project is " + (days * -1) + " days past deadline. Do something!";
                }
                else if (days == 0)
                {
                    //convertView.SetBackgroundColor(Android.Graphics.Color.Rgb(255, 82, 82));
                    myProjectNotify.SetBackgroundColor(Android.Graphics.Color.Rgb(240, 98, 146));
                    myProjectNotify.Text = "Today is the project deadline. Do something!";
                }
                else if (days > 0 && days <= 14)
                {
                    //convertView.SetBackgroundColor(Android.Graphics.Color.Rgb(239, 154, 154));
                    myProjectNotify.SetBackgroundColor(Android.Graphics.Color.Rgb(244, 143, 177));
                    myProjectNotify.Text = days + " days left to project deadline.";
                }
                else if (days > 14)
                {
                    myProjectNotify.Text = days + " days left to project deadline";
                }
            }
            if (project.ProjectStatus.Equals("Postponed"))
            {
                convertView.SetBackgroundColor(Android.Graphics.Color.Rgb(130, 177, 255));
                myProjectDeadline.Visibility = ViewStates.Gone;
                myProjectNotify.Visibility = ViewStates.Gone;
            }
            if (project.ProjectStatus.Equals("Completed"))
            {
                convertView.SetBackgroundColor(Android.Graphics.Color.Rgb(63, 81, 181));
                myProjectDeadline.Visibility = ViewStates.Gone;
                myProjectNotify.Visibility = ViewStates.Gone;
            }

            return convertView;
        }
    }
}