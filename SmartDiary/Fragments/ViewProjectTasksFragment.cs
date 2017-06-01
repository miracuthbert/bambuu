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

namespace SmartDiary.Droid
{
    public class ViewProjectTasksFragment : Fragment
    {
        private View view;
        private ListView mListTasks;
        private ProjectTasksAdapter adapter;
        private JavaList<ProjectTasks> pTasks;
        private int selProjectId;
        private int selProjectTask;
        private string selItemStatus;
        public Activity MyActivity;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            HasOptionsMenu = true;
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            menu.SetGroupVisible(Resource.Id.grp_edit_project, false);
            menu.SetGroupVisible(Resource.Id.grp_project_tasks, true);
            base.OnCreateOptionsMenu(menu, inflater);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            view = inflater.Inflate(Resource.Layout.TasksListMaster, container, false);

            //passed goal id
            MyActivity = this.Activity;
            selProjectId = Convert.ToInt32(MyActivity.Intent.Extras.Get("ProjectId"));

            mListTasks = view.FindViewById<ListView>(Resource.Id.listTrackTasks);

            //click
            mListTasks.ItemClick += MListTasks_ItemClick;

            //context menu
            mListTasks.ContextMenuCreated += MListTasks_ContextMenuCreated;

            //populate list
            populateItemsList(view, selProjectId);

            return view;
        }

        //list item click
        private void MListTasks_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            try
            {
                //set selected task id
                selProjectTask = Convert.ToInt32(pTasks[e.Position].Id);
                selItemStatus = pTasks[e.Position].TaskStatus;

                mListTasks.ShowContextMenu();
            }
            catch (Exception ex)
            {
                Toast.MakeText(view.Context, ex.Message, ToastLength.Short).Show();
            }
        }

        //context menu created
        private void MListTasks_ContextMenuCreated(object sender, View.CreateContextMenuEventArgs e)
        {
            e.Menu.SetHeaderTitle("Task options:");
            MenuInflater inflater = new MenuInflater(mListTasks.Context);
            inflater.Inflate(Resource.Menu.task_popup, e.Menu);
        }

        //context item click
        public override bool OnContextItemSelected(IMenuItem item)
        {
            string selItem = selProjectTask.ToString();
            DBHelper dbh;
            Intent intent;
            string status = "Pending";
            string sresult = null;

            switch (item.ItemId)
            {
                case Resource.Id.pop_task_view:    //view task
                    intent = new Intent(view.Context, typeof(ViewTaskActivity));
                    intent.PutExtra("TaskId", selProjectTask);
                    intent.PutExtra("TaskType", "Project");

                    StartActivity(intent);
                    return true;

                case Resource.Id.pop_task_delete:  //delete item
                    dbh = new DBHelper();
                    AlertDialog.Builder builder = new AlertDialog.Builder(view.Context);
                    AlertDialog alert = builder.Create();
                    alert.SetTitle("Delete task");
                    alert.SetIcon(Android.Resource.Drawable.IcDialogAlert);
                    alert.SetMessage("Are you sure?");

                    alert.SetButton2("Yes", (s, ev) =>
                    {   //yes
                        string dresult = dbh.DeleteProjectTask(selProjectTask);
                        if (dresult.Equals("ok"))
                        {
                            Toast.MakeText(view.Context, "Task deleted!", ToastLength.Short).Show();
                            populateItemsList(view, selProjectId);
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

                    status = "Completed";

                    sresult = dbh.UpdateProjectTaskStatus(selProjectTask, status);
                    if (sresult.Equals("ok"))
                    {
                        Toast.MakeText(view.Context, "Task status: " + status, ToastLength.Short).Show();
                        populateItemsList(view, selProjectId);
                    }
                    else
                    {
                        Toast.MakeText(view.Context, "Failed updating task status!", ToastLength.Short).Show();
                    }
                    return true;

                case Resource.Id.pop_task_undo:  //update item status
                    dbh = new DBHelper();

                    status = "Pending";

                    sresult = dbh.UpdateProjectTaskStatus(selProjectTask, status);
                    if (sresult.Equals("ok"))
                    {
                        Toast.MakeText(view.Context, "Task status: " + status, ToastLength.Short).Show();
                        populateItemsList(view, selProjectId);
                    }
                    else
                    {
                        Toast.MakeText(view.Context, "Failed updating task status!", ToastLength.Short).Show();
                    }
                    return true;

                case Resource.Id.menu_prj_tasks_del_all:
                    dbh = new DBHelper();
                    AlertDialog.Builder pbuilder = new AlertDialog.Builder(view.Context);
                    AlertDialog del_alert = pbuilder.Create();
                    del_alert.SetTitle("Delete current project tasks");
                    del_alert.SetIcon(Android.Resource.Drawable.IcDialogAlert);
                    del_alert.SetMessage("Are you sure?");

                    del_alert.SetButton2("Yes", (s, ev) =>
                    {   //yes
                        string dresult = dbh.DeleteAllProjectTasks(selProjectTask);
                        if (dresult.Equals("ok"))
                        {
                            Toast.MakeText(view.Context, "Project Tasks deleted!", ToastLength.Short).Show();
                            populateItemsList(view, selProjectId);
                        }
                        else
                        {
                            Toast.MakeText(view.Context, "Project tasks deletion failed!" + dresult, ToastLength.Short).Show();
                        }
                    });
                    del_alert.SetButton("No", (s, ev) =>
                    {   //no

                    });
                    del_alert.Show();

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
            populateItemsList(view, selProjectId);
        }

        //populate list
        private async void populateItemsList(View view, int pId)
        {
            try
            {
                pTasks = ProjectsCollection.GetProjectTasks(pId);
                adapter = new ProjectTasksAdapter(view.Context, pTasks);
                mListTasks.Adapter = adapter;
            }
            catch (Exception ex)
            {
                Toast.MakeText(view.Context, "" + ex.Message, ToastLength.Long).Show();
            }
        }

    }
}