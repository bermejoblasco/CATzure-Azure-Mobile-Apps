using CATZureMobileApp.Authentication;
using CATZureMobileApp.MobileApp;
using Microsoft.WindowsAzure.MobileServices;
using System;
using Windows.UI.Popups;
using System.Threading.Tasks;
using Windows.Networking.PushNotifications;

namespace CATZureMobileApp.UWP
{
    public sealed partial class MainPage : IAuthenticate
    {
        // Define a member variable for storing the signed-in user. 
        private MobileServiceUser user;

        // Define a method that performs the authentication process
        // using a Facebook sign-in. 
        public async Task<bool> Authenticate()
        {
            string message;
            bool success = false;
            try
            {               
                user = await ServiceManager.DefaultManager.CurrentClient.LoginAsync(MobileServiceAuthenticationProvider.WindowsAzureActiveDirectory);
                message = string.Format("You are now signed in - {0}", user.UserId);
                success = true;
            }
            catch (InvalidOperationException)
            {
                message = "You must log in. Login Required";
            }
            try
            { 
                await InitNotificationsAsync();
                message = $"{ message } - Push register ok";
            }
            catch(Exception)
            {
                message = $"{ message } - Push register ko";
            }

            var dialog = new MessageDialog(message);
            dialog.Commands.Add(new UICommand("OK"));
            await dialog.ShowAsync();
            return success;
        }

        public MainPage()
        {
            this.InitializeComponent();
            CATZureMobileApp.App.Init((IAuthenticate)this);
            LoadApplication(new CATZureMobileApp.App());
        }

        private async Task InitNotificationsAsync()
        {
            try
            {
                var channel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();
                await ServiceManager.DefaultManager.CurrentClient.GetPush().RegisterAsync(channel.Uri);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
