using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCommandTest.ViewModels
{
    public partial class PersonViewModel : ObservableObject
    {
        [ObservableProperty]
        public partial string Name { get; set; } = string.Empty;
        [ObservableProperty]
        public partial string Skills { get; set; } = string.Empty;
        [ObservableProperty]
        public partial double Age { get; set; }
    }
}
