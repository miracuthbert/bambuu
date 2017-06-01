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
using Java.Net;

namespace SmartVille.Droid.Models
{
    public class Model
    {
        public static HttpURLConnection DbConnect(string url, string method, int timeout, bool input)
        {
            try
            {
                URL mUrl = new URL(url);
                HttpURLConnection conn = (HttpURLConnection) mUrl.OpenConnection();

                //Set connection properties
                conn.RequestMethod = method;
                conn.ConnectTimeout = timeout;
                conn.ReadTimeout = timeout;
                conn.DoInput = input;

                //Return conn
                return conn;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }
    }
}