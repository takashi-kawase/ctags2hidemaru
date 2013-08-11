using System;
using System.Collections;
using System.IO;

namespace ctags2hm
{
    class ctags2hm
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Usage: ctags2hm.exe tags_file");
                return;
            }

            string inFileName = args[0];

            if (!File.Exists(inFileName))
            {
                Console.WriteLine("Input file not found.");
                return;
            }

            string[] inStr = File.ReadAllLines(inFileName);

            string dir = Path.GetDirectoryName(inFileName);
            dir = string.Concat(dir, "\\");

            ArrayList outputStringArray = new ArrayList();
            foreach(string str in inStr)
            {
                if (! str.StartsWith("!_TAG_"))
                {
                    string[] stringSeparators = { "\t" };
                    string[] strParts = str.Split(stringSeparators, 3, StringSplitOptions.RemoveEmptyEntries);
                    
                    string name = strParts[0];
                    string file = strParts[1];
                    string line = strParts[2];

                    int i = line.IndexOf(";\"");
                    if (i != -1)
                    {
                        line = line.Remove(i);
                    }

                    if (file.StartsWith(dir, StringComparison.CurrentCultureIgnoreCase))
                    {
                        file = file.Remove(0, dir.Length);
                    }

                    string outStrTemp = file + "(" + line + ")" + " : " + name;

                    outputStringArray.Add(outStrTemp);
                }
            }
            string[] outStr = (string[])outputStringArray.ToArray(typeof(string));

            File.WriteAllLines(args[0], outStr);
        }
    }
}
