using System;
using System.IO;
using System.Linq;
using AircraftLightsGUI;

namespace AircraftLightsGUITests
{
    public static class LightSystemAutoTests
    {
        private static readonly string logPath = "TestResults.txt";

        private static void WriteToLog(string testName, bool passed)
        {
            string result = passed ? "PASS" : "FAIL";
            File.AppendAllText(logPath, $"{testName}: {result}\n");
        }

        public static void RunAllTests()
        {
            File.WriteAllText(logPath, "");

            Test_GUI_LightToggleUpdates();
            Test_GUI_EmergencyModeUpdates();
            Test_GUI_FaultedLightsCannotBeChanged();
            Test_GUI_UpdateLightStatusMethodWorks();
            Test_GUI_AllLightsInitialized();

            Console.WriteLine("All GUI automated tests completed. Check 'TestResults.txt'.");
        }

        private static void Test_GUI_AllLightsInitialized()
        {
            bool passed = true;
            try
            {
                var gui = new GUI();

                // Collect all light IDs
                var allLightIDs = gui.lights.Select(l => l.ID).ToList();

                // Expected total: 3 cockpit + 8 seat + 3 aisle + 3 tail + 2 wing = 19 lights
                int expectedCount = 19;
                passed = allLightIDs.Count == expectedCount;

                // Write each light ID to the log for verification
                File.AppendAllText(logPath, "\n--- All Light IDs ---\n");
                foreach (var id in allLightIDs)
                    File.AppendAllText(logPath, id + "\n");

                File.AppendAllText(logPath, $"Total Lights Found: {allLightIDs.Count}\n");
            }
            catch
            {
                passed = false;
            }

            WriteToLog("Test_GUI_AllLightsInitialized", passed);
        }


        private static void Test_GUI_LightToggleUpdates()
        {
            bool passed = true;
            try
            {
                var gui = new GUI();

                // Test toggling a dimming light
                gui.UpdateLightStatus("co00", isFault: false, isOn: true);
                passed &= gui.lights.First(l => l.ID == "co00").Status == GUI.LightStatus.On;

                gui.UpdateLightStatus("co00", isFault: false, isOn: false);
                passed &= gui.lights.First(l => l.ID == "co00").Status == GUI.LightStatus.Off;

                // Test toggling an exterior light
                gui.UpdateLightStatus("ta00", isFault: false, isOn: true);
                passed &= gui.lights.First(l => l.ID == "ta00").Status == GUI.LightStatus.On;

                gui.UpdateLightStatus("ta00", isFault: false, isOn: false);
                passed &= gui.lights.First(l => l.ID == "ta00").Status == GUI.LightStatus.Off;
            }
            catch { passed = false; }

            WriteToLog("Test_GUI_LightToggleUpdates", passed);
        }

        private static void Test_GUI_EmergencyModeUpdates()
        {
            bool passed = true;
            try
            {
                var gui = new GUI();

                // Set emergency mode
                gui.UpdateLightStatus("co00", isFault: false, isEmergency: true, isOn: false);
                passed &= gui.lights.First(l => l.ID == "co00").Status == GUI.LightStatus.Emergency;

                // Reset to off
                gui.UpdateLightStatus("co00", isFault: false, isEmergency: false, isOn: false);
                passed &= gui.lights.First(l => l.ID == "co00").Status == GUI.LightStatus.Off;
            }
            catch { passed = false; }

            WriteToLog("Test_GUI_EmergencyModeUpdates", passed);
        }

        private static void Test_GUI_FaultedLightsCannotBeChanged()
        {
            bool passed = true;
            try
            {
                var gui = new GUI();

                // Force a fault
                gui.UpdateLightClass("co00", "fault");
                var light = gui.lights.First(l => l.ID == "co00");
                passed &= light.Status == GUI.LightStatus.Fault;

                // Attempt to turn on
                gui.UpdateLightClass("co00", "faultoff");
                passed &= light.Status == GUI.LightStatus.Fault; // Should remain faulted
            }
            catch { passed = false; }

            WriteToLog("Test_GUI_FaultedLightsCannotBeChanged", passed);
        }

        private static void Test_GUI_UpdateLightStatusMethodWorks()
        {
            bool passed = true;
            try
            {
                var gui = new GUI();

                // Use UpdateLightStatus for dimming light
                gui.UpdateLightStatus("co00", isFault: false, isOn: true);
                passed &= gui.lights.First(l => l.ID == "co00").Status == GUI.LightStatus.On;

                // Use UpdateLightStatus for exterior light
                gui.UpdateLightStatus("ta00", isFault: true, isOn: false);
                passed &= gui.lights.First(l => l.ID == "ta00").Status == GUI.LightStatus.Fault;
            }
            catch { passed = false; }

            WriteToLog("Test_GUI_UpdateLightStatusMethodWorks", passed);
        }
    }
}
