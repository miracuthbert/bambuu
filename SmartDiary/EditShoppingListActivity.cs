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
using Android.Support.V7.App;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using SmartDiary.Droid.ViewModel;
using Android.Database;

namespace SmartDiary.Droid
{
    [Activity(Label = "Edit List", Icon = "@drawable/storelogo", Theme = "@style/MyTheme")]
    public class EditShoppingListActivity : AppCompatActivity
    {
        private EditText list;
        private EditText listDesc;
        private EditText listBudget;
        private TextView shoppingDate;
        private TextView shoppingDateReset;
        private Spinner listStatus;
        private int selListId;

        protected int SelListId
        {
            get
            {
                return selListId;
            }

            set
            {
                selListId = value;
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.EditShoppingList);

            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetHomeButtonEnabled(true);

            //Get passed goal id
            SelListId = Convert.ToInt32(Intent.Extras.Get("ListId").ToString());

            list = FindViewById<EditText>(Resource.Id.txtListTitle);
            listDesc = FindViewById<EditText>(Resource.Id.txtListDescription);
            listBudget = FindViewById<EditText>(Resource.Id.txtListBudget);
            shoppingDate = FindViewById<TextView>(Resource.Id.shoppingDate);
            shoppingDateReset = FindViewById<TextView>(Resource.Id.shoppingDateReset);
            listStatus = FindViewById<Spinner>(Resource.Id.spinSListStatus);

            //shopping date click
            shoppingDate.Click += ShoppingDate_Click;

            //shopping date reset
            shoppingDateReset.Click += delegate
            {
                DBHelper dbh = new DBHelper();
                string[] values = dbh.ReadShoppingList(SelListId);
                shoppingDate.Text = values[3];
            };

            //populate activity
            populateActivity(SelListId);

        }

        //populate activity
        private void populateActivity(int id)
        {
            DBHelper dbh = new DBHelper();
            string[] values = dbh.ReadShoppingList(id);
            list.Text = values[1];
            listDesc.Text = values[2];
            shoppingDate.Text = values[3];
            listBudget.Text = values[4];

            if (values[6].Equals("Pending"))
            {
                listStatus.SetSelection(0);
            }
            else if (values[6].Equals("Postponed"))
            {
                listStatus.SetSelection(1);
            }
            else if (values[6].Equals("Completed"))
            {
                listStatus.SetSelection(0);
            }
        }

        //shopping date click
        private void ShoppingDate_Click(object sender, EventArgs e)
        {
            DBHelper dbh = new DBHelper();
            string[] values = dbh.ReadShoppingList(SelListId);

            DatePickerFragment dpf = new DatePickerFragment(DateTime.Parse(values[3]));

            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
            {
                shoppingDate.Text = time.ToShortDateString();
            });
            frag.Show(FragmentManager, DatePickerFragment.TAG);
        }

        //Override On create menu
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.add_cancel_menu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        //Override On Options Selected
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.menu_new_add:
                    ListCreate();
                    return true;

                case Resource.Id.menu_new_cancel:
                    Finish();
                    return true;

                case Android.Resource.Id.Home:
                    Finish();
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        //List Create
        private void ListCreate()
        {
            try
            {
                if (list.Text.Equals("") || listDesc.Text.Equals("") || listBudget.Text.Equals("") || shoppingDate.Text.Equals(""))
                {
                    Toast.MakeText(this, "Fill in all fields!", ToastLength.Long).Show();
                    return;
                }
                else
                {
                    if (DateTime.Parse(shoppingDate.Text) < DateTime.Today)
                    {
                        Toast.MakeText(this, "Shopping date should be greater than or equal to current date!", ToastLength.Long).Show();
                        return;
                    }
                    else
                    {
                        DBHelper dbh = new DBHelper();

                        string title = DatabaseUtils.SqlEscapeString(list.Text);
                        string details = DatabaseUtils.SqlEscapeString(listDesc.Text);
                        string date = shoppingDate.Text;
                        decimal budget = Convert.ToDecimal(listBudget.Text);
                        int stat = listStatus.SelectedItemPosition;
                        string status = "Pending";

                        if (stat == 0)
                        {
                            status = "Pending";
                        }
                        if (stat == 1)
                        {
                            status = "Postponed";
                        }
                        if (stat == 2)
                        {
                            status = "Completed";
                        }

                        string result = dbh.UpdateShoppingList(SelListId, title, details, date, budget, status);

                        if (result.Equals("ok"))
                        {
                            Toast.MakeText(this, "Shopping list updated!", ToastLength.Short).Show();
                            Finish();
                        }
                        else
                        {
                            Toast.MakeText(this, result, ToastLength.Short).Show();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, "Error:\n" + ex.Message, ToastLength.Long);
            }
        }
    }
}