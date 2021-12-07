using System;
using System.Diagnostics;
using System.Net;
using System.IO;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace test
{
    internal class Program
    {
        private static readonly HttpClient client = new HttpClient();
        public static void Main(string[] args)
        {
            Console.WriteLine("e-buda music cli ");
            Console.WriteLine("Wprowadź wyszukiwanie");
            string q = Console.ReadLine();
            string responseString = "";
            using (var client = new HttpClient())
            {
                var response = client.GetAsync("https://apps.e-buda.eu:445/muzyka/cli/query.php?q="+q).Result;

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = response.Content;
                    responseString = responseContent.ReadAsStringAsync().Result;
                    
                }
            }
            Odp odp = JsonConvert.DeserializeObject<Odp>(responseString);
            int i = 1;
            Console.WriteLine(odp.INFO);
            if (odp.STATUS != "OK")
            {
                return;
            }
            foreach (Utwor u in odp.DATA)
            {
                Console.WriteLine(i.ToString()+". "+u.tytul+" - "+u.autor);
                i++;
            }
            Console.WriteLine("Wprowadź numer");
            int chose = 1;
            try
            {
                chose  = int.Parse(Console.ReadLine());
            }
            catch
            {
                Console.WriteLine("Nieznana liczba\nWybieram 1");
            }
            string remoteUri = odp.DATA[chose-1].url;
            string fileName = "download.mp3", myStringWebResource = null;
            WebClient myWebClient = new WebClient();
            myStringWebResource = remoteUri;
            Console.WriteLine("Przygotowuję {0} - {1}", odp.DATA[chose-1].tytul, odp.DATA[chose-1].autor);
            myWebClient.DownloadFile(myStringWebResource,fileName);
            Process.Start(fileName);
        }
    }
}