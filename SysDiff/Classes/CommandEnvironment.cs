using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysDiff
{
    /// <summary>
    /// An interactive command environment for defining and comparing datasets.
    /// </summary>
    public class CommandEnvironment
    {
        public List<DataSet> DataSets { get; set; }
        public List<List<DataPointComparison>> Comparisons { get; set; }
        
        public CommandEnvironment()
        {
            DataSets = new List<DataSet>();
        }

        public void run()
        {
            /*
             * Hi SysDiff.
             * working = Registry
             * broken = Registry
             * load working working1.reg
             * load working working2.reg      ... or load working working1.reg working2.reg or working*.reg
             * load broken broken.reg
             * compare working broken
             * save out.csv
             * quit
             */

            Console.WriteLine(Strings.CE_Welcome);
            
            bool exit = false;
            while (!exit)
            {
                Console.Write(Strings.CE_Prompt);
                List<string> input = tokenize(Console.ReadLine());
                switch (input[0])
                {
                    case Strings.CE_Cmd_Quit:
                        Console.WriteLine("Command not recognized, try typing exit instead.");
                        break;
                    case Strings.CE_Cmd_Exit:
                        exit = true;
                        break;
                    default:
                        Console.WriteLine(Strings.CE_CommandNotRecognized, input[0]);
                        break;
                }
            }
        }
        
        public List<string> tokenize(string input)
        {
            List<string> tokens = new List<string>();
            string t = "";
            bool inQuote = false;
            foreach (char c in input)
            {
                if (c == '"')
                {
                    inQuote = !inQuote;
                }
                else if (c == ' ' && !inQuote)
                {
                    tokens.Add(t.ToLower());
                    t = "";
                }
                else
                {
                    t += c;
                }
            }
            if (t != "")
            {
                tokens.Add(t.ToLower());
            }

            return tokens;
        }
    }
}
