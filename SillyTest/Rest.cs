using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace SillyTest
{
    public class Rest
    {
        public void Run()
        {
            //string subscription = "ff554330-6901-4266-8410-f25d0f110792";
            string subscription = "b403f7d6-87fb-4a39-8b34-b2172f985b78";

            WebRequestHandler webRequestHandler = new WebRequestHandler();
            X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly);
            //var certificate = store.Certificates.Find(X509FindType.FindByThumbprint, "3B25F920D3BF3BF73C6A31988B1F2E628DF186EA", false).OfType<X509Certificate>().FirstOrDefault();
            var certificate = store.Certificates.Find(X509FindType.FindByThumbprint, "E26232E8E5C2551B023F8DDDC3689B8A41FE8FF2", false).OfType<X509Certificate>().FirstOrDefault();
            webRequestHandler.ClientCertificates.Add(certificate);
            HttpClient httpClient = new HttpClient(webRequestHandler);
            httpClient.DefaultRequestHeaders.Add("x-ms-version", "2013-08-01");
            MediaTypeWithQualityHeaderValue mediaType = new MediaTypeWithQualityHeaderValue("application/xml");
            httpClient.DefaultRequestHeaders.Accept.Add(mediaType);

            string uri = "https://management.core.windows.net/" + subscription + "/services/sqlservers/servers";
            
            // get
            Task<HttpResponseMessage> task = httpClient.GetAsync(uri);
            task.Wait();

            Console.WriteLine(task.Result.Content.ReadAsStringAsync().Result+"\n\n");
            string xml = task.Result.Content.ReadAsStringAsync().Result;

            using (XmlReader reader = XmlReader.Create(new StringReader(xml)))
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        if (reader.Name == "Name")
                        {
                            Console.WriteLine("db:{0}", reader.ReadElementContentAsString());
                        }
                    }
                }
            }


            uri = "https://management.core.windows.net/" + subscription + "/services/sqlservers/servers/dsoj4gaaxr/databases?contentview=generic";

            // get
            task = httpClient.GetAsync(uri);
            task.Wait();

            Console.WriteLine(task.Result.Content.ReadAsStringAsync().Result);
        }

    }
}
