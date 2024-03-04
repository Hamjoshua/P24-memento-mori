using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace app
{
    [Serializable]
    public class TextFile
    {
        public string FileName;
        public string Extension; // нужно ли?
        public string Data;

        public void SerializeToBinary()
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            using (Stream fileStream = new FileStream(FileName, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                binaryFormatter.Serialize(fileStream, this);
            }
        }

        static public TextFile DeserealizeFromBinary(string filePath)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            TextFile textFile;

            using (Stream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                textFile = (TextFile) binaryFormatter.Deserialize(fileStream);
            }

            return textFile;
        }
    }
}
