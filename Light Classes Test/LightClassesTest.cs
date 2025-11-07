using AircraftLightsGUI;
using Microsoft.VisualStudio.TestTools.UnitTesting;


[TestClass]
public class LightClassesTests
{
    [TestMethod]

    public void UniqueIDs()
    {
        // Ceate instances of each non abstract class
        ExteriorLight exteriorLight01 = new ExteriorLight("ex01");
        ExteriorLight exteriorLight02 = new ExteriorLight("ex02");
        DimmingLight dimmingLight01 = new DimmingLight("dl01");
        DimmingLight dimmingLight02 = new DimmingLight("dl02");

        // Get their IDs
        string ex01 = exteriorLight01.LightId;
        string ex02 = exteriorLight02.LightId;
        string dl01 = dimmingLight01.LightId;
        string dl02 = dimmingLight02.LightId;

        // Verify each light has a unique ID
        Assert.AreEqual("ex01", ex01);
        Assert.AreEqual("ex02", ex02);
        Assert.AreEqual("dl01", dl01);
        Assert.AreEqual("dl02", dl02);


        // Ensure all IDs are unique
        Assert.AreNotEqual(ex01, ex02);
        Assert.AreNotEqual(ex01, dl01);
        Assert.AreNotEqual(ex01, dl02);
        Assert.AreNotEqual(ex02, dl01);
        Assert.AreNotEqual(ex02, dl02);
        Assert.AreNotEqual(dl01, dl02);
    }
    [TestMethod]
    public void OnOffFunctionality()
    {
        // Ceate instances of each non abstract class
        ExteriorLight exteriorLight = new ExteriorLight("ex01");
        DimmingLight dimmingLight = new DimmingLight("ex02");

        // Verify lights turn on
        exteriorLight.TurnOn();
        Assert.IsTrue(exteriorLight.IsOn);
        dimmingLight.TurnOn();
        Assert.IsTrue(dimmingLight.IsOn);

        // Verify lights turn off
        exteriorLight.TurnOff();
        Assert.IsFalse(exteriorLight.IsOn);
        dimmingLight.TurnOff();
        Assert.IsFalse(dimmingLight.IsOn);


    }

    [TestMethod]
    public void ShowFaults()
    {
        ExteriorLight exteriorLight = new ExteriorLight("ex01");
        DimmingLight dimmingLight = new DimmingLight("dl01");

        // Turn lights on
        exteriorLight.TurnOn();
        dimmingLight.TurnOn();

        // Set lights to fault state
        exteriorLight.HasFault(true);
        dimmingLight.HasFault(true);

        // Verify fault states
        Assert.IsTrue(exteriorLight.IsFault);
        Assert.IsTrue(dimmingLight.IsFault);
    }

    [TestMethod]
    public void Brightness()
    {
        DimmingLight dimmingLight = new DimmingLight("dl01");

        //Set brightness
        dimmingLight.SetBrightness(6);

        //Verify brightness
        Assert.AreEqual(dimmingLight.Brightness, 6);

    }
    [TestMethod]
    public void Colour()
    {
        DimmingLight dimmingLight = new DimmingLight("dl01");

        //Set colour
        dimmingLight.SetColour("Green");

        //Verify colour
        Assert.AreEqual(dimmingLight.Colour, "Green");


    }
    [TestMethod]
    public void FlashingEnabled_LightOn()
    {
        ExteriorLight exteriorLight = new ExteriorLight("ex01");

        //Turn on light
        exteriorLight.TurnOn();

        //Enable flashing
        exteriorLight.EnableFlashing();

        //Verify flashing
        Assert.IsTrue(exteriorLight.IsFlashing);

    }
    [TestMethod]
    public void FlashingEnabled_LightOff()
    {
        ExteriorLight exteriorLight = new ExteriorLight("ex01");

        //Turn off light
        exteriorLight.TurnOff();

        //Enable flashing
        exteriorLight.EnableFlashing();

        //Verify flashing
        Assert.IsFalse(exteriorLight.IsFlashing);
    }
    [TestMethod]
    public void FlashingDisabled()
    {
        ExteriorLight exteriorLight = new ExteriorLight("ex01");

        //Turn on light
        exteriorLight.TurnOn();

        //Enable flashing
        exteriorLight.DisableFlashing();

        //Verify flashing
        Assert.IsFalse(exteriorLight.IsFlashing);
    }

    [TestMethod]
    public void LightOff_DisableFlashing()
    {
        ExteriorLight exteriorLight = new ExteriorLight("ex01");

        //Enable flashing
        exteriorLight.EnableFlashing();

        //Turn off light
        exteriorLight.TurnOff();

        //Verify flashing
        Assert.IsFalse(exteriorLight.IsFlashing);
    }
    [TestMethod]
    public void EmergencyMode()
    {
        DimmingLight dimmingLight = new DimmingLight("dl01");

        // Ensure light are off
        dimmingLight.TurnOff();

        //Activate emergency mode
        dimmingLight.EmergencyModeOn();

        //Verify emergency mode 
        Assert.IsTrue(dimmingLight.IsEmergency);
        Assert.IsTrue(dimmingLight.IsOn);
        Assert.AreEqual(dimmingLight.Colour, "Red");

    }
}