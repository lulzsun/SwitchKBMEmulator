using InputServer;
using SwitchKBMEmulator;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Windows.Input;

//using Button = InputServer.Button;
using Mouse = System.Windows.Forms.Cursor;

namespace KBMSwitchAdapter
{
    public partial class Form1 : Form
    {
        public SwitchInputSink sink;
        private Controller controller;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //set up new handler events for controls in mouse settings tab
            foreach (Control c in tabPage3.Controls)
            {
                if (c.GetType() == typeof(NoFocusTrackBar))
                    (c as NoFocusTrackBar).ValueChanged += MTrackBar;
                else if (c.GetType() == typeof(NumericUpDown))
                    (c as NumericUpDown).ValueChanged += MValue;
                else if (c.GetType() == typeof(CheckBox))
                    (c as CheckBox).CheckedChanged += MCheckBox;
            }

            //set up new handler events for controls in controller tab
            foreach (Control c in joyPanel_lf.Controls) // left face buttons
            {
                if (c.GetType() == typeof(System.Windows.Forms.Button))
                    (c as System.Windows.Forms.Button).Click += OpenKeyForm;
            }
            foreach (Control c in joyPanel_lt.Controls) // left shoulder buttons
            {
                if (c.GetType() == typeof(System.Windows.Forms.Button))
                    (c as System.Windows.Forms.Button).Click += OpenKeyForm;
            }
            foreach (Control c in joyPanel_rf.Controls) // right face buttons
            {
                if (c.GetType() == typeof(System.Windows.Forms.Button))
                    (c as System.Windows.Forms.Button).Click += OpenKeyForm;
            }
            foreach (Control c in joyPanel_rt.Controls) // right shoulder buttons
            {
                if (c.GetType() == typeof(System.Windows.Forms.Button))
                    (c as System.Windows.Forms.Button).Click += OpenKeyForm;
            }

            //misc
            foreach (Control c in Controls)
            {
                if (c.GetType() == typeof(System.Windows.Forms.Button))
                    (c as System.Windows.Forms.Button).Click += SaveLoadButton;
            }

            //First time setup
            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + @"Profiles\"))
            {
                System.IO.FileInfo file = new System.IO.FileInfo(AppDomain.CurrentDomain.BaseDirectory + @"Profiles\");
                file.Directory.Create();
            }

            //remember last selected profile
            pComboBox.Text = SwitchKBMEmulator.Properties.Settings.Default["Profile"].ToString();
            if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"Profiles\" + SwitchKBMEmulator.Properties.Settings.Default["Profile"].ToString() + ".profile"))
            {
                //if default does not exist then make it
                if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"Profiles\Default.profile"))
                {
                    MouseTranslation settings = new MouseTranslation();
                    mValueX1.Value = (decimal)settings.mc_sensitivity.X;
                    mValueY1.Value = (decimal)settings.mc_sensitivity.Y;

                    mValueX2.Value = (decimal)settings.mc_sensitivity2.X;
                    mValueY2.Value = (decimal)settings.mc_sensitivity2.Y;

                    mValueX3.Value = (decimal)settings.mc_mouse_delta_start_threshold.X;
                    mValueY3.Value = (decimal)settings.mc_mouse_delta_start_threshold.Y;

                    mValueX4.Value = (decimal)settings.mc_delta_sensitivity_sigmoid_constant.X;
                    mValueY4.Value = (decimal)settings.mc_delta_sensitivity_sigmoid_constant.Y;

                    mValueX5.Value = (decimal)settings.mc_delta_initial.X / 128;
                    mValueY5.Value = (decimal)settings.mc_delta_initial.Y / 128;

                    mValueX6.Value = (decimal)settings.mc_delta_stop_threshold.X / 128;
                    mValueY6.Value = (decimal)settings.mc_delta_stop_threshold.Y / 128;

                    mValueX7.Value = (decimal)settings.mc_delta_damping_origin.X / 128;
                    mValueY7.Value = (decimal)settings.mc_delta_damping_origin.Y / 128;

                    mValueX8.Value = (decimal)settings.mc_delta_damping.X;
                    mValueY8.Value = (decimal)settings.mc_delta_damping.Y;

                    mValueX9.Value = (decimal)settings.mc_delta_damping2.X;
                    mValueY9.Value = (decimal)settings.mc_delta_damping2.Y;

                    mValueX10.Value = (decimal)settings.mc_delta_damping_sigmoid_constant.X;
                    mValueY10.Value = (decimal)settings.mc_delta_damping_sigmoid_constant.Y;

                    saveProfile("Default");
                }
                pComboBox.Text = "Default";
            }
            loadProfile(pComboBox.Text);

