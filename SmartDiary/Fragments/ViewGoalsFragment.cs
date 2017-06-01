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
using Fragment = Android.Support.V4.App.Fragment;
using Android.Support.Design.Widget;
using SmartDiary.Droid.ViewModel;
using Android.Database;
using SmartDiary.Droid.Models;
using SmartDiary.Droid.Views;
using Android.Support.V7.Widget;
using Android.Content.Res;

namespace SmartDiary.Droid
{
    public class ViewGoalsFragment : Fragment
    {
        private ListView mListGoals;
        private RecyclerView mRecyclerView;
        private RecyclerView.LayoutManager mLayoutManager;
        GoalsRecyclerAdapter mGoalsRecyclerAdapter;
        private RecyclerView view;
        private GoalsAdapter adapter;
        private JavaList<Goals> goals;
        private int selGoal;

        public override void OnCreate(Bundle savedInstanceState)
        {
            HasOptionsMenu = true;
            base.OnCreate(savedInstanceState);
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            menu.SetGroupVisible(Resource.Id.grp_goals_frag, true);
            base.OnCreateOptionsMenu(menu, inflater);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            view = inflater.Inflate(Resource.Layout.ViewGoals, container, false) as RecyclerView;

            FloatingActionButton fab = Activity.FindViewById<FloatingActionButton>(Resource.Id.fab);

            fab.Click += (o, e) =>
            {
                Intent intent = new Intent(view.Context, typeof(AddGoalActivity));
                StartActivity(intent);
            };

            SetUpRecyclerView(view);

            //mListGoals = view.FindViewById<ListView>(Resource.Id.listAllGoals);

            //mRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.recycle_goals);

            //populate goal list
            //populateGoalList(view);

            //click
            //mListGoals.ItemClick += MListGoals_ItemClick;

            //context menu
            //mListGoals.ContextMenuCreated += MListGoals_ContextMenuCreated;

            return view;

        }

        private void SetUpRecyclerView(RecyclerView view)
        {
            //Create recycler layout manager
            mLayoutManager = new LinearLayoutManager(view.Context);

            //Set recycler layout manager
            view.SetLayoutManager(mLayoutManager);

            //Get adapter
            mGoalsRecyclerAdapter = new GoalsRecyclerAdapter(GoalsCollection.GetGoals(), view);

            //on click event
            mGoalsRecyclerAdapter.ItemClick += MGoalsRecyclerAdapter_ItemClick;

            //Set adapter
            view.SetAdapter(mGoalsRecyclerAdapter);

        }

        void MGoalsRecyclerAdapter_ItemClick(object sender, int position)
        {
            string goalId = mGoalsRecyclerAdapter.GetItemId(position).ToString();

            Intent intent = new Intent(view.Context, typeof(ViewGoalActivity));
            intent.PutExtra("GoalId", goalId);
            StartActivity(intent);
        }

        //private void MListGoals_ContextMenuCreated(object sender, View.CreateContextMenuEventArgs e)
        //{
        //    e.Menu.SetHeaderTitle("Goal options:");
        //    MenuInflater inflater = new MenuInflater(mListGoals.Context);
        //    inflater.Inflate(Resource.Menu.goals_popup, e.Menu);
        //}

        //public override bool OnContextItemSelected(IMenuItem item)
        //{
        //    string goalId = selGoal.ToString();
        //    DBHelper dbh;
        //    Intent intent;

        //    switch (item.ItemId)
        //    {
        //        case Resource.Id.pop_goal_view:
        //            intent = new Intent(view.Context, typeof(ViewGoalActivity));
        //            intent.PutExtra("GoalId", goalId);
        //            StartActivity(intent);
        //            return true;
        //        case Resource.Id.pop_goal_edit:
        //            intent = new Intent(view.Context, typeof(EditGoalActivity));
        //            intent.PutExtra("GoalId", goalId.ToString());
        //            StartActivity(intent);
        //            return true;
        //        case Resource.Id.pop_goal_delete:
        //            dbh = new DBHelper();
        //            AlertDialog.Builder builder = new AlertDialog.Builder(view.Context);
        //            AlertDialog alert = builder.Create();
        //            alert.SetTitle("Delete goal");
        //            alert.SetIcon(Android.Resource.Drawable.IcDialogAlert);
        //            alert.SetMessage("Are you sure?");

        //            alert.SetButton2("Yes", (s, ev) =>
        //            {   //yes
        //                string result = dbh.DeleteGoal(selGoal);
        //                if (result.Equals("ok"))
        //                {
        //                    Toast.MakeText(view.Context, "Goal deleted!", ToastLength.Short).Show();
        //                    //populateGoalList(view);
        //                    //mGoalsRecyclerAdapter.NotifyDataSetChanged();
        //                }
        //                else
        //                {
        //                    Toast.MakeText(view.Context, "Delletion failed!", ToastLength.Short).Show();
        //                }
        //            });
        //            alert.SetButton("No", (s, ev) =>
        //            {   //no

        //            });
        //            alert.Show();
        //            return true;
        //        case Resource.Id.pop_goal_status:
        //            Toast.MakeText(view.Context, "Clicked: " + "Update goal status", ToastLength.Short).Show();
        //            return true;
        //        default:
        //            base.OnContextItemSelected(item);
        //            return true;
        //    }
        //}

        ////goal list item click
        //private void MListGoals_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        //{

        //    this.selGoal = Convert.ToInt32(goals[e.Position].Id);

