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
    public class Projects
    {
        private long _id;

        private string project;

        private string projectDesc;

        private string projectStart;

        private string projectDeadline;

        private decimal projectBudget;

        private string projectStatus;

        private string projectAdded;

        private string projectUpdated;

        public Projects()
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

        public string Project
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

        public string ProjectDesc
        {
            get
            {
                return projectDesc;
            }

            set
            {
                projectDesc = value;
            }
        }

        public string ProjectStart
        {
            get
            {
                return projectStart;
            }

            set
            {
                projectStart = value;
            }
        }

        public string ProjectDeadline
        {
            get
            {
                return projectDeadline;
            }

            set
            {
                projectDeadline = value;
            }
        }

        public decimal ProjectBudget
        {
            get
            {
                return projectBudget;
            }

            set
            {
                projectBudget = value;
            }
        }

        public string ProjectStatus
        {
            get
            {
                return projectStatus;
            }

            set
            {
                projectStatus = value;
            }
        }

        public string ProjectAdded
        {
            get
            {
                return projectAdded;
            }

            set
            {
                projectAdded = value;
            }
        }

        public string ProjectUpdated
        {
            get
            {
                return projectUpdated;
            }

            set
            {
                projectUpdated = value;
            }
        }
    }
}