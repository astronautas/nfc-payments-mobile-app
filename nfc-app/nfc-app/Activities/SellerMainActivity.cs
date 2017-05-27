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

namespace nfc_app.Activities
{
    [Activity(Label = "SellerMainActivity")]
    public class SellerMainActivity : Activity
    {
        private User _seller;
        private TextView _txtSellerEmail;

        private Button _btnAddCard;
        private Button _btnCreateOrder;
        private Button _btnAddDiscount;
        private Button _btnDeleteDiscount;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SellerMain);

            _txtSellerEmail = FindViewById<TextView>(Resource.Id.txtSellerEmail);

            string userJson = Intent.GetStringExtra("User") ?? "";
            if (userJson != string.Empty)
            {
                _seller = Json.Deserialize<User>(userJson);
                _txtSellerEmail.Text = _seller.email;
            }
            
            _btnAddCard = FindViewById<Button>(Resource.Id.btnAddCard);
            _btnAddCard.Click += AddCard;

            _btnCreateOrder = FindViewById<Button>(Resource.Id.btnCreateOrder);
            _btnCreateOrder.Click += CreateOrder;
        }

        private void CreateOrder(object sender, EventArgs e)
        {
            Intent payAct = new Intent(this, typeof(ReaderActivity));
            payAct.PutExtra("User", Json.Serialize(_seller));
            StartActivity(payAct);
        }

        private void AddCard(object sender, EventArgs e)
        {
            var addCardAct = new Intent(this, typeof(AddCardActivity));
            addCardAct.PutExtra("User", Json.Serialize(_seller));
            StartActivity(addCardAct);
        }
    }
}