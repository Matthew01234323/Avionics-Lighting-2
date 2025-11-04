using System.IO;

namespace AircraftLightsGUI
{
    public static class LogFile
    {
        private static string file_path = "C:\\Users\\samho\\OneDrive\\Documents\\B&FC\\Year 2\\Object Oriented Programming\\Assignment 2\\Final_Repo\\Avionics-Lighting-2\\Aircraft Lighting System\\Backend (Sam add code)\\Log_Files\\";
        private static string file_name = "lights_logfile.txt";

        public static void WriteEvent(DateTime time, string id, string event_type)
        {
            try
            {
                FileStream stream = new($"{file_path}{file_name}", FileMode.Append);
                
                StreamWriter sw = new StreamWriter(stream);

                sw.WriteLine($"{time} {id}: {event_type}");
                sw.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }
    }
}