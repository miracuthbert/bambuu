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

namespace SmartDiary.Droid.Models
{
    public class GoalTasks
    {
        //+ "_id INTEGER PRIMARY KEY AUTOINCREMENT, "
        //+ "task INTEGER, "
        //+ "goal INTEGER, "
        //+ "taskDesc TEXT, "
        //+ "taskStart VARCHAR, "
        //+ "taskDeadline VARCHAR, "
        //+ "taskCost DECIMAL(10,5), "
        //+ "taskStatus VARCHAR, "
        //+ "taskAdded VARCHAR, "
        //+ "taskUpdated VARCHAR "

        private long _id;

        private string task;

        private long goal;

        private string taskDesc;

        private string taskStart;

        private string taskDeadline;

        private Decimal taskCost;

        private string taskStatus;

        private string taskAdded;

        private string taskUpdated;

        public GoalTasks()
        {

        }

        public long Id
        {
            get
            {
                return _id;
            }

            set
            {
                _id = value;
            }
        }

        public string Task
        {
            get
            {
                return task;
            }

            set
            {
                task = value;
            }
        }

        public long Goal
        {
            get
            {
                return goal;
            }

            set
            {
                goal = value;
            }
        }

        public string TaskDesc
        {
            get
            {
                return taskDesc;
            }

            set
            {
                taskDesc = value;
            }
        }

        public string TaskStart
        {
            get
            {
                return taskStart;
            }

            set
            {
                taskStart = value;
            }
        }

        public string TaskDeadline
        {
            get
            {
                return taskDeadline;
            }

            set
            {
                taskDeadline = value;
            }
        }

        public decimal TaskCost
        {
            get
            {
                return taskCost;
            }

            set
            {
                taskCost = value;
            }
        }

        public string TaskStatus
        {
            get
            {
                return taskStatus;
            }

            set
            {
                taskStatus = value;
            }
        }

        public string TaskAdded
        {
            get
            {
                return taskAdded;
            }

            set
            {
                taskAdded = value;
            }
        }

        public string TaskUpdated
        {
            get
            {
                return taskUpdated;
            }

            set
            {
                taskUpdated = value;
            }
        }
    }
}