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
using Android.Support.Design.Widget;

namespace SmartVille.Droid
{
    [Activity(Label = "@string/ApplicationName", MainLauncher = true, Theme = "@style/Theme.DesignDefault")]
    public class LoginActivity : Activity
    {
        private Button btnLogin;
        private TextInputLayout emailWrapper;
        private TextInputLayout passwordWrapper;
        private string mEmail;
        private string mPassword;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.Login);

            emailWrapper = FindViewById<TextInputLayout>(Resource.Id.txtInputLayoutEmail);

            passwordWrapper = FindViewById<TextInputLayout>(Resource.Id.txtInputLayoutPassword);

            btnLogin = FindViewById<Button>(Resource.Id.btnLogin);
            btnLogin.Click += BtnLogin_Click;
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            mEmail = emailWrapper.EditText.Text;
            mPassword = passwordWrapper.EditText.Text;


            if (mEmail.Equals("")) { emailWrapper.Error = "Please enter email!"; }

            if (mPassword.Equals("")) { passwordWrapper.Error = "Please enter password!"; }

            else if (mEmail != "" && mPassword != "")
            {
                Intent intent = new Intent(this, typeof(MainActivity));
                StartActivity(intent);
                //Toast.MakeText(this, "Email: " + mEmail + " Password: " + mPassword, ToastLength.Short).Show();

            }

        }
    }
}