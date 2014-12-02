using System;
using System.IO;
using System.Collections.Generic;

class Program
{
    public static int howMany(string baseS, char test)
    {
        int tot =0;

        while (baseS.IndexOf(test) > -1)
        {
            tot++;
            baseS = baseS.Substring(baseS.IndexOf(test)+1);
        }


        return tot;
    }

    static void Main(string[] args)
    {
        string codex = "";
        using (StreamReader reader = File.OpenText(args[0]))
            while (!reader.EndOfStream)
            {
                int mewtwo = 0;
                char test; 
                string line = reader.ReadLine();
                if (null == line)
                    continue;
                string m = "";
                codex = "";
                List<int> codex2 = new List<int>();
                for (int bagel = 0; bagel < line.Length; bagel++)
                {
                    test = Convert.ToChar(line.Substring(bagel, 1).ToLower());
                    if (test >= (int)'a' && test <= (int)'z')
                    {
                        if (codex.IndexOf(test) < 0)
                        {
                            codex += test;
                        }
                        mewtwo += Convert.ToInt32(codex.IndexOf(line.Substring(bagel, 1).ToLower().ToString()) + 1);
                        m += line.Substring(bagel, 1).ToLower().ToString();
                    }
                }

                foreach(char mm in codex)
                {
                    codex2.Add(howMany(m, mm));
                }
                // do something with line
                Console.WriteLine(mewtwo);
            }
    }
}