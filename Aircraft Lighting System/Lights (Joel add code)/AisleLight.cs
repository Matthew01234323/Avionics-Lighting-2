using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AircraftLightsGUI   
{
    public class AisleLight : DimmingLight
    {
        private bool autoBrightnessEnabled = true;

        public AisleLight(string id) : base(id)
        {

        }
        public bool AutoBrightnessEnabled
        {
            get { return autoBrightnessEnabled; }
            set { autoBrightnessEnabled = value; }

        }

        // Enable auto brightness, update GUI and log event
        public void EnableAutoBrightness()
        {
            AutoBrightnessEnabled = true;
            GUI.UpdateLightStatus(LightId, IsOn, IsFault, IsDisabled, IsEmergency, Colour, Brightness, AutoBrightnessEnabled);
            LogFile.WriteEvent(FlightInfo.CurrentTime, LightId, "Auto-brightness ENABLED");
        }

        // Disable auto brightness, update GUI and log event
        public void DisableAutoBrightness()
        {
            AutoBrightnessEnabled = false;
            GUI.UpdateLightStatus(LightId, IsOn, IsFault, IsDisabled, IsEmergency, Colour, Brightness, AutoBrightnessEnabled);
            LogFile.WriteEvent(FlightInfo.CurrentTime, LightId, "Auto-brightness DISABLED");
        }

        // Adjust brightness based on sunrise and sunset times if auto-brightness is enabled
        public void AdjustForTimeOfDay()
        {
            if (AutoBrightnessEnabled)
            {
                TimeSpan currentTime = TimeInfo.CurrentTime;
                TimeSpan daytimeStart = TimeInfo.SunriseTime;
                TimeSpan daytimeEnd = TimeInfo.SunsetTime;

                if (currentTime >= daytimeStart && currentTime < daytimeEnd)
                {
                    Brightness = 5;
                }
                else
                {
                    Brightness = 2;
                }

                GUI.UpdateLightStatus(LightId, IsOn, IsFault, IsDisabled, IsEmergency, Colour, Brightness, AutoBrightnessEnabled);
                LogFile.WriteEvent(FlightInfo.CurrentTime, LightId, $"Adjusting brightness for time of day to {Brightness}");

            }
        }
    }
}


