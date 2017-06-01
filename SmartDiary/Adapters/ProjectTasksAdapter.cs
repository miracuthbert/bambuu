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
    class ProjectTasksAdapter : BaseAdapter
    {
        private Context context;
        private JavaList<ProjectTasks> tasks;
        private LayoutInflater inflater;
        private ProjectTasks task;

        public ProjectTasksAdapter(Context context, JavaList<ProjectTasks> tasks)
        {
            this.context = context;
            this.tasks = tasks;
        }

        public override int Count
        {
            get
            {
                return tasks.Size();
            }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return tasks.Get(position);
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
                convertView = inflater.Inflate(Resource.Layout.TaskItemMaster, parent, false);
            }

            LinearLayout myTaskCost = convertView.FindViewById<LinearLayout>(Resource.Id.TaskCostWrap);
            TextView myTaskId = convertView.FindViewById<TextView>(Resource.Id.TaskID);
            TextView myTask = convertView.FindViewById<TextView>(Resource.Id.TaskName);
            TextView myTaskStart = convertView.FindViewById<TextView>(Resource.Id.TaskStarts);
            TextView myTaskDeadline = convertView.FindViewById<TextView>(Resource.Id.TaskDeadline);
            TextView myTaskExpCost = convertView.FindViewById<TextView>(Resource.Id.TaskExpCost);
            TextView myTaskActCost = convertView.FindViewById<TextView>(Resource.Id.TaskActCost);
            TextView myTaskStatus = convertView.FindViewById<TextView>(Resource.Id.TaskStatus);
            TextView myTaskNotify = convertView.FindViewById<TextView>(Resource.Id.TaskNotification);

            task = tasks[position];

            myTaskId.Text = task.Id.ToString();
            myTask.Text = task.Task;
            myTaskStart.Text = task.TaskStart;
            myTaskDeadline.Text = task.TaskDeadline;
            myTaskExpCost.Text = task.ExpectedCost.ToString();
            myTaskStatus.Text = task.TaskStatus;


            myTaskCost.Visibility = ViewStates.Gone;


            if (!task.ActualCost.Equals(""))
            {
                myTaskActCost.Text = task.ActualCost.ToString();
            }

            int days = Convert.ToInt32(DateTime.Parse(task.TaskDeadline).Subtract(DateTime.Today).TotalDays);

            if (task.TaskStatus.Equals("Pending"))
            {
                myTaskStatus.SetTextColor(Android.Graphics.Color.Rgb(183, 28, 28));
                if (days < 0)
                {
                    myTaskNotify.SetBackgroundColor(Android.Graphics.Color.Rgb(183, 28, 28));
                    myTaskNotify.SetTextColor(Android.Graphics.Color.White);
                    myTaskNotify.Text = "Task is " + (days * -1) + " past deadline. Do something!";
                }
                if (days == 0)
                {
                    myTaskNotify.Text = "Today is the task's deadline. Do something!";
                    myTaskNotify.SetBackgroundColor(Android.Graphics.Color.Rgb(240, 98, 146));
                }
                else if (days > 0 && days <= 14)
                {
                    myTaskNotify.Text = days + " day(s) left to task deadline. Do something!";
                    myTaskNotify.SetBackgroundColor(Android.Graphics.Color.Rgb(244, 143, 177));
                }
                else if (days > 14)
                {
                    myTaskNotify.Text = days + " days left to goal deadline";
                }
            }
            if (task.TaskStatus.Equals("Completed"))
            {
                myTaskNotify.Visibility = ViewStates.Gone;
                myTaskDeadline.Visibility = ViewStates.Gone;
            }

            return convertView;
        }
    }
}