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
using Android.Preferences;

namespace nfc_app
{
    static class NFCSettings
    {
        public static void SaveSettings(Context context, string settingName, string settingValue)
        {
            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(context);
            ISharedPreferencesEditor editor = prefs.Edit();
            editor.PutString(settingName, settingValue);
            editor.Apply();
        }

        public static string GetSettings(Context context, string settingName)
        {
            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(context);
            return prefs.GetString(settingName, "no-id");
        }
    }
}