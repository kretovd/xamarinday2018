using System;
using Android.App;
using Android.OS;
using System.Threading.Tasks;
using XamarinDay2018.Core;
using Android.Views;
using Android.Graphics;

namespace XamarinDay2018.Droid
{
    [Activity(Label = "XamarinDay 2018", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
        Client _client;

        public View View { get; private set; }

        void SetupSignalRClient()
        {
            Task.Run(async () =>
            {
                _client = new Client();
                _client.OnMessageReceived += ChangeColorEventHandler;

                try
                {
                    await _client.StartColorChanging();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"{ex.Message}\n{ex.StackTrace}");
                }
            });
        }

        void ChangeColorEventHandler(object sender, string e)
        {
            RunOnUiThread(() => View.SetBackgroundColor(Color.ParseColor(e)));
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            View = LayoutInflater.Inflate(Resource.Layout.Main, null);
            SetContentView(View);

            SetupSignalRClient();
        }

        protected override void OnDestroy()
        {
            if (_client != null)
            {
                _client.OnMessageReceived -= ChangeColorEventHandler;
                _client.Disconnnect();
            }
            
            base.OnDestroy();
        }
    }
}

