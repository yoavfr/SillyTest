using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace SillyTest
{
    [DataContract]
    public class Serialization
    {
        public ExtensionDataObject ExtensionData { get; set; }
        
        [DataMember]
        public string Val { get; set; }
        [DataMember]
        public SubObject Sub { get; set; }

        public override string ToString()
        {
            return string.Format("Serialization: Val:{0}, Sub:{1}", Val, Sub);
        }


        public static void Run()
        {
            string serialized = Serialize();
            Console.WriteLine("Serialized:\n{0}\n",serialized);
            Serialization obj = Deserialize(serialized);
            Console.WriteLine("Deserialized:\n{0}", obj);
            string newVersion = "{\"Sub\": {\"SubVal\":2},\"Val\":\"stringValue\",\"NewVal\":\"NewString\"}";
            obj = Deserialize(newVersion);
            Console.WriteLine("Deserialized with changes\n{0}",obj);

        }

        public static string Serialize ()
        {
            Serialization s = new Serialization()
            {
                Val = "stringValue",
                Sub = new SubObject() { SubVal = 2 },
            };
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Serialization));
            using (Stream stream = new MemoryStream())
            {
                serializer.WriteObject(stream, s);
                stream.Position = 0;
                StreamReader reader = new StreamReader(stream);
                string value = reader.ReadToEnd();
                return value;
            }
        }

        public static Serialization Deserialize(string json)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(json);
            using (Stream stream = new MemoryStream(bytes))
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Serialization));
                try
                {
                    Serialization result = (Serialization)serializer.ReadObject(stream);
                    return result;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return null;
                }
            }
        }
        public class SubObject
        {
            [DataMember]
            public int SubVal { get; set; }
            public override string ToString()
            {
                return string.Format("SubObject: SubVal:{0}", SubVal);
            }
        }
    }
}
