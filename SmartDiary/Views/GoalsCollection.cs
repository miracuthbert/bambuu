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
using SmartDiary.Droid.ViewModel;
using Android.Database;

namespace SmartDiary.Droid.Views
{
    public class GoalsCollection
    {
        private static DateTime last = DateTime.Now;

        //Check if a goal near deadline is available
        public static Boolean CheckItem(DateTime date)
        {
            DBHelper dbh = new DBHelper();          //db conn
            ICursor c = dbh.ReadGoalsDeadline(date);         //goal list
            if (c.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Get All Goals
        public static JavaList<Goals> GetGoals()
        {
            var goals = new JavaList<Goals>();
            Goals allGoals = null;
            goals.Clear();

            DBHelper dbh = new DBHelper();          //db conn
            ICursor c = dbh.ReadAllGoals();         //goal list
            while (c.MoveToNext())
            {
                string goalId = c.GetString(0);
                string goal = c.GetString(1);
                string goalDesc = c.GetString(2);
                string goalStart = c.GetString(3);
                string goalDeadline = c.GetString(4);
                string goalStatus = c.GetString(5);

                allGoals = new Goals();
                allGoals.Id = Convert.ToInt32(goalId);
                allGoals.Goal = goal;
                allGoals.GoalDesc = goalDesc;
                allGoals.GoalStart = goalStart;
                allGoals.GoalDeadline = goalDeadline;
                allGoals.GoalStatus = goalStatus;
                goals.Add(allGoals);

            }

            return goals;
        }

        //Get Goal Names
        public static JavaList<Goals> SpinnerGoals()
        {
            var goals = new JavaList<Goals>();
            Goals allGoals = null;
            goals.Clear();

            DBHelper dbh = new DBHelper();          //db conn
            ICursor c = dbh.ReadAllGoals();         //goal list
            while (c.MoveToNext())
            {
                string goalId = c.GetString(0);
                string goal = c.GetString(1);
                string goalDesc = c.GetString(2);
                string goalStart = c.GetString(3);
                string goalDeadline = c.GetString(4);
                string goalStatus = c.GetString(5);

                allGoals = new Goals();
                allGoals.Id = Convert.ToInt32(goalId);
                allGoals.Goal = goal;
                goals.Add(goal);

            }
            return goals;
        }

        //Get Goal Tasks
        public static JavaList<GoalTasks> GetGoalTasks(int gId)
        {
            var tasks = new JavaList<GoalTasks>();
            GoalTasks allTasks = null;
            tasks.Clear();

            DBHelper dbh = new DBHelper();
            ICursor c = dbh.ReadAllGoalTasks(gId);
            while (c.MoveToNext())
            {
                string taskId = c.GetString(0);
                string task = c.GetString(1);
                string taskGoal = c.GetString(2);
                string taskDesc = c.GetString(3);
                string taskStart = c.GetString(4);
                string taskDeadline = c.GetString(5);
                string taskStatus = c.GetString(7);

                allTasks = new GoalTasks();
                allTasks.Id = Convert.ToInt32(taskId);
                allTasks.Task = task;
                allTasks.Goal = Convert.ToInt32(taskGoal);
                allTasks.TaskDesc = taskDesc;
                allTasks.TaskStart = taskStart;
                allTasks.TaskDeadline = taskDeadline;
                allTasks.TaskStatus = taskStatus;
                tasks.Add(allTasks);
            }

            return tasks;
        }
    }
}