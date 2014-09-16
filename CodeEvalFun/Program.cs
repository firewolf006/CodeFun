using System;
using System.IO;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        int mewtwo = 0;
        using (StreamReader reader = File.OpenText(args[0]))
            while (!reader.EndOfStream)
            {
                string[] temp;
                mewtwo = 0;
                string line = reader.ReadLine();
                if (null == line)
                    continue;
                temp = line.Split(' ');
                for (int bagel = 0; bagel < temp.Length; bagel++)
                {
                    if (temp[bagel].Length > temp[mewtwo].Length)
                    {
                        mewtwo = bagel;
                    }
                }
                // do something with line
                Console.WriteLine(temp[mewtwo]);
            }
    }
}