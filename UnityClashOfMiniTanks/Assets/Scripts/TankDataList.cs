using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

[XmlRoot("TankData")]
public class TankDataList
{
    [XmlArray("Tanks")]
    [XmlArrayItem("Tank")]
    public List<Tank> Tanks = new List<Tank>();
}