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
using SmartDiary.Droid.Models;
using SmartDiary.Droid.Views;
using SmartDiary.Droid.ViewModel;
using Android.Support.Design.Widget;

namespace SmartDiary.Droid
{
    public class ViewProjectsFragment : Fragment
    {
        private View view;
        private ListView mListProjects;
        private ProjectsAdapter adapter;
        private JavaList<Projects> projects;
        private int selProject;

        public override void OnCreate(Bundle savedInstanceState)
        {
            HasOptionsMenu = true;
            base.OnCreate(savedInstanceState);
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            menu.SetGroupVisible(Resource.Id.grp_goals_frag, false);
            menu.SetGroupVisible(Resource.Id.grp_projects_frag, true);
            base.OnCreateOptionsMenu(menu, inflater);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            view = inflater.Inflate(Resource.Layout.ViewProjects, container, false);

            FloatingActionButton fab = Activity.FindViewById<FloatingActionButton>(Resource.Id.fab);

            fab.Click += (o, e) =>
            {
                Intent intent = new Intent(view.Context, typeof(AddProjectActivity));
                StartActivity(intent);
            };

            mListProjects = view.FindViewById<ListView>(Resource.Id.listProjects);

            //populate list
            populateProjectList(view);

            //click
            mListProjects.ItemClick += MListProjects_ItemClick;

            //context menu
            mListProjects.ContextMenuCreated += MListProjects_ContextMenuCreated;

            return view;
        }

        public override void OnPrepareOptionsMenu(IMenu menu)
        {
            //menu.FindItem(Resource.Id.menu_delete_goals).SetVisible(false);
            //menu.FindItem(Resource.Id.menu_delete_projects).SetVisible(true);
            base.OnPrepareOptionsMenu(menu);
        }

        private void MListProjects_ContextMenuCreated(object sender, View.CreateContextMenuEventArgs e)
        {
            e.Menu.SetHeaderTitle("Project options:");
            MenuInflater inflater = new MenuInflater(mListProjects.Context);
            inflater.Inflate(Resource.Menu.projects_popup, e.Menu);
        }

        public override bool OnContextItemSelected(IMenuItem item)
        {
            string projectId = selProject.ToString();
            DBHelper dbh;
            Intent intent;

            switch (item.ItemId)
            {
                case Resource.Id.pop_project_view:
                    intent = new Intent(view.Context, typeof(ViewProjectActivity));
                    intent.PutExtra("ProjectId", projectId);
                    StartActivity(intent);
                    return true;
                case Resource.Id.pop_project_edit:
                    intent = new Intent(view.Context, typeof(EditProjectActivity));
                    intent.PutExtra("ProjectId", projectId);
                    StartActivity(intent);
                    return true;
                case Resource.Id.pop_project_delete:
                    dbh = new DBHelper();
                    AlertDialog.Builder builder = new AlertDialog.Builder(view.Context);
                    AlertDialog alert = builder.Create();
                    alert.SetTitle("Delete project");
                    alert.SetIcon(Android.Resource.Drawable.IcDialogAlert);
                    alert.SetMessage("Are you sure?");

                    alert.SetButton2("Yes", (s, ev) =>
                    {   //yes
                        string result = dbh.DeleteProject(selProject);
                        if (result.Equals("ok"))
                        {
                            Toast.MakeText(view.Context, "Project deleted!", ToastLength.Short).Show();
                            populateProjectList(view);
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
                case Resource.Id.pop_project_status:
                    Toast.MakeText(view.Context, "Clicked: " + "Update project status", ToastLength.Short).Show();
                    return true;
                default:
                    base.OnContextItemSelected(item);
                    return true;
            }
        }

        private void MListProjects_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            this.selProject  = Convert.ToInt32(projects[e.Position].Id);

            Intent intent = new Intent(view.Context, typeof(ViewProjectActivity));
            intent.PutExtra("ProjectId", selProject);
            StartActivity(intent);

            //mListProjects.ShowContextMenu();
        }

        public override string ToString()
        {
            return "Projects";
        }

        public override void OnResume()
        {
            base.OnResume();
            populateProjectList(view);
        }

        //populate list
        private async void populateProjectList(View view)
        {
            projects = ProjectsCollection.GetProjects();
            adapter = new ProjectsAdapter(view.Context, projects);
            mListProjects.Adapter = adapter;
        }

    }
}