using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
using Agnus.Interfaces;
using Agnus.Interfaces.Repositories;
using Agnus.ViewModels;
using Agnus.Models;
using Syncfusion.Maui.Core.Hosting;
using CommunityToolkit.Maui.Markup;

namespace Agnus
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseMauiCommunityToolkitMarkup()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("fa-solid-900.ttf", "FontAwesomeSolid");
                    fonts.AddFont("fa-regular-400.ttf", "FontAwesomeRegular");
                    fonts.AddFont("fa-brands-400.ttf", "FontAwesomeBrands");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            builder.Services.AddHttpClient("API",client =>
            {
                client.BaseAddress = new Uri(Setting.BaseUrl);
                client.Timeout = TimeSpan.FromSeconds(40);
            });
            #region Repositories
            builder.Services.AddTransient<ICheckList, CheckListRepository>();
            builder.Services.AddTransient<IFiles, FilesRepository>();
            builder.Services.AddTransient<IMaintenance, MaintenanceRepository>();
            builder.Services.AddTransient<IManuals, ManualsRepository>();
            builder.Services.AddTransient<INF_Input, NF_InputRepository>();
            builder.Services.AddTransient<IPassword, PasswordRepository>();
            builder.Services.AddTransient<IPatrimony, PatrimonyRepository>();
            builder.Services.AddTransient<IProducts, ProductsRepository>();
            builder.Services.AddTransient<IProject, ProjectRepository>();
            builder.Services.AddTransient<IProviderOrder, ProviderOrderRepository>();
            builder.Services.AddTransient<IRequest, RequestRepository>();
            builder.Services.AddTransient<IService_Order, ServiceOrderRepository>();
            builder.Services.AddTransient<IUsers, UsersRepository>();
            #endregion

            #region ViewModels
            builder.Services.AddTransient<SD001ViewModel>();
            builder.Services.AddTransient<SS001ViewModel>();
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<SA337ViewModel>();
            builder.Services.AddTransient<SA264ViewModel>();
            builder.Services.AddTransient<SH474ViewModel>();
            builder.Services.AddTransient<SC273ViewModel>();
            builder.Services.AddTransient<SH908ViewModel>();
            builder.Services.AddTransient<SS159ViewModel>();
            builder.Services.AddTransient<SS328ViewModel>();
            builder.Services.AddTransient<SS573ViewModel>();
            builder.Services.AddTransient<SS582ViewModel>();
            builder.Services.AddTransient<SS603ViewModel>();
            builder.Services.AddTransient<SS617ViewModel>();
            builder.Services.AddTransient<SS634ViewModel>();
            builder.Services.AddTransient<SS751ViewModel>();
            builder.Services.AddTransient<SS815ViewModel>();
            builder.Services.AddTransient<SC001ViewModel>();
            builder.Services.AddTransient<SC683ViewModel>();
            builder.Services.AddTransient<SG429ViewModel>();
            builder.Services.AddTransient<SG472ViewModel>();
            builder.Services.AddTransient<SG644ViewModel>();
            builder.Services.AddTransient<SG677ViewModel>();
            builder.Services.AddTransient<SG737ViewModel>();
            builder.Services.AddTransient<SG842ViewModel>();
            builder.Services.AddTransient<SG884ViewModel>();
            builder.Services.AddTransient<SG970ViewModel>();
            builder.Services.AddTransient<SA173ViewModel>();
            builder.Services.AddTransient<SA191ViewModel>();
            builder.Services.AddTransient<SA227ViewModel>();
            builder.Services.AddTransient<SA713ViewModel>();
            builder.Services.AddTransient<SA803ViewModel>();
            builder.Services.AddTransient<SF136ViewModel>();
            builder.Services.AddTransient<SF731ViewModel>();
            builder.Services.AddTransient<SF852ViewModel>();
            builder.Services.AddTransient<SH492ViewModel>();
            builder.Services.AddTransient<SG699ViewModel>();
            builder.Services.AddTransient<SG897ViewModel>();
            builder.Services.AddTransient<SG974ViewModel>();
            builder.Services.AddTransient<SH261ViewModel>();
            builder.Services.AddTransient<SH853ViewModel>();
            builder.Services.AddTransient<SK172ViewModel>();
            builder.Services.AddTransient<SK323ViewModel>();
            builder.Services.AddTransient<SK472ViewModel>();
            builder.Services.AddTransient<SK538ViewModel>();
            builder.Services.AddTransient<SK872ViewModel>();
            builder.Services.AddTransient<SK991ViewModel>();
            builder.Services.AddTransient<SP001ViewModel>();
            builder.Services.AddTransient<SP794ViewModel>();
            builder.Services.AddTransient<SP823ViewModel>();
            builder.Services.AddTransient<SA221ViewModel>();
            builder.Services.AddTransient<SA278ViewModel>();
            builder.Services.AddTransient<SG351ViewModel>();
            builder.Services.AddTransient<SD641ViewModel>();
            builder.Services.AddTransient<SA559ViewModel>();
            builder.Services.AddTransient<SA568ViewModel>();
            builder.Services.AddTransient<SA645ViewModel>();
            #endregion

            return builder.Build();
        }
    }
}
