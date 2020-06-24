using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MADS.Classes
{
    class Assembler
    {
        /*
         *                                      Class Assembler
         *              
         *          A 2 pass assembler with a zero pass (total of 3 passes)
         *          Inputs: lines of strings
         *          Output: binary dump
         */
        public static string[] Directives = { ".data", ".text", ".asciiz", ".word" };
        //public static Dictionary<string, Instruction> InstructionOpcodes = new Dictionary<string, Instruction>(); //first string isthe opcode, second string is menmonic
        public static Dictionary<string, Instruction> InstructionMnem = new Dictionary<string, Instruction>(); //Mnem first, instr scond
        //public static Dictionary<string, string> MnemnonicOpcode = new Dictionary<string,string>(); //Mnemonic first, opcode second. 
        public static Dictionary<string, ulong> SymbolTable = new Dictionary<string, ulong>();
        public static Dictionary<string,ulong> UnresolvedSymbols = new Dictionary<string,ulong>();
        public List<string> InputLines = new List<string>(); //The unassembled lines
        public List<string> AssembledLines = new List<string>(); //Assembled ones



        public static ulong LC; //LineCOunter ... this is not the PC, pc is in simulator

        //Methods
        public int FirstPass()
        {
            for (int i = 0; i < InputLines.Count; i++)
            {
                Line line = new Line(InputLines.ElementAt<string>(i));  //Feed new line
                //Now extract the info from the line.
                string mc="NULL";
                switch (line.inst.format)
                {
                    case 0:
                        //R format
                        //mc = mcg_rFormat(line);
                        break;
                    case 1:
                        //mc = mcg_iFormat(line);
                        break;
                    case 2:
                        break;
                    default:
                        break;

                }
                AssembledLines.Add(mc);
                LC++; //increment the label counter (line counter)
                

            }
            return 0;
        }

                //Parsers
        public bool check_illegal(string line)
        {
            //Ignoring Comments
            line.Substring(0,line.IndexOf('#'));

            //Return true = pass
            //return false = failed
            bool x = true;
            for (int i = 0; i < line.Length; i++)
            {
                char c = line.ElementAt<char>(i);
                if (char.IsLetterOrDigit(c))
                {
                    x = true;

                }
                else if (c == '#' || c == '$' || c == ':' || c == '(' || c == ')' || c == '-' || c == ' ' || c == '+')
                {
                    x = true;
                }
                else
                {
                    x = false;
                }
               
            }
            return x;

        }

        //MC Generators
        //public string mcg_iFormat(Line x)
        //{
            /*
             * I Format Instructions come in 2 form of operands
             * tsi
             * stl
             * 
             * if stl check mnemonic if branch or load/store
             * 
             *      ALG:
             *      1 - get operand string
             *      2 - if tsi
             *      3 - parse string accordingly --DONE
             *      4 - else if stl 
             *      5 - check if load/store: then parse the line accordingly --DONE
             *      6 - else its branch: parse string accordingly --DONE
             *      
             *      Input: LINE, Output: Machine Code String
             */


            //string mc;
            ////Decode everything first, then the mnemonic
            //string[] regopcode = new string[x.operands.Length - 1];
            //string immediateopcode ="";
            //for (int i = 0; i < x.operands.Length; i++)
            //{
            //    if (x.operands[i].isReg)
            //    {
            //        int regnum = x.operands[i].registerNum;
            //        string regmc = Convert.ToString(regnum, 2);
            //        extendbin(ref regmc, 5);
            //        regopcode[i] = regmc;

            //    }
            //    else if (x.operands[i].isImm)
            //    {
            //        int imm = x.operands[i].immediate;
            //        string imm_mc = Convert.ToString(imm, 2);
            //        extendbin(ref imm_mc, 16);
            //        immediateopcode = imm_mc;
            //    }
            //    else if (x.operands[i].isLabel)
            //    {
            //        //Resolve the label
            //        string labelval = x.operands[i].label.getName();
            //        ulong address;
            //        SymbolTable.TryGetValue(labelval, out address);
            //        //Now convert the address to immdiate;
            //        string imm_mc = Convert.ToString((int)address, 2); //cast
            //        extendbin(ref imm_mc, 16);
            //        immediateopcode = imm_mc;

            //    }
            //}
            ////Now convert the mnemonic
            //string mn_mc;
            //string mnemonic = x.mnemonic;
            //MnemnonicOpcode.TryGetValue(mnemonic, out mn_mc);
            ////Now we have everything's opcode
            ////Now check the format 
            ////Ie if reverse is rue
            //Instruction currentInst;
            //InstructionMnem.TryGetValue(mnemonic, out currentInst);
            //if (currentInst.reverseOperands)
            //{
            //    //true, ie addi addiu andi...
            //    mc = MnemnonicOpcode + regopcode[1] + regopcode[0] + immediateopcode;


            //} 
            //else
            //{
            //    mc = MnemnonicOpcode + regopcode[0] + regopcode[1] + immediateopcode;
            //}

            //return mc;
        //}


        //public string mcg_rFormat(Line x)
        //{
        //    string mc;
        //    string sa_mc="sa_mc";
        //    //Decode everything first, then the mnemonic
        //    string[] regopcode = new string[3];
        //    for (int i = 0; i < x.operands.Length; i++)
        //    {
        //        if (x.operands[i].isReg)
        //        {
        //            int regnum = x.operands[i].registerNum;
        //            string regmc = Convert.ToString(regnum, 2);
        //            extendbin(ref regmc, 5);
        //            regopcode[i] = regmc;

        //        }
        //        if (x.operands[i].isImm) {
        //            int sa = x.operands[i].immediate;
        //            sa_mc = Convert.ToString(sa,2);
        //            extendbin(ref sa_mc,5);
        //        }
        //    }
        //    //Now convert the mnemonic
        //    string mn_mc;
        //    string mnemonic = x.mnemonic;
        //    MnemnonicOpcode.TryGetValue(mnemonic, out mn_mc); //Should be the funct here
        //    //Now we have everything's mc
        //    //Now check the format 
        //    //Ie if reverse is rue
        //    Instruction currentInst;
        //    InstructionMnem.TryGetValue(mnemonic, out currentInst);
        //    //Check if it is jr
        //    if (currentInst.mnemonic == "jr")
        //    {

        //        mc = "000000" + regopcode[0] + "000000000000000001000"; // i hate hardcoding but... :(
        //    }
        //    else
        //    {
        //        //Not Jr
        //        if (currentInst.reverseOperands)
        //        {
        //            if (currentInst.isShift)
        //            {

        //                mc = "000000" + regopcode[2] + regopcode[1] + sa_mc + mn_mc;
        //            }
        //            else
        //            {
        //                mc = "000000" + regopcode[2] + regopcode[0] + regopcode[1] + "00000" + mn_mc;
        //            }



        //        }
        //        else
        //        {
        //            if (currentInst.isShift)
        //            {

        //                mc = "000000" + regopcode[2] + regopcode[1] + sa_mc + mn_mc;
        //            }
        //            else
        //            {
        //                mc = "000000" + regopcode[2] + regopcode[0] + regopcode[1] + "00000" + mn_mc;
        //            }
        //        }
        //    }

        //    return mc;

        //}

                //Formatters
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
    }
    class Label
    {
        /*
         *                  Class Label
         *      merely defines the objects "labels"
         */
        
        //Members
        private string name;
        private ulong val;

        //Methods
        public string getName()
        {
            return name;
        }
        public ulong getValue()
        {
            return val;
        }
        public void setValue(ulong value)
        {
            this.val = value;
        }
        public void setName(string nam)
        {
            this.name = nam;
        }
        public Label(string name, ulong val)
        {
            this.name = name;
            this.val = val;
        }
    }
    class Operand
    {
        /*
         *                          Class Operand
         *          Operands are one of three things
         *          either Immediate values (integers, addresses
         *          or     Registers
         *          or     Label values (which should be translated to it's value on pass1)
         *          ------------------------------------------------------------------------
         */
        //Event Handlers and Exceptions
        public delegate void IncorrectRegisterHandler(int regnum);
        public delegate void ImmediateOutoutRangeHandler(int im, int size);

        public event IncorrectRegisterHandler invalidRegisterNumber;
        public event ImmediateOutoutRangeHandler ImmediateOOR;
        //Members
        public bool isImm;
        public bool isReg;
        public bool isLabel;

        public Label label;
        public int registerNum;
        public int immediate;

        //The Three constructors... one for a register operand, one for a label operand and one for an immediate operand
        public Operand(int RegisterNum)
        {
            for (int i = 0; i < 32; i++)
            {
                if (Register.Registers.ElementAt<Register>(i).getNum() == RegisterNum)
                {
                    this.registerNum = Register.Registers.ElementAt<Register>(i).getNum();

                    isImm = false;
                    isReg = true;
                    isLabel = false;

                    break;
                }
                else if (i == 31)
                {
                    invalidRegisterNumber.Invoke(RegisterNum);
                }
            }
        }
        public Operand(int Imm, int size)
        {
            //check range of immediate
            //Convert it to string, and see if it exceeds size
            string x = Convert.ToString(Imm, 2);
            if (x.Length > size)
            {
                ImmediateOOR.Invoke(Imm, size);
            }
            else
            {
                this.immediate = Imm;

                isImm = true;
                isReg = false;
                isLabel = false;
            }
        }
        public Operand(Label x)
        {
            this.label.setName(x.getName());
            this.label.setValue(x.getValue());
            isImm = false;
            isReg = false;
            isLabel = true;
        }

    }
    class Line
    {
        /*
         *                  Line Class
         *    Defines a line, which holds: A Label, Mnemonic, Operands, Comment;
         *    has methods to validateLine, 
         *    check if it contains label, mnemonic, operand, comment;
         *    
         *      This class also checks for syntax, as it accepts string construction;
         */
        //Members
        public Label label;
        public string mnemonic;
        public Operand[] operands;
        public string comment;
        public Instruction inst;

        //Methods
        public bool containsLabel()
        {
            return (label == null);
        }
        public bool containsMnemonic()
        {
            return (mnemonic == null);
        }
        public bool containsComment()
        {
            return (comment == null);
        }
        public int getNumberofOperands()
        {
            return operands.Length;
        }

        //Formatter Methods
        private void clearEdgeWS(ref string x)
        {
            //This Method Clears the whitespace inthe begining of the of text and the end of the text
            //Aka, give a string like : " aaa "... will return "aaa"... 
            //Tested and it works.

            if (x.ElementAt<char>(0) == ' ')
            {
                x = x.Substring(1, x.Length - 1); //The -1 is because "aaa" has a length of 3 but elements from 0-2.. so .. :)
                clearEdgeWS(ref x);
            }
            else if (x.ElementAt<char>(x.Length - 1) == ' ')
            {
                x = x.Substring(0, x.Length - 2); //Trims the last thing
                clearEdgeWS(ref x);

            }
        }

        //Extractor Methods
        private string extractLabelDef(string line)
        {
            clearEdgeWS(ref line);
            if (line.Contains(':'))
            {
                this.label.setName(line.Substring(0, line.IndexOf(':')));
                this.label.setValue(Assembler.LC);
                return (line.Substring(0, line.IndexOf(':')));
            }
            else
            {
                return null;
            }
            
        }
        private string extractMnemonic(string line)
        {
            //Either it contains a label, or it doesn't
            if (inputHasLabelDef(line))
            {
                string line2 = line.Substring(line.IndexOf(':') + 1);
                clearEdgeWS(ref line2);
                string mnem = line2.Substring(0, line2.IndexOf(' '));
                if (isKnownMnem(mnem))
                {
                    this.mnemonic = mnem;
                    //Match it with an instruction
                    this.inst = Assembler.InstructionMnem[mnem];
                    return mnem;
                }
                else
                {
                    //throw err instr mnem not recognized
                    return "ERR";
                }

            }
            else
            {
                clearEdgeWS(ref line);
                string mnem = line.Substring(0, line.IndexOf(' '));
                if (isKnownMnem(mnem))
                {
                    this.mnemonic = mnem;
                    this.inst = Assembler.InstructionMnem[mnem];
                    return mnem;
                }
                else
                {
                    //throw err instr mnem not recognized
                    return "ERR";
                }
            }

        }
        private void extractOperands(string line)
        {
            /*
             *          The Types the operands strings you might encounter
             * 
             */
            //First get the format, to see what opstring you might face
            switch (this.inst.format)
            {
                case 0:

                    break;
                case 1:
                    extractOp_i(line);
                    break;
                case 2:
                    break;
                default:
                    //throw an error or something
                    //FILL ME
                    break;
            }
            ////first see how many operands there is by counting the commas
            //int noc=0;
            //int[] indexofComma;
            //for (int i = 0; i < line.Length; i++)
            //{
            //    char x = line.ElementAt<char>(i);
            //    if (x == ',')
            //    {
            //        noc++;
            //    }
            //}
            //if (noc != 0)
            //{
            //    indexofComma = new int[noc];
            //    indexofComma[0] = line.IndexOf(',');
            //    for (int i = 0; i < indexofComma.Length -1; i++)
            //    {
            //        indexofComma[i + 1] = line.IndexOf(',', indexofComma[i]+1);
            //    }
            //    //Now get the operands, then check their types
            //    string[] StringOps = new string[noc];
            //    if (noc == 0)
            //    {
            //        this.operands = new Operand[1];
            //    }
            //    else
            //    {
            //        this.operands = new Operand[noc];
            //    }
            //    for (int i = 0; i < noc; i++)
            //    {
            //        //Remember, you should place the operands in theLine's operands
            //        //here you will extrac the strings only, next you will check wtf they are
            //        //op1 isbetween comma 1 and comma 2 exculding whitespace
            //        //First Reg is between before comma one after ws 1
            //        //Sec op is between sec comma and first comma 
            //        //add $t1 ,$t2, $t3 
            //        //
            //        int length = 0;
            //        if (i == 1)
            //        {
            //            length = line.Length - indexofComma[1];
                        
            //        }
            //        else if (i == 0)
            //        {
            //             length = indexofComma[i]+1 - line.IndexOf(' ');
                         
            //        }
            //        else if (i == 2)
            //        {
            //             length = line.Length - indexofComma[i];
            //        }
            //        StringOps[i] = line.Substring(indexofComma[i], length);
            //        clearEdgeWS(ref StringOps[i]);
            //    }
            //    //Now you extracted the operands, check what are they
            //    for (int i = 0; i < noc; i++)
            //    {
            //        char x = StringOps[i].ElementAt<char>(0); //Get the first char
            //        if (char.IsDigit(x))
            //        {
            //            //immediate
            //            this.operands[i].isImm = true;
            //            this.operands[i].isLabel = false;
            //            this.operands[i].isReg = false;
            //            this.operands[i].immediate = Convert.ToInt32(StringOps[i]);
            //        }
            //        else if (char.IsLetter(x))
            //        {
            //            //Label
            //            this.operands[i].isImm = false;
            //            this.operands[i].isLabel = true;
            //            this.operands[i].isReg = false;
            //            // ok now find that label
            //            // if you didnt find it throw label to unresolved Symbol Table
            //            if (Assembler.SymbolTable.ContainsKey(StringOps[i]))
            //            {
            //                string nam = StringOps[i];
            //                ulong val;
            //                if (Assembler.SymbolTable.TryGetValue(nam, out val))
            //                {
            //                    this.label.setValue(val);
            //                    this.label.setName(nam);
            //                }
            //            }
            //            else
            //            {
            //                Assembler.UnresolvedSymbols.Add(StringOps[i], Assembler.LC);
            //            }
                        
            //        }
            //        else if (x == '$')
            //        {
            //            //Register
            //            this.operands[i].isImm = false;
            //            this.operands[i].isLabel = false;
            //            this.operands[i].isReg = true;

            //            //we have the registers name, check it with all other names
            //            for (int k = 0; k < 32; k++)
            //            {
            //                string regname = Register.Registers.ElementAt<Register>(i).GetName();
            //                int regnum = Register.Registers.ElementAt<Register>(i).getNum();
            //                int num;
            //                try
            //                {
            //                    int stringNum = Convert.ToInt32(StringOps[i]);
            //                    num = stringNum;
            //                }
            //                catch (FormatException)
            //                {
                                
            //                    throw;
            //                }
            //                //now check both
            //                //but first remove that stupid dollar sign
            //                StringOps[i] = StringOps[i].Substring(1);
            //                if (StringOps[i] == regname)
            //                {
            //                    this.operands[i].isReg = true;
            //                    this.operands[i].isLabel = false;
            //                    this.operands[i].isImm = false;

            //                    this.operands[i].registerNum = regnum;

            //                }
            //                else if (num != null && num == regnum)
            //                {
            //                    this.operands[i].isReg = true;
            //                    this.operands[i].isLabel = false;
            //                    this.operands[i].isImm = false;

            //                    this.operands[i].registerNum = regnum;

            //                }

            //            }
            //        }
            //        else
            //        {
            //            //Unrecognizable piece of shit!
            //        }
            //    }
            //}
            //else
            //{
            //    //do nothing
            //}

        }
        private void extractOp_i(string line)
        {
            //reg = tsl     branch = tis    loadstore =tsi
            //addi $t1, $t2, $t3        beq $t1, $t2, label     lw $t1,3($t2)
            switch (this.inst.operands)
            {
                case "tsi":
                    //Regular I
                    //Op0
                    operands = new Operand[3];
                    //operands[0].isReg = true;
                    string rt = line.Substring(line.IndexOf('$'));
                    string remain = rt.Substring(line.IndexOf('$'));
                    
                    clearEdgeWS(ref rt);
                    operands[0].registerNum = Register.GetNumfromName(rt);
                    
                    //Op1
                    operands[1].isReg = true;
                    string rs = line.Substring(line.IndexOf(", $")+2,line.IndexOf(',',line.IndexOf(',')+1));
                    clearEdgeWS(ref rs);
                    operands[1].registerNum = Register.GetNumfromName(rs);

                    //Op2
                    operands[2].isImm = true;
                    string im = line.Substring(line.IndexOf(", $") + 2 + rs.Length, line.IndexOf(' ', line.IndexOf(", $") + 3));
                    operands[2].immediate = Convert.ToInt32(im);
                    break;
                case "stl":
                    //Branch
                    break;
                case "tis":
                    //StoreLoad
                    break;
            }
        }
        //Checker methods
        public bool checkSyntax(string line)
        {
            //return true if ok
            
            //Fill me later
            return true;

        }
        public bool checkComment(string line)
        {
            if (line.Contains('#'))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool inputHasLabelDef(string line)
        {
            return (line.Contains(':'));
        }
        private bool isKnownMnem(string mnem)
        {
            if (Assembler.InstructionMnem.ContainsKey(mnem))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Now the constructors
        public Line(string input)
        {
            //We need to check the input line for syntax first
            if (checkSyntax(input))
            {
                //First see if it has a label def
                extractLabelDef(input);
                extractMnemonic(input);
                extractOperands(input);
            } 
        }

        
    }
    class Instruction
    {
        /*
         *              A Class that defines objects "instruction"
         */
        //Members
        public string mnemonic;         //ie add
        public string description;      //Adds the value of rs and rt and stores in rd
        public string operands;         //A ordered string in this example "dst"
                                        //Which provides the basis for assembler to understand operand location
        public int numberofOperands;    
        public string opcodeorfunct;
        public int format; //0 r, 1 i, 2 j
        //public bool reverseOperands; //DELETE ME
        //public bool isShift;// DELETE ME

        //Constructor
        public Instruction(string mnem, string opfunct, int formatofInstruction, string OrderedOperands)
        {
            this.mnemonic = mnem;
            this.numberofOperands = OrderedOperands.Length;
            this.opcodeorfunct = opfunct;
            this.format = formatofInstruction;
            this.operands = OrderedOperands;
            this.description = "NA";
            switch (this.format)
            {
                case 0:
                    Simulator.Rformat.Add(this.opcodeorfunct, this);
                    break;
                case 1:
                    Simulator.Iformat.Add(this.opcodeorfunct, this);
                    break;
                case 2:
                    Simulator.Jformat.Add(this.opcodeorfunct, this);
                    break;
                default:
                    break;
            }
        }
        public Instruction(string mnem, string opfunct, int formatofInstruction, string OrderedOperands, string Desc)
        {
            this.mnemonic = mnem;
            this.numberofOperands = OrderedOperands.Length;
            this.opcodeorfunct = opfunct;
            this.format = formatofInstruction;
            this.operands = OrderedOperands;
            this.description = Desc;
            Assembler.InstructionMnem.Add(this.mnemonic, this);
            switch (this.format)
            {
                case 0:
                    Simulator.Rformat.Add(this.opcodeorfunct, this);
                    break;
                case 1:
                    Simulator.Iformat.Add(this.opcodeorfunct, this);
                    break;
                case 2:
                    Simulator.Jformat.Add(this.opcodeorfunct, this);
                    break;
                default:
                    break;
            }
        }
        

        //Reserved Constructor for simulator
        public Instruction(Instruction x) {
            this.mnemonic = x.mnemonic;
            this.description = x.description;
            this.numberofOperands = x.numberofOperands;
            this.opcodeorfunct = x.opcodeorfunct;
            this.format = x.format;
            this.operands = x.operands;
        }
    }
    class Disassembler 
    {
        /*
         * 
         *                  The Disassembler Class
         *                  
         *      This class takes in machine code and retranslates them into Assembly
         *      Takes the Machine code from the Memory's Text Section
         *      ===================================================================
         *      
         *      All input is 32 bit, all starting with opcode --> which will help us determine 
         *      the rest of the 32 bits. 
         *  
         */
        //========================
        // Lists and Tables
        //========================
        public static List<string> MachineCodes = new List<string>();//The Disassembler disassembles these only
                                                                    //So you betta place your mcs here! 

        public static List<string> AssembledLines = new List<string>(); //Disassembler Dumps his stuff here :P
        public static Dictionary<string, ulong> LabelTable = new Dictionary<string, ulong>();
        public static Dictionary<ulong, string> InvLabelTable = new Dictionary<ulong, string>();
        public static List<ulong> UnresolvedBLabelsLongs = new List<ulong>();
        public static List<ulong> unresolvedJLabel = new List<ulong>();
        public static Dictionary<ulong, int> unresolvedBlabel = new Dictionary<ulong, int>();
        public static List<int> iOfLinesMentionsLabel = new List<int>();
        public static int fileOrEditor = 1;
        public static ulong Current_Inst_Address = 4194304;
        //========================
        //Delegates and Events
        //========================

        public delegate void DissassemblerErrorHandler(int line);
        public delegate void DissassemblerHandler();
        public delegate void DisassemblerErrorHandler2(string sender, string err, string message);
        public delegate void DisassemblyCompleteHandler(int tofile1editor0);
        public delegate void DisassemblyStartedHandler();

        public static event DisassemblyCompleteHandler DisassemblyComplete;
        public static event DisassemblyStartedHandler DisassemblyStarted;
        public event DisassemblerErrorHandler2 UnsupportedFormat;
        public event DisassemblerErrorHandler2 FailedToLoadMC;
        public event DissassemblerHandler MachineCodeLoaded;
        public static event DisassemblerErrorHandler2 GeneralError;

        //============================
        // Methods
        //============================
        
                //MC Loaders
        public void loadMC_fromMem()
        {
            //Assuming there is only ONE program in the memory
            //It reads the .text address by address and loads 
            //each address until it reaches InstructionCounter. (no instructions after that)
            //first check if instructionCounter !=0
            if (Mem.instructionCounter != 0)
            {
                for (int i = 0; i < Mem.instructionCounter*4; i=i+4)
                {
                    MachineCodes.Add(Mem.loadInstruction(i));
                }
            }
            else
            {
                FailedToLoadMC.Invoke("loadMC_fromMem::Disassembler", "NO INSTRUCTIONS FOUND IN MEMORY", "Dear User, please ... assemble first or something... or load from file!");
            }

        }

                //MC Decoders

        public int getFormat(string line)
        {
            //Returns 0 for R, 1 for I, 2 for J, -1 for err
            //Take a substring from that line to capture opcode;
            string opcode = line.Substring(0, 6); //Opcode
            switch (opcode)
            {
                case "000000":

                    return 0;
                case "000010":

                    return 2;
                case "000011":

                    return 2;
                case "010000":
                    //Coproc1
                    return -1;
                case "010001":
                    //Coproc2
                    return -1;
                case "010010":
                    //Coproc3
                    return -1;
                    //Coproc4
                case "010011":
                    return -1;
                    
                default:
                    return 1;


            }
        }
        public string decodeI(string iLine)
        {
            //I format can hold rs, rt and immediate
            //In that order OPCODE(6) RS(5) RT(5) IMM(16)
            string opcode = iLine.Substring(0, 6);
            string rs = iLine.Substring(6, 5);
            string rt = iLine.Substring(11, 5);
            string imm = iLine.Substring(16);
            string imm_s = imm;
            signExtend(ref imm_s, 32); //So just in case we have a signed op.
            
            //rs rt and imm, then decode opcode so you know where to place em;
            //Declare the elements of the big string to retrun

            int imm_int = Convert.ToInt32(imm, 2);
            int imm_sint = Convert.ToInt32(imm_s, 2);
            string rs_name = getRegName(rs); //so if the namer didn't get them just throw the word err
            string rt_name = getRegName(rt);
            string mnemonic;
            string mipsline;
            ulong fulladdress = (ulong)imm_sint * 4 + Current_Inst_Address + 4;

            //Ok We got Register names, and IM now we need to decode OPCode to see where they fit in the STRING :)
            //Start decoding OPcode and return :) we're done
            switch (opcode)
            {
                case "001000": //Addi
                    mnemonic = "addi";
                    mipsline = mnemonic + " $" + rt_name + ", $" + rs_name + ", " + imm_sint;
                    return mipsline;
                case "001001": //Addiu
                     mnemonic = "addiu";
                    mipsline = mnemonic + " $" + rt_name + ", $" + rs_name + ", " + imm_int;
                    return mipsline;
                case "001100": //Andi
                    mnemonic = "andi";
                    mipsline = mnemonic + " $" + rt_name + ", $" + rs_name + ", " + imm_int;
                    return mipsline;
                case "000100": //beq
                    mnemonic = "beq";
                    int iol = (int)(Current_Inst_Address +4 - 4194304)/4;
                    iOfLinesMentionsLabel.Add(iol-1);
                    UnresolvedBLabelsLongs.Add(Current_Inst_Address + 4);
                    unresolvedBlabel.Add(Current_Inst_Address + 4, imm_sint);
                    mipsline = mnemonic + " $" + rt_name + ", $" + rs_name + ", " + "%"+ fulladdress;
                    return mipsline;
                case "000101": //bne
                    mnemonic = "bne";
                    iol = (int)(Current_Inst_Address +4 - 4194304)/4;
                    iOfLinesMentionsLabel.Add(iol-1);
                    UnresolvedBLabelsLongs.Add(Current_Inst_Address + 4);
                    unresolvedBlabel.Add(Current_Inst_Address + 4, imm_sint);
                    mipsline = mnemonic + " $" + rt_name + ", $" + rs_name + ", " + "%" + fulladdress;
                    return mipsline;
                case "100000": //lb
                    mnemonic = "lb";
                    mipsline = mnemonic + " $" + rt_name + ", " + imm_sint + "($" + rs_name + ")";
                    return mipsline;
                case "100100": //lbu
                    mnemonic = "lbu";
                    mipsline = mnemonic + " $" + rt_name + ", " + imm_int + "($" + rs_name + ")";
                    return mipsline;
                case "100001": //lh
                    mnemonic = "lh";
                    mipsline = mnemonic + " $" + rt_name + ", " + imm_sint + "($" + rs_name + ")";
                    return mipsline;
                case "100101": //lhu
                    mnemonic = "lhu";
                    mipsline = mnemonic + " $" + rt_name + ", " + imm_int + "($" + rs_name + ")";
                    return mipsline;
                case "100011": //lw
                    mnemonic = "lw";
                    mipsline = mnemonic + " $" + rt_name + ", " + imm_int + "($" + rs_name + ")";
                    return mipsline;
                case "001101": //ori
                    mnemonic = "ori";
                    mipsline = mnemonic + " $" + rt_name + ", $" + rs_name + ", " + imm_int;
                    return mipsline;
                case "101000": //sb
                    mnemonic = "sb";
                    mipsline = mnemonic + " $" + rt_name + ", " + imm_int + "($" + rs_name + ")";
                    return mipsline;
                case "001010": //slti
                    mnemonic = "slti";
                    mipsline = mnemonic + " $" + rt_name + ", $" + rs_name + ", " + imm_sint;
                    return mipsline;
                case "001011": //sltiu
                    mnemonic = "sltiu";
                    mipsline = mnemonic + " $" + rt_name + ", $" + rs_name + ", " + imm_int;
                    return mipsline;
                case "101001": //sh
                    mnemonic = "sh";
                    mipsline = mnemonic + " $" + rt_name + ", " + imm + "($" + rs_name + ")";
                    return mipsline;
                case "101011": //sw
                    mnemonic = "sw";
                    mipsline = mnemonic + " $" + rt_name + ", " + imm + "($" + rs_name + ")";
                    return mipsline;
                case "001110": //xori
                    mnemonic = "xori";
                    mipsline = mnemonic + " $" + rt_name + ", $" + rs_name + ", " + imm_int;
                    return mipsline;
                case "001111": //Lui
                    mnemonic = "lui";
                    mipsline = mnemonic + " $" + rt_name + ", " + imm_int;
                    return mipsline;
                    break;
                default:
                    //unsupported, throw error
                    GeneralError.Invoke("iFormatDecoder::Disassembler", "OPCODE: " + opcode, "Hi there, the disassembler encountered an unsupported i-format instruction");
                    return "ERR";
            }
        }
        public string decodeR(string rLine)
        {
            //r format holds opcode, rs, rt, rd, sa , and funct... 
            //OPCODE(6) RS(5) RT(5) RD(5) SA(5) FUNCT(6)
            string rs = rLine.Substring(6, 5);
            string rt = rLine.Substring(11, 5);
            string rd = rLine.Substring(16, 5);
            string sa = rLine.Substring(21, 5);
            string funct = rLine.Substring(26);

            //Lets first decode the rs, rd, rt , sa then see what funct it is and determine
            //the print format, just like in the Iformat;
            string rs_name = getRegName(rs);
            string rt_name = getRegName(rt);
            string rd_name = getRegName(rd);
            int sa_int = Convert.ToInt32(sa, 2);
            string mnemonic;
            string mipsline;

            //Ok now we got all the info except for funct, decode it and return string :)
            switch (funct)
            {
                case "100000": //add
                    mnemonic = "add";
                    mipsline = mnemonic + " $" + rd_name + ", $" + rs_name + ", $" + rt_name;
                    return mipsline;
                case "100001": //addu
                    mnemonic = "addu";
                    mipsline = mnemonic + " $" + rd_name + ", $" + rs_name + ", $" + rt_name;
                    return mipsline;
                case "100100": //and
                    mnemonic = "and";
                    mipsline = mnemonic + " $" + rd_name + ", $" + rs_name + ", $" + rt_name;
                    return mipsline;
                case "001000": //jr
                    mnemonic = "jr";
                    mipsline = mnemonic + " $" + rs_name;
                    return mipsline;
                case "100111": //nor
                    mnemonic = "nor";
                    mipsline = mnemonic + " $" + rd_name + ", $" + rs_name + ", $" + rt_name;
                    return mipsline;
                case "100101": //or
                    mnemonic = "or";
                    mipsline = mnemonic + " $" + rd_name + ", $" + rs_name + ", $" + rt_name;
                    return mipsline;
                case "000000": //sll
                    mnemonic = "sll";
                    mipsline = mnemonic + " $" + rd_name + ", $" + rt_name + ", " +sa_int;
                    return mipsline;
                case "000100": //sllv
                    mnemonic = "sllv";
                    mipsline = mnemonic + " $" + rd_name + ", $" + rs_name + ", $" + rt_name;
                    return mipsline;
                case "101010": //slt
                    mnemonic = "slt";
                    mipsline = mnemonic + " $" + rd_name + ", $" + rs_name + ", $" + rt_name;
                    return mipsline;
                case "101011": //sltu
                    mnemonic = "sltu";
                    mipsline = mnemonic + " $" + rd_name + ", $" + rs_name + ", $" + rt_name;
                    return mipsline;
                case "000011": //sra
                    mnemonic = "sra";
                    mipsline = mnemonic + " $" + rd_name + ", $" + rt_name + ", " + sa_int;
                    return mipsline;
                case "000111": //srav
                    mnemonic = "srav";
                    mipsline = mnemonic + " $" + rd_name + ", $" + rs_name + ", $" + rt_name;
                    return mipsline;
                case "000010": //srl
                    mnemonic = "srl";
                    mipsline = mnemonic + " $" + rd_name + ", $" + rt_name + ", " + sa_int;
                    return mipsline;
                case "000110": //srlv
                    mnemonic = "srlv";
                    mipsline = mnemonic + " $" + rd_name + ", $" + rs_name + ", $" + rt_name;
                    return mipsline;
                case "100010": //sub
                    mnemonic = "sub";
                    mipsline = mnemonic + " $" + rd_name + ", $" + rs_name + ", $" + rt_name;
                    return mipsline;
                case "100011": //subu
                    mnemonic = "subu";
                    mipsline = mnemonic + " $" + rd_name + ", $" + rs_name + ", $" + rt_name;
                    return mipsline;
                case "001100": //syscall
                    mnemonic = "syscall";
                    mipsline = mnemonic;
                    return mipsline;
                case "100110": //xor
                    mnemonic = "xor";
                    mipsline = mnemonic + " $" + rd_name + ", $" + rs_name + ", $" + rt_name;
                    return mipsline;
                default:
                    GeneralError.Invoke("decodeR::Disassembler", "FUNCT: " + funct, "Unsupported R instruction found, sorry for that");
                    return "ERR";

            }
        }
        public string decodeJ(string jLine)
        {
            //Contains opcode(6) + target(26);
            string address = jLine.Substring(6);
            ulong cia = Current_Inst_Address +4;
            string pcstring = Convert.ToString((uint)cia, 2);
            extendbin(ref pcstring, 32);
            pcstring = pcstring.Substring(0, 4);
            address = pcstring + address + "00";
            string opcode = jLine.Substring(0, 6);
            string mipsline;
            string mnem;
            //First get the address
            int add_int = Convert.ToInt32(address, 2);
            ulong address_ulong = Convert.ToUInt64(address, 2);

            //Now get the opcode and return the mipsline
            switch (opcode)
            {
                case "000010":
                    mnem = "j";
                    int iol = (int)(Current_Inst_Address +4 - 4194304)/4;
                    iOfLinesMentionsLabel.Add(iol-1);
                    unresolvedJLabel.Add(address_ulong);
                    mipsline = mnem + " " + "%"+address_ulong;
                    return mipsline;
                case "000011":
                    mnem = "jal";
                    iol = (int)(Current_Inst_Address + 4 - 4194304) / 4;
                    iOfLinesMentionsLabel.Add(iol-1);
                    unresolvedJLabel.Add(address_ulong);
                    mipsline = mnem + " " + "%" + address_ulong;
                    return mipsline;
                default:
                    //it should NEVER EVER reach here... i believe
                    return "ERR";
            }

        }
        public void ResolveLabels()
        {
            //resolve B labels
            for (int i = 0; i < unresolvedBlabel.Count; i++)
            {
                ulong add = UnresolvedBLabelsLongs[i];
                int offset = unresolvedBlabel[add];
                BLabel(offset, add);

            }
            //resolve J labels
            for (int i = 0; i < unresolvedJLabel.Count; i++)
            {
                ulong add = unresolvedJLabel[i];
                JLabel(add);
            }
            for (int i = 0; i < iOfLinesMentionsLabel.Count; i++)
            {
                int ind = iOfLinesMentionsLabel[i];
                string addtxt = AssembledLines[ind].Substring(AssembledLines[ind].IndexOf('%')+1);
                ulong addnum = Convert.ToUInt64(addtxt);
                string lbltxt = InvLabelTable[addnum];
                AssembledLines[ind] = AssembledLines[ind].Substring(AssembledLines[ind].IndexOf('%') - 1);
                AssembledLines[ind] += " " + lbltxt;
            }
        }
        public string JLabel(ulong address)
        {
            int x = LabelTable.Count;
            string lbl = "L_" + x;
            if (!(LabelTable.ContainsValue(address)))
            {
                LabelTable.Add(lbl, address);
                InvLabelTable.Add(address, lbl);
            }
            int index_line = (int)(address - 4194304) / 4;
            AssembledLines[index_line] = lbl + ":  " + AssembledLines[index_line];
            return lbl;
        }
        public string BLabel(int offset, ulong addressofBranch)
        {
            string lbl;
            int x = LabelTable.Count;
            lbl = "L_" + x;
            //Calc branch label
            ulong add = (ulong)(offset *4 + (int)Current_Inst_Address + 4);
            //Fixed Assembled Line
            int indexofLine = offset + (int)(addressofBranch - 4194304+4)/4;
            string l = lbl + ":  ";
            l = l+AssembledLines.ElementAt<string>(indexofLine);
            AssembledLines[indexofLine] = l;
            if (!(LabelTable.ContainsValue(add)))
            {
                LabelTable.Add(lbl, add);
                InvLabelTable.Add(add, lbl);
            }
            return lbl;
        }
                //MC Helpers
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
        private string getRegName(string regBinNumber)
        {
            for (int i = 0; i < 32; i++)
            {
                string rn = Register.Registers.ElementAt<Register>(i).GetBinNumExtended();
                if (regBinNumber == rn)
                {
                    return Register.Registers.ElementAt<Register>(i).GetName();
                }
            }
            return "ERR";

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
                //Main Methods
        public string DecodeLine(string MachineCode)
        {
            switch (getFormat(MachineCode))
            {
                case 0:
                    return decodeR(MachineCode);
                case 1:
                    return decodeI(MachineCode);
                case 2:
                    return decodeJ(MachineCode);
                case -1:
                    UnsupportedFormat.Invoke("DecodeLine::Disassembler", "MC : " + MachineCode, "Sorry, but COPROC instructions aren't currently supported.. tuff luck chump");
                    return "ERR-DecodeLine::Disassembler::Coproc_Error";
                default:
                    GeneralError.Invoke("DecodeLine::Disassembler", "--", "Lol, this is some wierd shit for realz!");
                    return "ERR-DecodeLine::Disassembler::LOL";

            }

        }
        public void Disassemble(int tofile0editor1)
        {
            //First Check if there are lines loading in MC list
            if (MachineCodes.Count != 0)
            {
                DisassemblyStarted.Invoke(); //just to notify anyone who's registered with the event ... 

                //Now load one line at a time
                //Then decode it and place it in assembledLine, then invoke complete~
                for (int i = 0; i < MachineCodes.Count; i++)
                {
                    //here is da decoding part
                    string x = DecodeLine(MachineCodes.ElementAt<string>(i));
                    //Here is the part to store...
                    AssembledLines.Add(x);
                    //Incrment counter
                    Current_Inst_Address += 4;
                }
                //Current_Inst_Address = 4194308;
                //ResolveLabels();
                DisassemblyComplete.Invoke(tofile0editor1); //yay!
            }
            else
            {
                GeneralError.Invoke("Disassemble::Disassembler", "We ain't got no machine codes", "Lol, u messin with me? I didn't find any machine codes to disassemble!");
            }
            
        }
                //Output handlers

    }
}
