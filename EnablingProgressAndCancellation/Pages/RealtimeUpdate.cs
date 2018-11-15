using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EnablingProgressAndCancellation.Pages
{
    public class RealtimeUpdate : Hub
    {
        private static bool isFirstTime = true;

        private static List<Image> GenerateTestImage()
        {
            List<Image> imageList = new List<Image>(1000);
            for (int i = 0; i < 1000; i++)
            {
                imageList.Add(new Image());
            }

            return imageList;
        }
        private static int counter = 0;
        public async Task SendRealtimeMessage(string message)
        {
            await Clients.All.SendAsync("UpdateProgressBar", "dummy user", message);
            if (Convert.ToInt32(message) == 91919191)
            {
                var obj = new RealtimeUpdate();
                var progressIndicator = new Progress<string>(obj.SendMessage);
                await obj.UploadPicturesAsync(GenerateTestImage(), progressIndicator);


            }

        }

        public void SendMessage(string message)
        {
            SendRealtimeMessage(message);
        }



        public async Task<int> UploadPicturesAsync(List<Image> imageList, IProgress<string> progress)
        {
            int totalCount = imageList.Count;
            int processCount = await Task.Run<int>(async () =>
           {
               int tempCount = 0;
               foreach (var image in imageList)
               {
                   //await the processing and uploading logic here
                   int processed = await UploadAndProcessAsync(image);
                   if (progress != null)
                   {
                       progress.Report((tempCount * 100000 / totalCount).ToString());
                   }
                   tempCount++;
               }

               return tempCount;
           });
            return processCount;
        }

        private async Task<int> UploadAndProcessAsync(Image image)
        {
            await Task.Delay(1000);
            return counter++;
        }
    }
}
