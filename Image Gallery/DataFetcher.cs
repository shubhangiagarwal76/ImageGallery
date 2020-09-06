using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using System.IO;
using System.Threading;

namespace Image_Gallery
{
    class DataFetcher
    {
       public bool flag = true;
        async Task<string> GetDatafromService(int count, string searchstring )
        {
            string readText = null;
            try
            {
                var azure = @"https://imagefetcher20200529182038.azurewebsites.net";
                string url = azure + @"/api/fetch_images?query=" +
               searchstring + "&max_count="+count;
                using (HttpClient c = new HttpClient())
                {
                    readText = await c.GetStringAsync(url);
                }
            }
            catch
            {
                flag = false;
                readText = File.ReadAllText(@"Data/sampleData.json");
            }
            return readText;
        }
        public async Task<List<ImageItem>> GetImageData( string search, int count=12)
        {
            string data = await GetDatafromService(count, search);
            return JsonConvert.DeserializeObject<List<ImageItem>>(data);
        }

    }
}
