using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MultiValueDictionary.Command;

namespace MultiValueDictionary.Console
{
    internal class Program
    {
        private static readonly Dictionary<string, IStringCommand> CommandStrategy = new()
        {
            {Constants.Commands.Add, new Add()},
            {Constants.Commands.AllMembers, new AllMembers()},
            {Constants.Commands.Clear, new Clear()},
            {Constants.Commands.Items, new Items()},
            {Constants.Commands.Keys, new Keys()},
            {Constants.Commands.KeyExists, new KeyExists()},
            {Constants.Commands.MemberExists, new MemberExists()},
            {Constants.Commands.Members, new Members()},
            {Constants.Commands.Remove, new Remove()},
            {Constants.Commands.RemoveAll, new RemoveAll()}
        };

        private static Task Main(string[] args)
        {
            using var host = CreateHostBuilder(args).Build();
            Run(host.Services, args);
            return host.RunAsync();
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices((_, services) =>
                    services
                        .AddSingleton(
                            typeof(IMultiValueDictionary<string, string>),
                            new MultiValueDictionary<string, string>(new Dictionary<string, ICollection<string>>())));
        }

        private static void PrintResponse(string message)
        {
            System.Console.Write(message);
        }

        private static void Run(IServiceProvider services, string[] args)
        {
            using var serviceScope = services.CreateScope();
            var provider = serviceScope.ServiceProvider;
            var multiValueDictionary = provider.GetRequiredService<IMultiValueDictionary<string, string>>();

            System.Console.WriteLine(Constants.Messages.Welcome);

            while (true)
            {
                System.Console.Write(Constants.Prompt);
                var commandArgs = System.Console.ReadLine()?.Split(Constants.InputSeparator);
                var command = commandArgs?.First().ToUpper();

                if (command == null ||
                    command.Equals(Constants.Commands.Quit, StringComparison.CurrentCultureIgnoreCase))
                    break;

                try
                {
                    PrintResponse(CommandStrategy[command].Execute(multiValueDictionary, commandArgs));
                }
                catch (IndexOutOfRangeException)
                {
                    System.Console.WriteLine($"{Constants.ResponseIndicator}{Constants.Messages.NotEnoughArguments}");
                }
                catch (KeyNotFoundException)
                {
                    System.Console.WriteLine($"{Constants.ResponseIndicator}{Constants.Messages.CommandNotRecognized}");
                }
            }
        }
    }
}