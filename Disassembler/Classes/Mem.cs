using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MADS.Classes
{
    class Mem
    {
        /* Memories as Lists for ease of access
         * Memory Addresses of a 2GB memory, in simulation that is not possible
         * Memoriesh here are byte addressable, 8 bits... addressability can be even furter reduced 
         * IE EACH ADDRESS IS ONE BYTE
         * by using boolean algebra.
         * Start Address of Memory: 0 = 0x0 End Address: 2147483644 = 2GB = 0x7FFFFFFC
         * ---------------------------------------------------------------------------
         * Reserved: 0-4194304 ::: size 64 MB
         * Text Area: 4194304 - 268435456 :::252MB
         * Static Area: 268435456 - 268468224 ::: 32KB
         * Dynamic Area (the heap): 268468224 
         * Stack : 268468224<address<2147483644
         */
        public static byte[] Dynamic = new byte[1024]; //Also for Special Stuff
        public static byte[] Stack = new byte[1024];   //For special stuff
        public static byte[] Text = new byte[1024];    //TEXT
        public static byte[] Static = new byte[1024];  //DATA
        public static int instructionCounter = 0;

        public static List<ulong> ChangedAddresses = new List<ulong>();


        //---------------------------------------------------------------------------------//
        // Members
        //---------------------------------------------------------------------------------//
        public static ulong dynamicEndAddress = 268468224; //Each time 
        public static ulong stackEndAddress = 2147483644;
        public static ulong startofText = 4194304;
        public static ulong startofStatic = 268435456;
        public static ulong startofDynamic = 268468224;
        public static ulong startofStack = 2147483644;

        public static ulong nextAvailableText = 4194304 ; //Points to the next available address of the text mem, 
        public static ulong nextAvailableData = 268435456; //Points to the next available address of the data (static) mem,
        public static ulong nextAvailableDynamic = 268468224;   //Same
        public static ulong nextAvailableStack = 2147483644;     //Same
        //---------------------------------------------------------------------------------//
        // Events
        //---------------------------------------------------------------------------------//
        public delegate void MemEventHandler(ulong address, byte data);
        public static event MemEventHandler MemoryChanged;
        public static event MemEventHandler MemoryOutofRange;
        public static event MemEventHandler AccessReserved;
        public static event MemEventHandler UnknownAccess;


        //---------------------------------------------------------------------------------//
        // Functions
        //---------------------------------------------------------------------------------//

        public static int MemSwitch(ulong addressinByte, ref byte data, bool read1write0) {
            if (addressinByte < 4194304) //accessing reserved
            {
                ulong i = 4194304 - addressinByte;
                return 1; //Return Reserved
            }
            else if (addressinByte < 268435456) //accessing Text
            {
                ulong i = addressinByte - 4194304;
                if (read1write0)
                {
                    
                    data = Text[i];

                }
                else
                {
                    Text[i] = data;
                    //Raise Memory Changed Event
                    MemoryChanged(addressinByte, data);

                }
                
                return 2; //Text
            }
            else if (addressinByte < 268468224) //Accessing Static
            {
                ulong i = 268435456 - addressinByte;
                if (read1write0)
                {
                    data = Static[i];
                }
                else
                {
                    Static[i] = data;

                    //Raise Memory Changed Event
                    //MemoryChanged(addressinByte, data);
                }
                return 3; //Static
            }
            else if ( addressinByte<= dynamicEndAddress) //Accessing Dynamic
            {
                ulong i = dynamicEndAddress - addressinByte;
                if (read1write0)
                {
                    data = Dynamic[i];
                }
                else
                {
                    Dynamic[i] = data;

                    //Raise Memory Changed Event
                   // MemoryChanged(addressinByte, data);
                }
                return 4; //Dynamic
            }
            else if (addressinByte <= stackEndAddress) //Accessing Stack
            {
                ulong i = stackEndAddress - addressinByte;
                if (read1write0)
                {
                    data = Stack[i];
                }
                else
                {
                    Stack[i] = data;

                    //Raise Memory Changed Event

                }
                return 5;//Stack
            }
            else if (addressinByte > 2147483644)
            {
                return -1; //Mem out of range
            }
            else
            {
                return 0; //Not Found
            }
        }
        public static int GetSegment(ulong FullAddress,ref int offset)
        {
            if (FullAddress < startofText)
            {
                return 0; //Return Reserved
            }
            else if (FullAddress <startofStatic)
            {
                offset = Convert.ToInt32(FullAddress - startofText);
                return 1; //Return Text
            }
            else if (FullAddress < startofDynamic)
            {
                offset = Convert.ToInt32(FullAddress - startofStatic);
                return 2; //Return Static
            }
            else if (FullAddress < startofStack)
            {
                offset = Convert.ToInt32(FullAddress - startofDynamic);
                return 3; //Dynamic
            }
            else if (FullAddress < stackEndAddress)
            {
                offset = Convert.ToInt32(FullAddress - startofStack);
                return 4; //Stack
            }
            else
            {
                return -1; //Not Found Out of Range
            }

        }
        public static byte[] Int32toByte(int data) {
            return BitConverter.GetBytes(data);
        }

        /*Load Store Operations*/
        public static void storeByte(ulong FullAddress, byte data)
        {
            int index = 0;
            switch (GetSegment(FullAddress, ref index))
            {
                case 0: //Reserved
                    AccessReserved(FullAddress, data);
                    break;
                case 1: //Text Segment
                    Text[index] = data;
                    MemoryChanged(FullAddress, data);
                    break;
                case 2: //Static Segment
                    Static[index] = data;
                    MemoryChanged(FullAddress, data);
                    break;
                case 3: //Dynamic Segment
                    Dynamic[index] = data;
                    MemoryChanged(FullAddress, data);
                    break;
                case 4: //Stack Segment
                    Stack[index] = data;
                    //we have to fix the stack pointer
                    startofStack -= 1;
                    MemoryChanged(FullAddress, data);
                    break;
                case -1: //OutOfRange
                    MemoryOutofRange(FullAddress, data);
                    break;
                default:
                    UnknownAccess(FullAddress, data);
                    break;
            }
        }
        //Needs Revision all of it, plus remember to change the pointers when writing stuff
        public static void storeData(ulong FullAddress, byte[] data, int size)
        {
            ulong addr = FullAddress;
            for (int i = 0; i < size; i++)
            {
                storeByte(addr, data[i]);
                addr++;
            }

        }
        public static void storeWord(ulong FullAddress, byte[] data) 
        {
            storeData(FullAddress, data, 4);
        }
        public static void storeHalfWord(ulong FullAddress, byte[] data)
        {
            storeData(FullAddress, data, 2);
        }
        public static void storeStream(ulong FullAddress, byte[] data) 
        {
            storeData(FullAddress, data, data.Length);
        }

        public static byte loadByte(ulong FullAddress)
        {
            byte data = 0;
            int index = 0;
            switch (GetSegment(FullAddress, ref index))
            {
                case 0: //Reserved
                    AccessReserved(FullAddress, data);
                    break;
                case 1: //Text Segment
                    data = Text[index];
                    break;
                case 2: //Static Segment
                    data = Static[index];

                    break;
                case 3: //Dynamic Segment
                    data = Dynamic[index];
                    break;
                case 4: //Stack Segment
                    data = Stack[index];
                    break;
                case -1: //OutOfRange
                    MemoryOutofRange(FullAddress, data);
                    break;
                default:
                    UnknownAccess(FullAddress, data);
                    break;
            }
            return data;
        }

        public static void writeByte(ulong fullAddinByte, byte data, ref int status) 
        {
            status = MemSwitch(fullAddinByte, ref data, false);
        }
        public static byte readByte(ulong fullAddinByte, ref int status)
        {
            byte x=0;
            status = MemSwitch(fullAddinByte, ref x, true);
            return x;
        }



        public static void StoreWord_onNextAvailable_data(int dat)
        {
            //Stores instruction in the next available .text segment
            //Then increments the next available .text segment
            //Stores them in this process:
            //lets say mc = 11110000
            //Add1 will store 1111, add 2 will store 0000
            //and since mc is actually 32 bits, or 4 8 bits
            //We need to first, cut this mc into four pieces
            string[] pieces = new string[4];
            string mc = Convert.ToString(dat, 2);
            extendbin(ref mc, 32);
            pieces[0] = mc.Substring(0, 8);
            pieces[1] = mc.Substring(8, 8);
            pieces[2] = mc.Substring(16, 8);
            pieces[3] = mc.Substring(24, 8);

            //Now store these pieces
            for (int i = 0; i < 4; i++)
            {
                int index = (int)(nextAvailableData - startofStatic);
                byte x = Convert.ToByte(pieces[i], 2);
                Static[index] = x;
                nextAvailableData += 1; //Increment
            }
            //Think we're done here.
        }
        public static int LoadWord_data(int dat, int index)
        {
            string x ="";
            //offset = offset * 4; //Fixed offset
            //Should read four bytes from mem and regroupthem.
            byte[] pieces = new byte[4];
            pieces[0] = Static[index];
            pieces[1] = Static[index + 1];
            pieces[2] = Static[index + 2];
            pieces[3] = Static[index + 3];
            //Now we got the pieces, 
            //First convert them to strings and extend the 0s 
            string[] y = new string[4];

            for (int i = 0; i < 4; i++)
            {
                y[i] = Convert.ToString(pieces[i], 2);
                extendbin(ref y[i], 8);
                x += y[i];
            }
            return Convert.ToInt32(x);
        }

        public static void StoreInstruction(string mc)
        {
            //Stores instruction in the next available .text segment
            //Then increments the next available .text segment
            //Stores them in this process:
            //lets say mc = 11110000
            //Add1 will store 1111, add 2 will store 0000
            //and since mc is actually 32 bits, or 4 8 bits
            //We need to first, cut this mc into four pieces
            string[] pieces = new string[4];
            pieces[0] = mc.Substring(0, 8);
            pieces[1] = mc.Substring(8, 8);
            pieces[2] = mc.Substring(16, 8);
            pieces[3] = mc.Substring(24, 8);

            //Now store these pieces
            for (int i = 0; i < 4; i++)
            {
                int index = (int)(nextAvailableText - startofText);
                byte x = Convert.ToByte(pieces[i],2);
                Text[index] = x;
                nextAvailableText += 1; //Increment
            }
            instructionCounter++; 
            //Think we're done here.
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
        public static string loadInstruction(int offset)
        {
            string x = "";
            //offset = offset * 4; //Fixed offset
            //Should read four bytes from mem and regroupthem.
            byte[] pieces = new byte[4];
            pieces[0] = Text[offset];
            pieces[1] = Text[offset + 1];
            pieces[2] = Text[offset + 2];
            pieces[3] = Text[offset + 3];
            //Now we got the pieces, 
            //First convert them to strings and extend the 0s 
            string[] y = new string[4];

            for (int i = 0; i<4;i++) {
                y[i] = Convert.ToString(pieces[i],2);
                extendbin(ref y[i], 8);
                x += y[i];
            }

            return x;
        }

    }
    class MemInfo : EventArgs
    {
        private ulong memaddress;
        private byte memcontent;
        public ulong Address
        {
            set
            {
                memaddress = value;
            }
            get
            {
                return memaddress;
            }
        }
        public byte Data
        {
            set
            {
                memcontent = value;
            }
            get
            {
                return memcontent;
            }
        }
    }
}
