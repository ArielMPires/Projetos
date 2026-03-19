using Agnus.DTO.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agnus.Helpers
{
    public class ThemeManager
    {
        public static void ApplyTheme(ThemeIdDTO settings)
        {
            var appResources = Application.Current.Resources;

            appResources["Primary"] = Color.FromArgb(settings.Primary);
            appResources["Secondary"] = Color.FromArgb(settings.Secondary);
            appResources["Tertiary"] = Color.FromArgb(settings.Tertiary);
            appResources["PrimaryDark"] = Color.FromArgb(settings.PrimaryDark);
            appResources["PrimaryDarkText"] = Color.FromArgb(settings.PrimaryDarkText);
            appResources["SecondaryDarkText"] = Color.FromArgb(settings.SecondaryDarkText);
        }
    }
}
