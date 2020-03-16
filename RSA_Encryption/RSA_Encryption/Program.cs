using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace RSA_Encryption
{
    class Program
    {
        static void Main(string[] args)
        {
            var p = GetPrimeNumber();
            var q = GetPrimeNumber();
            uint n = p * q;
            ulong fi = (p - 1) * (q - 1);
            ulong e = GetOpen(fi);
            ulong d = GetPrivate(e,fi);

            //Console.InputEncoding = Encoding.Unicode;

            while (true)
            {
                Console.WriteLine("Выберите действие:"+ Environment.NewLine + "1-Зашифровать методом RSA" + Environment.NewLine + "2-Расшифровать методом RSA" + Environment.NewLine + "3-Зашифровать методом Вернама" + Environment.NewLine + "4-Расшифровать методом Вернама" + Environment.NewLine + "5-Засшифровать методом Цезаря" + Environment.NewLine + "6-Расшифровать методом Цезаря" + Environment.NewLine + "7-Засшифровать методом Вижинера" + Environment.NewLine + "8-Расшифровать методом Вижинера" + Environment.NewLine + "Другой символ -Выход");
                var com = Console.ReadLine();
                if (int.TryParse(com, out int command))
                {
                    if (command == 1)
                    {
                        Console.WriteLine("Введите строку: ");
                        var list = Console.ReadLine().ToLower().ToCharArray().ToList();
                        var cryptList = list.ConvertAll(x => Crypt((uint)x, e, n));
                        string s = "";
                        cryptList.ForEach(x => s += x + "-");
                        s.Remove(s.Length - 1);
                        Console.WriteLine("Зашифрованная строка: " + s);
                    }
                    else if (command == 2)
                    {
                        Console.WriteLine("Введите строку: ");
                        var text = Console.ReadLine();
                        Regex regex1 = new Regex("\\d+-{1}");
                        Regex regex2 = new Regex("\\d+$");
                        var match = regex1.Matches(text).ToList();
                        var list = match.ConvertAll(x => Convert.ToUInt64(x.ToString().Replace("-", "")));
                        list.Add(Convert.ToUInt64(regex2.Match(text).ToString()));
                        var resultDeCrypt = list.ConvertAll(x => Decrypt(x, d, n)).ConvertAll(x => Convert.ToChar(x));
                        var s2 = "";
                        resultDeCrypt.ForEach(x => s2 += x);
                        Console.WriteLine("Расшифрованная строка: " + s2);
                    }
                    else if (command == 3)
                    {
                        Console.WriteLine("Введите Ключ: ");
                        var key = Console.ReadLine();
                        Console.WriteLine("Введите текст: ");
                        var text = Console.ReadLine();
                        List<BitArray> keyBits = new List<BitArray>();
                        List<BitArray> textBits = new List<BitArray>();
                        key.ToCharArray().ToList().ForEach(x =>
                        {
                            var a = new BitArray(new int[] { x });
                            keyBits.Add(a);
                        });
                        text.ToCharArray().ToList().ForEach(x =>
                        {
                            var a = new BitArray(new int[] { x });
                            textBits.Add(a);
                        });
                        List<BitArray> result = new List<BitArray>();
                        for (int i = 0; i < textBits.Count; i++)
                        {
                            int iKey = i % keyBits.Count;
                            result.Add(textBits[i].Xor(keyBits[iKey]));
                        }
                        var res = result.ConvertAll(x =>
                        {
                            byte[] bytes = new byte[x.Length];
                            x.CopyTo(bytes, 0);
                            return Encoding.Unicode.GetString(bytes).Replace("\0", "");
                        });
                        Console.WriteLine("Зашифрованная строка: ");
                        res.ForEach(x => Console.Write(x));
                        Console.WriteLine();
                    }
                    else if (command == 4)
                    {
                        Console.WriteLine("Введите Ключ: ");
                        var key = Console.ReadLine();
                        Console.WriteLine("Введите текст: ");
                        var text = Console.ReadLine();
                        List<BitArray> keyBits = new List<BitArray>();
                        List<BitArray> textBits = new List<BitArray>();
                        key.ToCharArray().ToList().ForEach(x =>
                        {
                            var a = new BitArray(new int[] { x });
                            keyBits.Add(a);
                        });
                        text.ToCharArray().ToList().ForEach(x =>
                        {
                            var a = new BitArray(new int[] { x });
                            textBits.Add(a);
                        });
                        List<BitArray> result = new List<BitArray>();
                        for (int i = 0; i < textBits.Count; i++)
                        {
                            int iKey = i % keyBits.Count;
                            result.Add(textBits[i].Xor(keyBits[iKey]));
                        }
                        var res = result.ConvertAll(x =>
                        {
                            byte[] bytes = new byte[x.Length];
                            x.CopyTo(bytes, 0);
                            return Encoding.Unicode.GetString(bytes).Replace("\0", "");
                        });
                        Console.WriteLine("Расшифрованная строка: ");
                        res.ForEach(x => Console.Write(x));
                        Console.WriteLine();
                    }
                    else if (command == 5)
                    {
                        Console.WriteLine("Введите Ключ: ");
                        var key = Console.ReadLine();
                        if (int.TryParse(key, out int ikey))
                        {
                            Console.WriteLine("Введите текст: ");
                            var text = Console.ReadLine();
                            var list = text.ToCharArray().ToList().ConvertAll(x => (int)x);
                            ikey = ikey % 65536;
                            var result = list.ConvertAll(x => (x + ikey) % 65536);
                            Console.Write("Зашифрованная строка: ");
                            result.ConvertAll(x => char.ConvertFromUtf32(x)).ForEach(x => Console.Write(x));
                            Console.WriteLine();
                        }
                        else
                        {
                            Console.WriteLine("Некорректный ключ");
                        }
                    }
                    else if (command == 6)
                    {
                        Console.WriteLine("Введите Ключ: ");
                        var key = Console.ReadLine();
                        if (int.TryParse(key, out int ikey))
                        {
                            Console.WriteLine("Введите текст: ");
                            var text = Console.ReadLine();
                            var list = text.ToCharArray().ToList().ConvertAll(x => (int)x);
                            ikey = ikey % 65536;
                            var result = list.ConvertAll(x => (x + 65536 - ikey) % 65536);
                            Console.Write("Расшифрованная строка: ");
                            result.ConvertAll(x => char.ConvertFromUtf32(x)).ForEach(x => Console.Write(x));
                            Console.WriteLine();
                        }
                        else
                        {
                            Console.WriteLine("Некорректный ключ");
                        }
                    }
                    else if (command == 7)
                    {
                        Console.WriteLine("Введите Ключ в следующем формате '1,2,3,4,5' ");
                        var key = Console.ReadLine();
                        Regex regex1 = new Regex("\\d+,{1}");
                        Regex regex2 = new Regex("\\d+$");
                        var keyList = regex1.Matches(key).ToList().ConvertAll(x => Convert.ToInt32(x.ToString().Replace(",", "")));
                        keyList.Add(Convert.ToInt32(regex2.Match(key).ToString()));
                        Console.WriteLine("Введите текст: ");
                        var text = Console.ReadLine();
                        var list = text.ToCharArray().ToList().ConvertAll(x => (int)x);
                        var result = new List<string>();
                        for(int i=0;i<list.Count;i++)
                        {
                            int ikey = i % keyList.Count;
                            result.Add(char.ConvertFromUtf32((list[i] + keyList[ikey]) % 65536));
                        }
                        Console.Write("Зашифрованная строка: ");
                        result.ForEach(x => Console.Write(x));
                        Console.WriteLine();
                    }
                    else if (command == 8)
                    {
                        Console.WriteLine("Введите Ключ в следующем формате '1,2,3,4,5' ");
                        var key = Console.ReadLine();
                        Regex regex1 = new Regex("\\d+,{1}");
                        Regex regex2 = new Regex("\\d+$");
                        var keyList = regex1.Matches(key).ToList().ConvertAll(x => Convert.ToInt32(x.ToString().Replace(",", "")));
                        keyList.Add(Convert.ToInt32(regex2.Match(key).ToString()));
                        Console.WriteLine("Введите текст: ");
                        var text = Console.ReadLine();
                        var list = text.ToCharArray().ToList().ConvertAll(x => (int)x);
                        var result = new List<string>();
                        for (int i = 0; i < list.Count; i++)
                        {
                            int ikey = i % keyList.Count;
                            result.Add(char.ConvertFromUtf32((list[i] + 65536 - keyList[ikey]) % 65536));
                        }

                        Console.Write("Расшифрованная строка: ");
                        result.ForEach(x => Console.Write(x));
                        Console.WriteLine();
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
        }

        private static uint GetPrimeNumber()
        {
            Random random = new Random();
            using (FileStream fs = new FileStream(Environment.CurrentDirectory + "\\ListPrimeNumber.txt", FileMode.Open))
            {
                byte[] array = new byte[fs.Length];
                fs.Read(array, 0, array.Length);
                string textFromFile = System.Text.Encoding.Default.GetString(array);
                Regex regex = new Regex("\\d+");
                var mat = regex.Matches(textFromFile);
                var rand = random.Next(0, mat.Count - 1);
                return Convert.ToUInt32(mat[rand].Value);
            }
        }
        private static ulong GetOpen(ulong fi)
        {
            for (ulong e = 2; e < fi; e++)
            {
                if (GreatestCommonDivisor(e, fi) == 1)
                {
                    return e;
                }
            }
            return 0;
        }

        private static ulong GreatestCommonDivisor(ulong e, ulong fi)
        {
            while (e != 0 && fi != 0)
            {
                if (e > fi)
                    e %= fi;
                else
                    fi %= e;
            }

            return e == 0 ? fi : e;
        }

        private static ulong GetPrivate(ulong e, ulong fi)
        {
            ulong d;
            ulong temp = 1;

            while(true)
            {
                temp += fi;
                if(temp%e==0)
                {
                    d = (temp / e);
                    return d;
                }
            }
        }
        private static int Crypt(ulong main, ulong e, ulong n)
        {
            var mod = Convert.ToInt32(Convert.ToString( BigInteger.ModPow(main, e, n)));
            return mod;
        }
        private static int Decrypt(ulong main, ulong d, ulong n)
        {
            var mod = Convert.ToInt32(Convert.ToString(BigInteger.ModPow(main, d, n)));
            return mod;
        }
    }
}
