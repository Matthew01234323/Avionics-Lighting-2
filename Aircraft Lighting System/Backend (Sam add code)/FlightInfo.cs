using System.ComponentModel.Design.Serialization;
using System.IO;
using System.Text.Json;


namespace AircraftLightsGUI
{
    public static class FlightInfo
    {
        static DateTime takeoff_time;
        static DateTime landing_time;
        static DateTime sunset_time;
        static DateTime sunrise_time;
        static string json_filepath = "C:\\Users\\samho\\OneDrive\\Documents\\B&FC\\Year 2\\Object Oriented Programming\\Assignment 2\\Final_Repo\\Avionics-Lighting-2\\Aircraft Lighting System\\Backend (Sam add code)\\Flight_Info_File\\";
        static string json_filename = "flight_data.json";
        static public DateTime current_time;

        static DateTime test_time_1 = new DateTime(2025, 11, 1, 8, 5, 0);
        static DateTime test_time_2 = new DateTime(2025, 11, 1, 8, 10, 0);
        static DateTime test_time_3 = new DateTime(2025, 11, 1, 8, 15, 0);

        static Random rnd = new Random();

        public static void ReadFlightInfo()
        {
            try
            {
                using FileStream json_stream = new FileStream($"{json_filepath}{json_filename}", FileMode.Open, FileAccess.Read);
                using JsonDocument doc = JsonDocument.Parse(json_stream);
                JsonElement root = doc.RootElement;

                takeoff_time = root.GetProperty("takeoff_time").GetDateTime();
                landing_time = root.GetProperty("landing_time").GetDateTime();
                sunset_time = root.GetProperty("sunset_time").GetDateTime();
                sunrise_time = root.GetProperty("sunrise_time").GetDateTime();

                current_time = takeoff_time;

                Program.InFlight = true;

                LogFile.WriteEvent(current_time, "System", "Flight Info read successfully");
            }
            catch (Exception e)
            {
                LogFile.WriteEvent(DateTime.Now, "System", $"Error when reading flight info file: {e}");
            }
        }
        
        static public void CheckEvents()
        {
            int rnd_value;

            if (DateTime.Compare(current_time, landing_time) >= 0)
            {
                foreach (Light l in GUI.exterior_lights_list)
                {
                    if (l.IsOn)
                    {
                        l.TurnOff();
                    }
                }
                foreach (Light l in GUI.dimming_lights_list)
                {
                    if (l.IsOn)
                    {
                        l.TurnOff();
                    }
                }
                LogFile.WriteEvent(current_time, "System", "Plane has landed");
                Program.InFlight = false;
            }
            else
            {
                if (DateTime.Compare(current_time, sunset_time) > 0 && DateTime.Compare(current_time, sunrise_time) < 0)
                {
                    foreach(ExteriorLight el in GUI.exterior_lights_list)
                    {
                        if (!el.IsOn)
                        {
                            el.TurnOn();
                        }

                    }

                    // foreach(AsileLight al in GUI.asile_lights_list)
                    // {
                    //     if (al.brightness != 3)
                    //     {
                    //         al.brightness = 3;
                    //     }
                    // }
                }
                else
                {
                    foreach(ExteriorLight el in GUI.exterior_lights_list)
                    {
                        if (el.IsOn)
                        {
                            el.TurnOff();
                        }

                    }
                    // foreach(AsileLight al in GUI.asile_lights_list)
                    // {
                    //     if (al.brightness != 7)
                    //     {
                    //         al.brightness = 7;
                    //     }
                    // }
                }
                
                foreach(DimmingLight dl in GUI.dimming_lights_list)
                {
                    if (dl.LightId == "se00")
                    {
                        if (DateTime.Compare(current_time, test_time_1) == 0)
                        {
                            dl.TurnOn();
                        }
                        else if (DateTime.Compare(current_time, test_time_2) == 0)
                        {
                            dl.TurnOff();
                        }
                        else if (DateTime.Compare(current_time, test_time_3) == 0)
                        {
                            dl.HasFault(true);
                            dl.TurnOn();
                        }
                    }
                }
            }

            current_time = current_time.AddMinutes(5);
        }
    }
}