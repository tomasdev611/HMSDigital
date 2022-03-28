using CaptuvoSDK;
using MobileApp.Assets.Constants;
using MobileApp.Interface;
using MobileApp.iOS.Service;
using Xamarin.Forms;

[assembly: Dependency(typeof(ScannerService))]
namespace MobileApp.iOS.Service
{
    public class ScannerService : CaptuvoEventsProtocol, IScanner
    {
        public Captuvo captuvo;

        public ScannerService()
        {
            IntializeCaptuvo();
        }

        private void IntializeCaptuvo()
        {
            try
            {
                captuvo = Captuvo.SharedCaptuvoDevice;

                captuvo.AddCaptuvoDelegate(this);
                captuvo.EnableMSRReader();
            }
            catch
            {
                MessagingCenter.Send<string>(string.Empty, MessagingConstant.CONNECTION_ERROR);
            }
        }

        public bool StartDecoder()
        {
            try
            {
                captuvo.StartDecoderHardware(250);
                captuvo.StartPMHardware(250);

                if (captuvo.IsDecoderRunning)
                {
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public void StopDecoder()
        {
            try
            {
                captuvo.StopDecoderHardware();
                captuvo.StopPMHardware();
            }
            catch
            {
                MessagingCenter.Send<string>(string.Empty, MessagingConstant.CONNECTION_ERROR);
            }
        }

        public override void DecoderDataReceived(string scannedData)
        {
            try
            {
                MessagingCenter.Send<string>(scannedData, MessagingConstant.BARCODE_SCANNED);
            }
            catch
            {
                MessagingCenter.Send<string>(string.Empty, MessagingConstant.CONNECTION_ERROR);
            }
        }
    }
}
