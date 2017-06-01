using System;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using SmartVille.Droid.Models;
using Android.Support.V7.Widget;
using Newtonsoft.Json;
using Org.Json;
using SmartVille.Droid.Fragments;
using System.Net;
using System.Threading.Tasks;
using System.Net.Http;
using System.Json;
using System.IO;

namespace SmartVille.Droid.Adapters
{
    public class EstatesAdapter : AsyncTask
    {

        //@param mContext
        private Context mContext;

        //@param mUrl
        private string mJsonData;

         //@param mRecyclerView
        private RecyclerView mRecyclerView;

        //@param mProgressBar
        private ProgressDialog mProgressDialog;

        //@param mEstates
        private static JavaList<Estates> mEstates = null;

        //@param mEstate
        Estates mEstate = null;


        public EstatesAdapter(Context context, string json, RecyclerView recyclerView)
        {
            mContext = context;
            mJsonData = json;
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
            return ParseData();
        }

        //Override OnPostExecute Methid
        protected override void OnPostExecute(Java.Lang.Object result)
        {
            base.OnPostExecute(result);

            //Dismiss progress dialog
            mProgressDialog.Dismiss();

            //Check if @param result is null
            if ((bool) result)
            {

                ////Get adapter
                //var mDashboardRecyclerAdapter = new DashboardRecyclerAdapter(EstatesAdapter.GetAllEstates(), mRecyclerView);

                ////on click event
                //mDashboardRecyclerAdapter.ItemClick += MDashboardRecyclerAdapter_ItemClick;

                ////Set adapter
                //mRecyclerView.SetAdapter(mDashboardRecyclerAdapter);

            }
            else
            {
                Toast.MakeText(mContext, "Failed fetching data. Try again!!", ToastLength.Short).Show();
            }

        }

        void MDashboardRecyclerAdapter_ItemClick(object sender, int position)
        {
            string id = mEstates[position].ToString();
            Toast.MakeText(mRecyclerView.Context, "Estate Id:" + id, ToastLength.Short).Show();
        }

        //ParseData Method
        private Boolean ParseData()
        {
            try
            {
                JSONArray mJsonArray = new JSONArray(mJsonData);
                JSONObject mJsonObject;

                //new estate javalist
                mEstates = new JavaList<Estates>();
                for (int i = 0; i < mJsonArray.Length(); i++)
                {
                    mJsonObject = mJsonArray.GetJSONObject(i);

                    //retrieve parsed data
                    long id = mJsonObject.GetLong("estateId");
                    string name = mJsonObject.GetString("estateName");
                    string email = mJsonObject.GetString("estateEmail");
                    string telno = mJsonObject.GetString("estateTelNo");
                    int status = mJsonObject.GetInt("regStatus");
                    string added = mJsonObject.GetString("regdate");

                    //new instance of estates
                    mEstate = new Estates();

                    mEstate.Id = id;
                    mEstate.EstateName = name;
                    mEstate.RegStatus = status;
                    mEstate.RegDate = added;

                    mEstates.Add(mEstate);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }


        private static HttpClient mClient;
        private static Uri mUrl;

        public async static Task<JavaList<Estates>> GetEstates(Context context)
        {

            //try
            //{
            //mEstates = new JavaList<Estates>();
            //mEstates.Clear();

            mClient = new HttpClient();
            mUrl = new Uri("https://apismartville-miracuthbert.c9users.io/public/api/estates");

            var mTask = mClient.GetStringAsync(mUrl);

            var content = await mTask.ConfigureAwait(false);

            var results = JsonConvert.DeserializeObject<Task<JavaList<Estates>>>(content);

            Console.WriteLine(results);
            Toast.MakeText(context, results.ToString(), ToastLength.Long);

            //}
            //catch (Exception ex)
            //{
            //    Toast.MakeText(context, "Error : " + ex.StackTrace + "\nSource : " + ex.Source + "\nMessage : " + ex.Message + "\n : " + ex.InnerException, ToastLength.Long);
            //}

            return await results;
        }


        public static async Task<JsonValue> FetchEstatesAsync(string url)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
            request.ContentType = "application/json";
            request.Method = "GET";

            //SEND AND WAIT
            using (WebResponse response = await request.GetResponseAsync())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    JsonValue jsonDoc = await Task.Run(() => JsonObject.Load(stream));

                    Console.Out.WriteLine("Response: {0}", jsonDoc.ToString());

                    return jsonDoc;
                }
            }
        }

        public static JavaList<Estates> GetAllEstates(JsonValue json)
        {
            var results = JsonConvert.DeserializeObject<JavaList<Estates>>(json);

            return results;
        }
    }
}