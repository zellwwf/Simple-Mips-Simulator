using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MADS.Classes
{
    class Simulator
    {
        /*
         *              Simulator Class
         *      this simulator is in no way an EMULATOR, it simulates these things only:
         *      -fetch-decode-execute cycles with no pipelining watsoever
         *      -Some mips instructions and their consequences:
         *          - syscalls
         *          - register changes
         *          
         *      -Program Termination:
         *          A program can terminate with one of these results:
         *              -Termination_Successful: Syscall of terminate
         *              -Terminate_ByUserRequest: Pressed on the stop button 
         *              -Terminate_Error: Shit happened from the simulators part.
         *              -Terminate_Dropped: The lines ended without a syscall terminate, pc address turned out to have a value of 0;
         *              
         *      -Cycles:
         *          - Fetch: 
         *                      1-Gets the current Instruction from memory address in PC
         *                      2-And Store it in IR (Instruction Register).
         *                      3-Then Increments PC to next Address;
         *          - Decode: 
         *                      1- Interprets the Instruction found in IR.
         *                      
         *          - Execute:
         *                      0- If it is jump or beq ... ---> Set PC to addres in jump...
         *                      1- If Instruction is Interrupt (here only syscall)... well interrupt the process.
         *                      2- If Memory Access Instruction:
         *                          - Place Address in Memory Address Register (MAR);
         *                          - Then Access Memory (either read data from MDR and place in Mem, or read from Mem and place in MDR)
         *                      3- Else excecute (on registers);
         *                      
         *          -Types of events that can happen:
         *                  - No Instructions to Simulate;
         *                  - Termination stuff
         *                  - Overflow error
         * -------------------------------------------------------------------------------------------------------------------------------
         */
        //Lists 
        public static Dictionary<string, Instruction> Rformat = new Dictionary<string,Instruction>();//Key: Funct -- Return Inst;
        public static Dictionary<string, Instruction> Iformat = new Dictionary<string,Instruction>(); //Key: Opcode -- Return Inst;
        public static Dictionary<string, Instruction> Jformat = new Dictionary<string, Instruction>(); //Key: Opcode -- Return Inst;

        //Members
        public static int PC = 4194304;
        public static string IR;             //values stored here are in string format, for ease of coding, BITSTRING
        public static int MAR;
        public static string MDR;             //BITSTRING
        public static int clock = 0;
        public static bool running;          //Denotes if the simulation is running or not.
        private Instruction instruction; 
        //Events
        public delegate void SimulatorEventHandler();
        public delegate void SimulatorTerminationHandler(string TerminationMessage);
        public delegate void SimulatorCycleHandler();
        public delegate void SimulatorOverFlowHandler();

        public event SimulatorTerminationHandler Terminate_UserRequest;
        public event SimulatorTerminationHandler Terminate_Successful;
        public event SimulatorTerminationHandler Terminate_Error;
        public event SimulatorTerminationHandler Terminate_Dropped;

        public event SimulatorEventHandler EmptyInstructions;
        public event SimulatorEventHandler SyscallLaunched;

        public event SimulatorCycleHandler Fetched;
        public event SimulatorCycleHandler Decoded;
        public event SimulatorCycleHandler Excecuted;

        public event SimulatorOverFlowHandler OverFlow;

        //Responders to Events

        private void Simulator_Terminate_Successful(string x)
        {
            running = false;
        }
        private void Simulator_Terminate_Dropped(string x)
        {
            running = false;
        }
        private void Simulator_Terminate_Error(string x)
        {
            running = false;
        }
        private void Simulator_Terminate_UserRequest(string x)
        {
            running = false;
        }
        //Methods

                //Helper Methods

        public void signExtend(ref string x, int tillsize)
        {
            if (x.ElementAt<char>(0) == '0')
            {
                //Extend 0s
                for (int i = 0; x.Length != tillsize; i++)
                {
                    x = "0" + x;

                }
            }
            else
            {
                //extend 1s
                for (int i = 0; x.Length != tillsize; i++)
                {
                    x = "1" + x;

                }

            }

        }
        private void extendbin(ref string bintoextend, int tosize)
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
        private bool overflow(int val, int size)
        {
            //My way is comupation extensive but uses less lines
            string v = Convert.ToString(val, 2);
            if (v.Length > size)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

                //Outsiders Methods 
        public void resetPC()
        {
            PC = 4194304;
        }
        public int getPC()
        {
            return PC;
        }
        public string getIR()
        {
            return IR;
        }
        public int getMAR()
        {
            return MAR;
        }
        public int getMDR_int()
        {
            return Convert.ToInt32(MDR, 2);
        }
        public string getMDR()
        {
            return MDR;
        }
        public int getClock()
        {
            return clock;
        }

                //Cycles
        public void fetch()
        {
            //First Get the instruction from the address of the pc counter.
            //Since the memory can load directly without the need of the full address, 
            //Calculate the offset
            int offset = PC - 4194304;
            
            //Now fetch;
            IR = Mem.loadInstruction(offset);

            //First check if IR =0
            int IR_int = Convert.ToInt32(IR, 2);
            if (IR_int == 0)
            {
                //Now check if Clock == 0;
                if (clock == 0)
                {
                    running = false;
                    EmptyInstructions.Invoke();

                }
                else
                {
                    running = false;
                    Terminate_Dropped.Invoke("Program Dropped...\n");

                }
            }
            else
            {
                running = true;

                //Now increment the PC;
                PC += 4; //by one because LoadInstruction handles the stuff.. ;)
                
                Fetched.Invoke();
            }
        }
        public void decode()
        {

            //Read the instruction from IR
            string inst = IR;

            //Decode it's opcode to see if it's I, or R or J:
            string opcode = IR.Substring(0, 6);

            switch (opcode)
            {
                case "000000":
                    //RFormat
                    string funct = inst.Substring(26);
                    instruction = Rformat[funct];

                    break;
                case "000010":
                    //J
                    instruction = Jformat[opcode];
                    break;
                case "000011":
                    //J
                    instruction = Jformat[opcode];
                    //Jformat.TryGetValue(opcode, out instruction);
                    break;
                case "010000":
                    //Coproc1
                    Terminate_Error.Invoke("COPROC Instructions aren't supported by simulator");
                    break;
                case "010001":
                    //Coproc2
                    Terminate_Error.Invoke("COPROC Instructions aren't supported by simulator");
                    break;
                case "010010":
                    //Coproc3
                    Terminate_Error.Invoke("COPROC Instructions aren't supported by simulator");
                    break;
                //Coproc4
                case "010011":
                    Terminate_Error.Invoke("COPROC Instructions aren't supported by simulator");
                    break;

                default:
                    //I instruction
                    instruction = Iformat[opcode];
                    //Iformat.TryGetValue(opcode, out instruction);
                    break;
            }
            //Increment Clock;
            
            Decoded.Invoke();
            
        }
        public void execute()
        {
         /*          - Execute:
          *                      0- If it is jump or beq ... ---> Set PC to addres in jump...
          *                      1- If Instruction is Interrupt (here only syscall)... well interrupt the process.
          *                      2- If Memory Access Instruction:
          *                          - Place Address in Memory Address Register (MAR);
          *                          - Then Access Memory (either read data from MDR and place in Mem, or read from Mem and place in MDR)
          *                      3- Else excecute (on registers);
          *                      
          */
            // 1- Check if its jump
            if (instruction.format == 2)
            {
                //Decode Address; 
                string address = IR.Substring(6);

                string pc_string = Convert.ToString(PC, 2);
                extendbin(ref pc_string, 32);
                //Put 00 in the end and the 4 most significant bits from pc in the front
                address = pc_string.Substring(0, 4) + address + "00";  //Absolute Address
                
                //Now check wether it's just jump or jump and link
                if (instruction.mnemonic == "jal")
                {
                    //set register RA = old PC +4
                    Register.Registers.ElementAt<Register>(31).write(PC);  //JAL sets the RA to next instruction after JAL.
                }
                //Add that to the PC
                int add = Convert.ToInt32(address,2);
                PC = add;
                //Increment Clock
                
                Excecuted.Invoke();
            }
            // 1- iformat
             else if (instruction.format == 1)
            {
                //Check if it's BRANCH
                switch (instruction.mnemonic)
                {
                    case "beq":
                        //Check for Condition.
                        string rs = IR.Substring(6, 5);
                        string rt = IR.Substring(11, 5);
                        int rs_val = Register.GetValfromBIN(rs);
                        int rt_val = Register.GetValfromBIN(rt);

                        if (rt_val == rs_val)
                        {
                            //Since its a branch, calculate the address like this
                            //add the two zeros, then sign extend, then turn to signed int
                            //then add it to PC+1; 
                            //... first load the 16 bit address :P
                            string address = IR.Substring(16);
                            address = address + "00";
                            signExtend(ref address, 32);
                            int adrs = Convert.ToInt32(address, 2);
                            PC = PC + adrs;  //Calculate new PC;
                            
                            Excecuted.Invoke();
                        }
                        //ELSE DO NOTHING, PC ALREADY INCREMENTED BY FETCH!
                        break;
                    case "bne":
                        rs = IR.Substring(6, 5);
                        rt = IR.Substring(11, 5);
                        rs_val = Register.GetValfromBIN(rs);
                        rt_val = Register.GetValfromBIN(rt);

                        if (rt_val != rs_val)
                        {
                            //Since its a branch, calculate the address like this
                            //add the two zeros, then sign extend, then turn to signed int
                            //then add it to PC+1; 
                            //... first load the 16 bit address :P
                            string address = IR.Substring(16);
                            address = address + "00";
                            signExtend(ref address, 32);
                            int adrs = Convert.ToInt32(address, 2);
                            PC = PC + adrs;  //Calculate new PC;
                            
                            Excecuted.Invoke();
                        }
                        break;
                    case "addi":
                            //Get the registers, do the operation.
                            //Remmber this has overflow!
                        rs = IR.Substring(6, 5);
                        rt = IR.Substring(11, 5);
                        rs_val = Register.GetValfromBIN(rs);
                        string imm = IR.Substring(16);
                        signExtend(ref imm, 32);
                        
                        //Good boy, now add and pc+1
                        int imm_sint = Convert.ToInt32(imm, 2);
                        int val = rs_val + imm_sint;

                        //Before writing to register, lets check for overflow. 
                        //By translating to bitstring and seeing the size, if more than 32 bits.. overflow
                        if (overflow(val, 32))
                        {
                            OverFlow.Invoke();
                            Terminate_Error.Invoke("Overflow from instruction in PC: " + PC.ToString() + " has been detected");
                        }
                        else
                        {
                            //No Overflow, so far so good, replace value in rt by val
                            Register.Registers.ElementAt<Register>(Convert.ToInt32(rt, 2)).write(val);
                            
                            Excecuted.Invoke();
                        }
                        
                        break;
                    case "addiu":
                        //No Overflow;
                        rs = IR.Substring(6, 5);
                        rt = IR.Substring(11, 5);
                        rs_val = Register.GetValfromBIN(rs);
                        imm = IR.Substring(16);
                        signExtend(ref imm, 32);
                        
                        //Good boy, now add and pc+1
                        imm_sint = Convert.ToInt32(imm, 2);
                        val = rs_val + imm_sint;
                        Register.Registers.ElementAt<Register>(Convert.ToInt32(rt, 2)).write(val);
                        
                        Excecuted.Invoke();
                        break;
                        
                        //Now the load store instructions
                    case "lb":
                        //              CANNOT TEST MARS DUMP BEFORE ADDING LUI AND SOME OTHER STUFF
                    //$t = MEM[$s + offset];
                        //first calculate the address to load from
                        rs = IR.Substring(6, 5);
                        rt = IR.Substring(11, 5);
                        rs_val = Register.GetValfromBIN(rs);
                        imm = IR.Substring(16);
                        signExtend(ref imm, 32);
                        imm_sint = Convert.ToInt32(imm, 2);

                        ulong addres = (ulong)rs_val + (ulong)imm_sint;
                        byte value_b = Mem.loadByte(addres);
                        Register.Registers.ElementAt<Register>(Convert.ToInt32(rt, 2)).write(value_b);
                        break;

                    case "sb":
                        rs = IR.Substring(6, 5);
                        rt = IR.Substring(11, 5);
                        rs_val = Register.GetValfromBIN(rs);
                        rt_val = Register.GetValfromBIN(rt);
                        string rt_vals = Convert.ToString(rt_val, 2);
                        imm = IR.Substring(16);
                        signExtend(ref imm, 32);
                        imm_sint = Convert.ToInt32(imm, 2);

                        addres = (ulong)rs_val + (ulong)imm_sint;
                        //Since the way i design the memory here is to access a stream of bytes
                        //I need to harnes a byte from my current int
                        byte rt_b = Convert.ToByte(rt_vals.Substring(27));
                        Mem.storeByte(addres, rt_b);
                        break;

                    case "andi":
                        rs = IR.Substring(6, 5);
                        rt = IR.Substring(11, 5);
                        rs_val = Register.GetValfromBIN(rs);
                        imm = IR.Substring(16);
                        int imm_uval = Convert.ToInt32(imm);
                        int res = rs_val & imm_uval;

                        Register.Registers.ElementAt<Register>(Convert.ToInt32(rt, 2)).write(res);

                        break;

                    default:
                        break;
                }
            }
            else if (instruction.format == 0)
            {
                string rs = IR.Substring(6, 5);
                string rt = IR.Substring(11, 5);
                string rd = IR.Substring(16, 5);

                string sa = IR.Substring(21, 5);
                int rs_i = Register.GetValfromBIN(rs); 
                int rt_i = Register.GetValfromBIN(rt);
                int sa_i = Convert.ToInt32(sa, 2);
                int rd_n = Convert.ToInt32(rd, 2);
                
                //r Format;
                switch (instruction.mnemonic)
                {
                    case "add":
                        //Addition with overflow
                        //rd = rs+rt
                        int val = rs_i + rt_i;
                        //Check for overflow;
                        if (overflow(val, 32))
                        {
                            OverFlow.Invoke();
                            Terminate_Error.Invoke("Overflow from instruction in PC: " + PC.ToString() + " has been detected");
                        }
                        else
                        {
                            Register.Registers.ElementAt<Register>(rd_n).write(val);
                            
                            Excecuted.Invoke();
                        }

                        break;
                    case "addu":
                        //No over Flow addition
                        //NOTE: Addu is broken down into three instructions in MARS, while here it is merely one instruction
                        val = rs_i + rt_i;
                        Register.Registers.ElementAt<Register>(rd_n).write(val);
                        
                        Excecuted.Invoke();
                        break;

                    case "and":
                        val = rs_i & rt_i;
                        Register.Registers.ElementAt<Register>(rd_n).write(val);
                        
                        Excecuted.Invoke();
                        break;

                    case "or":
                        val = rs_i | rt_i;
                        Register.Registers.ElementAt<Register>(rd_n).write(val);

                        Excecuted.Invoke();
                        break;
                    case "syscall":
                        Syscall();
                        break;

                    default:
                        break;
                }

            }
            //Increment Cycle
            clock++;

        }

                //Syscall Methods
        public static int syscall_int_inp;
        public static string syscall_stringz_inp;
        public static int syscall_inptype; //0 int, 1 string
        public void Syscall()
        {
            /*List of Supported syscalls:
             * - Read Int 5
             * - Print int 1
             * 
             * 
             * -----------
             * First read v0,
             * then switch and execute
             */
            int va = Register.Registers.ElementAt<Register>(2).read();
            switch (va)
            {
                case 5:
                    syscall_ReadInt();
                    break;
                case 10:
                    Terminate_Successful("Program Terminated by syscall Successfully!\n");
                    running = false;
                    break;
                default:
                    break;
            }

        }
        public void syscall_ReadInt() {
            SysInput x = new SysInput();
            x.ShowDialog();
            x.Activate();
        }
        public void syscall_PrintInt(int x)
        {

        }

        //Constructor
        public Simulator() {
            Terminate_Dropped +=new SimulatorTerminationHandler(Simulator_Terminate_Dropped);
            Terminate_Error +=new SimulatorTerminationHandler(Simulator_Terminate_Error);
            Terminate_Successful +=new SimulatorTerminationHandler(Simulator_Terminate_Successful);
            Terminate_UserRequest+=new SimulatorTerminationHandler(Simulator_Terminate_UserRequest);
        }
    }
}
