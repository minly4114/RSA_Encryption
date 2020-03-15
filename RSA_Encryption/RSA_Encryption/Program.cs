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

            while (true)
            {
                Console.WriteLine("Выберите действие:"+ Environment.NewLine + "1-Зашифровать методом RSA" + Environment.NewLine + "2-Расшифровать методом RSA" + Environment.NewLine + "3-Зашифровать методом Вернама" + Environment.NewLine + "4-Расшифровать методом Вернама" + Environment.NewLine + "Другой символ -Выход");
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
                        Regex regex = new Regex("\\d+-{1}");

                        var match = regex.Matches(text).ToList();
                        var list = match.ConvertAll(x => Convert.ToUInt64(x.ToString().Replace("-", "")));
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
                        List<BitArray> textBits =  new List<BitArray>();
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
                        var res = result.ConvertAll(x => Convert.ToChar(x));
                    }
                    else if (command == 4)
                    {

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
