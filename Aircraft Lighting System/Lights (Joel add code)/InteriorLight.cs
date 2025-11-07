using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AircraftLightsGUI
{
    public abstract class InteriorLight : Light
    {
        private bool isDisabled = false;
        private bool isEmergency = false;
        private string colour = "White";

        public bool IsDisabled
        {
            get { return isDisabled; }
            set { isDisabled = value; }
        }

        public bool IsEmergency
        {
            get { return isEmergency; }
            set { isEmergency = value; }
        }

        public string Colour
        {
            get { return colour; }
            set { colour = value; }
        }

        public InteriorLight(string id) : base(id)
        {

        }

        // Set the colour of the light, log event and update GUI
        public void SetColour(string newColour)
        {
            Colour = newColour;
            Program.MainFormInstance?.UpdateLightStatus(this.LightId, IsOn, IsFault, IsEmergency);
            LogFile.WriteEvent(FlightInfo.current_time, LightId, "Colour set to " + newColour);
        }

        // Activate emergency mode: set colour to Red, update GUI and log event
        public void EmergencyModeOn()
        {
            
            IsEmergency = true;
            IsDisabled = false;
            if (!IsOn)
            {
                TurnOn();
            }
            SetColour("Red");
            Program.MainFormInstance?.UpdateLightStatus(this.LightId, IsOn, IsFault, IsEmergency);
            LogFile.WriteEvent(FlightInfo.current_time, LightId, "Set to Emergency Mode");  
        }

        // Deactivate emergency mode: set colour to White, enable light, update GUI and log event
        public void EmergencyModeOff()
        {
            IsEmergency = false;
            IsDisabled = false;
            SetColour("White");
            Program.MainFormInstance?.UpdateLightStatus(this.LightId, IsOn, IsFault, IsEmergency);
            LogFile.WriteEvent(FlightInfo.current_time, LightId, "Emergency Mode OFF, colour set to White, light ENABLED");
        }

        /* FEATRUES FOR FUTURE VERSIONS

        //Control TurnOn based on disabled status and log event

        public override bool TurnOn()
        {
            if (!IsDisabled)
            {
                return base.TurnOn();
            }
            else
            {
                //LogFile.WriteEvent(FlightInfo.CurrentTime, LightId, "Turn ON blocked - light is DISABLED");
                return false;
            }
        
        }


        // Disable local control of the light, update GUI and log event

        public void Disable()
        {
            IsDisabled = true;
            base.TurnOff();
            Program.MainFormInstance?.UpdateLightStatus(this.LightId, IsOn, IsFault, IsEmergency);
            LogFile.WriteEvent(FlightInfo.current_time, LightId, "DISABLED");
        }

        // Enable local control of the light, update GUI and log event
        public void Enable()
        {
            IsDisabled = false;
            Program.MainFormInstance?.UpdateLightStatus(this.LightId, IsOn, IsFault, IsEmergency);
            LogFile.WriteEvent(FlightInfo.current_time, LightId, "ENABLED");
        }

        */
    }
}

