using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SendMail
{
    class Program
    {
        static void Main(string[] args)
        {
            SendMultiMail();
        }
        public static void SendMail(int length)
        {
            string toEmail = "buikha2011@gmail.com";            
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            string content = new string(Enumerable.Repeat(chars, length).
                Select(s => s[random.Next(s.Length)]).ToArray());
            new MailHelper().SenMail(toEmail, content, chars);
        }

        public static void SendMultiMail()
        {
            for (int i = 0; i < 1000; i++)
            {
                SendMail(13);
                Thread.Sleep(1000);
                Console.WriteLine(i);
            }
        }
    }
}
