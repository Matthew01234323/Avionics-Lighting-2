using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
    
namespace AircraftLightsGUI
{
    public class ExteriorLight : Light
    {
        private bool isFlashing = false;
        public ExteriorLight(string id) : base(id)
        {

        }
        public bool IsFlashing
        {
            get { return isFlashing; }
            set { isFlashing = value; }

        }

        // Enable flashing mode based on whether the light is off or on
        public void EnableFlashing()
        {
            if (!IsOn)
            {
                IsFlashing = false;
                GUI.UpdateLightStatus(LightId, IsOn, IsFault);
                LogFile.WriteEvent(FlightInfo.current_time, LightId, "Cannot enable flashing when light is OFF");
            }
            else
            {
                IsFlashing = true;
                GUI.UpdateLightStatus(LightId, IsOn, IsFault);
                LogFile.WriteEvent(FlightInfo.current_time, LightId, "Flashing mode ENABLED");
            }
        }

        // Disable flashing mode
        public void DisableFlashing()
        {
            IsFlashing = false;
            GUI.UpdateLightStatus(LightId, IsOn, IsFault);
            LogFile.WriteEvent(FlightInfo.current_time, LightId, "Flashing mode DISABLED");
        }

        // Turn off light and disable flashing mode if active
        public override bool TurnOff()
        {
            if (IsFlashing)
            {
                IsFlashing = false;
                GUI.UpdateLightStatus(LightId, IsOn, IsFault);
                LogFile.WriteEvent(FlightInfo.current_time, LightId, "turned OFF");
            }
            return base.TurnOff();
        }
    }
}

