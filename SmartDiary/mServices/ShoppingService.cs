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
using Android.Util;
using System.Threading;
using SmartDiary.Droid.Views;

namespace SmartDiary.Droid.mServices
{
    [Service]
    public class ShoppingService : Service
    {
        protected const int notifyId = 3000;

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            Log.Debug("ProjectsService", "StartCommandResult");
            //this._myBinder = new GoalsServiceBinder(this);
            CheckUpdate();
            return StartCommandResult.Sticky;
            //return base.OnStartCommand(intent, flags, startId);
        }

        //check if goal near deadline
        public void CheckUpdate()
        {
            DateTime current = DateTime.Now;
            int z = ShoppingCollection.CheckItem(current).Count;

            Console.WriteLine("Shopping service: " + z);

            //Toast.MakeText(this, "Shopping lists count: " + z, ToastLength.Long).Show();

            //while (i < z)
            //{
            //    string title = ShoppingCollection.CheckItem(current)[i].ListTitle;

            //    Toast.MakeText(this, "Shopping lists today: " + title, ToastLength.Long).Show();
            //}

            Thread t = new Thread(() =>
            {
                Thread.Sleep(1000);
                if (ShoppingCollection.CheckItem(DateTime.Now).Count > 0)
                {
                    var nMgr = (NotificationManager)GetSystemService(NotificationService);

                    //Details of notification in previous recipe
                    Notification.Builder builder = new Notification.Builder(this)
                    .SetAutoCancel(true)
                    .SetContentTitle("Shopping Day")
                    .SetNumber(notifyId)
                    .SetContentText("You have got some shopping to do today.")
                    .SetVibrate(new long[] { 100, 200, 300 })
                    .SetSmallIcon(Resource.Drawable.ic_cart);

                    nMgr.Notify(0, builder.Build());

                }
            });
            t.Start();
        }
    }
}