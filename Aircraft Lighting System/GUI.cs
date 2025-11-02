using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace AircraftLightsGUI {
    public class GUI : Form {
        private Panel planePanel;
        private Button emergencyButton;
        private Button exitButton;
        private TextBox infoTextBox;

        // Lists of all the lights
        private List<StatusLight> lights;
        private StatusLight selectedLight;

        // The different light states
        public enum LightStatus
        {
            Off,
            On,
            Fault,
            Emergency
        }
        
        static public List<ExteriorLight> exterior_lights_list = new List<ExteriorLight>();
        static public List<DimmingLight> dimming_lights_list = new List<DimmingLight>();
        static public List<AisleLight> aisle_lights_list = new List<AisleLight>();

        // Class that defines the properties for each light
        public class StatusLight {
            public string ID { get; set; } // Unique ID
            public string DisplayName { get; set; } // Display name
            public PointF Position { get; set; } // position
            public LightStatus Status { get; set; } // state
            public float Radius { get; set; } = 8f; // Size
            public bool IsAisleLight { get; set; } = false;
            public float Height { get; set; } = 30f; // Size (Seperate for isle lights)
            


            // Detects if a light has been clicked
            public bool Contains(PointF point) {
                if (IsAisleLight) {
                    RectangleF rect = new RectangleF(Position.X - 3, Position.Y - (Height / 2), 6, Height);
                    return rect.Contains(point);
                }
                else{
                    float dx = point.X - Position.X;
                    float dy = point.Y - Position.Y;
                    return (dx * dx + dy * dy) <= (Radius * Radius);
                }
            }

            // Updates the colour of the lights circle
            public Color GetColor() => Status switch {
                LightStatus.Off => Color.White,
                LightStatus.On => Color.LimeGreen,
                LightStatus.Fault => Color.Yellow,
                LightStatus.Emergency => Color.Red,
                _ => Color.White
            };
        }

        // Constructor for the form
        public GUI() {
            InitializeComponent();
            InitializeLights();
            InitializeGUILights();
        }
        // Sets up the form and the controls
        private void InitializeComponent()
        {
            Text = "Aircraft Status Monitor";
            Size = new Size(400, 550);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;

            // Plane drawing panel
            planePanel = new Panel
            {
                Location = new Point(10, 10),
                Size = new Size(370, 395),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White
            };
            planePanel.Paint += PlanePanel_Paint;
            planePanel.MouseClick += PlanePanel_MouseClick;
            Controls.Add(planePanel);
            // Emergency button
            emergencyButton = new Button
            {
                Text = "Emergency",
                Location = new Point(10, 415),
                Size = new Size(100, 40),
                Font = new Font("Arial", 9)
            };
            emergencyButton.Click += EmergencyButton_Click;
            Controls.Add(emergencyButton);

            // Exit button
            exitButton = new Button
            {
                Text = "Exit",
                Location = new Point(10, 465),
                Size = new Size(100, 40)
            };
            exitButton.Click += (s, e) => Close();
            Controls.Add(exitButton);

            // Textbox
            infoTextBox = new TextBox
            {
                Location = new Point(130, 415),
                Size = new Size(250, 90),
                Multiline = true,
                ReadOnly = true,
                BackColor = Color.White,
                Text = ""
            };
            Controls.Add(infoTextBox);
        }

        private void InitializeLights() {
            DimmingLight co00 = new DimmingLight("co00");
            dimming_lights_list.Add(co00);
            DimmingLight co01 = new DimmingLight("co01");
            dimming_lights_list.Add(co01);
            DimmingLight co02 = new DimmingLight("co02");
            dimming_lights_list.Add(co02);

            AisleLight ai00 = new AisleLight("ai00");
            aisle_lights_list.Add(ai00);
            AisleLight ai01 = new AisleLight("ai01");
            aisle_lights_list.Add(ai01);
            AisleLight ai02 = new AisleLight("ai02");
            aisle_lights_list.Add(ai02);

            DimmingLight se00 = new DimmingLight("se00");
            dimming_lights_list.Add(se00);
            DimmingLight se01 = new DimmingLight("se01");
            dimming_lights_list.Add(se01);
            DimmingLight se02 = new DimmingLight("se02");
            dimming_lights_list.Add(se02);
            DimmingLight se03 = new DimmingLight("se03");
            dimming_lights_list.Add(se03);
            DimmingLight se04 = new DimmingLight("se04");
            dimming_lights_list.Add(se04);
            DimmingLight se05 = new DimmingLight("se05");
            dimming_lights_list.Add(se05);
            DimmingLight se06 = new DimmingLight("se06");
            dimming_lights_list.Add(se06);
            DimmingLight se07 = new DimmingLight("se07");
            dimming_lights_list.Add(se07);

            ExteriorLight ta00 = new ExteriorLight("ta00");
            exterior_lights_list.Add(ta00);
            ExteriorLight ta01 = new ExteriorLight("ta01");
            exterior_lights_list.Add(ta01);
            ExteriorLight ta02 = new ExteriorLight("ta02");
            exterior_lights_list.Add(ta02);
            ExteriorLight wi00 = new ExteriorLight("wi00");
            exterior_lights_list.Add(wi00);
            ExteriorLight wi01 = new ExteriorLight("wi01");
            exterior_lights_list.Add(wi01);
            
        }

        // Creates and positions all of the lights
        private void InitializeGUILights() {
            lights = new List<StatusLight>();

            // Nose section
            lights.AddRange(new[]
            {
                new StatusLight { ID = "co00", DisplayName = "Cockpit Front", Position = new PointF(185, 70) },
                new StatusLight { ID = "co01", DisplayName = "Cockpit Left", Position = new PointF(170, 95) },
                new StatusLight { ID = "co02", DisplayName = "Cockpit Right", Position = new PointF(200, 95) }
            });

            // Passenger seat rows
            float startY = 140;
            int numRows = 4;
            int seatCounter = 0;
            for (int row = 1; row <= numRows; row++) {
                float y = startY + (row - 1) * 45;
                lights.Add(new StatusLight { ID = $"se{seatCounter:D2}", DisplayName = $"Seat {row}A", Position = new PointF(165, y) });
                seatCounter++;
                lights.Add(new StatusLight { ID = $"se{seatCounter:D2}", DisplayName = $"Seat {row}B", Position = new PointF(205, y) });
                seatCounter++;
            }

            // Aisle lights - these are different since they are rectuangular
            lights.AddRange(new[]
            {
                new StatusLight { ID = "ai00", DisplayName = "Aisle Light 1", Position = new PointF(185, 150), IsAisleLight = true, Height = 50f },
                new StatusLight { ID = "ai01", DisplayName = "Aisle Light 2", Position = new PointF(185, 200), IsAisleLight = true, Height = 50f },
                new StatusLight { ID = "ai02", DisplayName = "Aisle Light 3", Position = new PointF(185, 250), IsAisleLight = true, Height = 50f }
            });

            // Tail
            lights.Add(new StatusLight { ID = "ta00", DisplayName = "Left Tail", Position = new PointF(135, 335) });
            lights.Add(new StatusLight { ID = "ta01", DisplayName = "Tail", Position = new PointF(185, 350) });
            lights.Add(new StatusLight { ID = "ta02", DisplayName = "Right Tail", Position = new PointF(235, 335) });

            // Wing tips
            lights.AddRange(new[]
            {
                new StatusLight { ID = "wi00", DisplayName = "Left Wing", Position = new PointF(40, 195) },
                new StatusLight { ID = "wi01", DisplayName = "Right Wing", Position = new PointF(330, 195) }
            });
        }

          private void PlanePanel_Paint(object sender, PaintEventArgs e) {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            DrawAircraftOutline(g);
            //DrawSeperationLines(g);
            //DrawAircraftBorder(g);
            DrawLegend(g);
            DrawLights(g);
        }

        // Draws aircraft outline
        private void DrawAircraftOutline(Graphics g) {
            using (Pen planePen = new Pen(Color.Black, 2.5f)) {
                GraphicsPath path = new GraphicsPath();

                // Draws the  aircrafts main body
                path.AddArc(160, 40, 50, 50, 180, 180);
                path.AddLine(210, 65, 218, 120);
                path.AddLine(218, 120, 222, 185);
                path.AddLine(222, 185, 340, 185);
                path.AddLine(340, 185, 337, 205);
                path.AddLine(337, 205, 222, 205);
                path.AddLine(222, 205, 215, 310);
                path.AddLine(215, 310, 200, 355);
                path.AddLine(200, 355, 185, 362);
                path.AddLine(185, 362, 170, 355);
                path.AddLine(170, 355, 155, 310);
                path.AddLine(155, 310, 148, 205);
                path.AddLine(148, 205, 30, 205);
                path.AddLine(30, 205, 33, 185);
                path.AddLine(33, 185, 148, 185);
                path.AddLine(148, 185, 152, 120);
                //path.AddLine(152, 120, 160, 65);
                //path.AddLine(155, 310, 215, 310);
                path.CloseFigure();
                g.DrawPath(planePen, path);

                // Tail 
                g.DrawLine(planePen, 160, 330, 120, 330);
                g.DrawLine(planePen, 120, 330, 123, 342);
                g.DrawLine(planePen, 123, 342, 165, 342);
                g.DrawLine(planePen, 210, 330, 250, 330);
                g.DrawLine(planePen, 250, 330, 247, 342);
                g.DrawLine(planePen, 247, 342, 205, 342);

                g.DrawLine(planePen, 152, 120, 218, 120);
                g.DrawLine(planePen, 155, 310, 215, 310);
                g.DrawLine(planePen, 148, 185, 148, 205);
                g.DrawLine(planePen, 222, 185, 222, 205);
            }
        }

        // Draws the 'key' for the colours on the top right of the GUI
        private void DrawLegend(Graphics g) {
            int keyX = 265, keyY = 30;
            using (Font keyFont = new Font("Arial", 10, FontStyle.Bold))
            using (Font labelFont = new Font("Arial", 8)) {
                g.DrawString("Key:", keyFont, Brushes.Black, keyX, keyY);
                var legendItems = new (Color color, string text)[] {
                    (Color.Red, "Emergency"),
                    (Color.Yellow, "Fault"),
                    (Color.LimeGreen, "On"),
                    (Color.White, "Off")
                };

                for (int i = 0; i < legendItems.Length; i++) {
                    var item = legendItems[i];
                    int y = keyY + 25 + (i * 25);
                    g.FillEllipse(new SolidBrush(item.color), keyX, y, 14, 14);
                    g.DrawEllipse(Pens.Black, keyX, y, 14, 14);
                    g.DrawString(item.text, labelFont, Brushes.Black, keyX + 18, y + 1);
                }
            }
        }

        // Draws all the lights
        private void DrawLights(Graphics g) {
            foreach (var light in lights) {
                if (light.IsAisleLight) {
                    RectangleF rect = new RectangleF(light.Position.X - 3, light.Position.Y - (light.Height / 2), 6, light.Height);
                    g.FillRectangle(new SolidBrush(light.GetColor()), rect);
                    using var pen = new Pen(light == selectedLight ? Color.Blue : Color.Black, light == selectedLight ? 3 : 1);
                    g.DrawRectangle(pen, rect.X, rect.Y, rect.Width, rect.Height);
                }
                else {
                    g.FillEllipse(new SolidBrush(light.GetColor()),
                        light.Position.X - light.Radius, light.Position.Y - light.Radius,
                        light.Radius * 2, light.Radius * 2);
                    using var pen = new Pen(light == selectedLight ? Color.Blue : Color.Black, light == selectedLight ? 3 : 1);
                    g.DrawEllipse(pen, light.Position.X - light.Radius, light.Position.Y - light.Radius,
                        light.Radius * 2, light.Radius * 2);
                }
            }
        }

        // Handles mouse clicks to select lights
        private void PlanePanel_MouseClick(object sender, MouseEventArgs e) {
            PointF clickPoint = new PointF(e.X, e.Y);
            selectedLight = lights.FirstOrDefault(l => l.Contains(clickPoint));
            if (selectedLight != null)
                ShowLightContextMenu(selectedLight, e.Location);
            planePanel.Invalidate();
        }

        // Shows the pop-up when a light is clicked
        private void ShowLightContextMenu(StatusLight light, Point location) {
            ContextMenuStrip menu = new ContextMenuStrip();
            menu.Items.Add($"{light.DisplayName}").Enabled = false;
            menu.Items.Add(new ToolStripSeparator());

            void AddItem(string text, LightStatus status) {
                var item = new ToolStripMenuItem(text);
                //update below to call Joels method to update light status
                //item.Click += (s, e) => SetLightStatus(light, status);
                UpdateLightClass(light.ID, status.ToString());
                menu.Items.Add(item);
            }

            AddItem("Off (White)", LightStatus.Off);
            AddItem("On (Green)", LightStatus.On);
            AddItem("Fault (Yellow)", LightStatus.Fault);
            AddItem("Emergency (Red)", LightStatus.Emergency);

            menu.Show(planePanel, location);
        }

        // Function called to update lights
        private void UpdateLightStatus(StatusLight light, bool isFault, bool isEmergency, bool isOn) {
            if (isFault)
            { light.Status = LightStatus.Fault; }
            else if (isEmergency)
            { light.Status = LightStatus.Emergency; }
            else if (isOn)
            { light.Status = LightStatus.On; }
            else { light.Status = LightStatus.Off; }
            planePanel.Invalidate();
        }

        // The same function but without 'emergency' since not every light has that property
        private void UpdateLightStatus(StatusLight light, bool isFault, bool isOn) {
            if (isFault)
            { light.Status = LightStatus.Fault; }
            else if (isOn)
            { light.Status = LightStatus.On; }
            else { light.Status = LightStatus.Off; }
            planePanel.Invalidate();
        }

        // Emergency toggle
        bool IsEmergency = false;
        private void EmergencyButton_Click(object sender, EventArgs e) {
            if (IsEmergency == false) {
                IsEmergency = true;
                UpdateLightClass("ai01", "emergency");
                UpdateLightClass("ai02", "emergency");
                UpdateLightClass("ai03", "emergency");
                UpdateLightClass("co00", "emergency");
                UpdateLightClass("co01", "emergency");
                UpdateLightClass("co02", "emergency");
                
            }
            else {
                IsEmergency = false;
                UpdateLightClass("ai01", "emergencyoff");
                UpdateLightClass("ai02", "emergencyoff");
                UpdateLightClass("ai03", "emergencyoff");
                UpdateLightClass("co00", "emergencyoff");
                UpdateLightClass("co01", "emergencyoff");
                UpdateLightClass("co02", "emergencyoff");
            }
            planePanel.Invalidate();
        }

        private void UpdateLightClass(string lightID, string status) {
            var light = lights.FirstOrDefault(light => light.ID == lightID);
            bool found = false;

            foreach (DimmingLight dl in dimming_lights_list)
            {
                if (dl.LightId == lightID)
                {
                    found = true;
                    switch (status.ToLower())
                    {
                        case "off":
                            dl.TurnOff();
                            break;
                        case "on":
                            dl.TurnOn();
                            break;
                        case "fault":
                            dl.HasFault(true);
                            break;
                        case "emergency":
                            dl.EmergencyModeOn();
                            break;
                        case "emergencyoff":
                            dl.EmergencyModeOff();
                            break;
                    }
                }
            } if (!found)
            {
                foreach (AisleLight al in aisle_lights_list)
                {
                    if (al.LightId == lightID)
                    {
                        found = true;
                        switch (status.ToLower())
                        {
                            case "off":
                                al.TurnOff();
                                break;
                            case "on":
                                al.TurnOn();
                                break;
                            case "fault":
                                al.HasFault(true);
                                break;
                            case "emergency":
                                al.EmergencyModeOn();
                                break;
                            case "emergencyoff":
                                al.EmergencyModeOff();
                                break;
                        }
                    }
                } if (!found)
                {
                    foreach (ExteriorLight el in exterior_lights_list)
                    {
                        if (el.LightId == lightID)
                        {
                            found = true;
                            switch (status.ToLower())
                            {
                                case "off":
                                    el.TurnOff();
                                    break;
                                case "on":
                                    el.TurnOn();
                                    break;
                                case "fault":
                                    el.HasFault(true);
                                    break;
                            }
                        }
                    }
                }
            }

        }
    }
}