using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace puppeteer_demo
{
    class Program
    {
        static void Main(string[] args)
        {
            testPuppetAsync().GetAwaiter().GetResult();
            Console.WriteLine("Happy Hurray");
            Console.ReadKey();
        }

        private static async Task testPuppetAsync()
        {
            string baseUrl = "https://www.pgatour.com/leaderboard.html";
            Console.WriteLine("Loading Url " + baseUrl);
            await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);
            using (Browser browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = false
            }))
            {
                Page page = await browser.NewPageAsync();

                await page.SetRequestInterceptionAsync(true);
                page.Request += OnPageRequest;
                await page.GoToAsync(baseUrl);
            }
        }

        private static async void OnPageRequest(object sender, RequestEventArgs requestEvent)
        {
            string requestUrl = requestEvent.Request?.Url;
            if (requestUrl.Contains("leaderboard.json"))
                Console.WriteLine(requestEvent.Request?.Url);
            await requestEvent.Request.ContinueAsync();
        }


    }
}
