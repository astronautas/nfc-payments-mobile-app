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
using Android.Content.PM;

namespace nfc_app
{
    [Activity(MainLauncher = false, ScreenOrientation = ScreenOrientation.Portrait)]
    class AddCardActivity : Activity
    {
        private User _user;

        private string _tag = "_myapp";

        private EditText _cardNrInput;
        private EditText _cardCvcInput;
        private EditText _cardMonthInput;
        private EditText _cardYearInput;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.AddCard);

            Spinner spinner = FindViewById<Spinner>(Resource.Id.carTypeSpinner);

            var adapter = ArrayAdapter.CreateFromResource(
                    this, Resource.Array.cardArray, Android.Resource.Layout.SimpleSpinnerItem);

            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinner.Adapter = adapter;

            string userJson = Intent.GetStringExtra("User") ?? "";
            if (userJson != string.Empty)
            {
                _user = Json.Deserialize<User>(userJson);
            }

            _cardNrInput = FindViewById<EditText>(Resource.Id.cardNrInput);
            _cardCvcInput = FindViewById<EditText>(Resource.Id.cardCvcInput);
            _cardMonthInput = FindViewById<EditText>(Resource.Id.cardMonthInput);
            _cardYearInput = FindViewById<EditText>(Resource.Id.cardYearInput);

            FindViewById<Button>(Resource.Id.addCardButton).Click += (o, e) => AddCard();
        }

        protected async void AddCard()
        {
            FragmentTransaction transaction = FragmentManager.BeginTransaction();
            ProgressDialog progressDiag = new ProgressDialog();
            progressDiag.Show(transaction, "dialog fragment");

            //check the input

            string json = string.Format("{{ \"card\": {{ \"number\":\"{0}\", \"cvc\":\"{1}\", \"exp_year\":\"{2}\", \"exp_month\":\"{2}\"}} }}", _cardNrInput.Text, _cardCvcInput.Text, _cardYearInput.Text, _cardMonthInput.Text);
            try
            {
                string response = await Http.Request("https://thawing-ocean-8598.herokuapp.com/add-card", json, _user.stripeToken);
                if (response != string.Empty && response.Contains("Card added"))
                {
                    OpenDialog(typeof(UserMainActivity), "Koretelė sėkmingai pridėta!");
                }
                else
                {
                    
                    new Exception("Nepavyko pridėti kortelės!");
                }
            }
            catch (Exception ex)
            {
                Log.Warn(_tag, ex.Message);
                Log.Warn(_tag, _user.stripeToken);
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