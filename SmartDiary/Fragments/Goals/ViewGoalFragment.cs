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
    public class ViewGoalFragment : Fragment
    {
        private View view;
        private int selGoalId;
        TextView txtGoal;
        TextView txtGoalDesc;
        TextView txtDateAdded;
        TextView txtDateDeadline;
        TextView txtStatus;

        public override void OnCreate(Bundle savedInstanceState)
        {
            HasOptionsMenu = true;
            base.OnCreate(savedInstanceState);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            DBHelper dbh;
            string result = null;

            switch (item.ItemId)
            {
                case Resource.Id.menu_goalstatus:
                    dbh = new DBHelper();
                    string status = txtStatus.Text;                 //goal status
                    string newstatus = null;

                    if (status.Equals("Completed"))
                    {
                        newstatus = "Pending";
                    }
                    if (status.Equals("Pending"))
                    {
                        newstatus = "Completed";
                    }

                    result = dbh.UpdateGoalStatus(selGoalId, newstatus);

                    if (result.Equals("ok"))
                    {
                        populateAcitvity(selGoalId);
                        if (newstatus.Equals("Completed"))
                        {
                            var nMgr = (NotificationManager)Activity.GetSystemService(Context.NotificationService);

                            //Details of notification in previous recipe
                            Notification.Builder builder = new Notification.Builder(Context)
                            .SetAutoCancel(true)
                            .SetContentTitle("Hooray! Goal Achieved")
                            .SetContentText(txtGoal.Text + " is marked as completed.")
                            .SetSmallIcon(Resource.Drawable.ic_trophy);
                            
                            nMgr.Notify(0, builder.Build());
                        }

                        Toast.MakeText(view.Context, "Goal status updated!", ToastLength.Short).Show();
                    }
                    else
                    {
                        Toast.MakeText(view.Context, "Failed updating goal status!", ToastLength.Short).Show();
                    }
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }

        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            view = inflater.Inflate(Resource.Layout.ViewGoal, container, false);

            Activity MyActivity = Activity;
            selGoalId = Convert.ToInt32(MyActivity.Intent.Extras.Get("GoalId").ToString());

            txtGoal = view.FindViewById<TextView>(Resource.Id.ViewGoalName);
            txtGoalDesc = view.FindViewById<TextView>(Resource.Id.ViewGoalDescription);
            txtDateAdded = view.FindViewById<TextView>(Resource.Id.ViewGoalAdded);
            txtDateDeadline = view.FindViewById<TextView>(Resource.Id.ViewGoalDeadline);
            txtStatus = view.FindViewById<TextView>(Resource.Id.ViewGoalStatus);

            //populate activity
            populateAcitvity(selGoalId);

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
            populateAcitvity(selGoalId);
        }

        //populate activity
        private void populateAcitvity(int mygoal)
        {
            try
            {
                DBHelper dbh = new DBHelper();
                string[] result = dbh.ReadGoal(mygoal);

                txtGoal.Text = result[1];                           //goal
                txtGoalDesc.Text = result[2];                       //goal description
                txtDateAdded.Text = result[3];                      //goal start
                txtDateDeadline.Text = result[4];                   //goal deadline
                txtStatus.Text = result[5];                         //goal status
            }
            catch (Exception ex)
            {
                Toast.MakeText(view.Context, "Error: " + ex.Message, ToastLength.Long).Show();
            }
        }
    }
}