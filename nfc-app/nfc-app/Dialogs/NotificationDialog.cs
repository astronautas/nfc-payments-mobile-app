using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;

namespace nfc_app
{
    class NotificationDialog : DialogFragment
    {
        private TextView _txtNotification;
        private Button _btnProceed;

        private Type activityType;
        private string message;

        public NotificationDialog(Type activityIn, string messageIn)
        {
            activityType = activityIn;
            message = messageIn;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.Notification, container, false);

            _txtNotification = view.FindViewById<TextView>(Resource.Id.txtNotification);
            _txtNotification.Text = message;
            _btnProceed = view.FindViewById<Button>(Resource.Id.btnDialogProceed);

            _btnProceed.Click += (o, e) => ProceedButtonClicked();

            return view;
        }

        private void ProceedButtonClicked()
        {
            Dismiss();
            if(activityType != null)
            {
                Intent intent = new Intent(this.Activity, activityType);
                StartActivity(intent);
            }
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);
            base.OnActivityCreated(savedInstanceState);
            Dialog.Window.Attributes.WindowAnimations = Resource.Style.dialog_animation;
        }
    }
}