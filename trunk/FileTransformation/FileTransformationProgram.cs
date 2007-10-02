using System;
using System.Collections.Generic;
using System.Text;
using log4net;
using System.IO;

namespace Mousourouli.MDE.Recommendation
{
    class FileTransformationProgram
    {
        static FileTransformationProgram()
        {
            log4net.Config.XmlConfigurator.Configure();
        }


        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Usage: {0} input output", Path.GetFileName(System.Reflection.Assembly.GetExecutingAssembly().Location));
                return;
            }

            string input = args[0];
            string output = args[1];

            using (StreamReader srI = new StreamReader(File.Open(input, FileMode.Open)))
            {
                using (StreamWriter srO = new StreamWriter(File.Open(output, FileMode.Create)))
                {
                    string currentID = null;
                    string line;
                    
                    while ((line = srI.ReadLine()) != null)
                    {
                        string[] values = line.Split(' ', '\t');
                        if (values.Length != 2)
                            throw new Exception("Wrong file format");

                        string ID = values[0].Trim();
                        string item = values[1].Trim();

                        if (ID == currentID)
                        {
                            srO.Write(" " + item);
                        }
                        else
                        {
                            if (currentID==null)
                                srO.Write(item);
                            else
                                srO.Write("\r\n" + item);

                            currentID = ID;
                        }

                    }
                }
            }

        }
    }
}
