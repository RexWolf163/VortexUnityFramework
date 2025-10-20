using System.Xml.Serialization;

namespace Vortex.Unity.SaveSystem.Storage
{
    [XmlRoot]
    public class SaveStorage
    {
        [XmlElement] public string Id { get; set; }

        [XmlElement] public string Data { get; set; }
    }
}