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
using SmartDiary.Droid.Models;
using Fragment = Android.Support.V4.App.Fragment;
using SmartDiary.Droid.Views;
using Android.Support.V4.App;
using SmartDiary.Droid.Fragments;

namespace SmartDiary.Droid
{
    public class ViewGoalTasksFragment : Fragment, View.IOnTouchListener
    {
        private View view;
        private ListView mListTasks;
        private GoalTasksAdapter adapter;
        private JavaList<GoalTasks> gTasks;
        private int selGoalId;
        private int selGoalTask;
        private string selItemStatus;
        public FragmentActivity MyActivity;
        public FrameLayout tContainer;
        private float mLastPosY;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            HasOptionsMenu = true;
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            menu.SetGroupVisible(Resource.Id.grp_edit_goal, false);
            menu.SetGroupVisible(Resource.Id.grp_goal_tasks, true);
            base.OnCreateOptionsMenu(menu, inflater);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            view = inflater.Inflate(Resource.Layout.TasksListMaster, container, false);

            //get framelayout
            //tContainer = view.FindViewById<FrameLayout>(Resource.Id.taskFragementContainer);

            MyActivity = Activity;

            //passed goal id
            selGoalId = Convert.ToInt32(Activity.Intent.Extras.Get("GoalId").ToString());

            mListTasks = view.FindViewById<ListView>(Resource.Id.listTrackTasks);

            //transaction
            //var trans = MyActivity.SupportFragmentManager.BeginTransaction();
            //trans.Add(tContainer.Id, new GoalTaskFragment(), "TaskDetails");
            //trans.Commit();

            //tContainer.SetOnTouchListener(this);

            //click
            mListTasks.ItemClick += MListTasks_ItemClick;

            //context menu
            mListTasks.ContextMenuCreated += MListTasks_ContextMenuCreated;

            //populate list
            populateItemsList(view, selGoalId);


            return view;
        }

