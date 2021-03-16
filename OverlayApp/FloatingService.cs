using Android.App;
using Android.Content;
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
        
        public override void OnCreate()
    {
        base.OnCreate();
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

    private void showFloatingWindow()
    {
        windowManager = GetSystemService(WindowService).JavaCast<IWindowManager>();
        LayoutInflater mLayoutInflater = LayoutInflater.From(ApplicationContext);
        floatView = mLayoutInflater.Inflate(Resource.Layout.float_view, null);
        floatView.SetBackgroundColor(Android.Graphics.Color.Transparent);
        floatView.SetOnTouchListener(this);
        ImageView iv1 = floatView.FindViewById<ImageView>(Resource.Id.iv1);
        ImageView iv2 = floatView.FindViewById<ImageView>(Resource.Id.iv2);
        ImageView iv3 = floatView.FindViewById<ImageView>(Resource.Id.iv3);
        iv1.Click += delegate { Toast.MakeText(ApplicationContext, "The first Image Click", ToastLength.Short).Show(); };
        iv2.Click += delegate { Toast.MakeText(ApplicationContext, "The second Image Click", ToastLength.Short).Show(); };
        iv3.Click += delegate { Toast.MakeText(ApplicationContext, "The third Image Click", ToastLength.Short).Show(); };
        
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
            layoutParams.Height = 100;
            layoutParams.X = 300;
            layoutParams.Y = 300;
            windowManager.AddView(floatView, layoutParams);
            
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