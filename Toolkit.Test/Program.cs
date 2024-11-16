using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Toolkit.Foundation;

namespace Toolkit.Test
{
    public record User;

    public record Hello;
    public class LoggedInUserHandler3 : IHandler<Hello, User>
    {
        public LoggedInUserHandler3()
        {

        }
        public User Handle(Hello? args)
        {
            return new User();
        }
    }

    public class LoggedInUserHandler4 : IHandler<Hello>
    {
        public LoggedInUserHandler4()
        {

        }
        public void Handle(Hello? args)
        {
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            var test = DefaultHostBuilder.Create()
                .ConfigureServices((context, services) =>
                {
                    services.AddHandler<Hello, User, LoggedInUserHandler3>();
                    services.AddHandler<Hello, LoggedInUserHandler4>("Foo");

                    services.AddHostedService<AppService>();
                });


            var d = test.Build();

            d.Start();

            var dd = d.Services.GetRequiredService<IMessenger>();

           // var sdd = d.Services.GetRequiredService<LoggedInUserHandler3>();

           // dd.Send<Hello, User>();
            dd.Send<Hello>("Foo");

        }
    }
}
