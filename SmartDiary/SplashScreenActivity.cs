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
using SmartDiary.Droid.mServices;

namespace SmartDiary.Droid
{
    [Activity(Theme = "@style/mySplashScreen.Theme", MainLauncher = true, NoHistory = true)]
    public class SplashScreenActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            StartService(new Intent(this, typeof(GoalsService)));
            StartService(new Intent(this, typeof(ProjectsService)));
            StartService(new Intent(this, typeof(ShoppingService)));

            StartActivity(typeof(DashboardActivity));
        }
    }
}