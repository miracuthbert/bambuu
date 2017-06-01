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
using SmartDiary.Droid.Models;
using Android.Database;
using SmartDiary.Droid.ViewModel;

namespace SmartDiary.Droid.Views
{
    public class ProjectsCollection
    {
        private static DateTime last = DateTime.Now;

        //Check if a goal near deadline is available
        public static Boolean CheckItem(DateTime date)
        {
            DBHelper dbh = new DBHelper();          //db conn
            ICursor c = dbh.ReadProjectsDeadline(date);         //goal list
            if (c.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static JavaList<Projects> GetProjects()
        {
            var projects = new JavaList<Projects>();

            Projects allProjects = null;
            projects.Clear();

            DBHelper dbh = new DBHelper();
            ICursor c = dbh.ReadAllProjects();
            while(c.MoveToNext())
            {
                string projectId = c.GetString(0);
                string project = c.GetString(1);
                string projectDesc = c.GetString(2);
                string projectStart = c.GetString(3);
                string projectDeadline = c.GetString(4);
                string projectBudget = c.GetString(5);
                string projectStatus = c.GetString(6);

                allProjects = new Projects();
                allProjects.Id = Convert.ToInt32(projectId);
                allProjects.Project = project;
                allProjects.ProjectDesc = projectDesc;
                allProjects.ProjectStart = projectStart;
                allProjects.ProjectDeadline = projectDeadline;
                allProjects.ProjectBudget = Convert.ToDecimal(projectBudget);
                allProjects.ProjectStatus = projectStatus;
                projects.Add(allProjects);
            }

            return projects;
        }

        public static JavaList<Projects> SpinnerProjects()
        {
            var projects = new JavaList<Projects>();
            Projects allProjects = null;
            projects.Clear();

            DBHelper dbh = new DBHelper();          //db conn
            ICursor c = dbh.ReadAllProjects();         //goal list
            while (c.MoveToNext())
            {
                string projectId = c.GetString(0);
                string project = c.GetString(1);
                string projectDesc = c.GetString(2);
                string projectStart = c.GetString(3);
                string projectDeadline = c.GetString(4);
                string projectBudget = c.GetString(5);
                string projectStatus = c.GetString(6);

                allProjects = new Projects();
                allProjects.Id = Convert.ToInt32(projectId);
                allProjects.Project = project;
                projects.Add(project);

            }
            return projects;
        }

        //Get Project Tasks
        public static JavaList<ProjectTasks> GetProjectTasks(int pId)
        {
            var tasks = new JavaList<ProjectTasks>();
            ProjectTasks allTasks = null;
            tasks.Clear();

            DBHelper dbh = new DBHelper();
            ICursor c = dbh.ReadAllProjectTasks(pId);
            while (c.MoveToNext())
            {
                string taskId = c.GetString(0);
                string task = c.GetString(1);
                string taskProject = c.GetString(2);
                string taskDesc = c.GetString(3);
                string taskStart = c.GetString(4);
                string taskDeadline = c.GetString(5);
                string taskExpBudget = c.GetString(6);
                string taskActBudget = c.GetString(7);
                string taskStatus = c.GetString(8);

                allTasks = new ProjectTasks();
                allTasks.Id = Convert.ToInt32(taskId);
                allTasks.Task = task;
                allTasks.Project = Convert.ToInt32(taskProject);
                allTasks.TaskDesc = taskDesc;
                allTasks.TaskStart = taskStart;
                allTasks.TaskDeadline = taskDeadline;
                allTasks.ExpectedCost = Convert.ToDecimal(taskExpBudget);
                allTasks.ActualCost = Convert.ToDecimal(taskActBudget);
                allTasks.TaskStatus = taskStatus;
                tasks.Add(allTasks);
            }

            return tasks;
        }

    }
}