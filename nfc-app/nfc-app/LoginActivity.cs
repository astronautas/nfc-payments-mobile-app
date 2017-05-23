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

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Login);

            _createUserButton = FindViewById<Button>(Resource.Id.signupButton);
            _createUserButton.Click += (o, e) => StartActivity(typeof(RegistrationActivity));

            _loginButton = FindViewById<Button>(Resource.Id.loginButton);
            _loginButton.Click += (o, e) => Login();

            _emailInput = FindViewById<EditText>(Resource.Id.emailInput);
            _passwordInput = FindViewById<EditText>(Resource.Id.passwordInput);
        }

        protected async void Login()
        {
            string email = _emailInput.Text == string.Empty ? "-" : _emailInput.Text;
            string password = _passwordInput.Text == string.Empty ? "-" : _passwordInput.Text;
            //check the input
            string json = string.Format("{{ \"user\": {{ \"email\":\"{0}\", \"password\":\"{1}\"}} }}", email, password);
            try
            {
                string response = await Http.Request("https://thawing-ocean-8598.herokuapp.com/login", json);
                if (response != string.Empty && response.Contains("auth_token"))
                {
                    string temp = response.Split(':')[1].Trim();
                    string token = temp.Substring(1, temp.Length - 3);
                    Log.Warn(_tag, token);
                    //User.CreateUser(email, password, token);
                    //bybi man i aki, nes nx negalima statiniu objektu tarp activities perdavineti

                    StartActivity(typeof(UserMainActivity));
                }
                else
                {
                    new Exception("Nepavyko prisijungti");
                }
            }
            catch(Exception ex)
            {
                Log.Warn(_tag, ex.Message);
                //show message window that it failed
            }
        }
    }
}