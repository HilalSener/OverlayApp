using Android.App;
using Android.Content;
using Android.Net;
using Android.OS;
using Android.Provider;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;

namespace OverlayApp
{
    [Activity(Label = "@string/app_name", Name="overlay_app.main_activity", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private Button button;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            button = (Button) FindViewById(Resource.Id.button);
            button.Click += delegate
            {
                GoToTheMap();
            };

            StartOverlay();
        }

        private void GoToTheMap()
        {
            string uri = "http://maps.google.com/maps?daddr=Rotterdam"+"+to:Den Haag+to:Delft+to:Rotterdam";
            string a = "google.navigation:q=Coolblue, Rotterdam&mode=b";
            Intent intent = new Intent(Android.Content.Intent.ActionView, Uri.Parse(a));
            intent.SetFlags(Intent.Flags & ActivityFlags.NewTask);
            intent.SetClassName("com.google.android.apps.maps", "com.google.android.maps.MapsActivity"); 
            StartActivity(intent);
        }

        private void StartOverlay()
        {
            if (!Settings.CanDrawOverlays(this))
            {
                StartActivityForResult(
                    new Intent(Settings.ActionManageOverlayPermission,
                        Android.Net.Uri.Parse("package:" + PackageName)), 0);
            }
            else
            {
                StartService(new Intent(this, typeof(FloatingService)));
            }
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            if (requestCode == 0)
            {
                if (!Settings.CanDrawOverlays(this))
                {

                }
                else
                {
                    StartService(new Intent(this, typeof(FloatingService)));
                }
            }
        }
    }
}