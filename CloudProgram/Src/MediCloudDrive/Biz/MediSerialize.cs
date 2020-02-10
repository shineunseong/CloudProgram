using System.IO;
using System.Xml.Serialization;

namespace MediCloudDrive.Biz
{
    public static class MediSerialize<T>
    {
        public static void SerializeObject(T model, string strFullPath)
        {
            using (StreamWriter wr = new StreamWriter(strFullPath))
            {
                XmlSerializer xs = new XmlSerializer(typeof(T));
                xs.Serialize(wr, model);
            }
        }

        public static T DeserializeModel(string strFullPath)
        {
            T tempModel = default;
            using (var reader = new StreamReader(strFullPath))
            {
                XmlSerializer xs = new XmlSerializer(typeof(T));
                tempModel = (T)xs.Deserialize(reader);
            }
            return tempModel;
        }
    }
}