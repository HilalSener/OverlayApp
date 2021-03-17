using Android.App;
using Android.OS;

namespace OverlayApp
{
    [Activity (Name="overlay_app.package_activity")]
    public class PackageActivity : Activity
    {
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
            SetContentView(Resource.Layout.package_view);
        }
    }
}