            //load all profiles in \Profiles\
            string[] profiles = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + @"Profiles\", "*.profile");
            pComboBox.Items.Add("Default");
            foreach (string profile in profiles)
            {
                if(Path.GetFileNameWithoutExtension(profile) != "Default")
                    pComboBox.Items.Add(Path.GetFileNameWithoutExtension(profile));
            }

            //Initialize Controller
            controller = new Controller();
            sink = new SwitchInputSink("COM8");
            sink.Update(InputFrame.ParseInputString("P=8"));
        }

        private void Form1_Closing(object sender, FormClosingEventArgs e)
        {
            sink.Dispose();
        }

        private void Panel1_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            //load current settings
            controller.mouseTranslation.mc_sensitivity.X = (double)mValueX1.Value;
            controller.mouseTranslation.mc_sensitivity.Y = (double)mValueX1.Value;

            controller.mouseTranslation.mc_sensitivity2.X = (double)mValueX2.Value;
            controller.mouseTranslation.mc_sensitivity2.Y = (double)mValueX2.Value;

            controller.mouseTranslation.mc_mouse_delta_start_threshold.X = (double)mValueX3.Value;
            controller.mouseTranslation.mc_mouse_delta_start_threshold.Y = (double)mValueX3.Value;

            controller.mouseTranslation.mc_delta_sensitivity_sigmoid_constant.X = (double)mValueX4.Value;
            controller.mouseTranslation.mc_delta_sensitivity_sigmoid_constant.Y = (double)mValueX4.Value;

            controller.mouseTranslation.mc_delta_initial.X = 128 * (double)mValueX5.Value;
            controller.mouseTranslation.mc_delta_initial.Y = 128 * (double)mValueX5.Value;

            controller.mouseTranslation.mc_delta_stop_threshold.X = 128 * (double)mValueX6.Value;
            controller.mouseTranslation.mc_delta_stop_threshold.Y = 128 * (double)mValueX6.Value;

            controller.mouseTranslation.mc_delta_damping_origin.X = 128 * (double)mValueX7.Value;
            controller.mouseTranslation.mc_delta_damping_origin.Y = 128 * (double)mValueX7.Value;

            controller.mouseTranslation.mc_delta_damping.X = (double)mValueX8.Value;
            controller.mouseTranslation.mc_delta_damping.Y = (double)mValueX8.Value;

            controller.mouseTranslation.mc_delta_damping2.X = (double)mValueX9.Value;
            controller.mouseTranslation.mc_delta_damping2.Y = (double)mValueX9.Value;

            controller.mouseTranslation.mc_delta_damping_sigmoid_constant.X = (double)mValueX10.Value;
            controller.mouseTranslation.mc_delta_damping_sigmoid_constant.Y = (double)mValueX10.Value;

            timer1.Start();
            timer2.Start();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            controller.MouseUpdate(sink, panel1);
        }

        private void Form1_Leave(object sender, EventArgs e)
        {
            timer1.Stop();
            timer2.Stop();
            controller.firstTime = true;
            //Mouse.Clip = Rectangle.Empty; //unlocks mouse
        }

        private void Timer2_Tick(object sender, EventArgs e)
        {
            controller.KeyBoardUpdate(sink);

            if (Keyboard.IsKeyDown(Key.Escape) && Keyboard.IsKeyDown(Key.LeftShift))
            {
                timer1.Stop();
                timer2.Stop();
                controller.firstTime = true;
            }
        }

        void MTrackBar(object sender, EventArgs e)
        {
            NoFocusTrackBar trackbar = sender as NoFocusTrackBar;
            string id1 = trackbar.Name.Substring(9, 1); //is it x or y?
            int id2 = Convert.ToInt32(trackbar.Name.Substring(10)); //what index?

            //Console.WriteLine(id1 + " " + id2);
            if (id1 == "X") //x-value trackbar
            {
                if ((tabPage3.Controls["checkBox" + (id2)] as CheckBox).Checked)
                {
                    (tabPage3.Controls["mTrackBarY" + (id2)] as NoFocusTrackBar).Value = trackbar.Value;
                    (tabPage3.Controls["mValueY" + (id2)] as NumericUpDown).Value = trackbar.Value / (1000m);
                }
                (tabPage3.Controls["mValueX" + (id2)] as NumericUpDown).Value = trackbar.Value / (1000m);
            }
            else
            {
                (tabPage3.Controls["mValueY" + (id2)] as NumericUpDown).Value = trackbar.Value / (1000m);
            }
        }

        void MValue(object sender, EventArgs e)
        {
            NumericUpDown value = sender as NumericUpDown;
            string id1 = value.Name.Substring(6, 1); //is it x or y?
            int id2 = Convert.ToInt32(value.Name.Substring(7)); //what index?

            //Console.WriteLine((int)(value.Value * 1000));
            if (id1 == "X") //x-value trackbar
            {
                if ((tabPage3.Controls["checkBox" + (id2)] as CheckBox).Checked)
                {
                    (tabPage3.Controls["mTrackBarY" + (id2)] as NoFocusTrackBar).Value = (int)(value.Value * 1000);
                    (tabPage3.Controls["mValueY" + (id2)] as NumericUpDown).Value = value.Value;
                }
                (tabPage3.Controls["mTrackBarX" + (id2)] as NoFocusTrackBar).Value = (int)(value.Value * 1000);
            }
            else
            {
                (tabPage3.Controls["mTrackBarY" + (id2)] as NoFocusTrackBar).Value = (int)(value.Value * 1000);
            }
        }

        void MCheckBox(object sender, EventArgs e)
        {
            CheckBox checkbox = sender as CheckBox;
            string id1 = checkbox.Name.Substring(8); //what index

            if (checkbox == circleTestCheck)
            {
                timer3.Enabled = !timer3.Enabled;
                sink.Update(InputFrame.ParseInputString("RX=128"));
                sink.Update(InputFrame.ParseInputString("RY=128"));
                return;
            }

            if (checkbox.Checked)
            {
                (tabPage3.Controls["mTrackBarY" + (id1)] as NoFocusTrackBar).Enabled = false;
                (tabPage3.Controls["mValueY" + (id1)] as NumericUpDown).Enabled = false;

                //update to maintain ratio
                var temp = (tabPage3.Controls["mTrackBarX" + (id1)] as NoFocusTrackBar).Value;
                (tabPage3.Controls["mTrackBarX" + (id1)] as NoFocusTrackBar).Value = 0;
                (tabPage3.Controls["mTrackBarX" + (id1)] as NoFocusTrackBar).Value = temp;
            }
            else
            {
                (tabPage3.Controls["mTrackBarY" + (id1)] as NoFocusTrackBar).Enabled = true;
                (tabPage3.Controls["mValueY" + (id1)] as NumericUpDown).Enabled = true;
            }
        }

        void OpenKeyForm(object sender, EventArgs e)
        {
            EditKeyForm dialog = new EditKeyForm();
            DialogResult d = dialog.ShowDialog(this);

            // Show testDialog as a modal dialog and determine if DialogResult = OK.
            if (d == DialogResult.OK) //key press/click
            {
                Console.WriteLine(dialog.input);
            }
            else if (d == DialogResult.Yes) //mouse aim
            {

            }
            else if (d == DialogResult.Retry) //movement
            {

            }
            else
            {

            }
            dialog.Dispose();
        }

        void SaveLoadButton(object sender, EventArgs e)
        {
            System.Windows.Forms.Button button = sender as System.Windows.Forms.Button;
            if (button.Text == "Save")
            {
                saveProfile(pComboBox.Text);
                pComboBox.Items.Add(pComboBox.Text);
            }
            else if (button.Text == "Load")
            {
                loadProfile(pComboBox.Text);
            }
            else if (button.Text == "Delete")
            {
                File.Delete(AppDomain.CurrentDomain.BaseDirectory + @"Profiles\" + pComboBox.Text + ".profile");
                pComboBox.Items.Remove(pComboBox.Text);
                pComboBox.Text = "";
            }
        }

        void saveProfile(string profileName)
        {
            string[] lines = { mValueX1.Value + "," + mValueY1.Value,
                               mValueX2.Value + "," + mValueY2.Value,
                               mValueX3.Value + "," + mValueY3.Value,
                               mValueX4.Value + "," + mValueY4.Value,
                               mValueX5.Value + "," + mValueY5.Value,
                               mValueX6.Value + "," + mValueY6.Value,
                               mValueX7.Value + "," + mValueY7.Value,
                               mValueX8.Value + "," + mValueY8.Value,
                               mValueX9.Value + "," + mValueY9.Value,
                               mValueX10.Value + "," + mValueY10.Value };

            File.WriteAllLines(AppDomain.CurrentDomain.BaseDirectory + @"Profiles\" + profileName + ".profile", lines);
        }

        void loadProfile(string profileName)
        {
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"Profiles\" + profileName + ".profile"))
            {
                string[] lines = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + @"Profiles\" + profileName + ".profile");

                int id = 1;
                foreach (string line in lines)
                {
                    if (line != null)
                    {
                        string[] value = line.Split(',');
                        (tabPage3.Controls["mValueX" + (id)] as NumericUpDown).Value = decimal.Parse(value[0]);
                        (tabPage3.Controls["mValueY" + (id)] as NumericUpDown).Value = decimal.Parse(value[1]);
                        id++;
                    }
                }
            }
        }

        private void pComboBox_Update(object sender, EventArgs e)
        {
            SwitchKBMEmulator.Properties.Settings.Default["Profile"] = pComboBox.Text;
            SwitchKBMEmulator.Properties.Settings.Default.Save();
        }

        private void Timer3_Tick(object sender, EventArgs e)
        {
            controller.CircleTest(sink);
        }

        private void EditKeysButton_Click(object sender, EventArgs e)
        {
            if(editKeysButton.Text == "Edit Keybinds")
            {
                editKeysButton.Text = "Save Keybinds";
            }
            else if(editKeysButton.Text == "Save Keybinds")
            {
                editKeysButton.Text = "Edit Keybinds";
            }

            //hide/show buttons
            foreach (Control c in joyPanel_lf.Controls) // left face buttons
            {
                if (c.GetType() == typeof(System.Windows.Forms.Button))
                    (c as System.Windows.Forms.Button).Visible = !(c as System.Windows.Forms.Button).Visible;
            }
            foreach (Control c in joyPanel_lt.Controls) // left shoulder buttons
            {
                if (c.GetType() == typeof(System.Windows.Forms.Button))
                    (c as System.Windows.Forms.Button).Visible = !(c as System.Windows.Forms.Button).Visible;
            }
            foreach (Control c in joyPanel_rf.Controls) // right face buttons
            {
                if (c.GetType() == typeof(System.Windows.Forms.Button))
                    (c as System.Windows.Forms.Button).Visible = !(c as System.Windows.Forms.Button).Visible;
            }
            foreach (Control c in joyPanel_rt.Controls) // right shoulder buttons
            {
                if (c.GetType() == typeof(System.Windows.Forms.Button))
                    (c as System.Windows.Forms.Button).Visible = !(c as System.Windows.Forms.Button).Visible;
            }
        }
    }
}
