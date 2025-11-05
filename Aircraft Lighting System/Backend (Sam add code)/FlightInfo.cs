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
        static string json_filepath = "C:\\Users\\Matthew\\Desktop\\University\\Year 2\\Object Oriented Programming\\Assignment 2\\";
        static string json_filename = "flight_data.json";
        static public DateTime current_time;

        static Random rnd = new Random();
        static bool in_flight = false;

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

                in_flight = true;

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
                in_flight = false;
            }
            else
            {
                if (DateTime.Compare(current_time, sunset_time) > 0 && DateTime.Compare(current_time, sunrise_time) < 0)
                {
                    foreach (ExteriorLight el in GUI.exterior_lights_list)
                    {
                        if (!el.IsOn)
                        {
                            el.TurnOn();
                        }

                    }

                    foreach(DimmingLight al in GUI.dimming_lights_list)
                    {
                        if (al.LightId.Contains("ai"))
                        {
                            al.SetBrightness(3);
                        }
                    }
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
                    foreach(DimmingLight al in GUI.dimming_lights_list)
                    {
                        if (al.LightId.Contains("ai"))
                        {
                            al.SetBrightness(7);
                        }
                    }
                }

                foreach(DimmingLight sl in GUI.dimming_lights_list)
                {
                    if (!sl.IsFault && sl.LightId.Contains("se"))
                    {
                        rnd_value = rnd.Next(1, 101);

                        if (rnd_value == 1)
                        {
                            sl.IsFault = true;
                        }
                        else if (rnd_value > 80 && rnd_value <= 90)
                        {
                            sl.SetBrightness(rnd_value - 80);
                        }
                        else if (rnd_value > 90)
                        {
                            if (sl.IsOn)
                            {
                                sl.TurnOff();
                            }
                            else
                            {
                                sl.TurnOn();
                            }
                        }
                    }
                }
            }

            current_time = current_time.AddMinutes(5);
        }

        static public async Task UpdateTime()
        {
            while (in_flight)
            {
                await Task.Delay(2000);
                FlightInfo.CheckEvents();
            }
        }
    }
}