        //list click
        private void MListTasks_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            try
            {
                //set selected task id
                selGoalTask = Convert.ToInt32(gTasks[e.Position].Id);
                selItemStatus = gTasks[e.Position].TaskStatus;


                mListTasks.ShowContextMenu();
            }
            catch (Exception ex)
            {
                Toast.MakeText(view.Context, ex.Message, ToastLength.Short).Show();
            }
        }

        //list item context menu
        private void MListTasks_ContextMenuCreated(object sender, View.CreateContextMenuEventArgs e)
        {
            e.Menu.SetHeaderTitle("Task options:");
            MenuInflater inflater = new MenuInflater(mListTasks.Context);
            inflater.Inflate(Resource.Menu.task_popup, e.Menu);
        }

        //context item click
        public override bool OnContextItemSelected(IMenuItem item)
        {
            string selItem = selGoalTask.ToString();
            DBHelper dbh;
            Intent intent;
            AlertDialog alert;

            switch (item.ItemId)
            {
                case Resource.Id.pop_task_view:    //view task
                    //if (tContainer.TranslationY + 2 >= tContainer.Height)
                    //{
                    //    var interpolator = new Android.Views.Animations.OvershootInterpolator(5);
                    //    tContainer.Animate().SetInterpolator(interpolator)
                    //              .TranslationYBy(-200)
                    //              .SetDuration(500); 
                    //}

                    intent = new Intent(view.Context, typeof(ViewTaskActivity));
                    intent.PutExtra("TaskId", selGoalTask);
                    intent.PutExtra("TaskType", "Goal");
                    StartActivity(intent);

                    return true;

                case Resource.Id.pop_task_delete:  //delete item
                    dbh = new DBHelper();
                    AlertDialog.Builder builder = new AlertDialog.Builder(view.Context);
                    alert = builder.Create();
                    alert.SetTitle("Delete item");
                    alert.SetIcon(Android.Resource.Drawable.IcDialogAlert);
                    alert.SetMessage("Are you sure?");

                    alert.SetButton2("Yes", (s, ev) =>
                    {   //yes
                        string dresult = dbh.DeleteGoalTask(selGoalTask);
                        if (dresult.Equals("ok"))
                        {
                            Toast.MakeText(view.Context, "Task deleted!", ToastLength.Short).Show();
                            populateItemsList(view, selGoalId);
                        }
                        else
                        {
                            Toast.MakeText(view.Context, "Deletion failed!", ToastLength.Short).Show();
                        }
                    });
                    alert.SetButton("No", (s, ev) =>
                    {   //no

                    });
                    alert.Show();
                    return true;

                case Resource.Id.pop_task_done:  //update item status
                    dbh = new DBHelper();
                    string status = "Completed";

                    string sresult = dbh.UpdateGoalTaskStatus(selGoalTask, status);
                    if (sresult.Equals("ok"))
                    {
                        Toast.MakeText(view.Context, "Task status: " + status, ToastLength.Short).Show();
                        populateItemsList(view, selGoalId);
                    }
                    else
                    {
                        Toast.MakeText(view.Context, "Failed updating task status!", ToastLength.Short).Show();
                    }

                    return true;

                case Resource.Id.pop_task_undo:  //update item status
                    dbh = new DBHelper();
                    string istatus = "Pending";

                    string uresult = dbh.UpdateGoalTaskStatus(selGoalTask, istatus);
                    if (uresult.Equals("ok"))
                    {
                        Toast.MakeText(view.Context, "Task status: " + istatus, ToastLength.Short).Show();
                        populateItemsList(view, selGoalId);
                    }
                    else
                    {
                        Toast.MakeText(view.Context, "Failed updating task status!", ToastLength.Short).Show();
                    }

                    return true;

                case Resource.Id.menu_goal_tasks_del_all:
                    dbh = new DBHelper();
                    AlertDialog.Builder gbuilder = new AlertDialog.Builder(view.Context);
                    alert = gbuilder.Create();
                    alert.SetTitle("Delete current goal tasks");
                    alert.SetIcon(Android.Resource.Drawable.IcDialogAlert);
                    alert.SetMessage("Are you sure?");

                    alert.SetButton2("Yes", (s, ev) =>
                    {   //yes
                        string dresult = dbh.DeleteAllGoalTasks(selGoalId);
                        if (dresult.Equals("ok"))
                        {
                            Toast.MakeText(view.Context, "Goal tasks deleted!", ToastLength.Short).Show();
                            populateItemsList(view, selGoalId);
                        }
                        else
                        {
                            Toast.MakeText(view.Context, "Goal tasks deletion failed!", ToastLength.Short).Show();
                        }
                    });
                    alert.SetButton("No", (s, ev) =>
                    {   //no

                    });
                    alert.Show();

                    return true;

                default:
                    base.OnContextItemSelected(item);
                    return true;
            }
        }

        //title
        public override string ToString()
        {
            return "Tasks";
        }

        public override void OnResume()
        {
            base.OnResume();
            populateItemsList(view, selGoalId);
        }

        //populate list
        private void populateItemsList(View view, int gId)
        {
            try
            {
                gTasks = GoalsCollection.GetGoalTasks(gId);
                adapter = new GoalTasksAdapter(view.Context, gTasks);
                mListTasks.Adapter = adapter;
            }
            catch (Exception ex)
            {
                Toast.MakeText(view.Context, "Error: " + ex.Message, ToastLength.Long).Show();
            }
        }


        public bool OnTouch(View v, MotionEvent e)
        {
            switch (e.Action)
            {
                case MotionEventActions.Down:
                    //            mLastPosY = e.GetY();

                    return true;

                case MotionEventActions.Move:

                    //            var currentPosition = e.GetY();
                    //            var deltaY = mLastPosY - currentPosition;

                    //            var transY = v.TranslationY;

                    //            transY -= deltaY;

                    //            if (transY < 0)
                    //            {
                    //                transY = 0;
                    //            }

                    //            v.TranslationY = transY;

                    return true;

                default:
                    return v.OnTouchEvent(e);
            }
        }
    }
}