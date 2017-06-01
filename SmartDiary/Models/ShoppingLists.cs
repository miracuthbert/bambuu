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
    public class ShoppingLists
    {
        private long _id;

        private string listTitle;

        private string listDesc;

        private string shoppingDate;

        private decimal expectedBudget;

        private decimal actualBudget;

        private string listStatus;

        private string listAdded;

        private string listUpdated;

        public ShoppingLists()
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

        public string ListTitle
        {
            get
            {
                return listTitle;
            }

            set
            {
                listTitle = value;
            }
        }

        public string ListDesc
        {
            get
            {
                return listDesc;
            }

            set
            {
                listDesc = value;
            }
        }

        public string ShoppingDate
        {
            get
            {
                return shoppingDate;
            }

            set
            {
                shoppingDate = value;
            }
        }

        public decimal ExpectedBudget
        {
            get
            {
                return expectedBudget;
            }

            set
            {
                expectedBudget = value;
            }
        }

        public decimal ActualBudget
        {
            get
            {
                return actualBudget;
            }

            set
            {
                actualBudget = value;
            }
        }

        public string ListStatus
        {
            get
            {
                return listStatus;
            }

            set
            {
                listStatus = value;
            }
        }

        public string ListAdded
        {
            get
            {
                return listAdded;
            }

            set
            {
                listAdded = value;
            }
        }

        public string ListUpdated
        {
            get
            {
                return listUpdated;
            }

            set
            {
                listUpdated = value;
            }
        }
    }
}