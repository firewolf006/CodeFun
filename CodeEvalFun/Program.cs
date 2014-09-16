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
                string line = reader.ReadLine();
                if (null == line)
                    continue;
                mewtwo += int.Parse(line);
                // do something with line
                Console.WriteLine(mewtwo);
            }
        
    }
}