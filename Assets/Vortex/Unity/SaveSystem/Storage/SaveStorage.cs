using System.Collections.Generic;
using System.Xml.Serialization;
using Vortex.Core.SaveSystem.Abstraction;

namespace Vortex.Unity.SaveSystem.Storage
{
    [XmlRoot]
    public class SaveStorage
    {
        [XmlElement] public List<SaveData> data { get; set; }
    }
}