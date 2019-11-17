using System;
using System.IO;
using System.Collections;
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
            const ushort data_adr = 0x10;
            const ushort code_adr = 0x8000;
            List<byte> adata = new List<byte>();
            byte[] two = new byte[2];
            Dictionary<string, int> marks = new Dictionary<string, int>();
            Dictionary<string, int> data = new Dictionary<string, int>();
            ushort code_size = 0;
            ushort data_size = 0;
            string temp;
            string[] lines = File.ReadAllLines(args[0], System.Text.Encoding.Default);
            int inach = 0;
            if (lines[inach] == ".data")
            {
                inach++;
                while (lines[inach] != ".code")
                {
                    Console.WriteLine(lines[inach]);
                    string command;
                    command = lines[inach].Substring(lines[inach].IndexOf(" ") +1 , 2);
                    switch (command)
                    {
                        case "db":
                            {
                                data.Add(lines[inach].Substring(0, lines[inach].IndexOf(" ")), data_adr + data_size + 1);
                                if (lines[inach].Contains("?"))
                                {
                                    adata.Add(0);
                                }
                                else
                                {
                                    adata.Add(Convert.ToByte(lines[inach].Substring(lines[inach].LastIndexOf(" "))));
                                }
                                data_size++;
                                break;
                            }
                        case "dw":
                            {
                                data.Add(lines[inach].Substring(0, lines[inach].IndexOf(" ")), data_adr + data_size + 1);
                                if (lines[inach].Contains("?"))
                                {
                                    adata.Add(0);
                                    adata.Add(0);
                                }
                                else
                                {
                                    two = Get_two(lines[inach].Substring(lines[inach].LastIndexOf(" ")));
                                    adata.Add(two[0]);
                                    adata.Add(two[1]);
                                }
                                data_size += 2;
                                break;
                            }
                    }
                    inach++;
                }

            }
            for (int i = inach +1; i < lines.Length; i++)
            {
                string command;
                Console.WriteLine(lines[i]);
                if (lines[i].Contains(":") == false)
                {
                    try
                    {
                        command = lines[i].Substring(0, lines[i].IndexOf(" "));
                    }
                    catch
                    {
                        command = "HLT";
                    }
                    switch (command)
                    {
                        case "ADD":
                            {
                                adata.Add(0);
                                temp = lines[i].Replace("ADD ", "");
                                temp = temp.Replace("R", "");
                                adata.Add(Get_byte(temp.Remove(1)));
                                temp = temp.Remove(0, 2);
                                adata.Add(Get_byte(temp));
                                code_size +=3;
                                break;
                            }
                        case "SUB":
                            {
                                adata.Add(1);
                                temp = lines[i].Replace("SUB ", "");
                                temp = temp.Replace("R", "");
                                adata.Add(Get_byte(temp.Remove(1)));
                                temp = temp.Remove(0, 2);
                                adata.Add(Get_byte(temp));
                                code_size +=3;
                                break;
                            }
                        case "MUL":
                            {
                                adata.Add(2);
                                temp = lines[i].Replace("MUL ", "");
                                temp = temp.Replace("R", "");
                                adata.Add(Get_byte(temp.Remove(1)));
                                temp = temp.Remove(0, 2);
                                adata.Add(Get_byte(temp));
                                code_size +=3;
                                break;
                            }
                        case "DIV":
                            {
                                adata.Add(3);
                                temp = lines[i].Replace("DIV ", "");
                                temp = temp.Replace("R", "");
                                adata.Add(Get_byte(temp.Remove(1)));
                                temp = temp.Remove(0, 2);
                                adata.Add(Get_byte(temp));
                                code_size +=3;
                                break;
                            }
                        case "MOVC":
                            {
                                adata.Add(4);
                                temp = lines[i].Replace("MOVC ", "");
                                temp = temp.Replace("R", "");
                                adata.Add(Get_byte(temp.Remove(1)));
                                temp = temp.Remove(0, 2);
                                two = Get_two(temp);
                                adata.Add(two[0]);
                                adata.Add(two[1]);
                                code_size += 4;
                                break;
                            }
                        case "MOV":
                            {
                                adata.Add(5);
                                temp = lines[i].Replace("MOV ", "");
                                temp = temp.Replace("R", "");
                                adata.Add(Get_byte(temp.Remove(1)));
                                temp = temp.Remove(0, 2);
                                adata.Add(Get_byte(temp));
                                code_size +=3;
                                break;
                            }
                        case "LOAD":
                            {
                                adata.Add(6);
                                temp = lines[i].Replace("LOAD ", "");
                                temp = temp.Replace("R", "");
                                adata.Add(Get_byte(temp.Remove(1)));
                                temp = temp.Remove(0, 2);
                                if (data.ContainsKey(temp))
                                {
                                    two = Get_two(Convert.ToString(data[temp]));
                                    adata.Add(two[0]);
                                    adata.Add(two[1]);
                                }
                                code_size +=4;
                                break;
                            }
                        case "STOR":
                            {
                                adata.Add(7);
                                temp = lines[i].Replace("STOR ", "");
                                temp = temp.Replace("R", "");
                                adata.Add(Get_byte(temp.Remove(1)));
                                temp = temp.Remove(0, 2);
                                if (data.ContainsKey(temp))
                                {
                                    two = Get_two(Convert.ToString(data[temp]));
                                    adata.Add(two[0]);
                                    adata.Add(two[1]);
                                }
                                code_size +=4;
                                break;
                            }
                        case "CMP":
                            {
                                adata.Add(8);
                                temp = lines[i].Replace("CMP ", "");
                                temp = temp.Replace("R", "");
                                adata.Add(Get_byte(temp.Remove(1)));
                                temp = temp.Remove(0, 2);
                                adata.Add(Get_byte(temp));
                                code_size +=3;
                                break;
                            }
                        case "JMP":
                            {
                                adata.Add(9);
                                temp = lines[i].Replace("JMP ", "");
                                code_size++;
                                if (marks.ContainsKey(temp))
                                {
                                    int  r = marks[temp] - code_size + 1;
                                    string temp1 = Convert.ToString(r);
                                    two = Get_two(temp1);
                                    adata.Add(two[0]);
                                    adata.Add(two[1]);
                                    marks.Remove(temp);
                                }
                                else
                                {
                                    marks.Add(temp, code_size);
                                    adata.Add(0);
                                    adata.Add(0);
                                }
                                code_size +=2;
                                break;
                            }
                        case "JMPC":
                            {
                                adata.Add(10);
                                temp = lines[i].Replace("JMPC ", "");
                                temp = temp.Replace("R", "");
                                adata.Add(Get_byte(temp.Remove(1)));
                                temp = temp.Remove(0, 2);
                                code_size +=2;
                                if (marks.ContainsKey(temp))
                                {
                                    int r = marks[temp] - code_size + 1;
                                    string temp1 = Convert.ToString(r);
                                    two = Get_two(temp1);
                                    adata.Add(two[0]);
                                    adata.Add(two[1]);
                                    marks.Remove(temp);
                                }
                                else
                                {
                                    marks.Add(temp, code_size);
                                    adata.Add(0);
                                    adata.Add(0);
                                }
                                code_size +=2;
                                break;
                            }
                        case "PUSH":
                            {
                                adata.Add(11);
                                temp = lines[i].Replace("PUSH R", "");
                                adata.Add(Get_byte(temp));
                                code_size +=2;
                                break;
                            }
                        case "POP":
                            {
                                adata.Add(12);
                                temp = lines[i].Replace("POP R", "");
                                adata.Add(Get_byte(temp));
                                code_size +=2;
                                break;
                            }
                        case "HLT":
                            {
                                adata.Add(13);
                                break;
                            }

                    }
                }
                else
                {
                    if (marks.ContainsKey(lines[i].Replace(":", null)))
                    {
                        string tt = Convert.ToString(Convert.ToByte(code_size - marks[lines[i].Replace(":", null)]));
                        two = Get_two(tt);
                        adata[marks[lines[i].Replace(":", null)]- 1] = two[0];
                        adata[marks[lines[i].Replace(":", null)]] = two[1];
                        //Console.WriteLine("Эта метка идет после команды, запишем её");
                        marks.Remove(lines[i].Replace(":", null));
                    }
                    else
                    {
                        marks.Add(lines[i].Replace(":", null), code_size);
                        //Console.WriteLine($"Ага, тут у нас местка {lines[i].Replace(":", null)}");
                    }
                }
            }
            if (marks.Count != 0)
            {
                Console.WriteLine("Найдены нереализованные метки, строки не будут записаны");
            }
            else
            {
                byte[] final = adata.ToArray<byte>();
                using (BinaryWriter fstream = new BinaryWriter(File.Open(@"a.out", FileMode.OpenOrCreate)))
                {
                    fstream.Write(Encoding.ASCII.GetBytes("OSVM"));
                    fstream.Write(data_adr);
                    fstream.Write(data_size);
                    fstream.Write(code_adr);
                    fstream.Write(code_size);
                    fstream.Write(final);
                    Console.WriteLine("Строки записаны в файл в виде байт-кода");
                }
            }
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
            byte[] vozvr = new byte[2];
            try
            {
                short vhod = Int16.Parse(stroka);
                vozvr[0] = (byte)(vhod & 0xff);
                vozvr[1] = (byte)((vhod & 0xff00) >> 8);
            }
            catch
            {
                ushort vhod = UInt16.Parse(stroka);
                vozvr[0] = (byte)(vhod & 0xff);
                vozvr[1] = (byte)((vhod & 0xff00) >> 8);
            }
            return vozvr;
        }
    }
}
