
[doc]: http://dev.mailjet.com/
[api_credential]: https://app.mailjet.com/account/api_keys
[issues]: https://github.com/mailjet/mailjet-apiv3-dotnet/issues
[csharp_documentation]:http://dev.mailjet.com/guides/?csharp
[api_doc_repo]:https://github.com/mailjet/api-documentation
[nuget]:https://www.nuget.org/packages/Mailjet.Api/

![alt text](http://cdn.appstorm.net/web.appstorm.net/files/2012/02/mailjet_logo_200x200.png "Mailjet")


# Official Mailjet API v3 .NET wrapper

This .NET library is a client for version 3 of the [Mailjet API][doc].

[![Build Status](https://travis-ci.org/mailjet/mailjet-apiv3-dotnet.svg?branch=master)](https://travis-ci.org/mailjet/mailjet-apiv3-dotnet)

## Getting Started

Every code examples can be found on the [Mailjet Documentation][csharp_documentation]

(Please refer to the [Mailjet Documentation Repository][api_doc_repo] to contribute to the documentation examples)

### Prerequisites

Make sure you have the following requirements:
* .NET version xxxx and higher
* .NET Core xxxx and higher
* .NET Standard xxx support
* A Mailjet API Key and Secret Key

Both API key and API secret can be found [here][api_credential] after opening a Mailjet account.

### Dependencies 

 - .NETStandard 1.1
 - NETStandard.Library (>= 1.6.1)
 - Newtonsoft.Json (>= 9.0.1)

### Installation

You can either clone the present Github repository or use NuGet package manager whit the following command

```
PM> Install-Package Mailjet.Api -Pre
```

For more information about our Nuget package, follow this [link][nuget] 

## Examples

### List resources

```C#
using Mailjet.Client;
using Mailjet.Client.Resources;
using System;
using System.Configuration;
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
            MailjetClient client = new MailjetClient(ConfigurationManager.AppSettings["apiKey"], ConfigurationManager.AppSettings["apiSecret"]);

            MailjetRequest request = new MailjetRequest
            {
                Resource = Contact.Resource,
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
```

To use filter use the following code: 

```C#
	MailjetRequest request = new MailjetRequest
	{
	Resource = Contact.Resource,
	}
	.Filter("limit", 178);
```

### Create a resource

```C#

using Mailjet.Client;
using Mailjet.Client.Resources;
using System;
using System.Configuration;
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
            MailjetClient client = new MailjetClient(ConfigurationManager.AppSettings["apiKey"], ConfigurationManager.AppSettings["apiSecret"]);

            MailjetRequest request = new MailjetRequest()
            {
                Resource = Contact.Resource,
            }
            .Property(Contact.Email, "Mister@mailjet.com");

            MailjetResponse response = await client.PostAsync(request);

            Console.WriteLine(string.Format("StatusCode: {0}\n", response.StatusCode));

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine(string.Format("Total: {0}, Count: {1}\n", response.GetTotal(), response.GetCount()));
                Console.WriteLine(response.GetData());
            }
            else
            {
                Console.WriteLine(string.Format("ErrorInfo: {0}\n", response.GetErrorInfo()));
                Console.WriteLine(string.Format("ErrorMessage: {0}\n", response.GetErrorMessage()));
            }

            Console.ReadLine();
        }
    }
}

```

### Update a resource

```C#
using Mailjet.Client;
using Mailjet.Client.Resources;
using System;
using Newtonsoft.Json.Linq;

namespace Mailjet.ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            MailjetClient client = new MailjetClient(ConfigurationManager.AppSettings["apiKey"], ConfigurationManager.AppSettings["apiSecret"]);

            MailjetRequest request = new MailjetRequest
            {
                Resource = Contact.Resource,
                ResourceId = ResourceId.Alphanumeric("mister@mailjet.com"),
                Body = new JObject 
                {
                    { Contact.Name, "Mister Mailjet" },
                },
            };

            MailjetResponse response = client.PutAsync(request).Result;

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

```

### View a resource

Using the resource ID (ResourceId.Numeric):

```C#
using Mailjet.Client;
using Mailjet.Client.Resources;
using System;
using System.Configuration;
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
            MailjetClient client = new MailjetClient(ConfigurationManager.AppSettings["apiKey"], ConfigurationManager.AppSettings["apiSecret"]);

            MailjetRequest request = new MailjetRequest()
            {
                Resource = Contact.Resource,
                ResourceId = ResourceId.Numeric(1727238050),
            };

            MailjetResponse response = await client.GetAsync(request);

            Console.WriteLine(string.Format("StatusCode: {0}\n", response.StatusCode));

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine(string.Format("Total: {0}, Count: {1}\n", response.GetTotal(), response.GetCount()));
                Console.WriteLine(response.GetData());
            }
            else
            {
                Console.WriteLine(string.Format("ErrorInfo: {0}\n", response.GetErrorInfo()));
                Console.WriteLine(string.Format("ErrorMessage: {0}\n", response.GetErrorMessage()));
            }

            Console.ReadLine();
        }
    }
}
```

Using the resource alternate ID (ResourceId.Alphanumeric):

```C#
using Mailjet.Client;
using Mailjet.Client.Resources;
using System;
using System.Configuration;
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
            MailjetClient client = new MailjetClient(ConfigurationManager.AppSettings["apiKey"], ConfigurationManager.AppSettings["apiSecret"]);

            MailjetRequest request = new MailjetRequest()
            {
                Resource = Contact.Resource,
                ResourceId = ResourceId.Alphanumeric("mister@mailjet.com"),
            };

            MailjetResponse response = await client.GetAsync(request);

            Console.WriteLine(string.Format("StatusCode: {0}\n", response.StatusCode));

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine(string.Format("Total: {0}, Count: {1}\n", response.GetTotal(), response.GetCount()));
                Console.WriteLine(response.GetData());
            }
            else
            {
                Console.WriteLine(string.Format("ErrorInfo: {0}\n", response.GetErrorInfo()));
                Console.WriteLine(string.Format("ErrorMessage: {0}\n", response.GetErrorMessage()));
            }

            Console.ReadLine();
        }
    }
}
```

### Delete a resource

```C#
using Mailjet.Client;
using Mailjet.Client.Resources;
using System;

namespace Mailjet.ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            MailjetClient client = new MailjetClient(ConfigurationManager.AppSettings["apiKey"], ConfigurationManager.AppSettings["apiSecret"]);

            MailjetRequest request = new MailjetRequest
            {
                Resource = Listrecipient.Resource,
                ResourceId = ResourceId.Numeric(956611829),
            };

            MailjetResponse response = client.DeleteAsync(request).Result;

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
```

### Send a mail

Don't forget to set and validate your sender before trying the following code.

```C#
using Mailjet.Client;
using Mailjet.Client.Resources;
using System;
using Newtonsoft.Json.Linq;

namespace Mailjet.ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            MailjetClient client = new MailjetClient(ConfigurationManager.AppSettings["apiKey"], ConfigurationManager.AppSettings["apiSecret"]);;

            MailjetRequest request = new MailjetRequest
            {
                Resource = Send.Resource,
                Body = new JObject 
                {
                    { Send.FromEmail, "pilot@mailjet.com" },
                    { Send.FromName, "Mailjet Pilot" },
                    { Send.Subject, "Your email flight plan!" },
                    { Send.TextPart, "Dear passenger, welcome to Mailjet! May the delivery force be with you!" },
                    { Send.HtmlPart, "<h3>Dear passenger, welcome to Mailjet!</h3><br />May the delivery force be with you!" },
                    { 
                        Send.Recipients, new JArray 
                        {
                            new JObject
                            { 
                                { "Email", "passenger@mailjet.com" },
                            }, 
                        }
                    }
                },
            };

            MailjetResponse response = client.PostAsync(request).Result;

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
```

## Contribute

We welcome any contribution.

Please make sure you follow this step-by-step guide before contributing :

* Fork the project.
* Create a topic branch.
* Implement your feature or bug fix.
* Add documentation for your feature or bug fix.
* Commit and push your changes.
* Submit a pull request

Submit your issues [here][issues].

