using CATZureMobileApp.MobileApp;
using CATZureMobileApp.Portable.Services;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(PomodoroService))]
namespace CATZureMobileApp.Portable.Services
{
    public class PomodoroService
    {
        IMobileServiceSyncTable<Pomodoro> pomodoroTable;        
 
        public async Task Initialize()
        {
            if (ServiceManager.DefaultManager.CurrentClient?.SyncContext?.IsInitialized ?? false)
                return;
            try
            {
                var path = "catzuresync.db";
                path = Path.Combine(MobileServiceClient.DefaultDatabasePath, path);

                //setup our local sqlite store and intialize our table
                var store = new MobileServiceSQLiteStore(path);
                //Define table
                store.DefineTable<Pomodoro>();
                //Initialize SyncContext
                await ServiceManager.DefaultManager.CurrentClient.SyncContext.InitializeAsync(store);
                //Get our sync table that will call out to azure
                pomodoroTable = ServiceManager.DefaultManager.CurrentClient.GetSyncTable<Pomodoro>();
            }
            catch(Exception ex)
            {
                Debug.WriteLine("Error in init sync: " + ex);
            }
        }

        public async Task SyncPomodoro()
        {
            try
            {
                if (!CrossConnectivity.Current.IsConnected)
                    return;

                await pomodoroTable.PullAsync("allPomodoro", pomodoroTable.CreateQuery());

                await ServiceManager.DefaultManager.CurrentClient.SyncContext.PushAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to sync coffees, that is alright as we have offline capabilities: " + ex);
            }
        }

        public async Task<IEnumerable<Pomodoro>> GetPomodoros()
        {
            try
            {
                //Initialize & Sync
                await Initialize();
                await SyncPomodoro();

                return await pomodoroTable.OrderBy(c => c.InitPomodor).ToEnumerableAsync(); ;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to sync coffees, that is alright as we have offline capabilities: " + ex);
                throw ex;
            }
        }

        public async Task<Pomodoro> AddPomodoro(bool hardPomodor)
        {
            try
            {
                await Initialize();

                var pomodoro = new Pomodoro
                {
                    InitPomodor = DateTime.UtcNow,
                    EndPomodor = hardPomodor ? DateTime.UtcNow.AddMinutes(50) : DateTime.UtcNow.AddMinutes(25),
                    HardPomodoro = hardPomodor
                };

                await pomodoroTable.InsertAsync(pomodoro);
                await SyncPomodoro();
                return pomodoro;
            }
            catch(Exception ex)
            {
                Debug.WriteLine("Unable to sync coffees, that is alright as we have offline capabilities: " + ex);
                throw ex;
            }
        }       
    }
}
    
