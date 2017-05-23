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
    [Activity(MainLauncher = false)]
    class UserMainActivity : Activity
    {
        private TextView _userEmailText;
        private Button _payButton;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.UserMain);

            _userEmailText = FindViewById<TextView>(Resource.Id.userEmailText);
            //_userEmailText.Text = User.Init.email;

            _payButton = FindViewById<Button>(Resource.Id.payButton);
            _payButton.Click += (o, e) => StartActivity(typeof(PayActivity));
        }
    }
}