using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;
using System.Numerics;

namespace RSA_Encryption
{
    class Program
    {
        static void Main(string[] args)
        {
            var p = GetPrimeNumber();
            var q = GetPrimeNumber();
            uint n = p * q;
            uint d = GetPrimeNumber();
            //uint e = GetPrimeNumber();
            uint e = GetOpen(d, p,q);
            var text = Console.ReadLine().ToLower().ToCharArray().ToList();
            var list = text.ConvertAll(x => Convert.ToInt32( x)-1072);
            var cryptList = list.ConvertAll(x => Crypt((uint)x, e, n));
            var resultCrypt = cryptList.ConvertAll(x => Convert.ToChar(x));
            string s = "";
            resultCrypt.ForEach(x => s += x);
            Console.WriteLine(s);
            var resultDeCrypt = resultCrypt.ConvertAll(x => Convert.ToInt32(x) - 1072).ConvertAll(x => Decrypt((uint)x, d, n)).ConvertAll(x => Convert.ToChar(x));
            var s2 = "";
            resultDeCrypt.ForEach(x => s2 += x);
            Console.WriteLine(s2);
            Console.ReadLine();
        }

        private static uint GetPrimeNumber()
        {
            Random random = new Random();
            using(FileStream fs = new FileStream(Environment.CurrentDirectory+"\\ListPrimeNumber.txt",FileMode.Open))
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
        private static uint GetOpen(uint d, uint p, uint q)
        {
            ulong fi = (p - 1) * (q - 1);
            ulong e=0;
            for (ulong i = 0; i < 18446744073709551614; i++)
            {
                var a = (fi*i + 1) %d;
                ulong b0 = 0;
                if (a == b0) {
                    e = (fi * i + 1) / d;
                    break;
                }
            }
            return (uint)e;
        }
        private static int Crypt(uint main, uint e, uint n)
        {
            var mod = Convert.ToInt32(Convert.ToString( BigInteger.ModPow(main, (ulong)e, (ulong)n)))+1072;
            return mod;
        }
        private static int Decrypt(uint main, uint d, uint n)
        {
            var mod = Convert.ToInt32(Convert.ToString(BigInteger.ModPow(main, (ulong)d, (ulong)n))) + 1072;
            return mod;
        }
    }
}
