using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Http;
using System.IO;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Util;

namespace nfc_app
{
    [Activity(MainLauncher = true)]
    class LoginActivity : Activity
    {
        private Button _createUserButton;
        private Button _loginButton;
        private EditText _emailInput;
        private EditText _passwordInput;

        private string _tag = "_myapp";
        private static readonly HttpClient client = new HttpClient();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            Log.Warn(_tag, "this is an info message");
            base.OnCreate(savedInstanceState);
            this.SetContentView(Resource.Layout.Login);

            _createUserButton = FindViewById<Button>(Resource.Id.signupButton);
            _createUserButton.Click += (o, e) => StartActivity(typeof(RegistrationActivity));

            _loginButton = FindViewById<Button>(Resource.Id.loginButton);
            _loginButton.Click += (o, e) => Login();
        }

        protected void Login()
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://thawing-ocean-8598.herokuapp.com/login");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = "{ \"user\": { \"email\":\"dorkx@gmail.com\", \"password\":\"123123123\"} }";

                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                Log.Info(_tag, "Response string: " + result);
                Log.Warn(_tag, "Opa");
            }
        }
    }
}