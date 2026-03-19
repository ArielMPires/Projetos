using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Extensions.DependencyInjection;


namespace Agnus.Helpers
{
    public static class PageExtensions
    {
        public static TViewModel CreateViewModel<TViewModel>(this Page page, params object[] args)
            where TViewModel : class
        {
            var services = Application.Current?.Handler?.MauiContext?.Services
               ?? throw new InvalidOperationException("Serviços do Maui não foram inicializados.");

            // Insere o próprio Page como primeiro argumento (opcional)
            var allArgs = new object[] { page };
            if (args != null && args.Length > 0)
                allArgs = new object[] { page }.Concat(args).ToArray();

            return ActivatorUtilities.CreateInstance<TViewModel>(services, allArgs);
        }
    }
}
