using PolytechWebRef;
using S1Processor.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace S1Processor.Helpers
{
    class Config
    {
        public static ServerSetting ss = new ServerSetting();
        public static string url = string.Empty;
        public static string username = string.Empty;
        public static string password = string.Empty;
        public static string domain = string.Empty;
        public Config()
        {
            ss.GetSettings(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/Settings.txt");
            url = ss.url;
            username = ss.user;
            password = ss.pass; 
            domain = ss.domain;
        }
        public static MobileService_PortClient GetMobileServiceClient2()
        {
            ss.GetSettings(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/Settings.txt");
            url = ss.url;
            username = ss.user;
            password = ss.pass;
            domain = ss.domain;

            var binding = new BasicHttpBinding(BasicHttpSecurityMode.TransportCredentialOnly)
            {
                Security =
                        {
                            Transport =
                            {
                                ClientCredentialType = HttpClientCredentialType.Basic
                            }
                        }
            };

            var endpoint = new EndpointAddress(url);

            var client = new MobileService_PortClient(binding, endpoint);

            client.ClientCredentials.Windows.ClientCredential =
                new NetworkCredential(username, password, domain);

            return client;
        }
        public static MobileService_PortClient GetMobileServiceClient()
        {
            ss.GetSettings(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/Settings.txt");
            string url = ss.url;
            string username = ss.user;
            string password = ss.pass;
            string domain = ss.domain;

            var binding = new BasicHttpBinding(BasicHttpSecurityMode.TransportCredentialOnly)
            {
                Security =
            {
                Transport =
                {
                    ClientCredentialType = HttpClientCredentialType.Basic
                }
            }
            };

            var endpoint = new EndpointAddress(url);

            var client = new MobileService_PortClient(binding, endpoint);

            //client.ClientCredentials.Windows.ClientCredential =
            //    new NetworkCredential(username, password, domain);
            client.ClientCredentials.UserName.UserName = username;
            client.ClientCredentials.UserName.Password = password;
            return client;
        }
     
    }
}
