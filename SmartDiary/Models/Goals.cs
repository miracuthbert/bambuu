using System;
using System.Data;
using System.IO;
using SQLite;

namespace SmartDiary.Droid.Models
{
    //+ "_id INTEGER PRIMARY KEY AUTOINCREMENT, "
    //+ "goal VARCHAR, "
    //+ "goalDesc TEXT, "
    //+ "goalStart VARCHAR, "
    //+ "goalDeadline VARCHAR, "
    //+ "goalStatus VARCHAR, "
    //+ "goalAdded datetime, "
    //+ "goalUpdated datetime, "

    public class Goals
    {
        private long _id;

        private string goal;

        private string goalDesc;

        private string goalStart;

        private string goalDeadline;

        private string goalStatus;

        private string goalAdded;

        private string goalUpdated;

        public Goals()
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

        public string Goal
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

        public string GoalDesc
        {
            get
            {
                return goalDesc;
            }

            set
            {
                goalDesc = value;
            }
        }

        public string GoalStart
        {
            get
            {
                return goalStart;
            }

            set
            {
                goalStart = value;
            }
        }

        public string GoalDeadline
        {
            get
            {
                return goalDeadline;
            }

            set
            {
                goalDeadline = value;
            }
        }

        public string GoalStatus
        {
            get
            {
                return goalStatus;
            }

            set
            {
                goalStatus = value;
            }
        }

        public string GoalAdded
        {
            get
            {
                return goalAdded;
            }

            set
            {
                goalAdded = value;
            }
        }

        public string GoalUpdated
        {
            get
            {
                return goalUpdated;
            }

            set
            {
                goalUpdated = value;
            }
        }

        //public Goals (string goal, string goalDesc, DateTime goalStart, DateTime goalDeadline)
        //{
        //    this.goal = goal;
        //    this.goalDesc = goalDesc;
        //    this.goalStart = goalStart;
        //    this.goalDeadline = goalDeadline;
        //}            
    }
}