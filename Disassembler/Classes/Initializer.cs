using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MADS.Classes
{
    class Initializer
    {
       public int initRegisters()
        {
            Register zero = new Register(0, "zero");
            Register one = new Register(1, "at");
            Register two = new Register(2, "v0");
            Register three = new Register(3, "v1");
            Register four = new Register(4, "a0");
            Register five = new Register(5, "a1");
            Register six = new Register(6, "a2");
            Register seven = new Register(7, "a3");
            Register eight = new Register(8, "t0");
            Register nine = new Register(9, "t1");
            Register ten = new Register(10, "t2");
            Register eleven = new Register(11, "t3");
            Register twelve = new Register(12, "t4");
            Register thirteen = new Register(13, "t5");
            Register fourteen = new Register(14, "t6");
            Register fifteen = new Register(15, "t7");
            Register sixteen = new Register(16, "s0");
            Register seventeen = new Register(17, "s1");
            Register eighteen = new Register(18, "s2");
            Register nineteen = new Register(19, "s3");
            Register twenty = new Register(20, "s4");
            Register twentyone = new Register(21, "s5");
            Register twentytwo = new Register(22, "s6");
            Register twentythree = new Register(23, "s7");
            Register twentyfour = new Register(24, "t8");
            Register twentyfive = new Register(25, "t9");
            Register twentysix = new Register(26, "k0");
            Register twentyseven = new Register(27, "k1");
            Register twentyeight = new Register(28, "gp");
            Register twentynine = new Register(29, "sp");
            Register thirty = new Register(30, "fp");
            Register thirtyone = new Register(31, "ra");

            //OutPut Message
            
            return Register.initbit;
        }
       public void initKnownInstructions()
       {
           /*
            * For Debugging Purposes:
            * Tested R Instructions:
            *           -Simulator: {}
            *           -Assembler: {}
            *           -Disassebler: {}
            *           
            * Tested I Instructions:
            *           -Simulator: {addi bne}
            *           -Assembler: {}
            *           -Disassebler: {}
            *           
            * Tested J Instructions:
            *           -Simulator: {j}
            *           -Assembler: {}
            *           -Disassebler: {}
            */
           //R format

           //Consider Rewriting Constructor as Add = new instruction("add","100000",0,"dts") //Where dts 
           //Holds three values, operand size, and order,
           //Example: Bne = new Instruction("bne","opcode",1,"stl");
           //Where these characters stand for:
           //d = rd - 5 bits
           //s = rs - 5 bits
           //t = rt - 5 bits
           //l = label (converted to address) im - 16 bit or 26 bit depends if I or J
           //a = shiftAmount - 5 bits
           //i = immediate - 16 bits
           Instruction Add = new Instruction("add", "100000", 0, "dst", "Adds the signed value of rs and rt and stores them in rd");
           Instruction Addu = new Instruction("addu", "100001", 0, "dst", "Adds the unsigned value of rs and rt and stores them in rd");
           Instruction And = new Instruction("and", "100100", 0, "dst", "Ands the values of rs rt bitwisely and stores in it rd");
           Instruction Jr = new Instruction("jr", "001000", 0, "s", "Jumps to the Address holded in register rs");
           Instruction Nor = new Instruction("nor", "100111", 0, "dst", "Nors the values of rs rt bitwisely and stores in it rd");
           Instruction Or = new Instruction("or", "100101", 0, "dst", "Ors the values of rs rt bitwisely and stores in it rd");
           Instruction Sub = new Instruction("sub", "100010", 0, "dst", "DESC");
           Instruction Subu = new Instruction("subu", "100011", 0, "dst", "Desc");
           Instruction Sra = new Instruction("sra", "000011",0, "dta", "Desc");
           Instruction Srlv = new Instruction("srlv", "000110",0, "dts", "Shifts right rt by rs amount and stores in rd");
           Instruction Sll = new Instruction("sll", "000000", 0, "dta", "ADD DESC");  //rd rt sa
           Instruction Srl = new Instruction("srl", "000010", 0, "dta", "ADD DESC");  //rd rt sa
           Instruction Sllv = new Instruction("sllv", "000100", 0, "dts", "ADD DESC"); //rd rt rs 
           Instruction Slt = new Instruction("slt", "101010", 0, "dst", "ADD DESC"); //rd rs rt
           Instruction Sltu = new Instruction("sltu", "101011", 0, "dst", "ADD DESC");//rd rs rt
           Instruction Syscall = new Instruction("syscall", "001100", 0, "", "System Call");
           Instruction Xor = new Instruction("xor", "100110", 0, "dst", "DESC");

           //I format
           Instruction Addi = new Instruction("addi", "001000", 1, "tsi", "ADD DESC");
           Instruction Addiu = new Instruction("addiu", "001001", 1, "tsi", "ADD DESC");
           Instruction Andi = new Instruction("andi", "001100", 1, "tsi", "ADD DESC");
           Instruction Bne = new Instruction("bne", "000101", 1, "stl", "ADD DESC");
           Instruction Lb = new Instruction("lb", "100000", 1, "tis", "ADD DESC"); //TIS stands for 
           Instruction Sb = new Instruction("sb", "101000", 1, "tis", "ADD DESC"); //TIS stands for rt then immediate then RS, the assembler should also know                                                                        //That this can also have another version, tl
           Instruction Sh = new Instruction("sh", "101001", 1, "tis", "Add_Desc");
           Instruction Lh = new Instruction("lh", "100001", 1, "tis", "Add_Desc");
           Instruction Lbu = new Instruction("lbu", "100100", 1, "tis", "Add_Desc");
           Instruction Lhu = new Instruction("lhu", "100101", 1, "tis", "Add_Desc");
           Instruction Lui = new Instruction("lui", "001111", 1, "ti", "Desc");
           Instruction Lw = new Instruction("lw", "100011", 1, "tis", "Desc");
           Instruction Ori = new Instruction("ori", "001101", 1, "tsi", "Desc");
           Instruction Slti = new Instruction("slti", "001010", 1, "tsi", "Desc");
           Instruction Sltiu = new Instruction("sltiu", "001011", 1, "tsi", "Desc");
           Instruction Sw = new Instruction("sw", "101011", 1, "tis", "Desc");
           Instruction Xori = new Instruction("xori", "001110", 1, "tsi", "Desc");


           //J Format
           Instruction J = new Instruction("j", "000010", 2, "l", "ADD DESC");
           Instruction Jal = new Instruction("jal", "000011", 2, "l", "ADD DESC");

           //STILL MORE INSTRUCTIONS TO CODE
          
           //PSEUDOINSTRUCTIONS

       }
    }
}
