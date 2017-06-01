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

namespace SmartDiary.Droid.Receivers
{
    [BroadcastReceiver]
    public class StartReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            Intent goalIntent = new Intent(context, typeof(GoalsService));
            Intent projectIntent = new Intent(context, typeof(ProjectsService));
            Intent shoppingIntent = new Intent(context, typeof(ShoppingService));

            Application.Context.StartService(goalIntent);
            Application.Context.StartService(projectIntent);
            Application.Context.StartService(shoppingIntent);
        }
    }
}