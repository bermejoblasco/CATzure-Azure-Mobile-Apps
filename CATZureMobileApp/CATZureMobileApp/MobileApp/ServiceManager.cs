
namespace CATZureMobileApp.MobileApp
{
    using Microsoft.WindowsAzure.MobileServices;
    using Microsoft.WindowsAzure.MobileServices.Sync;
    using System;

    public class ServiceManager
    {
        static ServiceManager defaultInstance = new ServiceManager();
        MobileServiceClient client;        

        private ServiceManager()
        {
            try
            {

                this.client = new MobileServiceClient("https://{yourapp}.azurewebsites.net");
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public MobileServiceClient CurrentClient
        {
            get { return client; }
        }

        public static ServiceManager DefaultManager
        {
            get
            {
                return defaultInstance;
            }
            private set
            {
                defaultInstance = value;
            }
        }      

    }
}
