using InputServer;
using SwitchKBMEmulator;
using System;
using System.Drawing;
using System.Windows.Forms;

//using Button = InputServer.Button;
using Mouse = System.Windows.Forms.Cursor;

namespace KBMSwitchAdapter
{
    public partial class Form1 : Form
    {
        public SwitchInputSink sink;
        public Controller controller;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
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
        }
    }
}
