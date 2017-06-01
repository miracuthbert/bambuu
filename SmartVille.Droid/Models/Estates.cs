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

namespace SmartVille.Droid.Models
{
    public class Estates
    {
        private long estateId;
        private string estateName;
        private int regStatus;
        private string regDate;

        public long Id
        {
            get
            {
                return estateId;
            }

            set
            {
                estateId = value;
            }
        }

        public string EstateName
        {
            get
            {
                return estateName;
            }

            set
            {
                estateName = value;
            }
        }

        public int RegStatus
        {
            get
            {
                return regStatus;
            }

            set
            {
                regStatus = value;
            }
        }

        public string RegDate
        {
            get
            {
                return regDate;
            }

            set
            {
                regDate = value;
            }
        }
    }
}