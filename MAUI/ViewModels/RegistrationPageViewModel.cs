using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatAppTutorial.ViewModels
{
   public partial class RegistrationPageViewModel :BaseViewModel
    {
        [ObservableProperty] private string _userName;
    }
}
