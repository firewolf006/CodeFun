using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using HtmlAgilityPack;
using System.ServiceProcess;



namespace Importinator
{
    class Program
    {


        /// <summary>
        /// FEED ME STRING TO PARSE IT GOOD
        /// </summary>
        /// <param name="args"></param>
        static List<String> parseObject(String theString, List<String> theElements)
        {
            List<String> newObjects = new List<String>();

            int fromHere, toHere, thisLength;


            for (int x = 0; x < theElements.Count(); x++)
            {
                fromHere = theString.IndexOf("\"" + theElements[x] + "\"");
                if (x != theElements.Count()-1)
                {
                    toHere = theString.IndexOf("\"" + theElements[x + 1] + "\"");
                }
                else
                {
                    toHere = theString.Length;
                }


                if (x < theElements.Count()-1)
                {
                    toHere = theString.LastIndexOf(',', toHere);
                }
                thisLength = toHere - fromHere;
                newObjects.Add(theString.Substring(fromHere, thisLength));
            }
            
            return newObjects;
        }
        
        /// <summary>
        /// Breaks strings into two parts... it's left-heavy
        /// </summary>
        /// <param name="breakThisString">MotherString</param>
        /// <param name="withThisString">Ducklings</param>
        /// <returns>MeowMix</returns>
        static List<String> breakInHalf(String breakThisString, String withThisString){
            List<String> halves = new List<String>();
            int middle;
            
            middle = breakThisString.IndexOf(withThisString);
            halves.Add(breakThisString.Substring(0, middle).Trim());
            halves.Add(breakThisString.Substring(middle + 1, breakThisString.Length - middle-1).Trim());
            return halves;
        }

        private async void importing(){

        }

        static void Main(string[] args)
        {
            importer iMP = new importer();
            iMP.import(args);
        }

        static void MainOLD(string[] args)
        {
            string funText;
            string[] lineS;
            using (StreamReader reader = File.OpenText(args[0]))
                while (!reader.EndOfStream)
                {
                    //string[] temp;
                    string line = reader.ReadLine();
                    if (null == line)
                        continue;

                    //string[] PressXtoJSON = new string[] line;

                    List<String> elements = new List<String>();
                    List<String> moreElements = new List<String>();
                    List<String> PressXtoJSON = new List<String>();


                    elements.Add("key");
                    elements.Add("mealList");
                    elements.Add("datemaj");
                    elements.Add("validityBegin");
                    elements.Add("validityEnd");
                    elements.Add("caterer");
                    elements.Add("departureArrivalList");
                    elements.Add("pairingGrid");
                    elements.Add("cycle");
                    elements.Add("state");
                    elements.Add("version");
                    elements.Add("language");
                    elements.Add("classCodeList");
                    elements.Add("mealType");
                    elements.Add("menusList");

                    moreElements.Add("label");
                    moreElements.Add("content");
                    moreElements.Add("sequence");

                    List<int> commas = new List<int>();

                    commas.Add(0);
                    int temp, firstComma=0;

                    for (int x=0; x < line.Length; x++){
                        temp = line.IndexOf(',',x);
                        //nevermind

                    }

                    //line.Sub

                    lineS = line.Split(',');

                    PressXtoJSON = parseObject(line, elements);

                    String temp1 = PressXtoJSON[14].Substring(PressXtoJSON[14].IndexOf("\"coursesList\""), PressXtoJSON[14].Length - PressXtoJSON[14].IndexOf("\"coursesList\""));

                    //PressXtoJSON = parseObject(temp1, moreElements);

                    foreach (String moo in PressXtoJSON)
                    {
                        Console.WriteLine(moo);
                    }

                    string[] foo = new string[] { "loo" };

                    String ben = "waterloovilage";

                    ben.Split(foo, StringSplitOptions.None);

                    int fromHere, toHere, thisLength;

                    fromHere = PressXtoJSON[14].IndexOf('[', 20);
                    toHere = PressXtoJSON[14].LastIndexOf(']', PressXtoJSON[14].Length-1);
                    thisLength = toHere - fromHere;



                    theList test = new theList();
                    test = JsonConvert.DeserializeObject<theList>(line);
                    
                    //something = JsonConvert.DeserializeObject(line);



                    Console.ReadLine();
                }

        }
    }
}
