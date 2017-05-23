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
    [Activity(Label = "ReaderActivity",MainLauncher = false)]

    public class ReaderActivity : Activity
    {
        private EditText _edtPaymentAmount;
        private Button _btnMakePayment;
        private string _nfcReaderId = "nfc8008";

        private string _tag = "_myapp";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Reader);

            string token = Intent.GetStringExtra("Token") ?? "Data not available";

            _edtPaymentAmount = FindViewById<EditText>(Resource.Id.edtPaymentAmount);

            _btnMakePayment = FindViewById<Button>(Resource.Id.btnMakePayment);
            _btnMakePayment.Click += (o, e) => MakePayment(token);
        }

        private async void MakePayment(string token)
        {
            string paymentAmount = _edtPaymentAmount.Text == string.Empty ? "-" : _edtPaymentAmount.Text;
            //check the input
            string json = string.Format("{{ \"nfc_id\": \"{0}\", \"buyer_auth_token\": \"{1}\"}}", _nfcReaderId, token);  
            try
            {
                string response = await Http.Request("https://thawing-ocean-8598.herokuapp.com/pay-order", json);
                OpenDialog();
            }
            catch (Exception ex)
            {
                Log.Warn(_tag, ex.Message);
                //show message window that it failed
            }
        }

        private void OpenDialog()
        {
            FragmentTransaction transaction = FragmentManager.BeginTransaction();
            NotificationDialog notificationDialog = new NotificationDialog(typeof(Beam), "Apmokejimas issiustas sekmingai!");
            notificationDialog.Show(transaction, "dialog fragment");
        }
    }
}