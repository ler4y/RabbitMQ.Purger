using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Impl;

namespace RabbitMQ.Purger
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost", Password = "neofacewatch", UserName = "neofacewatch" };
            using (var web = new WebClient())
            {
                web.Credentials = new NetworkCredential("neofacewatch", "neofacewatch");
                var response = web.DownloadString("http://localhost:15672/api/queues");
                var result = JsonConvert.DeserializeObject<dynamic>(response);
                if (result.Count>0)
                {
                    using (var connection = factory.CreateConnection())
                    using (var channel = connection.CreateModel())
                    {
                        foreach (var item in result)
                        {
                            Console.WriteLine($"Purging queue with name: {item["name"].Value}");
                            channel.QueuePurge(item["name"].Value);
                        }

                        Console.WriteLine(" Press [enter] to exit.");
                        Console.ReadLine();
                    }

                }
            }
        }
    }
}
