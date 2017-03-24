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
using System.Net;
using System.Collections.Specialized;
using Newtonsoft.Json;
using Android.Graphics;

namespace PackStudy
{
    [Activity(Label = "MessageBoard")]
    public class MessageBoard : Activity
    {
        int lastmessageId = 0;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.MessageBoard);
            // Create your application here
            WebClient client = new WebClient();
            Uri uri = new Uri("http://packstudy-com.stackstaging.com/messages.php");
            NameValueCollection parameter = new NameValueCollection();
            GetMessages();
            Button btnSend = (Button)FindViewById(Resource.Id.btnSend);
            btnSend.Click += BtnSend_Click;
        }

        private void BtnSend_Click(object sender, EventArgs e)
        {
            EditText txtMessage = (EditText)FindViewById(Resource.Id.txtMessage);

            ISharedPreferences sharedPrefrences = GetSharedPreferences("MyData", FileCreationMode.Private);
            int UserId = sharedPrefrences.GetInt("id", 0);
            WebClient client = new WebClient();
            Uri uri = new Uri("http://packstudy-com.stackstaging.com/sendmessage.php");
            NameValueCollection parameter = new NameValueCollection();
            string FirstName = sharedPrefrences.GetString("FirstName", null);
            string LastName = sharedPrefrences.GetString("LastName", null);
            parameter.Add("UserId", UserId.ToString());
            parameter.Add("Semester", "Spring2017");
            parameter.Add("Course", "CS105");
            parameter.Add("Message", txtMessage.Text);
            parameter.Add("Name", FirstName + " " + LastName);

            byte[] returnValue = client.UploadValues(uri, parameter);
            string r = Encoding.ASCII.GetString(returnValue);

            txtMessage.Text = "";
            GetMessages();
        }

        private void GetMessages()
        {
            ISharedPreferences sharedPrefrences = GetSharedPreferences("MyData", FileCreationMode.Private);
            int UserId = sharedPrefrences.GetInt("id", 0);
            WebClient client = new WebClient();
            Uri uri = new Uri("http://packstudy-com.stackstaging.com/getmessages.php");
            NameValueCollection parameter = new NameValueCollection();

            parameter.Add("Course", "CS105");
            parameter.Add("LastId", lastmessageId.ToString());

            byte[] returnValue = client.UploadValues(uri, parameter);
            string r = Encoding.ASCII.GetString(returnValue);
            List<Message> messages = JsonConvert.DeserializeObject<List<Message>>(r);
            if(messages == null)
            {
                return;
            }
            TextView tvMessages = (TextView)FindViewById(Resource.Id.tvMessages);
            tvMessages.Text = "";
            LinearLayout llMessageBoard = (LinearLayout)FindViewById(Resource.Id.llMessageBoard);
            
            foreach (Message m in messages)
            {
           
                TextView textView1 = new TextView(this) { Text = m.Name + "\n\n" + m.aMessage + "\n\n" + m.reg_date + "\n\n\n" };
                textView1.SetBackgroundColor(Color.DeepSkyBlue);
                llMessageBoard.AddView(textView1);
                lastmessageId = m.id;
            } 
        }
    }
}