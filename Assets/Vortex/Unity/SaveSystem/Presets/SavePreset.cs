using System.Collections.Generic;
using System.Xml.Serialization;
using Vortex.Core.SaveSystem.Abstraction;

namespace Vortex.Unity.SaveSystem.Presets
{
    [XmlRoot]
    public class SavePreset
    {
        [XmlElement] public List<SaveData> data { get; set; }
    }
}