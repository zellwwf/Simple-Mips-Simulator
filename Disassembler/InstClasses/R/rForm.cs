using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MADS.InstClasses.R
{
    class rForm :Inst
    {
        //Public 
        public int opcode = 0;
        public int rs;
        public int rt;
        public int rd;
        public int sa;
        public int funct;
        public int simulateData;

        public void simulate()
        {
            //Get The Registers
            int rsVal = Classes.Register.Registers.ElementAt<Classes.Register>(rs).read(); //Source
            int rtVal = Classes.Register.Registers.ElementAt<Classes.Register>(rt).read(); //Target
            int rdVal = Classes.Register.Registers.ElementAt<Classes.Register>(rd).read(); //Destination

            //Switch and Simulate
            switch (Convert.ToString(funct, 2))
            {
                case "100000": //add
                    int x = rsVal + rtVal;
                    Classes.Register.Registers.ElementAt<Classes.Register>(rd).write(x);

                    break;
                case "100001": //addu
                    //covnert to unsigned
                    uint urtVal = (uint)rtVal;
                    uint ursVal = (uint)rsVal;

                    break;
                case "100100": //and

                    break;
                case "001101": //break

                    break;
                case "011010": //div

                    break;
                default:

                    break;
            }
            //Save the Registers
        }
    }
}
