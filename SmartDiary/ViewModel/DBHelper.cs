using System;

using System.Data;
using System.IO;
using SQLite;
using SmartDiary.Droid.Models;
using Android.Database.Sqlite;
using Android.Database;
using Java.Util;
using System.Collections.Generic;
using System.Linq;

namespace SmartDiary.Droid.ViewModel
{
    /// <summary>
    /// Database helper class
    /// </summary>
    public class DBHelper
    {
        const string filename = "smartdiary.db3";
        /// <summary>
        /// Db storage folder path
        /// </summary>
        public string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), filename);

        /// <summary>
        /// Sqlitedb init
        /// </summary>
        private SQLiteDatabase db;
        
        /// <summary>
        /// Date and time of resource creation
        /// </summary>
        private string dateAdded = DateTime.Now.ToLongDateString();

        /// <summary>
        /// Date and time of resource update
        /// </summary>
        private string dateUpdated = DateTime.Now.ToLongDateString();

        /// <summary>
        /// Default status of resource
        /// </summary>
        private string defaultStatus = "Pending";

        /// <summary>
        /// Default status of shopping resource
        /// </summary>
        private string defaultShoppingStatus = "Pending";

        /// <summary>
        /// Constructor
        /// </summary>
        public DBHelper()
        {
            this.createDB();
            //this.DropTables();
            this.createTable();
        }

        //create database
        /// <summary>
        /// 
        /// </summary>
        public void createDB()
        {
            bool bIsExists = File.Exists(dbPath);
            if (!bIsExists)
            {
                db = SQLiteDatabase.OpenOrCreateDatabase(dbPath, null);
            }
            else
            {
                db = SQLiteDatabase.OpenDatabase(dbPath, null, DatabaseOpenFlags.OpenReadwrite);
            }
        }

        //Create tables
        /// <summary>
        /// 
        /// </summary>
        public void createTable()
        {
            try
            {
                string sql = null;

                sql = "CREATE TABLE IF NOT EXISTS Goals " +
                "("
                        + "_id INTEGER PRIMARY KEY AUTOINCREMENT, "
                        + "goal VARCHAR, "
                        + "goalDesc TEXT, "
                        + "goalStart VARCHAR, "
                        + "goalDeadline VARCHAR, "
                        + "goalStatus VARCHAR, "
                        + "created_at VARCHAR, "
                        + "updated_at VARCHAR, "
                        + "completed_at VARCHAR "
                + "); ";
                db.ExecSQL(sql);

                sql = " CREATE TABLE IF NOT EXISTS GoalTasks " +
                "("
                        + "_id INTEGER PRIMARY KEY AUTOINCREMENT, "
                        + "task VARCHAR, "
                        + "goal INTEGER, "
                        + "taskDesc TEXT, "
                        + "taskStart VARCHAR, "
                        + "taskDeadline VARCHAR, "
                        + "taskCost DECIMAL(10,5), "
                        + "taskStatus VARCHAR, "
                        + "created_at VARCHAR, "
                        + "updated_at VARCHAR, "
                        + "completed_at VARCHAR "
                + "); ";
                db.ExecSQL(sql);

                sql = " CREATE TABLE IF NOT EXISTS ShoppingLists " +
                "("
                        + "_id INTEGER PRIMARY KEY AUTOINCREMENT, "
                        + "title VARCHAR(50), "
                        + "listDesc VARCHAR(160), "
                        + "shoppingDate VARCHAR, "
                        + "expectedBudget DECIMAL(10,5), "
                        + "actualBudget DECIMAL(10,5), "
                        + "listStatus VARCHAR, "
                        + "created_at VARCHAR, "
                        + "updated_at VARCHAR, "
                        + "completed_at VARCHAR "
                + "); ";
                db.ExecSQL(sql);

                sql = " CREATE TABLE IF NOT EXISTS ShoppingItems " +
                "("
                        + "_id INTEGER PRIMARY KEY AUTOINCREMENT, "
                        + "item VARCHAR(50), "
                        + "list INTEGER, "
                        + "itemQuantity DECIMAL(10,5), "
                        + "itemMeasure VARCHAR(20), "
                        + "expectedPrice DECIMAL(10,5), "
                        + "actualPrice DECIMAL(10,5), "
                        + "itemStatus VARCHAR, "
                        + "created_at VARCHAR, "
                        + "updated_at VARCHAR, "
                        + "completed_at VARCHAR "
                + "); ";
                db.ExecSQL(sql);

                sql = " CREATE TABLE IF NOT EXISTS Projects " +
                "("
                        + "_id INTEGER PRIMARY KEY AUTOINCREMENT, "
                        + "project VARCHAR, "
                        + "projectDesc TEXT, "
                        + "projectStart VARCHAR, "
                        + "projectDeadline VARCHAR, "
                        + "projectBudget DECIMAL(10,5), "
                        + "projectStatus VARCHAR, "
                        + "created_at VARCHAR, "
                        + "updated_at VARCHAR, "
                        + "completed_at VARCHAR "
                + "); ";
                db.ExecSQL(sql);

                sql = " CREATE TABLE IF NOT EXISTS ProjectTasks " +
                "("
                        + "_id INTEGER PRIMARY KEY AUTOINCREMENT, "
                        + "task VARCHAR, "
                        + "project INTEGER, "
                        + "taskDesc TEXT, "
                        + "taskStart VARCHAR, "
                        + "taskDeadline VARCHAR, "
                        + "expectedCost DECIMAL(10,5), "
                        + "actualCost DECIMAL(10,5), "
                        + "taskStatus VARCHAR, "
                        + "created_at VARCHAR, "
                        + "updated_at VARCHAR, "
                        + "completed_at VARCHAR "
                + "); ";
                db.ExecSQL(sql);

                sql = " CREATE TABLE IF NOT EXISTS Budgets " +
                "("
                        + "_id INTEGER PRIMARY KEY AUTOINCREMENT, "
                        + "budget VARCHAR, "
                        + "budgetDesc TEXT, "
                        + "budgetFrom VARCHAR, "
                        + "budgetTo VARCHAR, "
                        + "budgetExpected DECIMAL(10,5), "
                        + "budgetStatus VARCHAR, "
                        + "created_at VARCHAR, "
                        + "updated_at VARCHAR, "
                        + "completed_at VARCHAR "
                + "); ";
                db.ExecSQL(sql);

                sql = " CREATE TABLE IF NOT EXISTS BudgetItems " +
                    "("
                            + "_id INTEGER PRIMARY KEY AUTOINCREMENT, "
                            + "budget INTEGER, "
                            + "item VARCHAR, "
                            + "itemDesc TEXT, "
                            + "itemGroup VARCHAR, "
                            + "itemExpected DECIMAL(10,5), "
                            + "itemActual DECIMAL(10,5), "
                            + "created_at VARCHAR, "
                            + "updated_at VARCHAR, "
                            + "completed_at VARCHAR "
                    + "); ";
                db.ExecSQL(sql);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        //Drop tables
        /// <summary>
        /// Called to drop all tables in db
        /// </summary>
        public void DropTables()
        {
            try
            {
                var sql = "DROP TABLE IF EXISTS GoalTasks";
                db.ExecSQL(sql);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        //Create Goal
        /// <summary>
        /// Stores goal in db
        /// </summary>
        /// <param name="goal"></param>
        /// <param name="goalDesc"></param>
        /// <param name="goalStart"></param>
        /// <param name="goalDeadline"></param>
        /// <returns></returns>
        public string CreateGoal(string goal, string goalDesc, string goalStart, string goalDeadline)
        {
            try
            {
                string sql = "INSERT INTO Goals (goal, goalDesc, goalStart, goalDeadline , goalStatus, created_at)" +
                              "VALUES(" + goal + "," + goalDesc + ",'" + goalStart + "','" + goalDeadline + "','" + defaultStatus + "','" + dateAdded + "');";

                db.ExecSQL(sql);
                return "ok";
            }
            catch (Exception ex)
            {
                return "Error:\n" + ex.Message;
            }
            finally
            {
                db.Close();
            }
        }

        //Read All Goals
        /// <summary>
        /// Gets all goals in db
        /// </summary>
        /// <returns></returns>
        public ICursor ReadAllGoals()
        {
            ICursor output = null;

            try
            {
                string sql = "SELECT * FROM Goals";
                output = db.RawQuery(sql, null);

                if (!(output != null))
                {
                    Console.WriteLine("No records found");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error:\n" + ex.Message);
            }

            return output;
        }

        //Read All Goals Deadline
        /// <summary>
        /// Get goals deadline
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public ICursor ReadGoalsDeadline(DateTime date)
        {
            ICursor output = null;
            string cur = date.ToShortDateString();

            try
            {
                string sql = "SELECT * FROM Goals WHERE goalDeadline LIKE '%" + cur + "%'";
                output = db.RawQuery(sql, null);

                if (!(output != null))
                {
                    Console.WriteLine("No records found");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error:\n" + ex.Message);
            }

            return output;
        }

        //Read Goal
        /// <summary>
        /// Get goal from db
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string[] ReadGoal(int id)
        {
            ICursor output = null;
            var sql = "SELECT * FROM Goals WHERE _id = " + id;
            output = db.RawQuery(sql, null);

            string[] mygoal = new string[10];
            //var con = new SQLiteConnection(dbPath);
            //var goal = con.Table<Goals>().FirstOrDefault(u => u.Id == id);

            output.MoveToFirst();
            mygoal[0] = output.GetString(0);
            mygoal[1] = output.GetString(1);
            mygoal[2] = output.GetString(2);
            mygoal[3] = output.GetString(3);
            mygoal[4] = output.GetString(4);
            mygoal[5] = output.GetString(5);

            return mygoal;
        }

        //Update Goal
        /// <summary>
        /// Update goal in db
        /// </summary>
        /// <param name="id"></param>
        /// <param name="goal"></param>
        /// <param name="goalDesc"></param>
        /// <param name="goalStart"></param>
        /// <param name="goalDeadline"></param>
        /// <param name="goalStatus"></param>
        /// <returns></returns>
        public string UpdateGoal(int id, string goal, string goalDesc, string goalStart, string goalDeadline, string goalStatus)
        {
            try
            {
                var sql = "sql";
                if (goalStatus == "completed")
                    sql = "UPDATE Goals SET goal=" + goal 
                        + ", goalDesc=" + goalDesc + ", goalStart='" + goalStart 
                        + "', goalDeadline='" + goalDeadline + "', goalStatus='" + goalStatus 
                        + "', updated_at='" + dateUpdated + "', completed_at='" 
                        + dateUpdated + "' WHERE _id='" + id + "';";

                else
                    sql = "UPDATE Goals SET goal=" + goal + ", goalDesc=" + goalDesc + ", goalStart='" + goalStart + "', goalDeadline='" + goalDeadline + "', goalStatus='" + goalStatus + "', updated_at='" + dateUpdated + "', completed_at='" + null + "' WHERE _id='" + id + "';";

                db.ExecSQL(sql);
                return "ok";
            }
            catch (Exception ex)
            {
                return "Error:" + ex.Message;
            }
        }

        //Update Goal Status
        /// <summary>
        /// Update goal status in db
        /// </summary>
        /// <param name="id"></param>
        /// <param name="goalStatus"></param>
        /// <returns></returns>
        public string UpdateGoalStatus(int id, string goalStatus)
        {
            try
            {
                var sql = "update";
                if (goalStatus == "completed")
                    sql = "UPDATE Goals SET goalStatus='" + goalStatus + "', updated_at='" + dateUpdated + "', completed_at='" + dateUpdated + "' WHERE _id='" + id + "';";
                else
                    sql = "UPDATE Goals SET goalStatus='" + goalStatus + "', updated_at='" + dateUpdated + "', completed_at='" + null + "' WHERE _id='" + id + "';";
                db.ExecSQL(sql);
                return "ok";
            }
            catch (Exception ex)
            {
                return "Error:" + ex.Message;
            }
        }

        //Delete Goal
        /// <summary>
        /// Delete goal in db
        /// </summary>
        /// <param name="goal"></param>
        /// <returns></returns>
        public string DeleteGoal(int goal)
        {
            try
            {
                var sql = "DELETE FROM Goals WHERE _id = " + goal + ";";
                db.ExecSQL(sql);

                return "ok";
            }
            catch (Exception ex)
            {
                return "Error:" + ex.Message;
            }
        }

        //Delete all goals
        /// <summary>
        /// Delete all goals in db
        /// </summary>
        /// <returns></returns>
        public string DeleteAllGoals()
        {
            try
            {
                db.Delete("Goals", null, null);
                return "ok";
            }
            catch (Exception ex)
            {
                return "Error:" + ex.Message;
            }
        }

        //Create Goal Task
        /// <summary>
        /// Create goal task
        /// </summary>
        /// <param name="task"></param>
        /// <param name="goal"></param>
        /// <param name="taskDesc"></param>
        /// <param name="taskStart"></param>
        /// <param name="taskDeadline"></param>
        /// <returns></returns>
        public string CreateGoalTask(string task, int goal, string taskDesc, string taskStart, string taskDeadline)
        {
            try
            {
                string sql = "INSERT INTO GoalTasks (task, goal, taskDesc, taskStart, taskDeadline , taskStatus, created_at)" +
                              "VALUES(" + task + ",'" + goal + "', " + taskDesc + " ,'" + taskStart + "','" + taskDeadline + "','" + defaultStatus + "','" + dateAdded + "');";

                db.ExecSQL(sql);
                return "ok";
            }
            catch (Exception ex)
            {
                return "Error:\n" + ex.Message;
            }
            finally
            {
                db.Close();
            }
        }

        //Read All Goal Tasks
        /// <summary>
        /// 
        /// </summary>
        /// <param name="goal"></param>
        /// <returns></returns>
        public ICursor ReadAllGoalTasks(int goal)
        {
            ICursor output = null;

            try
            {
                string sql = "SELECT * FROM GoalTasks WHERE goal = '" + goal + "';";
                output = db.RawQuery(sql, null);

                if (!(output != null))
                {
                    Console.WriteLine("No records found");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error:\n" + ex.Message);
            }

            return output;
        }

        //Read Goal Task
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string[] ReadGoalTask(int id)
        {
            ICursor output = null;
            var sql = "SELECT * FROM GoalTasks WHERE _id = " + id;
            output = db.RawQuery(sql, null);

            string[] mygoal = new string[10];
            //var con = new SQLiteConnection(dbPath);
            //var goal = con.Table<Goals>().FirstOrDefault(u => u.Id == id);

            output.MoveToFirst();
            mygoal[0] = output.GetString(0);
            mygoal[1] = output.GetString(1);
            mygoal[2] = output.GetString(3);
            mygoal[3] = output.GetString(4);
            mygoal[4] = output.GetString(5);
            mygoal[5] = output.GetString(7);

            return mygoal;
        }

        //Update Goal Task
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="task"></param>
        /// <param name="goal"></param>
        /// <param name="taskDesc"></param>
        /// <param name="taskDeadline"></param>
        /// <param name="taskStatus"></param>
        /// <returns></returns>
        public string UpdateGoalTask(int id, string task, int goal, string taskDesc, string taskDeadline, string taskStatus)
        {
            try
            {
                var sql = "update";

                if (taskStatus == "completed")
                    sql = "UPDATE GoalTasks SET task=" + task + ", goal='" + goal + "', taskDesc=" + taskDesc + ", taskDeadline='" + taskDeadline + "', taskStatus='" + taskStatus + "', updated_at='" + dateUpdated + "', completed_at='" + dateUpdated + "' WHERE _id='" + id + "';";
                else
                    sql = "UPDATE GoalTasks SET task=" + task + ", goal='" + goal + "', taskDesc=" + taskDesc + ", taskDeadline='" + taskDeadline + "', taskStatus='" + taskStatus + "', updated_at='" + dateUpdated + "', completed_at='" + null + "' WHERE _id='" + id + "';";

                db.ExecSQL(sql);
                return "ok";
            }
            catch (Exception ex)
            {
                return "Error:" + ex.Message;
            }
        }

        //Update Goal Task Status
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="taskStatus"></param>
        /// <returns></returns>
        public string UpdateGoalTaskStatus(int id, string taskStatus)
        {
            try
            {
                var sql = "update";

                if (taskStatus == "completed")
                    sql = "UPDATE GoalTasks SET taskStatus ='" + taskStatus + "', updated_at='" + dateUpdated + "', completed_at='" + dateUpdated + "' WHERE _id='" + id + "';";
                else
                    sql = "UPDATE GoalTasks SET taskStatus ='" + taskStatus + "', updated_at='" + null + "', completed_at='" + null + "' WHERE _id='" + id + "';";

                db.ExecSQL(sql);

                return "ok";
            }
            catch (Exception ex)
            {
                return "Error:" + ex.Message;
            }
        }

        //Delete Goal Task
        /// <summary>
        /// 
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public string DeleteGoalTask(int task)
        {
            try
            {
                var sql = "DELETE FROM GoalTasks WHERE _id = " + task + ";";
                db.ExecSQL(sql);

                return "ok";
            }
            catch (Exception ex)
            {
                return "Error:" + ex.Message;
            }
        }

        //Delete All Goal Task
        /// <summary>
        /// 
        /// </summary>
        /// <param name="goal"></param>
        /// <returns></returns>
        public string DeleteAllGoalTasks(int goal)
        {
            try
            {
                var sql = "DELETE FROM GoalTasks WHERE goal = " + goal + ";";
                db.ExecSQL(sql);

                return "ok";
            }
            catch (Exception ex)
            {
                return "Error:" + ex.Message;
            }
        }

        //Create Shopping list
        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="listDesc"></param>
        /// <param name="shoppingDate"></param>
        /// <param name="expectedBudget"></param>
        /// <returns></returns>
        public string CreateShoppingList(string title, string listDesc, string shoppingDate, decimal expectedBudget)
        {
            try
            {
                string sql = "INSERT INTO ShoppingLists (title, listDesc, shoppingDate, expectedBudget, listStatus, created_at)" +
                              "VALUES(" + title + ", " + listDesc + " ,'" + shoppingDate + "','" + expectedBudget + "','" + defaultShoppingStatus + "','" + dateAdded + "');";

                db.ExecSQL(sql);
                return "ok";
            }
            catch (Exception ex)
            {
                return "Error:\n" + ex.Message;
            }
            finally
            {
                db.Close();
            }
        }

        //Read All Shopping Lists
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ICursor ReadAllShoppingLists()
        {
            ICursor output = null;

            try
            {
                string sql = "SELECT * FROM ShoppingLists";
                output = db.RawQuery(sql, null);

                if (!(output != null))
                {
                    Console.WriteLine("No records found");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error:\n" + ex.Message);
            }

            return output;
        }

        //Read All Goals Deadline
        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public ICursor ReadShoppingDeadline(DateTime date)
        {
            ICursor output = null;
            string cur = date.ToShortDateString();

            try
            {
                //var m = db.Query("ShoppingLists", null, null, selectionArgs, "shoppingDate", null, null);
                //output = m;
                //string[] selectionArgs = { cur };

                string sql = "SELECT * FROM ShoppingLists WHERE shoppingDate LIKE '%" + cur + "%'";

                output = db.RawQuery(sql, null);

                if (!(output != null))
                {
                    Console.WriteLine("No records found");
                }
                //else
                //{
                //    Console.WriteLine(cur);
                //    Console.WriteLine(output.Count);
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error:\n" + ex.Message);
            }

            return output;
        }

        //Read Shopping Lists
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string[] ReadShoppingList(int id)
        {
            ICursor output = null;
            var sql = "SELECT * FROM ShoppingLists WHERE _id = " + id;
            output = db.RawQuery(sql, null);

            string[] myproject = new string[10];

            output.MoveToFirst();
            myproject[0] = output.GetString(0);
            myproject[1] = output.GetString(1);
            myproject[2] = output.GetString(2);
            myproject[3] = output.GetString(3);
            myproject[4] = output.GetString(4);
            myproject[5] = output.GetString(5);
            myproject[6] = output.GetString(6);

            return myproject;
        }

        //Update Shopping List
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="title"></param>
        /// <param name="listDesc"></param>
        /// <param name="shoppingDate"></param>
        /// <param name="expectedBudget"></param>
        /// <param name="listStatus"></param>
        /// <returns></returns>
        public string UpdateShoppingList(int id, string title, string listDesc, string shoppingDate, decimal expectedBudget, string listStatus)
        {
            try
            {
                var sql = "UPDATE ShoppingLists SET title= " + title + ", listDesc= " + listDesc + ", shoppingDate='" + shoppingDate + "', expectedBudget='" + expectedBudget + "', listStatus='" + listStatus + "', updated_at='" + dateUpdated + "' WHERE _id='" + id + "';";

                db.ExecSQL(sql);
                return "ok";
            }
            catch (Exception ex)
            {
                return "Error:" + ex.Message;
            }
        }

        //Update Shopping List Status
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="listStatus"></param>
        /// <returns></returns>
        public string UpdateShoppingListStatus(int id, string listStatus)
        {
            try
            {
                var sql = "update";

                if (listStatus == "completed")
                    sql = "UPDATE ShoppingLists SET listStatus='" + listStatus + "', updated_at='" + dateUpdated + "', completed_at='" + dateUpdated + "' WHERE _id='" + id + "';";
                else
                    sql = "UPDATE ShoppingLists SET listStatus='" + listStatus + "', updated_at='" + dateUpdated + "', completed_at='" + null + "' WHERE _id='" + id + "';";

                db.ExecSQL(sql);
                return "ok";
            }
            catch (Exception ex)
            {
                return "Error:" + ex.Message;
            }
        }

        //Delete Shopping List
        /// <summary>
        /// 
        /// </summary>
        /// <param name="shoppinglist"></param>
        /// <returns></returns>
        public string DeleteShoppingList(int shoppinglist)
        {
            try
            {
                var sql = "DELETE FROM ShoppingLists WHERE _id = " + shoppinglist + ";";
                db.ExecSQL(sql);

                return "ok";
            }
            catch (Exception ex)
            {
                return "Error:" + ex.Message;
            }
        }

        //Delete all shopping lists
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string DeleteAllShoppingLists()
        {
            try
            {
                db.Delete("ShoppingLists", null, null);
                return "ok";
            }
            catch (Exception ex)
            {
                return "Error:" + ex.Message;
            }
        }

        //Create Shopping Item
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="list"></param>
        /// <param name="itemQuantity"></param>
        /// <param name="itemMeasure"></param>
        /// <param name="expectedPrice"></param>
        /// <returns></returns>
        public string CreateShoppingItem(string item, int list, decimal itemQuantity, string itemMeasure, decimal expectedPrice)
        {
            try
            {
                string sql = "INSERT INTO ShoppingItems (item, list, itemQuantity, itemMeasure, expectedPrice, itemStatus, created_at)" +
                             "VALUES(" + item + ", '" + list + "', '" + itemQuantity + "', " + itemMeasure + ",'" + expectedPrice + "','" + defaultShoppingStatus + "','" + dateAdded + "');";

                db.ExecSQL(sql);

                return "ok";
            }
            catch (Exception ex)
            {
                return "Error:" + ex.Message;
            }
        }

        //Read All Shopping List Items
        /// <summary>
        /// 
        /// </summary>
        /// <param name="shoppinglist"></param>
        /// <returns></returns>
        public ICursor ReadAllShoppingListItems(int shoppinglist)
        {
            ICursor output = null;

            try
            {
                string sql = "SELECT * FROM ShoppingItems WHERE list = '" + shoppinglist + "';";
                output = db.RawQuery(sql, null);

                if (!(output != null))
                {
                    Console.WriteLine("No records found");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error:\n" + ex.Message);
            }

            return output;
        }
        
        //Read ShoppingItem
        /// <summary>
        /// Get goal from db
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string[] ReadShoppingItem(int id)
        {
            ICursor output = null;
            var sql = "SELECT * FROM ShoppingItems WHERE _id = " + id;
            output = db.RawQuery(sql, null);

            string[] listItem = new string[10];

            output.MoveToFirst();
            listItem[0] = output.GetString(0);  //id
            listItem[1] = output.GetString(1);  //item
            listItem[2] = output.GetString(2);  //list
            listItem[3] = output.GetString(3);  //qty
            listItem[4] = output.GetString(4);  //msr
            listItem[5] = output.GetString(5);  //exp price
            listItem[6] = output.GetString(6);  //act price
            listItem[7] = output.GetString(7);  //status

            return listItem;
        }
        //Update Shopping Item
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="item"></param>
        /// <param name="list"></param>
        /// <param name="itemQuantity"></param>
        /// <param name="itemMeasure"></param>
        /// <param name="actualPrice"></param>
        /// <returns></returns>
        public string UpdateShoppingItem(int id, string item, int list, decimal itemQuantity, string itemMeasure, decimal actualPrice)
        {
            try
            {
                var sql = "update";

                sql = "UPDATE ShoppingItems SET item= " + item + " , list='" + list + "', itemQuantity='" + itemQuantity + "', itemMeasure= " + itemMeasure + ", actualPrice='" + actualPrice + "', updated_at='" + dateUpdated + "' WHERE _id='" + id + "';";

                db.ExecSQL(sql);
                return "ok";
            }
            catch (Exception ex)
            {
                return "Error:" + ex.Message;
            }
        }

        //Update Shopping Item
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="itemStatus"></param>
        /// <returns></returns>
        public string UpdateShoppingItemStatus(int id, string itemStatus)
        {
            try
            {
                var sql = "update";
                if (itemStatus == "completed")
                    sql = "UPDATE ShoppingItems SET itemStatus='" + itemStatus + "', updated_at='" + dateUpdated + "', completed_at='" + dateUpdated + "' WHERE _id='" + id + "';";
                else
                    sql = "UPDATE ShoppingItems SET itemStatus='" + itemStatus + "', updated_at='" + dateUpdated + "', completed_at='" + null + "' WHERE _id='" + id + "';";

                db.ExecSQL(sql);
                return "ok";
            }
            catch (Exception ex)
            {
                return "Error:" + ex.Message;
            }
        }

        //Delete Shopping Item
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public string DeleteShoppingItem(int item)
        {
            try
            {
                var sql = "DELETE FROM ShoppingItems WHERE _id = " + item + ";";
                db.ExecSQL(sql);

                return "ok";
            }
            catch (Exception ex)
            {
                return "Error:" + ex.Message;
            }
        }

        //Delete Shopping Item
        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public string DeleteShoppingListItems(int list)
        {
            try
            {
                var sql = "DELETE FROM ShoppingItems WHERE list = " + list + ";";
                db.ExecSQL(sql);

                return "ok";
            }
            catch (Exception ex)
            {
                return "Error:" + ex.Message;
            }
        }

        //Create Project
        /// <summary>
        /// 
        /// </summary>
        /// <param name="project"></param>
        /// <param name="projectDesc"></param>
        /// <param name="projectStart"></param>
        /// <param name="projectDeadline"></param>
        /// <param name="projectBudget"></param>
        /// <returns></returns>
        public string CreateProject(string project, string projectDesc, string projectStart, string projectDeadline, decimal projectBudget)
        {
            try
            {
                string sql = "INSERT INTO Projects (project, projectDesc, projectStart, projectDeadline, projectBudget, projectStatus, created_at)" +
                              "VALUES(" + project + ", " + projectDesc + ", '" + projectStart + "','" + projectDeadline + "','" + projectBudget + "','" + defaultStatus + "','" + dateAdded + "');";

                db.ExecSQL(sql);
                return "ok";
            }
            catch (Exception ex)
            {
                return "Error:\n" + ex.Message;
            }
            finally
            {
                db.Close();
            }
        }

        //Read All Projects
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ICursor ReadAllProjects()
        {
            ICursor output = null;

            try
            {
                string sql = "SELECT * FROM Projects";
                output = db.RawQuery(sql, null);

                if (!(output != null))
                {
                    Console.WriteLine("No records found");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error:\n" + ex.Message);
            }

            return output;
        }

        //Read All Goals Deadline
        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public ICursor ReadProjectsDeadline(DateTime date)
        {
            ICursor output = null;
            string cur = date.ToShortDateString();

            try
            {
                string sql = "SELECT * FROM Projects WHERE projectDeadline LIKE '%" + cur + "%'";
                output = db.RawQuery(sql, null);

                if (!(output != null))
                {
                    Console.WriteLine("No records found");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error:\n" + ex.Message);
            }

            return output;
        }

        //Read Project
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string[] ReadProject(int id)
        {
            ICursor output = null;

            //sql
            var sql = "SELECT * FROM Projects WHERE _id = " + id;
            output = db.RawQuery(sql, null);

            string[] myproject = new string[10];

            output.MoveToFirst();
            myproject[0] = output.GetString(0);
            myproject[1] = output.GetString(1);
            myproject[2] = output.GetString(2);
            myproject[3] = output.GetString(3);
            myproject[4] = output.GetString(4);
            myproject[5] = output.GetString(5);
            myproject[6] = output.GetString(6);

            return myproject;
        }

        //Update Project
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="project"></param>
        /// <param name="projectDesc"></param>
        /// <param name="projectStart"></param>
        /// <param name="projectDeadline"></param>
        /// <param name="projectBudget"></param>
        /// <param name="projectStatus"></param>
        /// <returns></returns>
        public string UpdateProject(int id, string project, string projectDesc, string projectStart, string projectDeadline, decimal projectBudget, string projectStatus)
        {
            try
            {
                var sql = "update";

                if (projectStatus == "completed")
                    sql = "UPDATE Projects SET project=" + project + ", projectDesc=" + projectDesc + ", projectStart='" + projectStart + "', projectDeadline='" + projectDeadline + "', projectBudget='" + projectBudget + "', projectStatus='" + projectStatus + "', updated_at='" + dateUpdated + "', completed_at='" + dateUpdated + "' WHERE _id='" + id + "';";
                else
                    sql = "UPDATE Projects SET project=" + project + ", projectDesc=" + projectDesc + ", projectStart='" + projectStart + "', projectDeadline='" + projectDeadline + "', projectBudget='" + projectBudget + "', projectStatus='" + projectStatus + "', updated_at='" + dateUpdated + "', completed_at='" + null + "' WHERE _id='" + id + "';";

                db.ExecSQL(sql);

                return "ok";
            }
            catch (Exception ex)
            {
                return "Error:" + ex.Message;
            }
        }

        //Update Project Status
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="projectStatus"></param>
        /// <returns></returns>
        public string UpdateProjectStatus(int id, string projectStatus)
        {
            try
            {
                var sql = "update";

                if (projectStatus == "completed")
                    sql = "UPDATE Projects SET projectStatus='" + projectStatus + "', updated_at='" + dateUpdated + "', completed_at='" + dateUpdated + "' WHERE _id='" + id + "';";
                else
                    sql = "UPDATE Projects SET projectStatus='" + projectStatus + "', updated_at='" + dateUpdated + "', completed_at='" + null + "' WHERE _id='" + id + "';";

                db.ExecSQL(sql);

                return "ok";
            }
            catch (Exception ex)
            {
                return "Error:" + ex.Message;
            }
        }

        //Delete Project
        /// <summary>
        /// 
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public string DeleteProject(int project)
        {
            try
            {
                var sql = "DELETE FROM Projects WHERE _id = " + project + ";";
                db.ExecSQL(sql);

                return "ok";
            }
            catch (Exception ex)
            {
                return "Error:" + ex.Message;
            }
        }

        //Delete all goals
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string DeleteAllProjects()
        {
            try
            {
                db.Delete("Projects", null, null);
                return "ok";
            }
            catch (Exception ex)
            {
                return "Error:" + ex.Message;
            }
        }

        //Create Project Tasks
        /// <summary>
        /// 
        /// </summary>
        /// <param name="task"></param>
        /// <param name="project"></param>
        /// <param name="taskDesc"></param>
        /// <param name="taskStart"></param>
        /// <param name="taskDeadline"></param>
        /// <param name="expectedCost"></param>
        /// <returns></returns>
        public string CreateProjectTask(string task, int project, string taskDesc, string taskStart, string taskDeadline, decimal expectedCost)
        {
            try
            {
                string sql = "INSERT INTO ProjectTasks (task, project, taskDesc, taskStart, taskDeadline, expectedCost, taskStatus, created_at)" +
                              "VALUES(" + task + ",'" + project + "'," + taskDesc + ",'" + taskStart + "','" + taskDeadline + "','" + expectedCost + "','" + defaultStatus + "','" + dateAdded + "');";

                db.ExecSQL(sql);
                return "ok";
            }
            catch (Exception ex)
            {
                return "Error:\n" + ex.Message;
            }
            finally
            {
                db.Close();
            }
        }

        //Read All Project Tasks
        /// <summary>
        /// 
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public ICursor ReadAllProjectTasks(int project)
        {
            ICursor output = null;

            try
            {
                string sql = "SELECT * FROM ProjectTasks WHERE project = '" + project + "';";
                output = db.RawQuery(sql, null);

                if (!(output != null))
                {
                    Console.WriteLine("No records found");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error:\n" + ex.Message);
            }

            return output;
        }

        //Read Project Task
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string[] ReadProjectTask(int id)
        {
            ICursor output = null;
            var sql = "SELECT * FROM ProjectTasks WHERE _id = " + id;
            output = db.RawQuery(sql, null);

            string[] mygoal = new string[10];
            //var con = new SQLiteConnection(dbPath);
            //var goal = con.Table<Goals>().FirstOrDefault(u => u.Id == id);

            output.MoveToFirst();
            mygoal[0] = output.GetString(0);
            mygoal[1] = output.GetString(1);
            mygoal[2] = output.GetString(3);
            mygoal[3] = output.GetString(4);
            mygoal[4] = output.GetString(5);
            mygoal[5] = output.GetString(8);

            return mygoal;
        }

        //Update Project Task
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="task"></param>
        /// <param name="project"></param>
        /// <param name="taskDesc"></param>
        /// <param name="taskDeadline"></param>
        /// <param name="actualCost"></param>
        /// <param name="taskStatus"></param>
        /// <returns></returns>
        public string UpdateProjectTask(int id, string task, int project, string taskDesc, string taskDeadline, decimal actualCost, string taskStatus)
        {
            try
            {
                var sql = "update";

                if (taskStatus == "completed")
                    sql = "UPDATE ProjectTasks SET task=" + task + ", project='" + project + "', taskDesc=" + taskDesc + ", taskDeadline='" + taskDeadline + "', actualCost='" + actualCost + "', taskStatus ='" + taskStatus + "', updated_at='" + dateUpdated + "', completed_at='" + dateUpdated + "' WHERE _id='" + id + "';";
                else
                    sql = "UPDATE ProjectTasks SET task=" + task + ", project='" + project + "', taskDesc=" + taskDesc + ", taskDeadline='" + taskDeadline + "', actualCost='" + actualCost + "', taskStatus ='" + taskStatus + "', updated_at='" + dateUpdated + "', completed_at='" + null + "' WHERE _id='" + id + "';";

                db.ExecSQL(sql);
                return "ok";
            }
            catch (Exception ex)
            {
                return "Error:" + ex.Message;
            }
        }

        //Update Project Task Status
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="taskStatus"></param>
        /// <returns></returns>
        public string UpdateProjectTaskStatus(int id, string taskStatus)
        {
            try
            {
                var sql = "update";

                if (taskStatus == "completed")
                    sql = "UPDATE ProjectTasks SET taskStatus ='" + taskStatus + "', updated_at='" + dateUpdated + "', completed_at='" + dateUpdated + "' WHERE _id='" + id + "';";
                else
                    sql = "UPDATE ProjectTasks SET taskStatus ='" + taskStatus + "', updated_at='" + dateUpdated + "', completed_at='" + null + "' WHERE _id='" + id + "';";


                db.ExecSQL(sql);
                return "ok";
            }
            catch (Exception ex)
            {
                return "Error:" + ex.Message;
            }
        }

        //Delete Project Tasks
        /// <summary>
        /// 
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public string DeleteProjectTask(int task)
        {
            try
            {
                var sql = "DELETE FROM ProjectTasks WHERE _id = " + task + ";";
                db.ExecSQL(sql);

                return "ok";
            }
            catch (Exception ex)
            {
                return "Error:" + ex.Message;
            }
        }

        //Delete All Project Tasks
        /// <summary>
        /// 
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public string DeleteAllProjectTasks(int project)
        {
            try
            {
                var sql = "DELETE FROM ProjectTasks WHERE project = " + project + ";";
                db.ExecSQL(sql);

                return "ok";
            }
            catch (Exception ex)
            {
                return "Error:" + ex.Message;
            }
        }

        //Create Budget
        /// <summary>
        /// 
        /// </summary>
        /// <param name="budget"></param>
        /// <param name="budgetDesc"></param>
        /// <param name="budgetFrom"></param>
        /// <param name="budgetTo"></param>
        /// <param name="budgetExpected"></param>
        /// <returns></returns>
        public string CreateBudget(string budget, string budgetDesc, string budgetFrom, string budgetTo, decimal budgetExpected)
        {
            try
            {
                string sql = "INSERT INTO Budgets (budget, budgetDesc, budgetFrom, budgetTo, budgetExpected, budgetStatus, created_at)" +
                              "VALUES(" + budget + ", " + budgetDesc + ", '" + budgetFrom + "','" + budgetTo + "','" + budgetExpected + "','" + defaultStatus + "','" + dateAdded + "');";

                db.ExecSQL(sql);
                return "ok";
            }
            catch (Exception ex)
            {
                return "Error:\n" + ex.Message;
            }
            finally
            {
                db.Close();
            }
        }

        //Read All Budgets
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ICursor ReadAllBudgets()
        {
            ICursor output = null;

            try
            {
                string sql = "SELECT * FROM Budgets";
                output = db.RawQuery(sql, null);

                if (!(output != null))
                {
                    Console.WriteLine("No records found");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error:\n" + ex.Message);
            }

            return output;
        }

        //Update Budget
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="budget"></param>
        /// <param name="budgetDesc"></param>
        /// <param name="budgetFrom"></param>
        /// <param name="budgetActual"></param>
        /// <param name="budgetTo"></param>
        /// <param name="budgetStatus"></param>
        /// <returns></returns>
        public string UpdateBudget(int id, string budget, string budgetDesc, string budgetFrom, decimal budgetActual, string budgetTo, string budgetStatus)
        {
            try
            {
                var sql = "UPDATE Budgets SET budget='" + budget + "', budgetDesc='" + budgetDesc + "', budgetFrom='" + budgetFrom + "', budgetTo='" + budgetTo + "', budgetActual='" + budgetActual + "', budgetStatus='" + budgetStatus + "', updated_at='" + dateUpdated + "' WHERE _id='" + id + "';";

                db.ExecSQL(sql);
                return "ok";
            }
            catch (Exception ex)
            {
                return "Error:" + ex.Message;
            }
        }

        //Delete Budget
        /// <summary>
        /// 
        /// </summary>
        /// <param name="budget"></param>
        /// <returns></returns>
        public string DeleteBudget(int budget)
        {
            try
            {
                var sql = "DELETE FROM Budgets WHERE _id = " + budget + ";";
                db.ExecSQL(sql);

                return "ok";
            }
            catch (Exception ex)
            {
                return "Error:" + ex.Message;
            }
        }

        //Delete all goals
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string DeleteAllBudgets()
        {
            try
            {
                db.Delete("Budgets", null, null);
                return "ok";
            }
            catch (Exception ex)
            {
                return "Error:" + ex.Message;
            }
        }

        //Create Budget Item
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="budget"></param>
        /// <param name="itemDesc"></param>
        /// <param name="itemGroup"></param>
        /// <param name="itemExpected"></param>
        /// <returns></returns>
        public string CreateBudgetItem(string item, int budget, string itemDesc, string itemGroup, decimal itemExpected)
        {
            //+ "item VARCHAR, "
            //+ "budget INTEGER, "
            //+ "itemDesc TEXT, "
            //+ "itemGroup VARCHAR, "
            //+ "itemExpected DECIMAL(10,5), "
            //+ "itemActual DECIMAL(10,5), "
            //+ "created_at VARCHAR "
            //+ "updated_at VARCHAR, "
            try
            {
                string sql = "INSERT INTO BudgetItems (item, budget, itemDesc, itemGroup, itemExpected, created_at)" +
                              "VALUES('" + item + "','" + budget + "','" + itemDesc + "','" + itemGroup + "','" + itemExpected + "','" + dateAdded + "');";

                db.ExecSQL(sql);
                return "ok";
            }
            catch (Exception ex)
            {
                return "Error:\n" + ex.Message;
            }
        }

        //Read All Budget Items
        /// <summary>
        /// 
        /// </summary>
        /// <param name="budget"></param>
        /// <returns></returns>
        public ICursor ReadAllBudgetItems(int budget)
        {
            ICursor output = null;

            try
            {
                string sql = "SELECT * FROM BudgetItems WHERE budget = '" + budget + "';";
                output = db.RawQuery(sql, null);

                if (!(output != null))
                {
                    Console.WriteLine("No records found");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error:\n" + ex.Message);
            }

            return output;
        }

        //Update Budget Item
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="item"></param>
        /// <param name="budget"></param>
        /// <param name="itemDesc"></param>
        /// <param name="itemGroup"></param>
        /// <param name="itemActual"></param>
        /// <returns></returns>
        public string UpdateBudgetItem(int id, string item, int budget, string itemDesc, string itemGroup, decimal itemActual)
        {
            try
            {
                var sql = "UPDATE BudgetItems SET item='" + item + "', budget='" + budget + "', itemDesc='" + itemDesc + "', itemGroup='" + itemGroup + "', itemActual='" + itemActual + "', updated_at='" + dateUpdated + "' WHERE _id='" + id + "';";

                db.ExecSQL(sql);
                return "ok";
            }
            catch (Exception ex)
            {
                return "Error:" + ex.Message;
            }
        }

        //Delete Budget Item
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public string DeleteBudgetItem(int item)
        {
            try
            {
                var sql = "DELETE FROM BudgetItems WHERE _id = " + item + ";";
                db.ExecSQL(sql);

                return "ok";
            }
            catch (Exception ex)
            {
                return "Error:" + ex.Message;
            }
        }

    }
}