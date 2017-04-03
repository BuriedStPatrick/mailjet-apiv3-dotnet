﻿using Mailjet.Client;
using Mailjet.Client.Resources;
using System;
using System.Threading.Tasks;

namespace Mailjet.ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            RunAsync().Wait();
        }

        static async Task RunAsync()
        {
            MailjetClient client = new MailjetClient(Environment.GetEnvironmentVariable("MJ_APIKEY_PUBLIC"), Environment.GetEnvironmentVariable("MJ_APIKEY_PRIVATE"))
            {
                //BaseAdress = "https://api.mailjet.com",
                //Version = ApiVersion.V3,
                //Proxy = new DefaultProxy("http://127.0.0.1:8888"),
            };

            MailjetRequest request = new MailjetRequest
            {
                Resource = Apikey.Resource,
            };

            MailjetResponse response = await client.GetAsync(request);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine(string.Format("Total: {0}, Count: {1}\n", response.GetTotal(), response.GetCount()));
                Console.WriteLine(response.GetData());
            }
            else
            {
                Console.WriteLine(string.Format("StatusCode: {0}\n", response.StatusCode));
                Console.WriteLine(string.Format("ErrorInfo: {0}\n", response.GetErrorInfo()));
                Console.WriteLine(string.Format("ErrorMessage: {0}\n", response.GetErrorMessage()));
            }

            Console.ReadLine();
        }
    }
}
