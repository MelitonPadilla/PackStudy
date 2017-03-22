using Android.App;
using Android.Widget;
using Android.OS;
using System;
using System.Net;
using System.Collections.Specialized;
using System.Text;
using Android.Content;

namespace PackStudy
{
    [Activity(Label = "PackStudy", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        
        
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);
            

            Button btnLogin = (Button)FindViewById(Resource.Id.btnLogin);
            btnLogin.Click += BtnLogin_Click;

        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            //setup web service for login at login.php
            WebClient client = new WebClient();
            Uri uri = new Uri("http://packstudy-com.stackstaging.com/login.php");
            NameValueCollection parameter = new NameValueCollection();

            //get input values for email and password
            EditText txtEmail = (EditText)FindViewById(Resource.Id.txtEmail);
            EditText txtPassword = (EditText)FindViewById(Resource.Id.txtPassword);

            string email = txtEmail.Text;
            string password = txtPassword.Text;

            //set POST data for login.php
            parameter.Add("Email", email);
            parameter.Add("Password", password);

            client.UploadValuesCompleted += Client_UploadValuesCompleted;
            client.UploadValuesAsync(uri, parameter);

        }

        private void Client_UploadValuesCompleted(object sender, UploadValuesCompletedEventArgs e)
        {
            string data = Encoding.UTF8.GetString(e.Result);
            //when echoing nothing it doesnt send NULL so you need to just see if there is an empty string
            //if there is an empty string this means that no user was found so invalid username of password
            if(data == "")
            {
                string message = "Invalid Email";
                Context context = ApplicationContext;
                Toast toast = Toast.MakeText(context, message, ToastLength.Long);
                toast.Show();
            }
            else if(data == "Invalid Password")
            {
                Context context = ApplicationContext;
                Toast toast = Toast.MakeText(context, data, ToastLength.Long);
                toast.Show();
            }
            else
            {
                string message = "Welcome " + data;
                Context context = ApplicationContext;
                Toast toast = Toast.MakeText(context, message, ToastLength.Long);
                toast.Show();
            }
            
        }
    }
}

