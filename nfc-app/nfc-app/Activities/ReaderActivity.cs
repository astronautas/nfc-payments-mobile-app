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
using Newtonsoft.Json.Linq;

namespace nfc_app
{
    [Activity(Label = "ReaderActivity",MainLauncher = false)]

    public class ReaderActivity : Activity
    {
        private EditText _edtPaymentAmount;
        private Button _btnMakePayment;

        private User _seller;

        private string _tag = "_myapp";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Reader);

            string userJson = Intent.GetStringExtra("User") ?? "";
            if (userJson != string.Empty)
            {
                _seller = Json.Deserialize<User>(userJson);
            }

            _edtPaymentAmount = FindViewById<EditText>(Resource.Id.edtPaymentAmount);

            _btnMakePayment = FindViewById<Button>(Resource.Id.btnMakePayment);
            _btnMakePayment.Click += (o, e) => MakePayment();
        }

        private async void MakePayment()
        {
            FragmentTransaction transaction = FragmentManager.BeginTransaction();
            ProgressDialog progressDiag = new ProgressDialog();
            progressDiag.Show(transaction, "dialog fragment");

            string paymentAmount = _edtPaymentAmount.Text == string.Empty ? "-" : _edtPaymentAmount.Text;
            string nfcReaderId = NFCSettings.GetSettings(ApplicationContext, "nfc_id");
            string json = string.Format("{{ \"nfc_id\": \"{0}\", \"amount\": \"{1}\"}}", nfcReaderId, paymentAmount);  
            try
            {
                Log.Warn(_tag, json);
                string response = await Http.Request("https://thawing-ocean-8598.herokuapp.com/create-order", json, _seller.stripeToken);
                OpenDialog(typeof(Beam), "Apmokejimas sekmnigas");
            }
            catch (Exception ex)
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