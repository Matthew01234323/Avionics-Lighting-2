namespace AircraftLightsGUI
{
    internal static class Program
    {
        public static GUI? MainFormInstance { get; private set; }
        static public bool InFlight = false;

        [STAThread]
        static void Main()
        {
            FlightInfo.ReadFlightInfo();

            // Run automated tests first
            AircraftLightsGUITests.LightSystemAutoTests.RunAllTests();

            ApplicationConfiguration.Initialize();
            MainFormInstance = new GUI();
            Application.Run(MainFormInstance);

            while(InFlight)
            {
                Task.Delay(2000);
                FlightInfo.CheckEvents();
            }
        }
    }
}
