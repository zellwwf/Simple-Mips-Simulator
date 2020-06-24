using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MADS.Formats
{
    class rInst
    {
        /*
         * Lists
         */
        public static List<rInst> rInstList = new List<rInst>();

        /*
         * Private Properties
         */
        private string mnmemonic;
        private string opcode;
        private string rs;
        private string rt;
        private string rd;
        private string shamt;
        private string funct;

        /*
         * Public Functions
         */
        //Sets
        public void setRSinBin(string binNum)
        {
            rs = binNum;
        }
        public void setRS(int num)
        {
            rs = Convert.ToString(num, 2);
        }
        public void setRS(string RegName)
        {
            string x;
            for (int i = 0; i < 32; i++)
            {
                x = Classes.Register.Registers.ElementAt<Classes.Register>(i).GetName();
                if (x == RegName)
                {
                    rs = Classes.Register.Registers.ElementAt<Classes.Register>(i).GetBinNum();
                    break;
                }
            }
        }

        public void setRTinBin(string binNum)
        {
            rt = binNum;
        }
        public void setRT(int num)
        {
            rt = Convert.ToString(num, 2);
        }
        public void setRT(string RegName)
        {
            string x;
            for (int i = 0; i < 32; i++)
            {
                x = Classes.Register.Registers.ElementAt<Classes.Register>(i).GetName();
                if (x == RegName)
                {
                    rt = Classes.Register.Registers.ElementAt<Classes.Register>(i).GetBinNum();
                    break;
                }
            }
        }

        public void setRDinBIN(string binNum)
        {
            rd = binNum;
        }
        public void setRD(int num)
        {
            rd = Convert.ToString(num, 2);
        }
        public void setRD(string RegName)
        {
            string x;
            for (int i = 0; i < 32; i++)
            {
                x = Classes.Register.Registers.ElementAt<Classes.Register>(i).GetName();
                if (x == RegName)
                {
                    rd = Classes.Register.Registers.ElementAt<Classes.Register>(i).GetBinNum();
                    break;
                }
            }
        }

        public void setShamt(int amount)
        {
            shamt = Convert.ToString(amount, 2);

        }
        public void setShamt(string binAmount)
        {
            shamt = binAmount;
        }

        //Gets
        public string getRSinBin()
        {
            return rs;
        }
        public int getRSinInt()
        {
            return Convert.ToInt32(rs, 2);
        }

        public string getRTinBin()
        {
            return rt;
        }
        public int getRTinInt()
        {
            return Convert.ToInt32(rt, 2);
        }

        public string getRDinBin()
        {
            return rd;
        }
        public int getRDinInt()
        {
            return Convert.ToInt32(rd, 2);
        }

        public string getMnemonic()
        {
            return mnmemonic;
        }

        public string getOpcode()
        {
            return opcode;
        }

        public string getFunct()
        {
            return funct;
        }

        /*
         * Constructors
         */
        public rInst(string mnemonic, string opcode, string funct)
        {
            this.mnmemonic = mnemonic;
            this.opcode = opcode;
            this.funct = funct;
        }
    }
}
