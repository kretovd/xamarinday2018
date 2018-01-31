using System;
using System.Threading.Tasks;
using UIKit;
using XamarinDay2018.Core;

namespace XamarinDay2018.iOS
{
    public partial class ViewController : UIViewController
    {
        Client _client;

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
            InvokeOnMainThread(() => View.BackgroundColor = FromHex(e));
        }

        UIColor FromHex(string hexValue)
        {
            try
            {
                if (hexValue.Length >= 8)
                {
                    var colorString = hexValue.Replace("#", "");
                    var hexValueInt = Convert.ToInt64(colorString, 16);
                    return UIColor.FromRGBA(
                        ((((hexValueInt & 0xFF0000) >> 16)) / 255.0f),
                        ((((hexValueInt & 0xFF00) >> 8)) / 255.0f),
                        (((hexValueInt & 0xFF)) / 255.0f),
                        ((((hexValueInt & -16777216) >> 24)) / 255.0f)
                    );
                }
                else
                {
                    var colorString = hexValue.Replace("#", "");
                    var hexValueInt = Convert.ToInt32(colorString, 16);
                    return UIColor.FromRGB(
                        ((((hexValueInt & 0xFF0000) >> 16)) / 255.0f),
                        ((((hexValueInt & 0xFF00) >> 8)) / 255.0f),
                        (((hexValueInt & 0xFF)) / 255.0f)
                    );
                }
            }
            catch
            {
                return UIColor.Black;
            }
        }

        public ViewController(IntPtr handle) 
            : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            SetupSignalRClient();
        }
    }
}
