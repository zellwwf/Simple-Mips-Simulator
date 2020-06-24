using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MADS.Classes
{
    public partial class SysInput : Form
    {
        public SysInput()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            switch (Simulator.syscall_inptype)
            {
                case 5:
                    try
                    {
                        int x = Convert.ToInt32(textBox1.Text);
                        Mem.StoreWord_onNextAvailable_data(x);
                    }
                    catch (FormatException)
                    {
                        MessageBox.Show("Invalid Integer Input", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        textBox1.Text = "";
                    }
                    break;
                case 1: //Change Case
                    //Read string, and length

                    break;
                default:
                    break;

            }
            this.Close();
        }
    }
}
