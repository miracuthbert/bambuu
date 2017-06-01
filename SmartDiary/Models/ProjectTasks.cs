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
    public class ProjectTasks
    {
        //+ "_id INTEGER PRIMARY KEY AUTOINCREMENT, "
        //+ "task VARCHAR, "
        //+ "project INTEGER, "
        //+ "taskDesc TEXT, "
        //+ "taskStart VARCHAR, "
        //+ "taskDeadline VARCHAR, "
        //+ "expectedCost DECIMAL(10,5), "
        //+ "actualCost DECIMAL(10,5), "
        //+ "taskStatus VARCHAR, "
        //+ "taskAdded VARCHAR, "
        //+ "taskUpdated VARCHAR "

        private long _id;

        private string task;

        private long project;

        private string taskDesc;

        private string taskStart;

        private string taskDeadline;

        private Decimal expectedCost;

        private Decimal actualCost;

        private string taskStatus;

        private string taskAdded;

        private string taskUpdated;

        public ProjectTasks()
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

        public long Project
        {
            get
            {
                return project;
            }

            set
            {
                project = value;
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

        public decimal ExpectedCost
        {
            get
            {
                return expectedCost;
            }

            set
            {
                expectedCost = value;
            }
        }

        public decimal ActualCost
        {
            get
            {
                return actualCost;
            }

            set
            {
                actualCost = value;
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