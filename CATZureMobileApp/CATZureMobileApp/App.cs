namespace CATZureMobileApp
{
    using Authentication;
    using View;
    using Xamarin.Forms;

    public class App : Application
    {
        public static IAuthenticate Authenticator { get; private set; }

        public static void Init(IAuthenticate authenticator)
        {
            Authenticator = authenticator;
        }

        public App()
        {
            // The root page of your application
            //MainPage = new NavigationPage(new Page1())
            MainPage = new NavigationPage(new View.Pomodoro())
            {
                BarTextColor = Color.White,
                BarBackgroundColor = Color.FromHex("#F3608C")
            };
        }

        protected override void OnStart()
        {

            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}

