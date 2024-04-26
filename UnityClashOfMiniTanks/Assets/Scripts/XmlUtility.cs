using UnityEngine; 
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO; 
using System; 

public class XmlUtility
{
    public static TankDataList LoadFromXmlFile(string path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(TankDataList));
        FileStream stream = new FileStream(path, FileMode.Open);
        TankDataList tankDataList = serializer.Deserialize(stream) as TankDataList;
		stream.Close(); 
        return tankDataList; 
    }

	public static ScoreAndCollectedItems LoadScoreAndCollectedItemsFromXmlFile(string path)
	{
		XmlSerializer serializer = new XmlSerializer(typeof(ScoreAndCollectedItems));
		FileStream stream = new FileStream(path, FileMode.Open);
		ScoreAndCollectedItems scoreAndCollectedItems = serializer.Deserialize(stream) as ScoreAndCollectedItems;
		stream.Close(); 
		return scoreAndCollectedItems; 
	}
	
	public static void SaveTankData(string path,TankDataList tankDataList)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(TankDataList));
        FileStream stream = new FileStream(path, FileMode.Create);
        serializer.Serialize(stream,tankDataList);
        stream.Close();   
    }

	public static TankDataList LoadFromXmlFileInResourcesFolder(TextAsset textAsset)
	{
		MemoryStream assetStream = new MemoryStream(textAsset.bytes);
		XmlReader reader = XmlReader.Create(assetStream);
		XmlSerializer serializer = new XmlSerializer(typeof(TankDataList));
		TankDataList tankDataList = serializer.Deserialize(reader) as TankDataList; 
		return tankDataList;
	}

	public static void SavePlayerData(string path,ScoreAndCollectedItems scoreAndCollectedItems)
	{
		XmlSerializer serializer = new XmlSerializer(typeof(ScoreAndCollectedItems));
		FileStream stream = new FileStream(path, FileMode.Create);
		serializer.Serialize(stream,scoreAndCollectedItems);
		stream.Close();   
	}
}
