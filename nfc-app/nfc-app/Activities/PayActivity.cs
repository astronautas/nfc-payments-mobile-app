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

namespace nfc_app
{
    [Activity(MainLauncher = false, ScreenOrientation = ScreenOrientation.Portrait)]
    class PayActivity : Activity
    {
        private Button _btnPayBank;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Pay);

            _btnPayBank = FindViewById<Button>(Resource.Id.payFromBankButton);
            _btnPayBank.Click += PayFromBankClick;
        }

        private void PayFromBankClick(object sender, EventArgs e)
        {
            StartActivity(typeof(Beam));
        }
    }
}