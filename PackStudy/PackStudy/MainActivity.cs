using Android.App;
using Android.Widget;
using Android.OS;
using System;
using System.Net;
using System.Collections.Specialized;
using System.Text;

namespace PackStudy
{
    [Activity(Label = "PackStudy", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            SetContentView(Resource.Layout.Main);
            WebClient client = new WebClient();
            Uri uri = new Uri("http://packstudy-com.stackstaging.com/test.php");
            NameValueCollection parameter = new NameValueCollection();

            parameter.Add("Name", "Bob");

            client.UploadValuesCompleted += Client_UploadValuesCompleted;
            client.UploadValuesAsync(uri, parameter);
        }

        private void Client_UploadValuesCompleted(object sender, UploadValuesCompletedEventArgs e)
        {
            string data = Encoding.UTF8.GetString(e.Result);
        }
    }
}

