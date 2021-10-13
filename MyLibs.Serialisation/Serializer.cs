using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml.Serialization;

namespace MyLibs.Serialisation
{
   public class Serializer<T>
    {
        public enum ModeSerialisation { JSON,XML,BIN}
        private ModeSerialisation _mode;
        private Dictionary<ModeSerialisation, Action<T, string>> serializers;
        private Dictionary<ModeSerialisation, Func<string,T>> deserializers;
        public Serializer(ModeSerialisation mode)
        {
            _mode = mode;
            serializers = new Dictionary<ModeSerialisation, Action<T, string>>();
            serializers.Add(ModeSerialisation.BIN, SerializeBinary);
            serializers.Add(ModeSerialisation.XML, SerializeXml);
            serializers.Add(ModeSerialisation.JSON, SerializeJson);

            deserializers = new Dictionary<ModeSerialisation, Func< string,T>>();
            deserializers.Add(ModeSerialisation.BIN, DeserializeBinary);
            deserializers.Add(ModeSerialisation.XML, DeserializeXml);
            deserializers.Add(ModeSerialisation.JSON, DeserializeJson);
        }

        #region Serialize
        public void Serialize(T data, string path)// path :nom de fichier,data :objet de tyupt T
        {
            serializers[_mode](data, path);
        }
        private void SerializeBinary(T data,string path)
        {
            using(StreamWriter file = new StreamWriter(path, true))
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(file.BaseStream, data);
            }
        }
        private void SerializeXml(T data, string path)
        {
            XmlSerializer xs = new XmlSerializer(typeof(T));
            using (StreamWriter file = new StreamWriter(path, true))
            {
                 xs.Serialize(file.BaseStream, data);
            }
        }
        private void SerializeJson (T data,string path)
        {
            using (StreamWriter file = new StreamWriter(path, true))
            {
                file.WriteLine(JsonConvert.SerializeObject(data));     
            }
        }
        #endregion Serialize
        #region Deserialize
        public void Deserialize( string path, T data)// path :nom de fichier,data :objet de type T
        {
            deserializers[_mode](path);
        }
        private void DeserializeBinary(T data, string path)
        {
            using (StreamWriter file = new StreamWriter(path, true))
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(file.BaseStream, data);
                
            }
        }
        private void DeserializeXml(T data, string path)
        {
            XmlSerializer xs = new XmlSerializer(typeof(T));
            using (StreamReader  file = new StreamReader(path, true))
            {
                xs.Deserialize(file.BaseStream);
            }
        }
        private void DeserializeJson(T data, string path)
        {
            using (StreamReader file = new StreamReader(path, true))
            {
                file.ReadLine(JsonConvert.SerializeObject(data));
            }
        }
        #endregion Deserialize
    }

}
     
    

