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
using SQLite;

namespace SmartDiary.Droid.Models
{
    public class ShoppingItems
    {
        //+ "_id INTEGER PRIMARY KEY AUTOINCREMENT, "
        //+ "item VARCHAR(50), "
        //+ "list INTEGER, "
        //+ "itemQuantity DECIMAL(10,5), "
        //+ "itemMeasure VARCHAR(20), "
        //+ "expectedPrice DECIMAL(10,5), "
        //+ "actualPrice DECIMAL(10,5), "
        //+ "itemStatus VARCHAR, "
        //+ "itemAdded VARCHAR, "
        //+ "itemUpdated VARCHAR "

        private long _id;

        private string item;

        private long list;

        private decimal itemQuantity;

        private string itemMeasure;

        private decimal expectedPrice;

        private decimal actualPrice;

        private string itemStatus;

        private string itemAdded;

        private string itemUpdated;

        public ShoppingItems()
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

        public string Item
        {
            get
            {
                return item;
            }

            set
            {
                item = value;
            }
        }

        public long List
        {
            get
            {
                return list;
            }

            set
            {
                list = value;
            }
        }

        public decimal ItemQuantity
        {
            get
            {
                return itemQuantity;
            }

            set
            {
                itemQuantity = value;
            }
        }

        public string ItemMeasure
        {
            get
            {
                return itemMeasure;
            }

            set
            {
                itemMeasure = value;
            }
        }

        public decimal ExpectedPrice
        {
            get
            {
                return expectedPrice;
            }

            set
            {
                expectedPrice = value;
            }
        }

        public decimal ActualPrice
        {
            get
            {
                return actualPrice;
            }

            set
            {
                actualPrice = value;
            }
        }

        public string ItemStatus
        {
            get
            {
                return itemStatus;
            }

            set
            {
                itemStatus = value;
            }
        }

        public string ItemAdded
        {
            get
            {
                return itemAdded;
            }

            set
            {
                itemAdded = value;
            }
        }

        public string ItemUpdated
        {
            get
            {
                return itemUpdated;
            }

            set
            {
                itemUpdated = value;
            }
        }
    }
}