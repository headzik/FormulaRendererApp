using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Acr.UserDialogs;
using Android.Support.V4.Content;
using Android;
using Android.Support.V4.App;
using Android.Support.Design.Widget;
using Plugin.CurrentActivity;
using Android.Util;
using Plugin.Permissions;

namespace FormulaRendererApp.Droid
{
    [Activity(Label = "FormulaRendererApp", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            //changeAccentColor
            var themeAccentColor = new TypedValue();
            this.Theme.ResolveAttribute(Resource.Attribute.colorAccent, themeAccentColor, true);
            var droidAccentColor = new Android.Graphics.Color(themeAccentColor.Data);

            // set Xamarin Color.Accent to match the theme's accent color
            var accentColorProp = typeof(Xamarin.Forms.Color).GetProperty(nameof(Xamarin.Forms.Color.Accent), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            var xamarinAccentColor = new Xamarin.Forms.Color(droidAccentColor.R / 255.0, droidAccentColor.G / 255.0, droidAccentColor.B / 255.0, droidAccentColor.A / 255.0);
            accentColorProp.SetValue(null, xamarinAccentColor, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static, null, null, System.Globalization.CultureInfo.CurrentCulture);

            CrossCurrentActivity.Current.Init(this, savedInstanceState);
            UserDialogs.Init(this);
            LoadApplication(new App());
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

    }
}