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
        private User _user;

        private Button _btnPayBank;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Pay);

            string userJson = Intent.GetStringExtra("User") ?? "";
            if (userJson != string.Empty)
            {
                _user = Json.Deserialize<User>(userJson);
            }

            _btnPayBank = FindViewById<Button>(Resource.Id.payFromBankButton);
            _btnPayBank.Click += PayFromBankClick;
        }

        private void PayFromBankClick(object sender, EventArgs e)
        {
            var payAct = new Intent(this, typeof(Beam));
            payAct.PutExtra("User", Json.Serialize(_user));
            StartActivity(payAct);
        }
    }
}