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
    public class ShoppingCollection
    {
        private static DateTime last = DateTime.Now;

        //Check if a goal near deadline is available
        public static List<ShoppingLists> CheckItem(DateTime date)
        {
            var list = new List<ShoppingLists>();
            ShoppingLists allLists = null;
            list.Clear();

            DBHelper dbh = new DBHelper();          //db conn
            ICursor c = dbh.ReadShoppingDeadline(date);         //goal list

            while (c.MoveToNext())
            {
                string title = c.GetString(1);
                allLists = new ShoppingLists();
                allLists.ListTitle = title;
                list.Add(allLists);
            }

            return list;
        }

        //Check if a goal near deadline is available
        public static string[] CheckedItems(DateTime date)
        {
            string[] items = null;
            int i = 0;
            DBHelper dbh = new DBHelper();          //db conn
            ICursor c = dbh.ReadShoppingDeadline(date);         //goal list
            if (c.Count > 0)
            {
                while (c.MoveToNext())
                {
                    items[i] = c.GetString(1);
                    i++;
                }
            }
            return items;
        }

        //Get All Shopping Lists
        public static JavaList<ShoppingLists> GetShoppingLists()
        {
            var lists = new JavaList<ShoppingLists>();
            ShoppingLists allLists = null;
            lists.Clear();

            DBHelper dbh = new DBHelper();                      //db conn
            ICursor c = dbh.ReadAllShoppingLists();             //shopping lists
            while (c.MoveToNext())
            {
                string listId = c.GetString(0);
                string title = c.GetString(1);
                string listDesc = c.GetString(2);
                string shoppingDate = c.GetString(3);
                string expBudget = c.GetString(4);
                string actBudget = c.GetString(5);
                string listStatus = c.GetString(6);

                allLists = new ShoppingLists();
                allLists.Id = Convert.ToInt32(listId);
                allLists.ListTitle = title;
                allLists.ListDesc = listDesc;
                allLists.ShoppingDate = shoppingDate;
                allLists.ExpectedBudget = Convert.ToDecimal(expBudget);
                allLists.ListStatus = listStatus;
                lists.Add(allLists);

            }

            return lists;
        }

        //Spinner List Items
        public static JavaList<ShoppingLists> SpinnerLists()
        {
            var lists = new JavaList<ShoppingLists>();
            ShoppingLists allLists = null;
            lists.Clear();

            DBHelper dbh = new DBHelper();                      //db conn
            ICursor c = dbh.ReadAllShoppingLists();             //shopping lists
            while (c.MoveToNext())
            {
                string listId = c.GetString(0);
                string title = c.GetString(1);

                allLists = new ShoppingLists();
                allLists.Id = Convert.ToInt32(listId);
                allLists.ListTitle = title;
                lists.Add(title);

            }
            return lists;
        }

        //Get All Shopping List Items
        public static JavaList<ShoppingItems> GetShoppingItems(int lId)
        {
            var items = new JavaList<ShoppingItems>();
            ShoppingItems allItems = null;
            items.Clear();

            DBHelper dbh = new DBHelper();
            ICursor c = dbh.ReadAllShoppingListItems(lId);
            while (c.MoveToNext())
            {
                string itemId = c.GetString(0);
                string item = c.GetString(1);
                string list = c.GetString(2);
                string itemQty = c.GetString(3);
                string itemMsr = c.GetString(4);
                string itemExpPrice = c.GetString(5);
                string itemActPrice = c.GetString(6);
                string itemStatus = c.GetString(7);

                allItems = new ShoppingItems();
                allItems.Id = Convert.ToInt32(itemId);
                allItems.Item = item;
                allItems.List = Convert.ToInt32(list);
                allItems.ItemQuantity = Convert.ToDecimal(itemQty);
                allItems.ItemMeasure = itemMsr;
                allItems.ExpectedPrice = Convert.ToInt32(itemExpPrice);
                allItems.ActualPrice = Convert.ToInt32(itemActPrice);
                allItems.ItemStatus = itemStatus;

                items.Add(allItems);
            }

            return items;
        }
    }
}