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
            List<byte> adata = new List<byte>();
            byte[] two = new byte[2];
            Dictionary<string, int> marks = new Dictionary<string, int>();
            int counter = 0;
            string temp;
            string[] lines = File.ReadAllLines(args[0], System.Text.Encoding.Default);
            foreach (string line in lines)
            {
                Console.WriteLine(line);
                if (line.Contains(":") == false)
                {
                    string command;
                    try
                    {
                         command = line.Substring(0, line.IndexOf(" "));
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
                                temp = line.Replace("ADD ", "");
                                temp = temp.Replace("R", "");
                                adata.Add(Get_byte(temp.Remove(1)));
                                temp = temp.Remove(0, 2);
                                adata.Add(Get_byte(temp));
                                counter = counter + 3;
                                break;
                            }
                        case "SUB":
                            {
                                adata.Add(1);
                                temp = line.Replace("SUB ", "");
                                temp = temp.Replace("R", "");
                                adata.Add(Get_byte(temp.Remove(1)));
                                temp = temp.Remove(0, 2);
                                adata.Add(Get_byte(temp));
                                counter = counter + 3;
                                break;
                            }
                        case "MUL":
                            {
                                adata.Add(2);
                                temp = line.Replace("MUL ", "");
                                temp = temp.Replace("R", "");
                                adata.Add(Get_byte(temp.Remove(1)));
                                temp = temp.Remove(0, 2);
                                adata.Add(Get_byte(temp));
                                counter = counter + 3;
                                break;
                            }
                        case "DIV":
                            {
                                adata.Add(3);
                                temp = line.Replace("DIV ", "");
                                temp = temp.Replace("R", "");
                                adata.Add(Get_byte(temp.Remove(1)));
                                temp = temp.Remove(0, 2);
                                adata.Add(Get_byte(temp));
                                counter = counter + 3;
                                break;
                            }
                        case "MOVC":
                            {
                                adata.Add(4);
                                temp = line.Replace("MOVC ", "");
                                temp = temp.Replace("R", "");
                                adata.Add(Get_byte(temp.Remove(1)));
                                temp = temp.Remove(0, 2);
                                two = Get_two(temp);
                                adata.Add(two[0]);
                                adata.Add(two[1]);
                                counter = counter + 4;
                                break;
                            }
                        case "MOV":
                            {
                                adata.Add(5);
                                temp = line.Replace("MOV ", "");
                                temp = temp.Replace("R", "");
                                adata.Add(Get_byte(temp.Remove(1)));
                                temp = temp.Remove(0, 2);
                                adata.Add(Get_byte(temp));
                                counter = counter + 3;
                                break;
                            }
                        case "LOAD":
                            {
                                adata.Add(6);
                                temp = line.Replace("LOAD ", "");
                                temp = temp.Replace("R", "");
                                adata.Add(Get_byte(temp.Remove(1)));
                                temp = temp.Remove(0, 2);
                                adata.Add(Get_byte(temp));
                                counter = counter + 3;
                                break;
                            }
                        case "STOR":
                            {
                                adata.Add(7);
                                temp = line.Replace("STOR ", "");
                                temp = temp.Replace("R", "");
                                adata.Add(Get_byte(temp.Remove(1)));
                                temp = temp.Remove(0, 2);
                                adata.Add(Get_byte(temp));
                                counter = counter + 3;
                                break;
                            }
                        case "CMP":
                            {
                                adata.Add(8);
                                temp = line.Replace("CMP ", "");
                                temp = temp.Replace("R", "");
                                adata.Add(Get_byte(temp.Remove(1)));
                                temp = temp.Remove(0, 2);
                                adata.Add(Get_byte(temp));
                                counter = counter + 3;
                                break;
                            }
                        case "JMP":
                            {
                                adata.Add(9);
                                counter++;
                                temp = line.Replace("JMP ", "");
                                if (marks.ContainsKey(temp))
                                {
                                    adata.Add(1);
                                    adata.Add(Convert.ToByte(counter - marks[temp] - 1));
                                    marks.Remove(temp);
                                }
                                else
                                {
                                    marks.Add(temp, counter+1);
                                    adata.Add(0);
                                    adata.Add(0);
                                }
                                counter = counter + 2;
                                break;
                            }
                        case "JMPC":
                            {
                                adata.Add(10);
                                temp = line.Replace("JMPC ", "");
                                temp = temp.Replace("R", "");
                                adata.Add(Get_byte(temp.Remove(1)));
                                temp = temp.Remove(0, 2);
                                counter = counter +2;
                                if (marks.ContainsKey(temp))
                                {
                                    adata.Add(1);
                                    adata.Add(Convert.ToByte(counter - marks[temp] - 1));
                                    marks.Remove(temp);
                                }
                                else
                                {
                                    marks.Add(temp, counter+1);
                                    adata.Add(0);
                                    adata.Add(0);
                                }
                                counter = counter + 2;
                                break;
                            }
                        case "PUSH":
                            {
                                adata.Add(11);
                                temp = line.Replace("PUSH R", "");
                                adata.Add(Get_byte(temp));
                                counter = counter + 2;
                                break;
                            }
                        case "POP":
                            {
                                adata.Add(12);
                                temp = line.Replace("POP R", "");
                                adata.Add(Get_byte(temp));
                                counter = counter + 2;
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
                    if (marks.ContainsKey(line.Replace(":", null)))
                    {
                        adata[marks[line.Replace(":", null)]] = Convert.ToByte(counter - marks[line.Replace(":", null)] - 1);
                        //Console.WriteLine("Эта метка идет после команды, запишем её");
                        marks.Remove(line.Replace(":", null));
                    }
                    else
                    {
                        marks.Add(line.Replace(":", null),counter);
                        //Console.WriteLine($"Ага, тут у нас местка {line.Replace(":", null)}");
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
                using (FileStream fstream = new FileStream(@"a.out", FileMode.OpenOrCreate))
                {
                    fstream.Write(final, 0, final.Length);
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
