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
using Android.Content.PM;
using Android.Util;
using Newtonsoft.Json.Linq;

namespace nfc_app
{
    [Activity(MainLauncher = true, ScreenOrientation = ScreenOrientation.Portrait)]
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
            FragmentTransaction transaction = FragmentManager.BeginTransaction();
            ProgressDialog progressDiag = new ProgressDialog();
            progressDiag.Show(transaction, "dialog fragment");

            string email = _emailInput.Text == string.Empty ? "-" : _emailInput.Text;
            string password = _passwordInput.Text == string.Empty ? "-" : _passwordInput.Text;
            //check the input
            string json = string.Format("{{ \"user\": {{ \"email\":\"{0}\", \"password\":\"{1}\"}} }}", email, password);
            try
            {
                string response = await Http.Request("https://thawing-ocean-8598.herokuapp.com/login", json, null);
                if (response != string.Empty && response.Contains("auth_token"))
                {
                    //string temp = response.Split(':')[1].Trim();
                    //string token = temp.Substring(1, temp.Length - 3);
                    JObject dict = JObject.Parse(response);                     //NuGet packet: Newtonsoft.Json
                    string token = dict["auth_token"].ToString();
                    string group = dict["type"].ToString();
                    Log.Warn(_tag, "tocken: " + token);
                    User user = new User(email, password, token);
                    user.group = group;


                    if(group == "seller" && NFCSettings.GetSettings(ApplicationContext, "nfc_id") == "no-id")
                    {
                        Log.Warn(_tag, "not yet registered");
                        string sellerInfo = Http.GetRequest("https://thawing-ocean-8598.herokuapp.com/register-nfc", token);
                        JObject seller = JObject.Parse(sellerInfo);
                        string nfc_id = seller["device_id"].ToString();
                        NFCSettings.SaveSettings(ApplicationContext, "nfc_id", nfc_id);
                        Log.Warn(_tag, "nfc id: " + nfc_id);
                    }

                    Intent userActivity;
                    userActivity = new Intent(this, typeof(UserMainActivity)); 
                    userActivity.PutExtra("User", Json.Serialize(user));
                    StartActivity(userActivity);
                }
                else
                {
                    new Exception("Nepavyko prisijungti");
                }
            }
            catch(Exception ex)
            {
                Log.Warn(_tag, ex.Message);
                OpenDialog(null, ex.Message);
            }
            finally
            {
                progressDiag.Dismiss();
            }
        }

        private void OpenDialog(Type activity, string msg)
        {
            FragmentTransaction transaction = FragmentManager.BeginTransaction();
            NotificationDialog notificationDialog = new NotificationDialog(activity, msg);
            notificationDialog.Show(transaction, "dialog fragment");
        }
    }
}