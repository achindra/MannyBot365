using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using Windows.ApplicationModel.Background;
using Windows.Storage;
using Windows.Media.MediaProperties;
using Windows.Media.Capture;
using System.Threading.Tasks;
using Windows.System.Threading;

// The Background Application template is documented at http://go.microsoft.com/fwlink/?LinkID=533884&clcid=0x409

namespace MannyBotCam
{
    public sealed class StartupTask : IBackgroundTask
    {
        private MediaCapture mediaCapture;
        BackgroundTaskDeferral _deferral;
        private ThreadPoolTimer timer;

        public void Run(IBackgroundTaskInstance taskInstance)
        {
            mediaCapture = new MediaCapture();
            _deferral = taskInstance.GetDeferral();
            timer = ThreadPoolTimer.CreatePeriodicTimer(Timer_TickAsync, TimeSpan.FromSeconds(1));
            mediaCapture.Failed += MediaCapture_Failed;
        }

        private async void Timer_TickAsync(ThreadPoolTimer timer)
        {
            StorageFile pictureFile = await KnownFolders.PicturesLibrary.CreateFileAsync(
                            Guid.NewGuid().ToString() + ".jpg", CreationCollisionOption.GenerateUniqueName);
            ImageEncodingProperties imageProperties = ImageEncodingProperties.CreateJpeg();
            await mediaCapture.CapturePhotoToStorageFileAsync(imageProperties, pictureFile);            
        }

        private void MediaCapture_Failed(MediaCapture sender, MediaCaptureFailedEventArgs errorEventArgs)
        {
            throw new NotImplementedException();
        }
    }
}
