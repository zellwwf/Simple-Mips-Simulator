using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MADS.Classes
{
    class Register
    {
        /* EVENTS AND DELEGATES */
        public delegate void RegChangeHandler();
        public static event RegChangeHandler RegisterChanged;
        //Inits
        public static int initbit = 0;

        //List
        public static List<Register> Registers = new List<Register>();
        //Members
        int num;
        string name;
        int sizeinbyte = 4;
        int sizeinbit = 32;
    int data;

        //Functions
        public void write(int d)
        {
            if (num == 0)
            {
                data = 0;

            }
            else
            {
                data = d;

            }

            RegisterChanged.Invoke();
        }
        public int read()
        {
            return data;
        }
        public string GetName()
        {

            return name;
        }
        public string GetBinNum()
        {
            return (Convert.ToString(num, 2));
        }
        private static void extendbin(ref string bintoextend, int tosize)
        {
            //Takes stuff like "10" and extends them to size (ie 6) to "000010"
            int length = bintoextend.Length; //here its 2
            int diff = tosize - length;      //here its 6 -2 = 4
            string x = ""; //dummy string
            for (int i = 0; i < diff; i++)
            {
                x += "0";
            }
            bintoextend = x + bintoextend;
        }
        public string GetBinNumExtended()
        {
            string x = Convert.ToString(num, 2);
            extendbin(ref x, 5);
            return x;
        }
        public static int GetValfromBIN(string num)
        {
            int i = Convert.ToInt32(num, 2);
            int x = Registers.ElementAt<Register>(i).read();
            return x;
        }
        public static int GetNumfromName(string regname)
        {
            for (int i = 0; i < 32; i++)
            {
                if (regname == Registers.ElementAt<Register>(i).name)
                {
                    return Registers.ElementAt<Register>(i).num;
                }
            }
            return -1;
        }
        public static Register GetRegfromName(string regname)
        {
            for (int i = 0; i < 32; i++)
            {
                if (regname == Registers.ElementAt<Register>(i).name)
                {
                    return Registers.ElementAt<Register>(i);
                }
            }
            return null;
        }
        public int getNum()
        {
            return num;
        }
        //Constructor
        public Register(int nu, string na)
        {
            this.num = nu;
            this.name = na;
            this.data = 0;
            Registers.Add(this);
            initbit = 0;
        }
        public Register()
        {
            this.num = 32;
            this.name = "dummy";
            this.data = 0;
        }
    }
}
