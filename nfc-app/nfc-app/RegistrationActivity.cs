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
    [Activity(Label = "RegistrationActivity", MainLauncher = true)]
    public class RegistrationActivity : Activity
    {
        private EditText _edtEmailInput;
        private EditText _edtPasswordInput;
        private EditText _edtPasswordConfimInput;
        private Button _btnCreateAccount;

        private string _tag = "_myapp";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Account);

            _btnCreateAccount = FindViewById<Button>(Resource.Id.btnCreateAccount); //What todo?
            _btnCreateAccount.Click += (o, e) => CreateAccount();

            _edtEmailInput = FindViewById<EditText>(Resource.Id.edtEmailInput);
            _edtPasswordInput = FindViewById<EditText>(Resource.Id.edtPasswordInput);
            _edtPasswordConfimInput = FindViewById<EditText>(Resource.Id.edtPasswordConfirmInput);

            Spinner spinner = FindViewById<Spinner>(Resource.Id.spinUserTypes);

            spinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);
            var adapter = ArrayAdapter.CreateFromResource(
                    this, Resource.Array.usersArray, Android.Resource.Layout.SimpleSpinnerItem);

            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinner.Adapter = adapter;
        }

        private void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;

            string toast = string.Format("Your type is {0}", spinner.GetItemAtPosition(e.Position));
            Toast.MakeText(this, toast, ToastLength.Long).Show();
        }

        protected async void CreateAccount()
        {
            string email = _edtEmailInput.Text == string.Empty ? "-" : _edtEmailInput.Text;
            string password = _edtPasswordInput.Text == string.Empty ? "-" : _edtPasswordInput.Text;
            string passwordConfirmation = _edtPasswordConfimInput.Text == string.Empty ? "-" : _edtPasswordConfimInput.Text;
            //check the input
            string json = string.Format("{{ \"user\": {{ \"email\":\"{0}\", \"password\":\"{1}\",  \"password_confirmation\":\"{2}\"}} }}", email, password, passwordConfirmation);
            try
            {
                string response = await Http.Request("https://thawing-ocean-8598.herokuapp.com/register", json);
                StartActivity(typeof(UserMainActivity));
            }
            catch (Exception ex)
            {
                Log.Warn(_tag, ex.Message);
                //show message window that it failed
            }
        }

    }
}