using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Sogeti.Provisioning.Domain;

namespace Sogeti.Provisioning.WebJob.Helpers
{
    public class Updatehelper
    {
        public static string GetDashboardUrl()
        {
            return ConfigurationManager.AppSettings["SignalR.DashboardUrl"]; 
        }

        private static HttpClient GetWebApiClient()
        {
            var client = new HttpClient {BaseAddress = new Uri(GetDashboardUrl())};
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }

        public static async void UpdateDashboard(string status)
        {
            Console.WriteLine(status);

            using (var client = GetWebApiClient())
            {
                await client.PostAsJsonAsync<string>("api/Update",status);
            }
        }

        public async static void UpdateProgressView(string update, ActionRequest request)
        {
            ProgressState prg = request.ToProgressObject(request, update);

            var info = JsonConvert.SerializeObject(prg);
            using (var client = GetWebApiClient())
            {
                await client.PostAsJsonAsync<string>("api/Update", info);
            }
        }
    }
}
