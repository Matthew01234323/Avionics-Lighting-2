using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AircraftLightsGUI
{
    public abstract class Light
    {
        private string lightId;
        private bool isOn = false;
        private bool isFault = false;

        protected Light(string id)
        {
            lightId = id;
        }
        public string LightId
        {
            get { return lightId; }
            set { lightId = value; }
        }
        public bool IsOn
        {
            get { return isOn; }
            set { isOn = value; }
        }

        public bool IsFault
        { 
            get { return isFault; }
            set { isFault = value; }
        }

        // Turn on light if no fault, update GUI and log event
        public virtual void TurnOn()
        {
            if (!IsFault)
            {
                IsOn = true;
                Program.MainFormInstance?.UpdateLightStatus(this.LightId, IsOn, IsFault);
                LogFile.WriteEvent(FlightInfo.current_time, LightId, "turned ON");
            }
            else
            {
                Program.MainFormInstance?.UpdateLightStatus(this.LightId, IsOn, IsFault);
                LogFile.WriteEvent(FlightInfo.current_time, LightId, "FAULT detected");
            }
        }

        // Turn off light, update GUI and log event
        public virtual void TurnOff()
        {
            IsOn = false;
            Program.MainFormInstance?.UpdateLightStatus(this.LightId, IsOn, IsFault);
            LogFile.WriteEvent(FlightInfo.current_time, LightId, "turned OFF");
        }

        // Sets fault status and turns off light if fault is true, update GUI and log event
        public void HasFault(bool faultStatus)
        {
            IsFault = faultStatus;
            if (IsFault && IsOn)
            {
                TurnOff();
                Program.MainFormInstance?.UpdateLightStatus(this.LightId, IsOn, IsFault);
                LogFile.WriteEvent(FlightInfo.current_time, LightId, "FAULT detected");
            }
        }
    }
}