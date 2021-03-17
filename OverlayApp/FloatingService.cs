using System.Timers;
using Android.App;
using Android.Content;
using Android.Net;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace OverlayApp
{
    [Service]
    class FloatingService : Service,Android.Views.View.IOnTouchListener
    {
        WindowManagerLayoutParams layoutParams;
        IWindowManager windowManager;
        View floatView;
        private static Timer aTimer;
        private TextView timer;
        
        public override void OnCreate()
        {
            base.OnCreate();
        }
        
        private void SetTimer()
        {
            // Create a timer with a two second interval.
            aTimer = new System.Timers.Timer(100);
            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }

        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            timer.Text = e.SignalTime.ToLongTimeString();
        }

        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            showFloatingWindow();
            return StartCommandResult.NotSticky;
        }
        
        public override IBinder? OnBind(Intent intent)
        {
            return null;
        }
        
        public void dialPhoneNumber(string phoneNumber) 
        {
            Intent intent = new Intent(Intent.ActionCall);
            intent.SetData(Uri.Parse("tel:" + phoneNumber));
            intent.SetFlags(ActivityFlags.NewTask);
            StartActivity(intent);
        }

        private void showFloatingWindow()
        {
            windowManager = GetSystemService(WindowService).JavaCast<IWindowManager>();
            LayoutInflater mLayoutInflater = LayoutInflater.From(ApplicationContext);
            floatView = mLayoutInflater.Inflate(Resource.Layout.float_view, null);
            floatView.SetBackgroundColor(Android.Graphics.Color.RoyalBlue);
            floatView.SetOnTouchListener(this);
            ImageView iv1 = floatView.FindViewById<ImageView>(Resource.Id.iv1);
            ImageView iv2 = floatView.FindViewById<ImageView>(Resource.Id.iv2);
            ImageView iv3 = floatView.FindViewById<ImageView>(Resource.Id.iv3);
            timer = floatView.FindViewById<TextView>(Resource.Id.timer);
            iv1.Click += delegate
            {
                Intent intent = new Intent(this, typeof(PackageActivity));
                intent.SetFlags(ActivityFlags.NewTask);
                StartActivity(intent);
            };
            iv2.Click += delegate
            {
                //Toast.MakeText(ApplicationContext, "The second Image Click", ToastLength.Short).Show();
                dialPhoneNumber("2125551212");
            };
            iv3.Click += delegate
            {
                GoToTheMap();
            };
            SetTimer();
            
            // set LayoutParam
            layoutParams = new WindowManagerLayoutParams();
                if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Base)
                {
                    layoutParams.Type = WindowManagerTypes.ApplicationOverlay;
                }
                else
                {
                    layoutParams.Type = WindowManagerTypes.Phone;
                }
                layoutParams.Flags = WindowManagerFlags.NotTouchModal;
                layoutParams.Flags = WindowManagerFlags.NotFocusable;

                layoutParams.Width = 400;
                layoutParams.Height = 400;
                layoutParams.X = 300;
                layoutParams.Y = 300;
                windowManager.AddView(floatView, layoutParams);
                
            }

        private void GoToTheMap()
        {
            string uri = "http://maps.google.com/maps?daddr=Rotterdam"+"+to:Den Haag+to:Delft+to:Rotterdam";
            string a = "google.navigation:q=Coolblue, Rotterdam&mode=b";
            Intent intent = new Intent(Android.Content.Intent.ActionView);
            intent.SetFlags(ActivityFlags.NewTask);
            intent.SetPackage("com.google.android.apps.maps");
            //intent.SetClassName("com.google.android.apps.maps", "com.google.android.maps.MapsActivity"); 
            StartActivity(intent);
        }
        
        private int x;
        private int y;
        public bool OnTouch(View? v, MotionEvent? e)
        {
            switch (e.Action)
            {
        
                case MotionEventActions.Down:
                    x = (int)e.RawX;
                    y = (int)e.RawY;
                    break;

                case MotionEventActions.Move:
                    int nowX = (int) e.RawX;
                    int nowY = (int) e.RawY;
                    int movedX = nowX - x;
                    int movedY = nowY - y;
                    x = nowX;
                    y = nowY;
                    layoutParams.X = layoutParams.X+ movedX;
                    layoutParams.Y = layoutParams.Y + movedY;

        
                    windowManager.UpdateViewLayout(floatView, layoutParams);
                    break;

                default:
                    break;
            }
            return false;
        }
    }
}