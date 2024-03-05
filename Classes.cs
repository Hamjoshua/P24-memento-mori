using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace app
{
    class Memento
    {
        public string FileName { get; set; }
        public string Extension { get; set; }
        public string Data { get; set; }

    }

    public interface IOriginator
    {
        object GetMemento();
        void SetMemento(object mementoObj);
    }

    public class CareTaker
    {
        private object _memento;
        public void SaveState(IOriginator originator)
        {
            _memento = originator.GetMemento();
        }

        public void RestoreState(IOriginator originator)
        {
            originator.SetMemento(_memento);
        }

    }

    [Serializable]
    public class TextFile : IOriginator
    {
        public string FullFileName
        {
            get
            {
                return FileName + Extension;
            }
        }

        public string FileName { get; set; }
        public string Extension { get; set; }
        public string Data { get; set; }

        public TextFile()
        {

        }
        public TextFile(string fileName, string data = "")
        {
            var splited = fileName.Split('.');
            FileName = splited[0];
            Extension = "." + splited[1];
            Data = data;
        }

        public void SerializeToBinary(string path = "")
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();

            string savePath = $"{FileName}.dat";
            if (path != "")
            {
                savePath = $"{path}/{savePath}";
            }

            using (Stream fileStream = new FileStream(savePath, FileMode.Create, FileAccess.Write, FileShare.None))
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
                textFile = (TextFile)binaryFormatter.Deserialize(fileStream);
            }

            return textFile;
        }

        public void SerializeToXml(string path = "")
        {
            string savePath = $"{FileName}.xml";
            if (path != "")
            {
                savePath = $"{path}/{savePath}";
            }

            using (Stream fileStream = new FileStream(savePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(this.GetType());
                xmlSerializer.Serialize(fileStream, this);
            }
        }

        public static TextFile DeserealizeFromXml(string fullPath)
        {
            using (StreamReader streamReader = new StreamReader(fullPath))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(TextFile));
                return (TextFile)xmlSerializer.Deserialize(streamReader);
            }
        }

        public object GetMemento()
        {
            return new Memento
            {
                FileName = this.FileName,
                Data = this.Data,
                Extension = Extension
            };
        }

        public void SetMemento(object mementoObj)
        {
            if (mementoObj is Memento)
            {
                var memento = mementoObj as Memento;
                this.FileName = memento.FileName;
                this.Data = memento.Data;
                this.Extension = memento.Extension;
            }
        }
    }
}
