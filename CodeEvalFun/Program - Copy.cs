using System;
using System.IO;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        using (StreamReader reader = File.OpenText(args[0]))
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                if (null == line)
                    continue;
                // do something with line
                int space1 = 0;
                int space2 = 0;
                for (int m = line.Length - 1; m >= 0; m--)
                {
                    if (line[m] == ' ')
                    {
                        space1 = m - 1;
                        m = -1;
                    }

                }
                for (int m = space1; m >= 0; m--)
                {
                    if (line[m] == ' ')
                    {
                        space2 = m + 1;
                        m = -1;
                    }
                }

                Console.WriteLine(line.Substring(space2,space1-space2+1));
            }
    }
}