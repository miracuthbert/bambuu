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
using Java.Lang;
using SmartDiary.Droid.Views;

namespace SmartDiary.Droid.mServices
{
    [Service]
    //[IntentFilter(new String[] { "ca.services.GoalsService" })]
    public class GoalsService : Service
    {
        protected const int notifyId = 1000;

        IBinder _myBinder = null;

        //Invoke on start of service
        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            Log.Debug("GoalsService", "StartCommandResult");
            //this._myBinder = new GoalsServiceBinder(this);
            CheckUpdate();
            return StartCommandResult.Sticky;
            //return base.OnStartCommand(intent, flags, startId);
        }

        //check if goal near deadline
        public void CheckUpdate()
        {
            Thread t = new Thread(() =>
            {
                Thread.Sleep(6000);
                if (GoalsCollection.CheckItem(DateTime.Now))
                {
                    var nMgr = (NotificationManager)GetSystemService(NotificationService);

                    //Details of notification in previous recipe
                    Notification.Builder builder = new Notification.Builder(this)
                    .SetAutoCancel(true)
                    .SetContentTitle("Goals Deadline")
                    .SetNumber(notifyId)
                    .SetContentText("Some goals deadline are today. Please check to update progress.")
                    .SetVibrate(new long[] { 100, 200, 300 })
                    .SetSmallIcon(Resource.Drawable.ic_bell);

                    nMgr.Notify(0, builder.Build());

                }
            });
            t.Start();
        }

        public override IBinder OnBind(Intent intent)
        {
            throw new NotImplementedException();
        }
    }
}