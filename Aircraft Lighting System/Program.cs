namespace AircraftLightsGUI
{
    internal static class Program
    {
        public static GUI? MainFormInstance { get; private set; }

        [STAThread]
        static void Main()
        {
            FlightInfo.ReadFlightInfo(); //read JSON file to get flight, sunset and sunrise times
            FlightInfo.UpdateTime(); //start time incrementation and checks

            ApplicationConfiguration.Initialize();
            MainFormInstance = new GUI();
            Application.Run(MainFormInstance); //run GUI


        }

    }
}