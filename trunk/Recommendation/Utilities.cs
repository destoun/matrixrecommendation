using System;
using System.Collections.Generic;
using System.Text;


using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters.Soap;

using System.Xml.Serialization;

namespace Mousourouli.MDE.Recommendation
{
    public class Utilities
    {

        public static void SaveToFile(string filename, object obj)
        {
            using (Stream s = File.Open(filename, FileMode.Create))
            {
                BinaryFormatter b = new BinaryFormatter();
                b.Serialize(s, obj);
            }

        }

         public static object ReadFromFile(string filename)
        {
            object result;

            using (Stream s = File.Open(filename, FileMode.Open))
            {
                BinaryFormatter b = new BinaryFormatter();
                result = b.Deserialize(s);
            }
            return result;

        }



    }
}
