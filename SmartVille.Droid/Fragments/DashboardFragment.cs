using System;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using SupportFragment = Android.Support.V4.App.Fragment;
using System.Collections.Generic;
using SmartVille.Droid.ViewModel;
using Android.Graphics;
using Android.Widget;
using Android.Content;
using Android.Util;
using Android.Runtime;
using SmartVille.Droid.Models;
using SmartVille.Droid.Adapters;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Json;
using System.IO;

namespace SmartVille.Droid.Fragments
{
    public class DashboardFragment : SupportFragment
    {
        private RecyclerView.LayoutManager mLayoutManager;
        DashboardRecyclerAdapter mDashboardRecyclerAdapter;
        private RecyclerView view;
        private int selItem;
        private static string mUrl = "https://apismartville-miracuthbert.c9users.io/public/api/estates";
        JsonValue mJson;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
            LoadData();
        }

        private async void LoadData()
        {
            mJson = await EstatesAdapter.FetchEstatesAsync(mUrl);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            RecyclerView recyclerView = inflater.Inflate(Resource.Layout.Dashboard, container, false) as RecyclerView;

            SetUpRecyclerView(recyclerView);

            return recyclerView;
        }

        private void SetUpRecyclerView(RecyclerView recyclerView)
        {
            try
            {
                //Task list
                JavaList<Estates> mTaskList = EstatesAdapter.GetAllEstates(mJson);

                //Create recycler layout manager
                mLayoutManager = new LinearLayoutManager(recyclerView.Context);

                //Set recycler layout manager
                recyclerView.SetLayoutManager(mLayoutManager);

                //new DownloadAdapter(recyclerView.Context, mUrl, "GET", recyclerView).Execute();

                //Get adapter
                mDashboardRecyclerAdapter = new DashboardRecyclerAdapter(mTaskList, recyclerView);

                //on click event
                mDashboardRecyclerAdapter.ItemClick += MDashboardRecyclerAdapter_ItemClick;

                //Set adapter
                recyclerView.SetAdapter(mDashboardRecyclerAdapter);


            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : " + ex.Message + " trace : " + ex.StackTrace);
            }
        }

        private void MDashboardRecyclerAdapter_ItemClick(object sender, int position)
        {
            string id = mDashboardRecyclerAdapter.GetItemId(position).ToString();
            Toast.MakeText(view.Context, "Estate Id:" + id, ToastLength.Short).Show();
        }
    }

    public class DashboardRecyclerAdapter : RecyclerView.Adapter
    {
        // Event handler for item clicks:
        public event EventHandler<int> ItemClick;

        private JavaList<Estates> mEstates;
        private RecyclerView mRecyclerView;
        private Task<JavaList<Estates>> task;
        private RecyclerView recyclerView;

        public DashboardRecyclerAdapter(JavaList<Estates> estates, RecyclerView recyclerView)
        {
            mEstates.Add(estates);
            mRecyclerView = recyclerView;
            Toast.MakeText(recyclerView.Context, estates.ToString(), ToastLength.Short);
        }

        public override int ItemCount
        {
            get
            {
                return mEstates.Size();
            }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            DashboardMainView mHolder = holder as DashboardMainView;

            mHolder.mEstName.Text = mEstates[position].EstateName;
        }

        // Raise an event when the item-click takes place:
        void OnClick(int position)
        {
            if (ItemClick != null)
                ItemClick(this, position);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.row_dash_main, parent, false);

            return new DashboardMainView(view, OnClick);
        }

        public override long GetItemId(int position)
        {
            return mEstates[position].Id;
        }
    }

    public class DashboardMainView : RecyclerView.ViewHolder
    {
        public readonly View mView;
        public TextView mEstName;
        public TextView mEstExtra;

        public DashboardMainView(View view, Action<int> listener) : base(view)
        {
            mView = view;
            mEstName = mView.FindViewById<TextView>(Resource.Id.txtRow1);
            mEstExtra = mView.FindViewById<TextView>(Resource.Id.txtRow2);
        }
    }

}