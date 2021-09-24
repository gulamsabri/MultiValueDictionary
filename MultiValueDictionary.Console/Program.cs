using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MultiValueDictionary.Console
{
    class Program
    {
        static Task Main(string[] args)
        {
            using var host = CreateHostBuilder(args).Build();
            Run(host.Services, args);
            return host.RunAsync();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((_, services) =>
                    services
                        .AddSingleton(
                            typeof(IMultiValueDictionary<string, string>),
                            new MultiValueDictionary<string, string>(new Dictionary<string, ICollection<string>>()))
                        .AddTransient<ICommandService<string, string>, CommandService<string, string>>());

        private static void PrintResponse(string message)
        {
            System.Console.WriteLine(
                $"{Constants.ResponseIndicator}{message}");
        }
        
        private static void Run(IServiceProvider services, string[] args)
        {
            using var serviceScope = services.CreateScope();
            var provider = serviceScope.ServiceProvider;

            var service = provider.GetRequiredService<ICommandService<string, string>>();

            System.Console.WriteLine("Welcome to MultiValue Dictionary.");
            
            while (true)
            {
                System.Console.Write(Constants.Prompt);
                var commandArgs = System.Console.ReadLine()?.Split(" ");
                var command = commandArgs?.First();

                if (command == null || command.Equals(Constants.Commands.Quit, StringComparison.CurrentCultureIgnoreCase))
                    break;

                switch (command.ToUpper())
                {
                    case Constants.Commands.Add:
                        PrintResponse(service.Add(commandArgs[1], commandArgs[2]));
                        break;
                    case Constants.Commands.Clear:
                        PrintResponse(service.Clear());
                        break;
                    case Constants.Commands.Items:
                        System.Console.Write(service.Items());
                        break;
                    case Constants.Commands.Keys:
                        System.Console.Write(service.Keys());
                        break;
                    case Constants.Commands.Members:
                        break;
                    case Constants.Commands.Remove:
                        break;
                    case Constants.Commands.AllMembers:
                        break;
                    case Constants.Commands.KeyExists:
                        PrintResponse(service.KeyExists(commandArgs[1]));
                        break;
                    case Constants.Commands.MemberExists:
                        break;
                    case Constants.Commands.RemoveAll:
                        break;
                    default:
                        System.Console.WriteLine(Constants.Messages.CommandNotRecognized);
                        break;
                }
            }
        }
    }
}