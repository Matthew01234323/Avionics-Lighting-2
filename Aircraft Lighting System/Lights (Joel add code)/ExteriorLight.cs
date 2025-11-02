using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
    
namespace AircraftLightsGUI
{
    public class ExteriorLight : Light
    {
        private bool isExteriorLightAuto = false;
        private bool isFlashing = false;
        public ExteriorLight(string id) : base(id)
        {

        }
        public bool IsExteriorLightAuto
        {
            get { return isExteriorLightAuto; }
            set { isExteriorLightAuto = value; }

        }
        public bool IsFlashing
        {
            get { return isFlashing; }
            set { isFlashing = value; }

        }

        // Enable exterior light auto mode, update GUI and log event
        public void EnableExteriorLightAuto()
        {
            IsExteriorLightAuto = true;
            GUI.UpdateLightStatus(LightId, IsOn, IsFault, IsExteriorLightAuto, IsFlashing);
            LogFile.WriteEvent(FlightInfo.current_time, LightId, "Exterior Light Auto ENABLED");
        }

        // Disable exterior light auto mode, update GUI and log event
        public void DisableExteriorLightAuto()
        {
            IsExteriorLightAuto = false;
            GUI.UpdateLightStatus(LightId, IsOn, IsFault, IsExteriorLightAuto, IsFlashing);
            LogFile.WriteEvent(FlightInfo.current_time, LightId, "Exterior Light Auto DISABLED");
        }

        // Operate light based on sunrise and sunset times if exterior light auto mode is enabled
        public void ExteriorLightAuto()
        {
            if (IsExteriorLightAuto)
            {
                TimeSpan currentTime = TimeInfo.CurrentTime;
                TimeSpan daytimeStart = TimeInfo.SunriseTime;
                TimeSpan daytimeEnd = TimeInfo.SunsetTime;

                if (currentTime >= daytimeStart && currentTime < daytimeEnd)
                {
                    base.TurnOff();
                    LogFile.WriteEvent(FlightInfo.CurrentTime, LightId, "Auto turned OFF for daytime");
                }
                else
                {
                    base.TurnOn();
                    LogFile.WriteEvent(FlightInfo.CurrentTime, LightId, "Auto turned ON for nighttime");
                }
                GUI.UpdateLightStatus(LightId, IsOn, IsFault, IsExteriorLightAuto, IsFlashing);
            }
        }

        // Enable flashing mode based on whether the light is off or on
        public void EnableFlashing()
        {
            if (!IsOn)
            {
                IsFlashing = false;
                GUI.UpdateLightStatus(LightId, IsOn, IsFault, IsExteriorLightAuto, IsFlashing);
                LogFile.WriteEvent(FlightInfo.CurrentTime, LightId, "Cannot enable flashing when light is OFF");
            }
            else
            {
                IsFlashing = true;
                GUI.UpdateLightStatus(LightId, IsOn, IsFault, IsExteriorLightAuto, IsFlashing);
                LogFile.WriteEvent(FlightInfo.CurrentTime, LightId, "Flashing mode ENABLED");
            }
        }

        // Disable flashing mode
        public void DisableFlashing()
        {
            IsFlashing = false;
            GUI.UpdateLightStatus(LightId, IsOn, IsFault, IsExteriorLightAuto, IsFlashing);
            LogFile.WriteEvent(FlightInfo.CurrentTime, LightId, "Flashing mode DISABLED");
        }

        // Turn off light and disable flashing mode if active
        public override bool TurnOff()
        {
            if (IsFlashing)
            {
                IsFlashing = false;
                GUI.UpdateLightStatus(LightId, IsOn, IsFault, IsExteriorLightAuto, IsFlashing);
                LogFile.WriteEvent(FlightInfo.current_time, LightId, "Flashing mode DISABLED due to light OFF");
            }
            return base.TurnOff();
        }
    }
}

