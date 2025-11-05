namespace AircraftLightsGUI
{
    internal static class Program
    {
        public static GUI? MainFormInstance { get; private set; }

        [STAThread]
        static void Main()
        {
            FlightInfo.ReadFlightInfo();
            FlightInfo.UpdateTime();

            ApplicationConfiguration.Initialize();
            MainFormInstance = new GUI();
            Application.Run(MainFormInstance);


        }

    }
}