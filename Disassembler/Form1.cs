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
    public partial class Form1 : Form
    {
        //Variables
        bool recmem;
        int oldlinecount;
        int newlinecount;
        //Sim Vars
        Classes.Simulator sim;
        Classes.Disassembler sim_dis;
        //END VARS
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Inits
            Sim_gb.Enabled = false;
            Classes.Initializer x = new Classes.Initializer();
            x.initRegisters();
            x.initKnownInstructions();
            DataStyleMenu.SelectedIndex = 0;
            OutputInitText();
            oldlinecount = richTextBox1.Lines.Length;
            MemoryStatusLabel("EMPTY", Color.DarkRed);
            //Connect Events
            Classes.Register.RegisterChanged += new Classes.Register.RegChangeHandler(RegChangedShoot);
            Classes.Mem.MemoryChanged += new Classes.Mem.MemEventHandler(MemoryChangedShoot);
            Classes.Mem.UnknownAccess +=new Classes.Mem.MemEventHandler(AccessUnknown);
            Classes.Mem.AccessReserved += new Classes.Mem.MemEventHandler(AccessReservedShoot);
            richTextBox1.VScroll += new EventHandler(richTextBox1_VScroll);
            Classes.Disassembler.DisassemblyComplete += new Classes.Disassembler.DisassemblyCompleteHandler(onDisComplete);
            Classes.Disassembler.DisassemblyStarted += new Classes.Disassembler.DisassemblyStartedHandler(onDisStart);
            Classes.Disassembler.GeneralError +=new Classes.Disassembler.DisassemblerErrorHandler2(onGeneralDisError);
        }

        //Responders to Cstom Events
        private void RegChangedShoot()
        {
            RegDatatxt.Text = "";
            RDNum.Text = "";
            RDName.Text = "";

            int style = DataStyleMenu.SelectedIndex;
            if (style == 0) //Hex
            {
                for (int i = 0; i < 32; i++)
                {
                    string name = Classes.Register.Registers.ElementAt<Classes.Register>(i).GetName();
                    int dat = Classes.Register.Registers.ElementAt<Classes.Register>(i).read();
                    RDNum.Text += i.ToString() + "\n";
                    RDName.Text += "$" + name + "\n";
                    RegDatatxt.Text += "0x" + Convert.ToString(dat, 16) + "\n";
                }
            }
            else if (style == 1) //Bin
            {
                for (int i = 0; i < 32; i++)
                {
                    string name = Classes.Register.Registers.ElementAt<Classes.Register>(i).GetName();
                    int dat = Classes.Register.Registers.ElementAt<Classes.Register>(i).read();
                    RDNum.Text += i.ToString() + "\n";
                    RDName.Text += "$" + name + "\n";
                    RegDatatxt.Text +=Convert.ToString(dat, 2) + "\n";
                }
            }
            else if (style == 2) //Dec
            {
                for (int i = 0; i < 32; i++)
                {
                    string name = Classes.Register.Registers.ElementAt<Classes.Register>(i).GetName();
                    int dat = Classes.Register.Registers.ElementAt<Classes.Register>(i).read();
                    RDNum.Text += i.ToString() + "\n";
                    RDName.Text += "$" + name + "\n";
                    RegDatatxt.Text += dat.ToString() + "\n";
                }
            }
            else if (style == 3) //ASCII
            {
                for (int i = 0; i < 32; i++)
                {
                    string name = Classes.Register.Registers.ElementAt<Classes.Register>(i).GetName();
                    int dat = Classes.Register.Registers.ElementAt<Classes.Register>(i).read();
                    RDNum.Text += i.ToString() + "\n";
                    RDName.Text += "$" + name + "\n";
                    switch (dat)
                    {
                        case 0:
                            RegDatatxt.Text += "NUL\n";
                            break;
                        case 1:
                            RegDatatxt.Text += "SOH\n";
                            break;
                        case 2:
                            RegDatatxt.Text += "STX\n";
                            break;
                        case 3:
                            RegDatatxt.Text += "ETX\n";
                            break;
                        case 4:
                            RegDatatxt.Text += "EOT\n";
                            break;
                        case 5:
                            RegDatatxt.Text += "ENQ\n";
                            break;
                        case 6:
                            RegDatatxt.Text += "ACK\n";
                            break;
                        case 7:
                            RegDatatxt.Text += "BEL\n";
                            break;
                        case 8:
                            RegDatatxt.Text += "BS\n";
                            break;
                        case 9:
                            RegDatatxt.Text += "TAB\n";
                            break;
                        case 10:
                            RegDatatxt.Text += "LF\n";
                            break;
                        case 11:
                            RegDatatxt.Text += "VT\n";
                            break;
                        case 12:
                            RegDatatxt.Text += "FF\n";
                            break;
                        case 13:
                            RegDatatxt.Text += "CR\n";
                            break;
                        case 14:
                            RegDatatxt.Text += "SO\n";
                            break;
                        case 15:
                            RegDatatxt.Text += "SI\n";
                            break;
                        case 16:
                            RegDatatxt.Text += "DLE\n";
                            break;
                        case 17:
                            RegDatatxt.Text += "DC1\n";
                            break;
                        case 18:
                            RegDatatxt.Text += "DC2\n";
                            break;
                        case 19:
                            RegDatatxt.Text += "DC3\n";
                            break;
                        case 20:
                            RegDatatxt.Text += "DC4\n";
                            break;
                        case 21:
                            RegDatatxt.Text += "NAK\n";
                            break;
                        case 22:
                            RegDatatxt.Text += "SYN\n";
                            break;
                        case 23:
                            RegDatatxt.Text += "ETB\n";
                            break;
                        case 24:
                            RegDatatxt.Text += "CAN\n";
                            break;
                        case 25:
                            RegDatatxt.Text += "EM\n";
                            break;
                        case 26:
                            RegDatatxt.Text += "SUB\n";
                            break;
                        case 27:
                            RegDatatxt.Text += "ESC\n";
                            break;
                        case 28:
                            RegDatatxt.Text += "FS\n";
                            break;
                        case 29:
                            RegDatatxt.Text += "GS\n";
                            break;
                        case 30:
                            RegDatatxt.Text += "RS\n";
                            break;
                        case 31:
                            RegDatatxt.Text += "US\n";
                            break;
                        default:
                            RegDatatxt.Text += Convert.ToChar(dat) + "\n";
                            break;
                    }
                }
            }

            
           
        }
        private void MemoryChangedShoot(ulong addr, byte val)
        {
            //Add The Changed address to a list called changed addrresses to facilitate resetting memory
            Classes.Mem.ChangedAddresses.Add(addr);
            
            //Check if Checkbox is ticked;
            if (recmem)
            {
                OutputWrite("Memory Changed\nAddr\t\tData");
                string x = addr.ToString() + "\t\t" + val.ToString();
                OutputWrite(x);
            }
        }
        private void AccessReservedShoot(ulong addr, byte val)
        {
            if (recmem)
            {
                OutputWrite(("An Attempt to modify the reserved memory addresses has been caught... naughty naughty\n\nAddress: " +
                    addr.ToString() + " \nData you tried to write, lol: " + val.ToString()));
            }
            else
            {
                MessageBox.Show("An Attempt to modify the reserved memory addresses has been caught... naughty naughty\n\nAddress: " +
                    addr.ToString() + " \nData you tried to write: " + val.ToString());
            }
        }
        private void AccessUnknown(ulong addr, byte val)
        {

        }
        private void AccessOOR(ulong addr, byte val)
        {

        }
        private void onDisComplete(int file0edit1)
        {
            if (file0edit1 == 0)
            {
                
            }
            else if (file0edit1 == 1)
            {
                richTextBox1.Text = "";
                for (int i = 0; i < Classes.Disassembler.AssembledLines.Count; i++)
                {
                    richTextBox1.Text += Classes.Disassembler.AssembledLines.ElementAt<string>(i);
                    richTextBox1.Text += "\n";

                }
            }
            else
            {
                //Throw Error
            }
            
            
            statusBar_txt1.Text = "Disassembly Completed!";
        }
        private void onDisStart()
        {
            statusBar_txt1.Text = "Disassembling ... ... ";
        }
        private void onGeneralDisError(string sender, string err, string msg)
        {
            MessageBox.Show("FROM: " + sender + "\n\nMSG: " + msg + "What Happened: " + err, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        //END Responders

        private void button2_Click(object sender, EventArgs e)
        {
            string x = "Contents Are:\n\n";
            
            for (int i = 0; i < 31; i++)
            {
                Classes.Register y = new Classes.Register();
                x += i;
                x += " Data: ";
                x += Classes.Register.Registers.ElementAt<Classes.Register>(i).read();
                x +="\n";

                


            }
            MessageBox.Show(x);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            int dat = Convert.ToInt32(datastr.Text);
            int ind = Convert.ToInt32(rstr.Text);
            Classes.Register.Registers.ElementAt<Classes.Register>(ind).write(dat);

        }

        private void DataStyleMenu_SelectedItemChanged(object sender, EventArgs e)
        {
            RegChangedShoot();
        }

        private void MemWriteBtn_Click(object sender, EventArgs e)
        {
            byte[] data;
            ulong addr = Convert.ToUInt64(MemoryWriteAddressTxt.Text);
            switch (MemoryDomainMenu.SelectedIndex)
            {
                case 0: //No Offset

                    break;
                case 1: //Text
                    addr += Classes.Mem.startofText;
                    break;
                case 2: //Static
                    addr += Classes.Mem.startofStatic;
                    break;
                case 3:
                    addr += Classes.Mem.startofDynamic;
                    break;
                case 4:
                    addr += Classes.Mem.startofStack;
                    break;
            }
            switch (datatypeDomain.SelectedIndex)
            {
                    
                case 0: //Int 32
                    data = Classes.Mem.Int32toByte(Convert.ToInt32(MemWriteDataTxt.Text));
                    Classes.Mem.storeWord(addr, data);
                    break;
                case 1: //Hex 4

                    break;
                case 2: //Hex 8

                    break;
                case 3: //ASCII

                    break;

            }
        }

        //My Damn Functions!!
        private void OutputInitText()
        {
            OutputWrite("Welcome");
            OutputWrite("----------------");
        }
        private void OutputWrite(string x)
        {
            output.Text += x + "\n";
        }

        private void RecordMemory_CheckedChanged(object sender, EventArgs e)
        {
            if (RecordMemory.Checked)
            {
                OutputWrite("Record Memory Changes : ON");
                recmem = true;
            }
            else
            {
                OutputWrite("Record Memory Changes : OFF");
                recmem = false;
            }
        }

        private void ViewMemBtn_Click(object sender, EventArgs e)
        {
            MemoryViewer mview = new MemoryViewer();
            mview.Show();
        }

        //GUI functions (lines... colors...)
        private void MemoryStatusLabel(string Status, Color x)
        {
            StatusBar_MemoryStatus.Text = "Memory Instruction Status: " + Status;
            StatusBar_MemoryStatus.ForeColor = x;

        }
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            //Handle Lines
            bool numofLinesChanged; //If the last char to enter is newline, true else false
            newlinecount = richTextBox1.Lines.Length;
            int lengthofSelection = 0;
            Color lastlinecolor = Color.Black;

            if (oldlinecount == newlinecount) //don't change number of lines;
            {
                lengthofSelection++;
                numofLinesChanged = false;
            }
            else if (oldlinecount > newlinecount) //Decrease Number of lines
            {
                numofLinesChanged = true;

                if (oldlinecount < 10)
                {
                    lineTB.Text = lineTB.Text.Substring(0, lineTB.Text.Length - 2);
                    oldlinecount--;
                }
                else if (oldlinecount < 100)
                {
                    lineTB.Text = lineTB.Text.Substring(0, lineTB.Text.Length - 3);
                    oldlinecount--;
                }
                else
                {
                    lineTB.Text = lineTB.Text.Substring(0, lineTB.Text.Length - 4);
                    oldlinecount--;
                }
            }
            else //Increase Number of lines
            {
                numofLinesChanged = true;
                lineTB.Text += newlinecount.ToString() + "\n";
                oldlinecount++;
            }
            /*Handles Coloring Text
             * 
             * Problem happens that all the text is colored, not only a single line :( baaaad
             * 
             * Currently Disabled, because very buggy
             */
            //Check first of the line, is it a hash? \
            if (false)
            {
                char firstCharofLastLine = richTextBox1.Lines.Last<string>().ElementAt<char>(0);
                int fcllIndex = richTextBox1.Text.IndexOf(firstCharofLastLine, richTextBox1.TextLength - 1);
                switch (firstCharofLastLine)
                {
                    case '#': //Color Comments
                        richTextBox1.SelectionStart = fcllIndex;
                        richTextBox1.SelectionLength = lengthofSelection + 1;
                        lastlinecolor = Color.MediumSlateBlue;
                        break;
                    case '.': //Color Directives
                        richTextBox1.SelectionStart = fcllIndex;
                        richTextBox1.SelectionLength = lengthofSelection + 1;
                        lastlinecolor = Color.MediumSeaGreen;
                        break;
                    default:

                        break;
                }
                if (numofLinesChanged)
                {
                    richTextBox1.SelectedText = "";
                }
                else
                {
                    richTextBox1.SelectionColor = lastlinecolor;
                }
            }
        }
        private void richTextBox1_VScroll(object sender, EventArgs e)
        {
            // This Doesn't work LOL 

            int x = richTextBox1.AutoScrollOffset.X;
            int y = richTextBox1.AutoScrollOffset.Y;
            lineTB.AutoScrollOffset.Offset(x, y);
        }

        private void assembleToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void parseOnlyToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }


        private void iFormatTstToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(getImmediateMC("AX"));
        }
        private string getImmediateMC(string string_only_containing_immediate)
        {
            string x = string_only_containing_immediate; // Too Long of a name;
            //X will be something like '-12' or '32';
            //The Convereter Class will take care of the sign, as we dont need to, we just need to converet
            //it into a bitstring
            try
            {
                int n = Convert.ToInt32(x); //ie "32" -> 32;
                string bitstring = Convert.ToString(n, 2); //convert 32 to base 2. 
                return bitstring;
            }
            catch (FormatException)
            {
                return "ERR";
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < Classes.Mem.instructionCounter; i++)
            {
                MachineCodeTxt.Text += Classes.Mem.loadInstruction(i) + "\n";
            }
        }

        private void SaveMC_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < MachineCodeTxt.Lines.Length; i++)
            {
                Classes.Mem.StoreInstruction(MachineCodeTxt.Lines.ElementAt<string>(i));
            }
            MessageBox.Show("Store Completed");
            MemoryStatusLabel("Instructions Loaded", Color.Green);
        }

        private void ClearMC_Click(object sender, EventArgs e)
        {
            Classes.Mem.nextAvailableText = Classes.Mem.startofText;
            Classes.Mem.instructionCounter = 0;

        }

        private void disassembleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void test2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Classes.Assembler x = new Classes.Assembler();
            for (int i = 0; i < richTextBox1.Lines.Length ; i++)
            {
                x.InputLines.Add(richTextBox1.Lines[i]);
            }
            x.FirstPass();

        }

        private void sim_start_Click(object sender, EventArgs e)
        {
            Sim_gb.Enabled = true;
            sim_decode.Enabled = false;
            sim_execute.Enabled = false;

            sim_dis = new Classes.Disassembler();
            sim = new Classes.Simulator();

            //Connect Events
            //Fetch Decode Execute
            sim.Fetched +=new Classes.Simulator.SimulatorCycleHandler(onFetch);
            sim.Decoded +=new Classes.Simulator.SimulatorCycleHandler(onDecode);
            sim.Excecuted +=new Classes.Simulator.SimulatorCycleHandler(onExecute);

            //TerminateHEvents
            sim.Terminate_Dropped += new Classes.Simulator.SimulatorTerminationHandler(sim_Terminate_Dropped);
            sim.Terminate_Successful += new Classes.Simulator.SimulatorTerminationHandler(sim_Terminate_Success);
            sim.Terminate_UserRequest += new Classes.Simulator.SimulatorTerminationHandler(sim_Terminate_UserRequest);
            sim.Terminate_Error +=new Classes.Simulator.SimulatorTerminationHandler(sim_Terminate_Error);
        }

        private void sim_stop_Click(object sender, EventArgs e)
        {
            Sim_gb.Enabled = false;
            


        }
        //Simulator Stuff
        private void onFetch()
        {
            sim_currentInst.Text = Classes.Simulator.IR;
            updateClockandPC();
        }
        private void onDecode()
        {
            sim_decodeInst.Text = sim_dis.DecodeLine(Classes.Simulator.IR);
            updateClockandPC();
        }
        private void onExecute()
        {
            sim_executeInst.Text = Classes.Simulator.IR;
            updateClockandPC();
        }
        private void updateClockandPC()
        {
            sim_PC.Text = Classes.Simulator.PC.ToString();
            sim_clock.Text = Classes.Simulator.clock.ToString();
        }

        private void sim_fetch_Click(object sender, EventArgs e)
        {
            sim.fetch();
            sim_decode.Enabled = true;
            sim_fetch.Enabled = false;
            sim_run.Enabled = false;
        }

        private void sim_decode_Click(object sender, EventArgs e)
        {
            sim.decode();
            sim_execute.Enabled = true;
            sim_decode.Enabled = false;
            sim_run.Enabled = false;
        }

        private void sim_execute_Click(object sender, EventArgs e)
        {
            sim.execute();
            sim_execute.Enabled = false;
            sim_fetch.Enabled = true;
            sim_run.Enabled = false;
        }

        private void sim_run_Click(object sender, EventArgs e)
        {
            bool getrun;
            do
            {
                sim.fetch();
                getrun = Classes.Simulator.running;
                if (!getrun)
                {
                    break;
                }
                sim.decode();
                sim.execute();
                getrun = Classes.Simulator.running;
                if (!getrun)
                {
                    break;
                }
            } while (getrun);
        }
        private void sim_Terminate_Dropped(string message)
        {
            sim_output.Text += "Simulation Stopped: " +message;
            Sim_gb.Enabled = false;
        }
        private void sim_Terminate_Success(string message)
        {
            sim_output.Text += "Simulation Stopped: " + message;
            Sim_gb.Enabled = false;
        }
        private void sim_Terminate_Error(string message)
        {
            sim_output.Text += "Simulation Stopped: " + message;
            Sim_gb.Enabled = false;
        }
        private void sim_Terminate_UserRequest(string message)
        {
            sim_output.Text += "Simulation Stopped: " + message;
            Sim_gb.Enabled = false;
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            sim.fetch();
            sim.decode();
            sim.execute();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            sim.resetPC();
            sim_output.Text += "Program Counter Resetted!\n";
        }

        private void outputToFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            saveFile_Dis.ShowDialog();
            string filename = saveFile_Dis.FileName;
            Classes.Disassembler x = new Classes.Disassembler();
            x.loadMC_fromMem();
            x.Disassemble(0);

            System.IO.StreamWriter file = new System.IO.StreamWriter(filename);
            ulong add;
            file.WriteLine("#####################################");
            file.WriteLine("# Disassembler by Abdulilah Azzazi");
            file.WriteLine("#####################################");
            file.WriteLine("\n\n");
            string r = "";
            file.WriteLine("----------------------------------------");
            file.WriteLine("\tLabel Table:");
            file.WriteLine("----------------------------------------");
            for (int i = 0; i < Classes.Disassembler.LabelTable.Count; i++)
            {
                r += "\nLABEL: " + "L_" + i + "\tAddress: " + Classes.Disassembler.LabelTable["L_" + i];
                file.WriteLine(r);

            }
            file.WriteLine("----------------------------------------");
            for (int i = 0; i < Classes.Disassembler.MachineCodes.Count; i++)
            {
                add = (ulong)(i * 4) + Classes.Mem.startofText;
                file.WriteLine(add +"\t" +Classes.Disassembler.MachineCodes.ElementAt<string>(i)+"\t" + Classes.Disassembler.AssembledLines.ElementAt<string>(i));
            }
            file.WriteLine("\nEND OF FILE");
            //Now open a file stream
            file.Close();
            MessageBox.Show("Output file Generated Successfully!\n FileName:"+filename);

        }

        private void outputToEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Classes.Disassembler x = new Classes.Disassembler();
            x.loadMC_fromMem();
            x.Disassemble(1);
        }

        private void labelTableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string x = "";
            for (int i = 0; i < Classes.Disassembler.LabelTable.Count; i++)
            {
                x += "\nLABEL: " + "L_" + i + "\tAddress: " + Classes.Disassembler.LabelTable["L_" + i];


            }
            MessageBox.Show(x);
        }

    }
}
