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

namespace nfc_app
{
    [Activity(Label = "ReaderActivity")]

    public class ReaderActivity : Activity
    {
        private EditText _edtPaymentAmount;
        private Button _btnMakePayment;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            string token = Intent.GetStringExtra("Token") ?? "Data not available";

            _edtPaymentAmount = FindViewById<EditText>(Resource.Id.edtPaymentAmount);

            _btnMakePayment = FindViewById<Button>(Resource.Id.btnMakePayment);
            _btnMakePayment.Click += (o, e) => MakePayment(token);
        }

        private void MakePayment(string token)
        {

        }
    }
}