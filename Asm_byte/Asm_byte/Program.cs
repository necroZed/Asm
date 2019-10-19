using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asm_byte
{
    class Program
    {
        static void Main(string[] args)
        {
            byte[] final = new byte[];
            byte[] two = new byte[2];
            string temp;
            string[] lines = File.ReadAllLines(args[0], System.Text.Encoding.Default);
            using (FileStream fstream = new FileStream(@"a.out", FileMode.OpenOrCreate))
            {
                foreach (string line in lines)
                {
                    Console.WriteLine(line);
                    if (line != "HLT")
                    {
                        string command = line.Substring(0, line.IndexOf(" "));
                        switch (command)
                        {
                            case "ADD":
                                {
                                    fstream.WriteByte(0);
                                    temp = line.Replace("ADD ", "");
                                    temp = temp.Replace("R", "");
                                    fstream.WriteByte(Get_byte(temp.Remove(1)));
                                    temp = temp.Remove(0, 2);
                                    fstream.WriteByte(Get_byte(temp));
                                    break;
                                }
                            case "SUB":
                                {
                                    fstream.WriteByte(1);
                                    temp = line.Replace("SUB ", "");
                                    temp = temp.Replace("R", "");
                                    fstream.WriteByte(Get_byte(temp.Remove(1)));
                                    temp = temp.Remove(0, 2);
                                    fstream.WriteByte(Get_byte(temp));
                                    break;
                                }
                            case "MUL":
                                {
                                    fstream.WriteByte(2);
                                    temp = line.Replace("MUL ", "");
                                    temp = temp.Replace("R", "");
                                    fstream.WriteByte(Get_byte(temp.Remove(1)));
                                    temp = temp.Remove(0, 2);
                                    fstream.WriteByte(Get_byte(temp));
                                    break;
                                }
                            case "DIV":
                                {
                                    fstream.WriteByte(3);
                                    temp = line.Replace("DIV ", "");
                                    temp = temp.Replace("R", "");
                                    fstream.WriteByte(Get_byte(temp.Remove(1)));
                                    temp = temp.Remove(0, 2);
                                    fstream.WriteByte(Get_byte(temp));
                                    break;
                                }
                            case "MOVC":
                                {
                                    fstream.WriteByte(4);
                                    temp = line.Replace("MOVC ", "");
                                    temp = temp.Replace("R", "");
                                    fstream.WriteByte(Get_byte(temp.Remove(1)));
                                    temp = temp.Remove(0, 2);
                                    two = Get_two(temp);
                                    fstream.WriteByte(two[0]);
                                    fstream.WriteByte(two[1]);
                                    break;
                                }
                            case "MOV":
                                {
                                    fstream.WriteByte(5);
                                    temp = line.Replace("MOV ", "");
                                    temp = temp.Replace("R", "");
                                    fstream.WriteByte(Get_byte(temp.Remove(1)));
                                    temp = temp.Remove(0, 2);
                                    fstream.WriteByte(Get_byte(temp));
                                    break;
                                }
                            case "LOAD":
                                {
                                    fstream.WriteByte(6);
                                    temp = line.Replace("LOAD ", "");
                                    temp = temp.Replace("R", "");
                                    fstream.WriteByte(Get_byte(temp.Remove(1)));
                                    temp = temp.Remove(0, 2);
                                    fstream.WriteByte(Get_byte(temp));
                                    break;
                                }
                            case "STOR":
                                {
                                    fstream.WriteByte(7);
                                    temp = line.Replace("STOR ", "");
                                    temp = temp.Replace("R", "");
                                    fstream.WriteByte(Get_byte(temp.Remove(1)));
                                    temp = temp.Remove(0, 2);
                                    fstream.WriteByte(Get_byte(temp));
                                    break;
                                }
                            case "CMP":
                                {
                                    fstream.WriteByte(8);
                                    temp = line.Replace("CMP ", "");
                                    temp = temp.Replace("R", "");
                                    fstream.WriteByte(Get_byte(temp.Remove(1)));
                                    temp = temp.Remove(0, 2);
                                    fstream.WriteByte(Get_byte(temp));
                                    break;
                                }
                            case "JMP":
                                {
                                    fstream.WriteByte(9);
                                    temp = line.Replace("JMP ", "");
                                    two = Get_two(temp);
                                    fstream.WriteByte(two[0]);
                                    fstream.WriteByte(two[1]);
                                    break;
                                }
                            case "JMPC":
                                {
                                    fstream.WriteByte(10);
                                    temp = line.Replace("JMPC ", "");
                                    two = Get_two(temp);
                                    fstream.WriteByte(two[0]);
                                    fstream.WriteByte(two[1]);
                                    break;
                                }
                            case "PUSH":
                                {
                                    fstream.WriteByte(11);
                                    temp = line.Replace("PUSH R", "");
                                    fstream.WriteByte(Get_byte(temp));
                                    break;
                                }
                            case "POP":
                                {
                                    fstream.WriteByte(12);
                                    temp = line.Replace("POP R", "");
                                    fstream.WriteByte(Get_byte(temp));
                                    break;
                                }
                        }
                    }
                    else
                    {
                        fstream.WriteByte(13);
                    }
                }
            }
            Console.WriteLine("Строки записаны в файл в виде байт-кода");
            Console.ReadLine();
        }
        public static byte Get_byte(string stroka)
        {
            byte vozvr;
            int vhod = Int32.Parse(stroka) - 1;
            vozvr = Convert.ToByte(vhod);
            return vozvr;
        }
        public static byte[] Get_two(string stroka)
        {
            byte [] vozvr = new byte [2];
            int vhod = Int32.Parse(stroka);
            int first = vhod / 256;
            int second = vhod % 256;
            vozvr[1] = Convert.ToByte(first);
            vozvr[0] = Convert.ToByte(second);
            return vozvr;
        }
    }
}
