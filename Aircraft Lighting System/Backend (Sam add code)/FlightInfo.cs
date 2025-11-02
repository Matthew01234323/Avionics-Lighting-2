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
        static string json_filepath = "C:\\Users\\samho\\OneDrive\\Documents\\B&FC\\Year 2\\Object Oriented Programming\\Assignment 2\\Local_Repo\\Avionics-Lighting-Interface\\Avionics-Lighting-Interface\\Flight_Info_File\\";
        static string json_filename = "flight_data.json";
        static public DateTime current_time;

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
                foreach (Light l in dimming_lights_list)
                {
                    if (l.IsOn)
                    {
                        l.TurnOff();
                    }
                }
                foreach(Light l in aisle_lights_list)
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
                    // foreach(ExteriorLight el in exterior_lights_list)
                    // {
                    //     if (!el.IsOn)
                    //     {
                    //         el.TurnOn();
                    //     }

                    // }

                    // foreach(AsileLight al in asile_lights_list)
                    // {
                    //     if (al.brightness != 3)
                    //     {
                    //         al.brightness = 3;
                    //     }
                    // }
                }
                else
                {
                    // foreach(ExteriorLight el in exterior_lights_list)
                    // {
                    //     if (el.IsOn)
                    //     {
                    //         el.TurnOff();
                    //     }

                    // }
                    // foreach(AsileLight al in asile_lights_list)
                    // {
                    //     if (al.brightness != 5)
                    //     {
                    //         al.brightness = 5;
                    //     }
                    // }
                }
                
                // foreach(DimmingLight dl in dimming_lights_list)
                // {
                //     if (!dl.IsFault)
                //     {
                //         rnd_value = rnd.Next(1, 101);

                //         if (rnd_value == 1)
                //         {
                //             dl.IsFault = true;
                //         }
                //         else if (rnd_value > 90)
                //         {
                //             if (dl.IsOn)
                //             {
                //                 dl.TurnOff();
                //             }
                //             else
                //             {
                //                 dl.TurnOn();
                //             }
                //         }
                //     }
                // }
            }

            current_time = current_time.AddMinutes(5);
        }
    }
}