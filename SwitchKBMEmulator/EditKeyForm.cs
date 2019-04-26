using System;
using System.Windows.Forms;
using System.Windows.Input;

namespace SwitchKBMEmulator
{
    public partial class EditKeyForm : Form
    {
        public Object input;
        public EditKeyForm()
        {
            InitializeComponent();
            button1.DialogResult = DialogResult.OK;
            button2.DialogResult = DialogResult.Yes;
            button3.DialogResult = DialogResult.Retry;
        }

        private void Input_KeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            button1.Enabled = true;
            input = KeyInterop.KeyFromVirtualKey((int)e.KeyCode);
            inputBox.Text = input.ToString();
        }


        private void Input_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            button1.Enabled = true;
            input = e.Button;
            inputBox.Text = "Mouse Click " + input;
        }
    }
}
