namespace CATZureMobileApp
{
    using System;

    public class Pomodoro
    {
        [Newtonsoft.Json.JsonProperty("Id")]
        public string Id { get; set; }

        [Microsoft.WindowsAzure.MobileServices.Version]
        public string AzureVersion { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        [Newtonsoft.Json.JsonProperty("userId")]
        public string UserId { get; set; }
        
        public DateTime InitPomodor { get; set;}


        public DateTime EndPomodor { get; set; }
      
        public bool HardPomodoro{ get; set; }       


        [Newtonsoft.Json.JsonIgnore]
        public string DateInitDisplay { get { return  "Init Pomodoro: " + InitPomodor.ToLocalTime().ToString("hh:mm:ss tt") +" - End Pomodoro: " + EndPomodor.ToLocalTime().ToString("hh:mm:ss tt"); } }

        [Newtonsoft.Json.JsonIgnore]
        public string DateEndDisplay { get { return "End Pomodoro: " + InitPomodor.ToLocalTime().ToString("d"); } }

        [Newtonsoft.Json.JsonIgnore]
        public string DateGroup { get { return InitPomodor.ToLocalTime().ToString("d"); } }


        [Newtonsoft.Json.JsonIgnore]
        public string AtHardPomodoroDisplay { get { return HardPomodoro ? "Hard Pomodoro" : string.Empty; } }
    }
}

