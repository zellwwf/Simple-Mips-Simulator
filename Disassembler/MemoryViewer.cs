using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MADS
{
    public partial class MemoryViewer : Form
    {
        public MemoryViewer()
        {
            InitializeComponent();
        }

        private void LoadDataBtn_Click(object sender, EventArgs e)
        {
            string add, dat;
            ulong adr = Classes.Mem.startofText;
            for (int i = 0; i < 1024; i++)
            {
                dat = Classes.Mem.Text[i].ToString();
                adr += 1;
                TextSeg.Text += adr.ToString() +"\t\t" + dat + "\n";
            }
        }

        private void MemoryViewer_Load(object sender, EventArgs e)
        {
            //Adjust the Lazy Viewer's Text
            int num = Classes.Mem.ChangedAddresses.Count;
            LazyViewer.Text = "";
            for (int i = 0; i < num; i++)
            {
                ulong add = Classes.Mem.ChangedAddresses.ElementAt<ulong>(i);
                byte dat = Classes.Mem.loadByte(add);
                LazyViewer.Text += "\nAddress: " + add.ToString() + "\t\tData: " + dat.ToString();
            }
        }
    }
}
