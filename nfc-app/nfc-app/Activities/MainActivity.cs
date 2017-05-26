using System;
using System.Text;
using Android.App;
using Android.Nfc;
using Android.Content;
using Android.Provider;
using Android.Runtime;
using Android.OS;
using Android.Text.Format;
using Android.Views;
using Android.Widget;
using Android.Util;
using Android.Content.PM;

namespace nfc_app
{
    [Activity(MainLauncher = false, ScreenOrientation = ScreenOrientation.Portrait)]
    public class Beam : Activity, NfcAdapter.ICreateNdefMessageCallback, NfcAdapter.IOnNdefPushCompleteCallback
    {
        public Beam()
        {
            mHandler = new MyHandler(HandlerHandleMessage);
        }

        private User _user;
        NfcAdapter mNfcAdapter;
        TextView mInfoText;
        private const int MESSAGE_SENT = 1;

        private string _tag = "_myapp";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.SetContentView(Resource.Layout.Main);
            mInfoText = FindViewById<TextView>(Resource.Id.textView);
            // Check for available NFC Adapter
            mNfcAdapter = NfcAdapter.GetDefaultAdapter(this);

            string userJson = Intent.GetStringExtra("User") ?? "";
            if (userJson != string.Empty)
            {
                _user = Json.Deserialize<User>(userJson);
                Log.Warn(_tag, "user tok in beam: " + _user.stripeToken);
            }

            if (mNfcAdapter == null)
            {
                mInfoText = FindViewById<TextView>(Resource.Id.textView);
                mInfoText.Text = "NFC is not available on this device.";
            }
            else {
                // Register callback to set NDEF message
                mNfcAdapter.SetNdefPushMessageCallback(this, this);
                // Register callback to listen for message-sent success
                mNfcAdapter.SetOnNdefPushCompleteCallback(this, this);
            }
        }

        public NdefMessage CreateNdefMessage(NfcEvent evt)
        {
            DateTime time = DateTime.Now;
            var text = _user.stripeToken;
            NdefMessage msg = new NdefMessage(
            new NdefRecord[] { CreateMimeRecord (
                "application/nfc_app.nfc_app", Encoding.UTF8.GetBytes (text))
			});
            return msg;
        }

        public void OnNdefPushComplete(NfcEvent arg0)
        {
            // A handler is needed to send messages to the activity when this
            // callback occurs, because it happens from a binder thread
            mHandler.ObtainMessage(MESSAGE_SENT).SendToTarget();
        }

        class MyHandler : Handler
        {
            public MyHandler(Action<Message> handler)
            {
                this.handle_message = handler;
            }

            Action<Message> handle_message;
            public override void HandleMessage(Message msg)
            {
                handle_message(msg);
            }
        }

        private readonly Handler mHandler;

        protected void HandlerHandleMessage(Message msg)
        {
            switch (msg.What)
            {
                case MESSAGE_SENT:
                    Toast.MakeText(this.ApplicationContext, "Message sent!", ToastLength.Long).Show();
                    break;
            }
        }

        protected override void OnResume()
        {
            base.OnResume();
            // Check to see that the Activity started due to an Android Beam
            if (NfcAdapter.ActionNdefDiscovered == Intent.Action)
            {
                ProcessIntent(Intent);
            }
        }

        protected override void OnNewIntent(Intent intent)
        {
            // onResume gets called after this to handle the intent
            Intent = intent;
        }

        void ProcessIntent(Intent intent)
        {
            IParcelable[] rawMsgs = intent.GetParcelableArrayExtra(                     //WHERE SENT TEXT IS RECEIVED
                NfcAdapter.ExtraNdefMessages);
            // only one message sent during the beam
            NdefMessage msg = (NdefMessage)rawMsgs[0];
            // record 0 contains the MIME type, record 1 is the AAR, if present
            string message = Encoding.UTF8.GetString(msg.GetRecords()[0].GetPayload());     //WHAT TO DO WITH SENT DATA

            MakePayment(message);
        }

        public NdefRecord CreateMimeRecord(String mimeType, byte[] payload)
        {
            byte[] mimeBytes = Encoding.UTF8.GetBytes(mimeType);
            NdefRecord mimeRecord = new NdefRecord(
                NdefRecord.TnfMimeMedia, mimeBytes, new byte[0], payload);
            return mimeRecord;
        }

        private async void MakePayment(string token)
        {
            string nfcReaderId = NFCSettings.GetSettings(ApplicationContext, "nfc_id");
            string json = string.Format("{{ \"nfc_id\": \"{0}\", \"buyer_auth_token\": \"{1}\"}}", nfcReaderId, token);
            try
            {
                string response = await Http.Request("https://thawing-ocean-8598.herokuapp.com/pay-order", json, null);
                OpenDialog();
            }
            catch (Exception ex)
            {
                Log.Warn(_tag, ex.Message);
            }
        }

        private void OpenDialog()
        {
            FragmentTransaction transaction = FragmentManager.BeginTransaction();
            NotificationDialog notificationDialog = new NotificationDialog(typeof(ReaderActivity), "Apmokejimas issiustas sekmingai!"); //Change to SellerMainActivity
            notificationDialog.Show(transaction, "dialog fragment");
        }
    }
}


