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
    class GoalsAdapter : BaseAdapter
    {
        private Context context;
        private JavaList<Goals> goals;
        private LayoutInflater inflater;
        private Goals goal;

        public GoalsAdapter(Context context, JavaList<Goals> goals)
        {
            this.context = context;
            this.goals = goals;
        }

        public override int Count
        {
            get
            {
                return goals.Size();
            }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return goals.Get(position);
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            if(inflater == null)
            {
                inflater = (LayoutInflater) context.GetSystemService(Context.LayoutInflaterService);
            }

            if(convertView == null)
            {
                convertView = inflater.Inflate(Resource.Layout.GoalItem, parent, false);
            }

            TextView myGoalId = convertView.FindViewById<TextView>(Resource.Id.GoalID);
            TextView myGoal = convertView.FindViewById<TextView>(Resource.Id.GoalName);
            TextView myGoalDeadline = convertView.FindViewById<TextView>(Resource.Id.GoalDeadline);
            TextView myGoalStatus = convertView.FindViewById<TextView>(Resource.Id.GoalStatus);
            ImageView myGoalStatusIcon = convertView.FindViewById<ImageView>(Resource.Id.GoalStatusIcon);
            TextView myGoalNotify = convertView.FindViewById<TextView>(Resource.Id.GoalNotification);

            goal = goals[position];

            myGoalId.Text = goal.Id.ToString();
            myGoal.Text = goal.Goal;
            myGoalDeadline.Text = goal.GoalDeadline;
            myGoalStatus.Text = goal.GoalStatus;
            myGoalStatusIcon.SetImageResource(Resource.Drawable.ic_hour_glass);

            int days = Convert.ToInt32(DateTime.Parse(goal.GoalDeadline).Subtract(DateTime.Today).TotalDays);

            if (goal.GoalStatus.Equals("Pending"))
            {
                myGoalStatus.SetTextColor(Android.Graphics.Color.Rgb(183, 28, 28));
                if (days < 0)
                {
                    myGoalNotify.Text = "Goal is "+ (days * -1) +" day(s) past deadline. Do something!";
                    myGoalNotify.SetTextColor(Android.Graphics.Color.Rgb(236, 64, 122));
                }
                else if (days == 0)
                {
                    myGoalNotify.Text = "Today is the goal's deadline. Do something!";
                    myGoalNotify.SetTextColor(Android.Graphics.Color.Rgb(240, 98, 146));
                }
                else if (days > 0 && days <= 14)
                {
                    myGoalNotify.Text = days + " day(s) left to goal deadline. Do something!";
                    myGoalNotify.SetTextColor(Android.Graphics.Color.Rgb(244, 143, 177));
                }
                else if (days > 14)
                {
                    myGoalNotify.Text = days + " days left to goal deadline";
                    myGoalNotify.SetTextColor(Android.Graphics.Color.Rgb(183, 28, 28));
                }
            }
            else if(goal.GoalStatus.Equals("Completed"))
            {
                myGoal.SetTextColor(Android.Graphics.Color.Gray);
                myGoalStatus.SetTextColor(Android.Graphics.Color.Green);
                myGoalStatusIcon.SetImageResource(Resource.Drawable.ic_checkmark);
                myGoalNotify.Text = "Goal completed at " + goal.GoalUpdated;
                myGoalDeadline.SetTextColor(Android.Graphics.Color.Gray);
            }

            return convertView;
        }
    }
}