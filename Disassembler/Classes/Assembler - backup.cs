using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Disassembler.Classes
{
    class Assembler2
    {

    }

    class Assembler
    {
        /*
         *          The Assembler Class:
         *                              This class has specific goals which are
         *                              1- Take string lines and transform them into machine code
         *                              2- Take machine code and transform them into strings of mips code
         *                              
         *          The way we approach the first problem is as follows:
         *          1- send current line to parser:
         *              Parser will check for errors, figure out what kind of line it is, and format it, then it will
         *              a- Send it to comment table, along with it's line number
         *              b- Send it to be transformed to machine code
         *              c- Adjust  memory... other directive shit
         *          ====================================================================================================
         *          SHIT TO DO:
         *              - Still left in method ParseStringText the full directive checkup.
         */


        //Assembler Tables and stuff
        public static List<string> AssembledLines = new List<string>();
        public static List<int> LabelIndexes = new List<int>();
        public static List<Label> LabelTable = new List<Label>();
        public static List<string> StringLines = new List<string>();
        public static List<Line> ParsedLines = new List<Line>();

        private string[] knownInstructions = {"add", "addiu", "addu", "addi", "sub", "and", "or", "xor" ,
                                                 "andi", "ori", "xori", "lw", "sw", "sb", "lb", "lbu", 
                                                 "lh", "lhu", "sh", "beq", "bne", "slt", "slti", "j",
                                                 "jr", "jal","sll","srl", "sra", "sllv", "srlv", "srav",
                                                 "syscall"
                                             };
        private string[] knownDirectives = { ".data", ".text", ".asciiz", ".word" };


        //Assembler Events
        public delegate void AssemblerErrorHandler(string line, int lineindex);
        public delegate void Boo(Line x);
        public delegate void AssemblerHandler1();
        public delegate void AssemblerErrorHandler2(string Sender, string Message, string theError);

        public event AssemblerErrorHandler UnknownLineFormatError;
        public event Boo AssemblerMadeABooBoo;
        public static event AssemblerHandler1 AssembleComplete;
        public event AssemblerHandler1 DisassembleComplete;
        public event AssemblerHandler1 DataSectionMissing;
        public event AssemblerHandler1 TextSectionMissing;
        public event AssemblerErrorHandler2 GeneralError;
        
        //Assembler Global Variables

        public struct Directiveflags
        {
            bool isData, isText, isAlign, isAscii, isAsciiz, isbyte, isDouble, isFloat, isExtern, isglobl, isHalf, isWord, iskdata, isktext, isSet, isSpace;

        }

        //Assembler's Methods
        public void ParseStringLines()
        {
            /*
             *          This Method Takes lines from the list of StringLine and dumps them into ParsedLines
             *          It checks for errors too, it doesn't do anything but fill them into the ParsedLines.
             * 
             */
            //Initiatlize temp variables
            string currentLine;
            int currentIndex;
            int linetype;
            int start_data;
            int end_data;
            int start_text;
            int end_text;
            bool inData = false;
            bool inText = false;
            //Loop thru all the lines, convert them to 
            for (int i = 0; i < StringLines.Count; i++)
            {
                //First Check if Line is NULL; if null, go to next line;
                if (!(StringLines.ElementAt<string>(i) == ""))
                {
                    //Get the line
                    currentLine = StringLines.ElementAt<string>(i);
                    currentIndex = i;
                    //Clear Whitespace from line
                    clearEdgeWS(ref currentLine);

                    //Check Main Directives (.data and .text)
                    if (currentLine.Contains(".data"))
                    {
                        start_data = currentIndex;
                        inData = true;
                        if (inText)
                        {
                            end_text = currentIndex - 1;
                        }
                        inText = false;
                    }
                    else if (currentLine.Contains(".text"))
                    {
                        start_text = currentIndex;
                        inText = true;
                        if (inData)
                        {
                            end_data = currentIndex - 1;
                        }
                        inData = false;
                    }

                    //Check if it contains a label so you can store it in the label table
                    if (currentLine.Contains(':'))
                    {
                        string labelName = currentLine.Substring(0, currentLine.IndexOf(':'));
                        int labelIndex = currentIndex;
                        Label l = new Label(labelName, labelIndex);  //Create new label
                        if (inData)
                        {
                            l.setinData();
                        }
                        if (inText)
                        {
                            l.setinText();
                        }
                        LabelTable.Add(l);
                        MessageBox.Show("Captured Label: " + labelName);
                        //Now remove the label from the currentline and clear whitespace
                        currentLine = currentLine.Substring(currentLine.IndexOf(':')+1);

                        //--------------------------------
                        //By now, the labels aren't address resolved...
                        //The resolution will be done on simulation, 
                        //Because The AssembledLine will go to memory when we start Simulation

                    }

                    //Now Check the line's type;
                    //Switch will check for directive or comment only;
                    //Check if Line is null
                    if (currentLine == "")
                    {
                        continue;
                    }
                    char x = currentLine.ElementAt<char>(0);
                    switch (x)
                    {
                        case ' ':
                            currentLine.Remove(0, 1); //Removes the damn white space.. 
                            break;

                        case '#':
                            //Since it's a comment throw it away.. lol
                            break;

                        case '.':
                            //The reason here that i didn't put it in the parsedLines is that we need to still check the directive.
                            //Check if it's a known directive:
                            string guessedDir;
                            if (!currentLine.Contains(' '))
                            {
                                guessedDir = currentLine;
                            }
                            else
                            {
                                guessedDir = currentLine.Substring(0, currentLine.IndexOf(' ')); //The directive to check.
                            }
                            if (isKnownDirective(guessedDir))
                            {
                                linetype = 1;
                                Line Directive = new Line(currentLine, currentIndex, linetype);
                                ParsedLines.Add(Directive);
                            }
                            else
                            {
                                GeneralError.Invoke("Parser::Assembler", "Directive not found, or error in directive", "The Directive we're talking about: " + guessedDir);
                            }
                            break;

                        default:
                            //Check if it's wierd stuff
                            int y = (int)x; //Get the ascii code of the char

                            if (y >= 64 && y <= 90)
                            {
                                //then its a capital letter
                                //We are not sure if the rest is good, but hey, lets match it do the list of all instructions we know
                                //First Make this small and stop at the first whitespace, if whitespace exceeds 5, throw exception
                                y += 33;
                                char z = (char)y;
                                int FirstWhiteSpace = currentLine.IndexOf(' ');
                                string guessedMnem = z.ToString() + currentLine.Substring(1, FirstWhiteSpace - 1);

                                if (isKnownMnem(guessedMnem))
                                {
                                    linetype = 0;
                                    //Woohoo!! ok, just hope the isKnownMnem Method works correctly :P
                                    //Now, add it to the PARSEDLINES !! :P
                                    Line Inst = new Line(currentLine, linetype, currentIndex);
                                    ParsedLines.Add(Inst);
                                }
                                else
                                {
                                    //Throw exception and break... lol;
                                    linetype = -1;
                                    UnknownLineFormatError.Invoke(currentLine, currentIndex);
                                }

                            }
                            else if (y >= 97 && y <= 122)
                            {
                                //then it's a small letter
                                //We are not sure if the rest is good, but hey, lets match it do the list of all instructions we know
                                //Copy pastd code above and fixed it
                                char z = (char)y;
                                int FirstWhiteSpace = currentLine.IndexOf(' ');
                                string guessedMnem = z.ToString() + currentLine.Substring(1, FirstWhiteSpace - 1);

                                //Give it to the isKnownMnme Method
                                if (isKnownMnem(guessedMnem))
                                {
                                    linetype = 0;
                                    //Woohoo!! ok, just hope the isKnownMnem Method works correctly :P
                                    Line Inst = new Line(currentLine, linetype, currentIndex);
                                    ParsedLines.Add(Inst);
                                }
                                else
                                {
                                    //Throw exception and break... lol;
                                    linetype = -1;
                                    UnknownLineFormatError.Invoke(currentLine, currentIndex);
                                }

                            }
                            else
                            {
                                //Its not a . nor a # nor a letter, wtf... throw exception
                                linetype = -1;
                                UnknownLineFormatError.Invoke(currentLine, currentIndex);
                            }
                            //Since it's passed the test and exceptions, add the label's index 
                            //to the LabelIndexes so you can relocated faster in the Encoding part
                            break;
                    }
                }

            }
        }
        public void EncodeParsedLines()
        {
            /* This is one of the most important methods we've got... an error here will kill
             * THE LABELS FOR GODS SAKE... THE LABELS!!
             * 
             *              ENCODE_PARSED_LINES
             *      Takes in lines from parsedlines, 
             *      if they are comments, it ignores them
             *      if they are dirrectives it executes them
             *      if they are codes it encodes them
             *      
             *      if they contain label, resolve what's after it, and see what is the address of the label
             *      if they contain a comment later on, ignore the last section.
             * 
             *          Directives:
             *          
             *          Codes:
             *              - If R Format, encode regularly
             *              - If J Format, and remember the labels
             *              
             * 
             */
            //START

            //First Clear the Comments;
            for (int i = 0; i < ParsedLines.Count; i++)
            {
                ParsedLines.ElementAt<Line>(i).removeinlineComment();
            }



            for (int i = 0; i < ParsedLines.Count; i++)
            {
                //Create Dummy first
                Line x = new Line(ParsedLines.ElementAt<Line>(i)); //Just for ease of manipulation ... 
                //Now Fetch It's Type
                switch (x.getTypeInt())
                {
                    case 0:             //Type Instruction
                        char instFormat = GetInstFormat(x);
                        switch (instFormat)
                        {
                            case 'r':
                                //FILL ME
                                break;
                            case 'i':
                                string mc_iformat = GenerateMachineCode_Iformat(x.text);
                                AssembledLines.Add(mc_iformat);
                                break;
                            case 'j':
                                //FILL ME
                                break;
                            case 'x':
                                //FILL ME
                                break;
                            default:
                                //FILL ME
                                break;
                        }
                        break;

                    case 1:             //Type Directive

                        break;

                    case 2:             //Type Comment

                        break;

                    default:
                        AssemblerMadeABooBoo.Invoke(x);
                        break;
                }

            }
            AssembleComplete.Invoke();

        }

                     //Helpers "privates"
                        //Checkers
        private bool isKnownMnem(string guess)
        {
            for (int r = 0; r < knownInstructions.Length; r++)
            {
                if (knownInstructions[r] == guess)
                {
                    //yay it's an instruction we KNOW!
                    return true;
                }
            }
            return false;
        }
        private bool isKnownDirective(string guess)
        {
            for (int i = 0; i < knownDirectives.Length; i++)
            {
                if (guess == knownDirectives[i])
                {
                    return true;
                }
            }
            return false;
        }


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
        private void clearALL()
        {
            ParsedLines.Clear();
            StringLines.Clear();
            AssembledLines.Clear();
            LabelTable.Clear();
        }


                        //Extractors
        private string GetMnem(Line x)
        {
            return x.text.Substring(0, x.text.IndexOf(' '));
        }
        private char GetInstFormat(Line x)
        {
            string mnem = GetMnem(x);
            switch (mnem)
            {
                case "add":
                    return 'r';

                case "addi":
                    return 'i';


                case "addu":
                    return 'r';

                case "addiu":
                    return 'i';

                case "sub":
                    return 'r';

                case "and":
                    return 'r';

                case "or":
                    return 'r';

                case "xor":
                    return 'r';

                case "andi":
                    return 'i';

                case "ori":
                    return 'i';

                case "xori":
                    return 'i';

                case "lw":
                    return 'i';

                case "sw":

                    return 'i';

                case "sb":

                    return 'i';

                case "lb":

                    return 'i';

                case "lbu":

                    return 'i';

                case "lh":

                    return 'i';

                case "sh":

                    return 'i';

                case "beq":

                    return 'i';

                case "bne":

                    return 'i';

                case "slt":

                    return 'r';

                case "slti":

                    return 'i';

                case "j":

                    return 'j';

                case "jr":

                    return 'r';

                case "jal":

                    return 'j';


                case "sll":

                    return 'r';

                case "srl":

                    return 'r';

                case "sra":

                    return 'r';

                case "sllv":

                    return 'r';

                case "srlv":

                    return 'r';

                case "srav":

                    return 'r';

                case "syscall":

                    return 'r';

                default:
                    //Throw Ex

                    return 'x'; //X here denotes woopsy

            }

        }
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

                        //Machine Code Generators
        private string GenerateMachineCode_Iformat(string iFormatLine)
        {
            //Format : opcode 6 - rs 5 - rt 5 - imm 16;
            string x = "";
            int opcode;
            bool isLoadStore = false;
            bool isbranch = false;


            switch (iFormatLine.Substring(0, iFormatLine.IndexOf(' ')))
            {

                case "addi":
                    opcode = 8;
                    break;
                case "addiu":
                    opcode = 9;
                    break;
                case "andi":
                    opcode = 12;
                    break;
                case "ori":
                    opcode = 13;
                    break;
                case "xori":
                    opcode = 14;
                    break;
                case "lw":
                    //Has Label or sometin
                    isLoadStore = true;
                    opcode = 35;
                    break;
                case "sw":
                    isLoadStore = true;
                    opcode = 43;
                    break;

                case "sb":
                    isLoadStore = true;
                    opcode = 40;
                    break;

                case "lb":
                    isLoadStore = true;
                    opcode = 32;
                    break;

                case "lbu":
                    isLoadStore = true;
                    opcode = 34;
                    break;

                case "lh":
                    isLoadStore = true;
                    opcode = 33;
                    break;

                case "sh":
                    isLoadStore = true;
                    opcode = 41;
                    break;

                case "beq":
                    isbranch = true;
                    opcode = 4;
                    break;

                case "bne":
                    isbranch = true;
                    opcode = 5;
                    break;

                case "slti":
                    isbranch = true;
                    opcode = 10;
                    break;
                default:
                    //Throw Ex

                    opcode = -1;
                    break; //X here denotes woopsy

            }
            //Add the opcode part to the string
            //Extends opcode to cover a six bit bitstring.
            string opcodeBitString = Convert.ToString(opcode, 2);
            extendbin(ref opcodeBitString, 6);
            x += opcodeBitString;

            //Take care of nonLabel requiring I format codes
            if (isbranch == false && isLoadStore == false)
            {
                //Then get the registers
                //Ex of I code: addi $t1, $t2, 100;
                //A register is either t1, or its number... its located between either after $ and ends after either a whitespace or a comma
                int index_1dollar = iFormatLine.IndexOf('$');
                int index_1stComma = iFormatLine.IndexOf(',', index_1dollar); //so it gets the first ws after the first hash not after the mnem
                int index_2dollar = iFormatLine.IndexOf('$', index_1stComma); //the index of the 2nd $ after the first , after the first $ .. :P
                int index_2Comma = iFormatLine.IndexOf(',', index_2dollar);
                //Now we have where $1 is $2 is and ,1 is and ,2 is... 
                //The registers are between $1 ,1 and $2 ,2 .. remove white space
                string reg1 = iFormatLine.Substring(index_1dollar + 1, index_1stComma - index_1dollar - 1); //+1 so it doesnt capture $, and the sub .. gets the length
                string reg2 = iFormatLine.Substring(index_2dollar + 1, index_2Comma - index_2dollar - 1); //and the minus one clears the , from the text... tested.
                //Now since we got the Registers correctly, we might have white space in them. Clear WhiteSPace
                clearEdgeWS(ref reg1);  //Clears whitespace from register
                clearEdgeWS(ref reg2);  //Clears whitespace from register

                //Now we are sure the registers contain no whitespace; 
                //Give them to the GetRegMC method to get their machine codes;

                string reg1MC = getRegMC(reg1); //so now we got x = ######regnum;
                string reg2MC = getRegMC(reg2); //so now we got x = ###########regnum;

                //Now extend the zeros
                extendbin(ref reg1MC, 5);
                extendbin(ref reg2MC, 5);

                x += reg2MC; //Now add them to the full MC in reverse order
                x += reg1MC; //rs -> rt, and we have written rt then rs.. so fix dis shit!

                //Now get the immediate;
                //Generally the immediate is located after the first second comma and before the next whitespace
                //ie t2, 33
                string imm = iFormatLine.Substring(index_2Comma + 1);
                //Now we got imm = watever is after the 2nd comma
                //Could have an error
                //or              imm = " 12 blabla" //WRONG 
                //Now check everything else, is it whitespace or digit? if else throw expection
                for (int i = 0; i < imm.Length; i++)
                {
                    if (char.IsDigit(imm.ElementAt<char>(i)) || imm.ElementAt<char>(i) == ' ')
                    {

                    }
                    else
                    {
                        //Throw exception, imm contains wierd characters
                        GeneralError.Invoke("iFormat_MC::Assembler", "WHY!! Fix your god damn immediate values!", "We have this imm: " + imm);
                        break;
                    }

                }
                //It should have passed the check now, it contains only digits and whitespaces by now
                clearEdgeWS(ref imm); //Clean whitespaces from it
                string bitstring = getImmediateMC(imm);
                extendbin(ref bitstring, 16);
                x += bitstring; //Voila! X is done now;
            }
            else //They are either loadstore instr, or branch instr... 
            {
                if (isLoadStore)
                {
                    //By now we have the opcode, and since its a load
                    //We need to check if it has a label or a 2 regs and an offset.

                }
                if (isbranch)
                {

                }

            }
            if (x.Length != 32)
            {
                GeneralError.Invoke("GenerateMachineCode_iFormat::Assembler", "The resultant machinecode for the instruction was not 32 bit long!", "Generated Machine Code: " + x);
                return "ERR";
            }
            else
            {
                return x;
            }

        }
        private void ExecuteDirective(Line x)
        {
            //Lol Fill me in

        }
        private void EncodeInstruction(Line x)
        {


        }
        private int GetOPCODE(Line x)
        {
            string y = x.text;
            char format = GetInstFormat(x);
            if (format == 'r')
            {
                return 0;
            }
            else if (format == 'i')
            {
                int i = x.text.IndexOf(' ') - 1;
                switch (x.text.Substring(0,i))
                {

                    case "addi":
                        return 8;

                    case "addiu":
                        return 9;

                    case "andi":
                        return 12;

                    case "ori":
                        return 13;

                    case "xori":
                        return 14;

                    case "lw":
                        return 35;

                    case "sw":

                        return 43;

                    case "sb":

                        return 40;

                    case "lb":

                        return 32;

                    case "lbu":

                        return 34;

                    case "lh":

                        return 33;

                    case "sh":

                        return 41;

                    case "beq":

                        return 4;

                    case "bne":

                        return 5;

                    case "slti":

                        return 10;
                    default:
                        //Throw Ex

                        return -1; //X here denotes woopsy

                }

            }
            else if (format == 'j')
            {
                return 0;
            }
            else
            {
                return -1;
            }


        }
        private bool containsLabel(Line x)
        {
            return x.text.Contains(':');
        }
        
        private string getRegMC(string reg)
        {
            //First Assume Reg is the name of the register
            //ie t3
            for (int i = 0; i < 31; i++)
            {
                //Check if the names are equal
                if (reg == Register.Registers.ElementAt<Register>(i).GetName())
                {
                    //If the nnames are equal, enclude the number in x; which should containt the opcode by now;
                    return Register.Registers.ElementAt<Register>(i).GetBinNum();
                    
                }
            }
            for (int i = 0; i <31; i++) {
                try
                {
                    int regn = Convert.ToInt32(reg);
                    if (regn > 0 && regn < 31)
                    {
                        string regnBin = Convert.ToString(regn, 2);
                        return regnBin;

                    }
                    else
                    {
                        //Throw error 
                        GeneralError.Invoke("getRegisterMachineCode::Assembler", "Register Number is incorrect", "Reg: " + reg);
                        break;
                    }
                }
                catch (FormatException)
                {
                    GeneralError.Invoke("getRegisterMachineCode::Assembler", "Failed to Convert reg to int32", "Reg1: " + reg);
                    break;
                }
            }
            return "ERR";

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
                GeneralError.Invoke("GetImmediateMC::Assembler", "Failed to convert Imm --> Bitstring", "Given Imm: " + x);
                return "ERR";
            }
        }
        
        

        //Responders to Events
        private void OnUnknownLineFormatError(string line, int index)
        {
            MessageBox.Show("While Parsing, an error was found in line: " + index.ToString() + "\nLine Contents: " + line);
            //Now Clear all the parsed lines and the regular ones too
            clearALL();
        }
        private void onParseComplete()
        {


        }
        private void onAssembleComplete()
        {
            //STORE the MC to memory
            for (int i = 0; i < AssembledLines.Count; i++)
            {
                Mem.StoreInstruction(AssembledLines.ElementAt<string>(i));
            }
        }
        private void onAssemblerBooBoo(Line x)
        {
            MessageBox.Show("Dear user,\n The Parser (or me) Made a BooBoo..." + 
            "\nI, the Encoder, Caught a Bug, Namely in this line, which i think shouldn't belong in the ParsedLines List.. "+
            "\nLine: " + x.index + "\nContent: " +x.text + "\nBecause it's of Type: " + x.getTypeinString() + "\nSo, to mess with the Parser, I cleared everything he parsed... lol","WoOps :(", MessageBoxButtons.OK);
            clearALL();
        }
        private void onDataSectionMissing()
        {
            MessageBox.Show("Woops, looks like you forgot to type in your data section :)", "Woopsies");
        }
        private void onGeneralError(string Sender, string msg, string error)
        {
            MessageBox.Show("Woops, looks like there is something wrong. \n" +
                "Sender: " + Sender + "\nSender's Message: " + msg + "Error: " +
                error, "Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            clearALL();
        }

        //Junk shit

        //Assembler Constructor
        public Assembler()
        {
            //Connect the Speciliazed Event Handler with the event.
            UnknownLineFormatError += new AssemblerErrorHandler(OnUnknownLineFormatError);
            AssemblerMadeABooBoo += new Boo(onAssemblerBooBoo);
            GeneralError += new AssemblerErrorHandler2(onGeneralError);
            AssembleComplete += new AssemblerHandler1(onAssembleComplete);

        }
    }
    class Line
    {
        /*
         *              Line Class:
         *              
         *                  This class merely contains the info of the class;
         *                  
         */


        //Properties
        public string text; //Text of the line, full
        private int type;  //Type. 0 for instruction || 1 for directive || 2 for comment || 3 for undetermined || -1 for unknown || Assumed Code 4* reserved for assembler
        public int index; //zero based index

        //Methods
        public void SetTypeAsComment() {
            this.type = 2;
        }
        public void SetTypeAsInstruction()
        {
            this.type = 0;
        }
        public void SetTypeAsDirective()
        {
            this.type = 1;
        }
        public void SetTypeAsOther()
        {
            this.type = 3;
        }

        public bool isComment()
        {
            if (this.type == 2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool isDirective()
        {
            if (this.type == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool isInstruction()
        {
            if (this.type == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool isUndetermined()
        {
            if (this.type == 3)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool isUnknown()
        {
            if (this.type == -1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool definesLabel()
        {
            return text.Contains(':');
        }
        public bool containsComment()
        {
            //The difference between this and IS comment is the diff
            //between : #********* and addiu blabla #comment.
            return (text.Contains('#'));
        }
        public void removeinlineComment()
        {
            if (containsComment())
            {
                int x; //start of comment;
                x = this.text.IndexOf('#');
                this.text = this.text.Substring(0, x);

            }
        }

        public string getTypeinString()
        {
            switch (type)
            {
                case 0:
                    return "INST";
                case 1:
                    return "DIR"; 
                case 2:
                    return "COMT";
                case 3:
                    return "UND";
                case -1:
                    return "ERR";
                default:
                    return "LOL";
            }
        }
        public int getTypeInt()
        {
            return type;
        }

        //Constructor
        public Line(string content, int ind)
        {
            this.text = content;
            this.type = 3;
            this.index = ind;
        }
        public Line(string cont, int type, int index)
        {
            this.text = cont;
            this.type = type;
            this.index = index;

        }
        public Line(Line x)
        {
            this.text = x.text;
            this.type = x.type;
            this.index = x.index;
        }
    }
    class Label
    {
        //Holds the information for Labels

        //Properties
        string name;  //The Name of the Label
        int index; //Which Line does it occure in
        string address; //Where does it point to
        bool ptInstruction;
        bool ptData; //so address can be resolved by the directive resolver

        //Methods
        public void setinData()
        {
            ptData = true;
            ptInstruction = false;
        }
        public void setinText()
        {
            ptData = false;
            ptInstruction = true;
        }
        public bool isinData()
        {
            return ptData;
        }
        public bool isinText()
        {
            return ptInstruction;
        }
        public int getIndex()
        {
            return index;

        }
        public string getName()
        {
            return name;
        }
        public string getAddress()
        {
            return address;
        }

        //Constructor
        public Label(string name, string address, int index)
        {
            this.name = name;
            this.index = index;
            this.address = address;
        }
        public Label(string name, int index)
        {
            this.name = name;
            this.index = index;

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

        //========================
        //Delegates and Events
        //========================

        public delegate void DissassemblerErrorHandler(int line);
        public delegate void DissassemblerHandler();
        public delegate void DisassemblerErrorHandler2(string sender, string err, string message);
        public delegate void DisassemblyCompleteHandler();
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
                for (int i = 0; i < Mem.instructionCounter; i++)
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
        private int getFormat(string line)
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
        private string decodeI(string iLine)
        {
            //I format can hold rs, rt and immediate
            //In that order OPCODE(6) RS(5) RT(5) IMM(16)
            string opcode = iLine.Substring(0, 6);
            string rs = iLine.Substring(6, 5);
            string rt = iLine.Substring(11, 5);
            string imm = iLine.Substring(16); 

            
            //rs rt and imm, then decode opcode so you know where to place em;
            //Declare the elements of the big string to retrun

            int imm_int = Convert.ToInt32(imm, 2);
            string rs_name = getRegName(rs); //so if the namer didn't get them just throw the word err
            string rt_name = getRegName(rt);
            string mnemonic;
            string mipsline;

            //Ok We got Register names, and IM now we need to decode OPCode to see where they fit in the STRING :)
            //Start decoding OPcode and return :) we're done
            switch (opcode)
            {
                case "001000": //Addi
                    mnemonic = "addi";
                    mipsline = mnemonic + " $" + rt_name + ", $" + rs_name + " " + imm_int;
                    return mipsline;
                case "001001": //Addiu
                     mnemonic = "addiu";
                    mipsline = mnemonic + " $" + rt_name + ", $" + rs_name + " " + imm_int;
                    return mipsline;
                case "001100": //Andi
                    mnemonic = "andi";
                    mipsline = mnemonic + " $" + rt_name + ", $" + rs_name + " " + imm_int;
                    return mipsline;
                case "000100": //beq
                    mnemonic = "beq";
                    mipsline = mnemonic + " $" + rt_name + ", $" + rs_name + " " + imm_int;
                    return mipsline;
                case "000101": //bne
                    mnemonic = "bne";
                    mipsline = mnemonic + " $" + rt_name + ", $" + rs_name + " " + imm_int;
                    return mipsline;
                case "100000": //lb
                    mnemonic = "lb";
                    mipsline = mnemonic + " $" + rt_name + ", " + imm + "($" + rs_name + ")";
                    return mipsline;
                case "100100": //lbu
                    mnemonic = "lbu";
                    mipsline = mnemonic + " $" + rt_name + ", " + imm + "($" + rs_name + ")";
                    return mipsline;
                case "100001": //lh
                    mnemonic = "lh";
                    mipsline = mnemonic + " $" + rt_name + ", " + imm + "($" + rs_name + ")";
                    return mipsline;
                case "100101": //lhu
                    mnemonic = "lhu";
                    mipsline = mnemonic + " $" + rt_name + ", " + imm + "($" + rs_name + ")";
                    return mipsline;
                case "100011": //lw
                    mnemonic = "lw";
                    mipsline = mnemonic + " $" + rt_name + ", " + imm + "($" + rs_name + ")";
                    return mipsline;
                case "001101": //ori
                    mnemonic = "ori";
                    mipsline = mnemonic + " $" + rt_name + ", $" + rs_name + " " + imm_int;
                    return mipsline;
                case "101000": //sb
                    mnemonic = "sb";
                    mipsline = mnemonic + " $" + rt_name + ", " + imm + "($" + rs_name + ")";
                    return mipsline;
                case "001010": //slti
                    mnemonic = "slti";
                    mipsline = mnemonic + " $" + rt_name + ", $" + rs_name + " " + imm_int;
                    return mipsline;
                case "001011": //sltiu
                    mnemonic = "sltiu";
                    mipsline = mnemonic + " $" + rt_name + ", $" + rs_name + " " + imm_int;
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
                    mipsline = mnemonic + " $" + rt_name + ", $" + rs_name + " " + imm_int;
                    return mipsline;
                default:
                    //unsupported, throw error
                    GeneralError.Invoke("iFormatDecoder::Disassembler", "OPCODE: " + opcode, "Hi there, the disassembler encountered an unsupported i-format instruction");
                    return "ERR";
            }
        }
        private string decodeR(string rLine)
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
        private string decodeJ(string jLine)
        {
            //Contains opcode(6) + target(26);
            string address = jLine.Substring(6);
            string opcode = jLine.Substring(0, 6);
            string mipsline;
            string mnem;
            //First get the address
            int add_int = Convert.ToInt32(address, 2);

            //Now get the opcode and return the mipsline
            switch (opcode)
            {
                case "000010":
                    mnem = "j";
                    mipsline = mnem + " " + address;
                    return mipsline;
                case "000011":
                    mnem = "jal";
                    mipsline = mnem + " " + address;
                    return mipsline;
                default:
                    //it should NEVER EVER reach here... i believe
                    return "ERR";
            }

        }

                //MC Helpers
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
        public void Disassemble()
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
                }
                DisassemblyComplete.Invoke(); //yay!
            }
            else
            {
                GeneralError.Invoke("Disassemble::Disassembler", "We ain't got no machine codes", "Lol, u messin with me? I didn't find any machine codes to disassemble!");
            }
            
        }
    }
}
