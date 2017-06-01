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
using Java.Lang;
using SmartDiary.Droid.Views;
using Android.Util;

namespace SmartDiary.Droid.mServices
{
    [Service]
    public class ProjectsService : Service
    {
        protected const int notifyId = 2000;

        IBinder _myBinder = null;

        //Invoke on start of service
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
            Thread t = new Thread(() =>
            {
                Thread.Sleep(6000);
                if (ProjectsCollection.CheckItem(DateTime.Now))
                {
                    var nMgr = (NotificationManager)GetSystemService(NotificationService);

                    //Details of notification in previous recipe
                    Notification.Builder builder = new Notification.Builder(this)
                    .SetAutoCancel(true)
                    .SetContentTitle("Project Deadline")
                    .SetNumber(notifyId)
                    .SetContentText("Some project deadline are today. Please check to update progress.")
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