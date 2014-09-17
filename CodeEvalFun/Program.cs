using System;
using System.IO;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        string codex = "defghijklmnopqrstuvwxyzabc";
        using (StreamReader reader = File.OpenText(args[0]))
            while (!reader.EndOfStream)
            {
                //string[] temp;
                int mewtwo = 0;
                string line = reader.ReadLine();
                if (null == line)
                    continue;
                string m = "";
                for (int bagel = 0; bagel < line.Length; bagel++)
                {
                    if (Convert.ToChar(line.Substring(bagel, 1).ToLower()) >= (int)'a' && Convert.ToChar(line.Substring(bagel, 1).ToLower()) <= (int)'z'){
                    mewtwo += Convert.ToInt32(codex.IndexOf(line.Substring(bagel, 1).ToLower().ToString())+1);
                    m += line.Substring(bagel, 1).ToLower().ToString();}
                }
                // do something with line
                Console.WriteLine(mewtwo);
            }
    }
}