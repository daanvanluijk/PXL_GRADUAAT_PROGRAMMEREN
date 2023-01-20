using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMProject.ViewModels
{
    public class SettingsViewModel : ObservableObject
    {
        public bool RememberCredentials
        {
            get => Preferences.Get(nameof(App.rememberCredentials), false);
            set
            {
                Preferences.Set(nameof(App.rememberCredentials), value);
                if (value == true)
                    return;
                if (Preferences.ContainsKey(nameof(App.userName)))
                    Preferences.Remove(nameof(App.userName));
                if (Preferences.ContainsKey(nameof(App.password)))
                    Preferences.Remove(nameof(App.password));
            }
        }
    }
}
