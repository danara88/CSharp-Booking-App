using BookingApp.Application.Controllers;
using BookingApp.Application.Helpers;
using BookingApp.Application.Enums;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using BookingApp.Infrastructure.Repositories.BookingRepository;
using BookingApp.Application.Services.ReservationService;

/**
 * Configure Inversion Control Container
 * 
 */
var hostBuilder = Host.CreateDefaultBuilder(args);
hostBuilder = hostBuilder.ConfigureServices(ServiceConfiguration);
using var host = hostBuilder.Build();

void ServiceConfiguration(IServiceCollection services)
{
    // Services
    services.AddTransient<IReservationService, ReservationService>();

    // Repositories
    services.AddTransient<IReservationRepository, ReservationRepository>();

    // Controllers
    services.AddTransient<BookingController>();

}

/**
 * Start program execution
 * 
 */
var userOperation = 0;
var bookingController = host.Services.GetRequiredService<BookingController>();

while (userOperation == 0)
{
    UIHelpers.PrintMenu();
    var selectedOption = UIHelpers.GetSelectedOption();
    availableOptions(selectedOption);
}

void availableOptions(AppOptionsEnum selectedOption)
{
    switch (selectedOption)
    {
        case AppOptionsEnum.Create:
            bookingController.Create();
            break;
        case AppOptionsEnum.Update:
            bookingController.Update();
            break;
        case AppOptionsEnum.List:
            bookingController.List();
            break;
        case AppOptionsEnum.Delete:
            bookingController.Delete();
            break;
        case AppOptionsEnum.Exit:
            Console.Clear();
            Console.WriteLine("Exit");
            Environment.Exit(0);
            break;
        case AppOptionsEnum.ErrorFormat:
            Console.Clear();
            UIHelpers.TriggerErrorMessage("Format error. Please select a valid option.");
            break;
        case AppOptionsEnum.ErrorNull:
            Console.Clear();
            UIHelpers.TriggerErrorMessage("Empty option. Please select a valid option.");
            break;
        default:
            Console.Clear();
            UIHelpers.TriggerErrorMessage($"Sorry, the command {(int)selectedOption} is not valid.");
            break;
    }
}
