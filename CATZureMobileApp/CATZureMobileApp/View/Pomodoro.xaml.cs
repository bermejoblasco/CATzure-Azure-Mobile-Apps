using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace CATZureMobileApp.View
{
    public partial class Pomodoro : ContentPage
    {
        PomodoroViewModel vm;

        public Pomodoro()
        {
            InitializeComponent();
            BindingContext = vm = new PomodoroViewModel();
            ListViewPomodoros.ItemTapped += (sender, e) =>
            {
                if (Device.OS == TargetPlatform.iOS || Device.OS == TargetPlatform.Android)
                    ListViewPomodoros.SelectedItem = null;
            };

            if (Device.OS != TargetPlatform.iOS && Device.OS != TargetPlatform.Android)
            {
                ToolbarItems.Add(new ToolbarItem
                {
                    Text = "Refresh",
                    Command = vm.LoadPomodoroCommand
                });
            }
        }
    }
}
