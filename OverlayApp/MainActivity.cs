using Android.App;
using Android.Content;
using Android.OS;
using Android.Provider;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;

namespace OverlayApp
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private Button button;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            button = (Button)FindViewById(Resource.Id.button);
            button.Click += async delegate
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
            };
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