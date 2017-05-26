using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.IO;
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
    class UserMainActivity : Activity
    {
        User _user;

        private TextView _userEmailText;
        private Button _payButton;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.UserMain);

            _userEmailText = FindViewById<TextView>(Resource.Id.userEmailText);

            string userJson = Intent.GetStringExtra("User") ?? "";
            if (userJson != string.Empty)
            {
                _user = Json.Deserialize<User>(userJson);
                _userEmailText.Text = _user.email;
            }

            _payButton = FindViewById<Button>(Resource.Id.payButton);
            _payButton.Click += (o, e) => OpnePayActivity();

            Button openAddCardButton = FindViewById<Button>(Resource.Id.openAddCardButton);
            openAddCardButton.Click += (o, e) => OpenAddCardActivity();
            openAddCardButton.Enabled = _user.group == "buyer" ? true : false;
        }

        private void OpenAddCardActivity()
        {
            var addCardAct = new Intent(this, typeof(AddCardActivity));
            addCardAct.PutExtra("User", Json.Serialize(_user));
            StartActivity(addCardAct);
        }

        private void OpnePayActivity()
        {
            Intent payAct;
            if (_user.group == "buyer")
            {
                payAct = new Intent(this, typeof(PayActivity));
            }
            else
            {
                payAct = new Intent(this, typeof(ReaderActivity));
            }
            payAct.PutExtra("User", Json.Serialize(_user));
            StartActivity(payAct);
        }
    }
}