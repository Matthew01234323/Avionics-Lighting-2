namespace AircraftLightsGUI
{
    internal static class Program
    {
        public static GUI? MainFormInstance { get; private set; }

        static public bool InFlight = false;

        [STAThread]
        static async void Main()
        {
            FlightInfo.ReadFlightInfo();

            ApplicationConfiguration.Initialize();
            MainFormInstance = new GUI();
            Application.Run(MainFormInstance);

            while(InFlight)
            {
                await Task.Delay(2000);
                FlightInfo.CheckEvents();
            }
        }

    }
}