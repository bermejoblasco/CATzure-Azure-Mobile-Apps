
namespace CATZureMobileApp
{
    using MvvmHelpers;
    using Portable.Services;
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Xamarin.Forms;
    using System.Linq;

    public class PomodoroViewModel : BaseViewModel
    {
        PomodoroService pomodoroCupService;
        public ObservableRangeCollection<Pomodoro> Pomodoros { get; } = new ObservableRangeCollection<Pomodoro>();
        public ObservableRangeCollection<Grouping<string, Pomodoro>> PomodoroGrouped { get; } = new ObservableRangeCollection<Grouping<string, Pomodoro>>();

        string loadingMessage;

        public string LoadingMessage
        {
            get { return loadingMessage; }
            set { SetProperty(ref loadingMessage, value); }
        }

        bool hardPomodoro;
        public bool HardPomodoro
        {
            get { return hardPomodoro; }
            set { SetProperty(ref hardPomodoro, value); }
        }

        public PomodoroViewModel()
        {
            if (App.Authenticator != null)
            {
                var auth = App.Authenticator.Authenticate();
                pomodoroCupService = DependencyService.Get<PomodoroService>();
            }
        }

        ICommand loadPomodoroCommand;
        public ICommand LoadPomodoroCommand => loadPomodoroCommand ?? (loadPomodoroCommand = new Command(async () => await ExecuteLoadPomodoroCommandAsync()));

        async Task ExecuteLoadPomodoroCommandAsync()
        {
            if (IsBusy) { return; }
            try
            {
                LoadingMessage = "Loading Pomodoros...";
                IsBusy = true;
                var pomodoros = await pomodoroCupService.GetPomodoros();
                Pomodoros.ReplaceRange(pomodoros);
                SortPomodoros();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error Loading Pomodoros: !" + ex);

                await Application.Current.MainPage.DisplayAlert("Sync Error", "Unable to sync coffees, you may be offline", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        void SortPomodoros()
        {
            var groups = from pomodoro in Pomodoros
                         orderby pomodoro.DateInitDisplay descending
                         group pomodoro by pomodoro.DateGroup
                         into pomodoroGroup
                         select new Grouping<string, Pomodoro>($"{pomodoroGroup.Key} ({pomodoroGroup.Count()})", pomodoroGroup);


            PomodoroGrouped.ReplaceRange(groups);
        }
        
        ICommand newPomodoroCommand;
        public ICommand NewPomodoroCommand => newPomodoroCommand ?? (newPomodoroCommand = new Command(async () => await ExecuteNewPomodoroCommandAsync()));

        async Task ExecuteNewPomodoroCommandAsync()
        {
            if (IsBusy) { return; }
            try
            {
                LoadingMessage = "Adding New Pomodoro...";
                IsBusy = true;
                var coffee = await pomodoroCupService.AddPomodoro(HardPomodoro);
                Pomodoros.Add(coffee);
                SortPomodoros();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in Add  Pomodor: " + ex);
            }
            finally
            {
                LoadingMessage = string.Empty;
                IsBusy = false;
            }
        }       
    }
}