        //    Intent intent = new Intent(view.Context, typeof(ViewGoalActivity));
        //    intent.PutExtra("GoalId", selGoal);
        //    StartActivity(intent);

        //    //mListGoals.ShowContextMenu();
        //}

        public override string ToString()
        {
            return "Goals";
        }

        //public override void OnResume()
        //{
        //    base.OnResume();
        //    //mGoalsRecyclerAdapter.NotifyDataSetChanged();
        //    //populateGoalList(view);
        //}

        //populate list
        //private async void populateGoalList(View view)
        //{
        //    goals = GoalsCollection.GetGoals();
        //    adapter = new GoalsAdapter(view.Context, goals);
        //    mListGoals.Adapter = adapter;
        //}

    }

    public class GoalsRecyclerAdapter : RecyclerView.Adapter
    {
        // Event handler for item clicks:
        public event EventHandler<int> ItemClick;

        private Context context;
        private JavaList<Goals> mGoals;
        private LayoutInflater inflater;
        private Goals goal;
        private RecyclerView mRecyclerView;

        public GoalsRecyclerAdapter(JavaList<Goals> goals, RecyclerView recyclerView)
        {
            mGoals = goals;
            mRecyclerView = recyclerView;
        }

        public override int ItemCount
        {
            get
            {
                return mGoals.Size();
            }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            try
            {
                GoalView mHolder = holder as GoalView;

                int days = Convert.ToInt32(DateTime.Parse(mGoals[position].GoalDeadline).Subtract(DateTime.Today).TotalDays);

                if (mGoals[position].GoalStatus.Equals("Pending"))
                {
                    //mHolder.myGoalStatus.SetTextColor(Android.Graphics.Color.Rgb(183, 28, 28));
                    mHolder.myGoalStatus.SetTextColor(Android.Graphics.Color.White);
                    if (days < 0)
                    {
                        mHolder.mView.SetBackgroundColor(Android.Graphics.Color.Rgb(183, 28, 28));
                        mHolder.myGoalNotify.Text = "Goal is " + (days * -1) + " day(s) past deadline. Do something!";
                        mHolder.myGoalNotify.SetBackgroundColor(Android.Graphics.Color.Rgb(236, 64, 122));
                    }
                    else if (days == 0)
                    {
                        mHolder.mView.SetBackgroundColor(Android.Graphics.Color.Rgb(255, 82, 82));
                        mHolder.myGoalNotify.Text = "Today is the goal's deadline. Do something!";
                        mHolder.myGoalNotify.SetBackgroundColor(Android.Graphics.Color.Rgb(240, 98, 146));
                    }
                    else if (days > 0 && days <= 14)
                    {
                        mHolder.mView.SetBackgroundColor(Android.Graphics.Color.Rgb(239, 154, 154));
                        mHolder.myGoalNotify.Text = days + " day(s) left to goal deadline. Do something!";
                        mHolder.myGoalNotify.SetBackgroundColor(Android.Graphics.Color.Rgb(244, 143, 177));
                    }
                    else if (days > 14)
                    {
                        mHolder.myGoalStatus.SetTextColor(Android.Graphics.Color.Rgb(183, 28, 28));
                        mHolder.myGoalNotify.Text = days + " days left to goal deadline";
                    }
                }
                else if (goal.GoalStatus.Equals("Completed"))
                {
                    mHolder.mView.SetBackgroundColor(Android.Graphics.Color.Rgb(46, 125, 50));
                    mHolder.myGoal.SetTextColor(Android.Graphics.Color.White);
                    mHolder.myGoalStatus.SetTextColor(Android.Graphics.Color.White);
                    mHolder.myGoalNotify.Visibility = ViewStates.Gone;
                    mHolder.myGoalDeadline.Visibility = ViewStates.Gone;
                }

                mHolder.myGoalId.Text = mGoals[position].Id.ToString();
                mHolder.myGoal.Text = mGoals[position].Goal;
                mHolder.myGoalDeadline.Text = mGoals[position].GoalDeadline;
                mHolder.myGoalStatus.Text = mGoals[position].GoalStatus;

            }
            catch (Exception ex)
            {
                //Toast.MakeText(this.context, ex.Message + "\n" + ex.Source, ToastLength.Short).Show();
                Console.WriteLine(ex.StackTrace);
            }

        }

        // Raise an event when the item-click takes place:
        void OnClick(int position)
        {
            if (ItemClick != null)
                ItemClick(this, position);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.GoalItem, parent, false);

            return new GoalView(view, OnClick);
        }

        public override long GetItemId(int position)
        {
            return mGoals[position].Id;
        }
    }

    public class GoalView : RecyclerView.ViewHolder
    {
        public readonly View mView;
        public TextView myGoalId;
        public TextView myGoal;
        public TextView myGoalDeadline;
        public TextView myGoalStatus;
        public TextView myGoalNotify;


        public GoalView(View view, Action<int> listener) : base(view)
        {
            mView = view;
            myGoalId = mView.FindViewById<TextView>(Resource.Id.GoalID);
            myGoal = mView.FindViewById<TextView>(Resource.Id.GoalName);
            myGoalDeadline = mView.FindViewById<TextView>(Resource.Id.GoalDeadline);
            myGoalStatus = mView.FindViewById<TextView>(Resource.Id.GoalStatus);
            myGoalNotify = mView.FindViewById<TextView>(Resource.Id.GoalNotification);

            view.Click += (sender, e) => listener(base.Position);
        }

    }

}