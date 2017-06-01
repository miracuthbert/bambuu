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
using Android.Support.V7.Widget;
using Java.Net;
using SmartVille.Droid.Models;
using System.IO;
using Java.IO;
using Java.Lang;

namespace SmartVille.Droid.Adapters
{
    public class DownloadAdapter : AsyncTask
    {

        //@param mContext
        private Context mContext;

        //@param mUrl
        private string mUrl;

        //@param mMethod
        private string mMethod = "GET";

        //@param mTimeout
        private int mTimeout = 30000;

        //@param mRecyclerView
        private RecyclerView mRecyclerView;

        //@param mProgressBar
        private ProgressDialog mProgressDialog;

        public DownloadAdapter(Context context, string url, string method, RecyclerView recyclerView)
        {
            mContext = context;
            mUrl = url;
            mMethod = method;
            mRecyclerView = recyclerView;
        }

        //Override OnPreExecute Method
        protected override void OnPreExecute()
        {
            base.OnPreExecute();

            mProgressDialog = new ProgressDialog(mContext);
            mProgressDialog.SetTitle("Status:");
            mProgressDialog.SetMessage("Loading. Please wait...");
            mProgressDialog.Show();
        }

        //Override and Run Background Process
        protected override Java.Lang.Object DoInBackground(params Java.Lang.Object[] @params)
        {
            return DownloadData();
        }

        //Override OnPostExecute Methid
        protected override void OnPostExecute(Java.Lang.Object result)
        {
            base.OnPostExecute(result);

            //Dismiss progress dialog
            mProgressDialog.Dismiss();

            //Check if @param result is null
            if (result == null)
            {
                Toast.MakeText(mContext, "Failed fetching data. Try again!\n" + result, ToastLength.Short).Show();
            }
            else
            {
                //Data Parser
                new EstatesAdapter(mContext, result.ToString(), mRecyclerView).Execute();
            }

        }

        //DownloadData Method
        private string DownloadData()
        {
            HttpURLConnection conn = Model.DbConnect(mUrl, mMethod, mTimeout, true);

            if (conn == null)
            {
                return null;
            }

            try
            {
                Stream mStream = new BufferedStream(conn.InputStream);
                BufferedReader mBufferReader = new BufferedReader(new InputStreamReader(mStream));

                string mJson;
                StringBuffer mJsonData = new StringBuffer();

                while ((mJson = mBufferReader.ReadLine()) != null)
                {
                    mJsonData.Append(mJson);
                    Toast.MakeText(mContext, "Failed fetching data. Try again!\n" + mJson, ToastLength.Short).Show();
                }

                //close buffers
                mBufferReader.Close();
                mStream.Close();

                Toast.MakeText(mContext, mJsonData.ToString(), ToastLength.Short).Show();

                return mJsonData.ToString();
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }

            return null;
        }

    }
}