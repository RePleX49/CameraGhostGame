using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public interface ISaveable
{
    int Version { get; }
    string GetString();
    object SetFromString(string textData);
    int NumberOfBytes();
}

public class PlayerSaveData : ISaveable
{
    public bool clearedSection1;
    public int pillsCollected;
    public bool isCompletedSave;
    public int masterVolume;

    public const string saveFileName = "playerData.txt";   

    public PlayerSaveData()
    {
        clearedSection1 = false;
        pillsCollected = 0;
        isCompletedSave = true;
        masterVolume = 100;
    }

    public int NumberOfBytes()
    {
        // 4 bytes for pillcount and 1 byte for section1 bool
        // 1 byte for isEmptySave bool
        // 4 bytes for masterVolume setting
        return 4 + 1 + 1 + 4;
    }

    public string GetString()
    {
        MemoryStream ms = new MemoryStream(NumberOfBytes());
        BinaryWriter bw = new BinaryWriter(ms);

        bw.Write(clearedSection1);
        bw.Write(pillsCollected);
        bw.Write(isCompletedSave);
        bw.Write(masterVolume);

        ByteSerializer saveData = new ByteSerializer(ms.GetBuffer());

        bw.Close();
        ms.Close();

        return saveData.GetAsString();
    }

    public object SetFromString(string textData)
    {
        ByteSerializer dataLoader = new ByteSerializer(textData);
        byte[] data = dataLoader.GetAsBytes();

        if (data == null || data.Length <= 1)
            return this;

        MemoryStream ms = new MemoryStream(data);
        BinaryReader br = new BinaryReader(ms);

        clearedSection1 = br.ReadBoolean();
        pillsCollected = br.ReadInt32();
        isCompletedSave = br.ReadBoolean();
        masterVolume = br.ReadInt32();

        br.Close();
        ms.Close();

        return this;
    }

    public int Version => 1;

    public static void DeletePlayerSave()
    {
        // Reset pills to 0
        // Reset level clear flags to false

        PlayerSaveData saveDataBuffer = new PlayerSaveData();
        saveDataBuffer.SetFromString(ReadTextFile("", "playerData.txt"));
        saveDataBuffer.clearedSection1 = false;
        saveDataBuffer.pillsCollected = 0;
        saveDataBuffer.isCompletedSave = true;
        WriteString("playerData.txt", saveDataBuffer.GetString());
    }

    public static void WriteString(string fileName, string data)
    {
        using (var outputFile = new StreamWriter(Path.Combine(Application.dataPath, fileName)))
        {
            outputFile.Write(data);
        }
    }

    public static string ReadTextFile(string filePath, string fileName)
    {
        if(!File.Exists(Application.dataPath + filePath + "/" + fileName))
        {
            Debug.Log("Save file doesn't exist");
            File.Create(Path.Combine(Application.dataPath, fileName));
            Debug.Log("Made new save file");
        }

        var toReturn = "";
        using (var fileReader = new StreamReader(Application.dataPath + filePath + "/" + fileName))
        {
            string line;
            do
            {
                line = fileReader.ReadLine();
                if (!string.IsNullOrEmpty(line))
                {
                    toReturn += line + '\n';
                }
            } while (line != null);

            fileReader.Close();
        }

        return toReturn;
    }

    public static PlayerSaveData GetPlayerSave()
    {
        PlayerSaveData saveData = new PlayerSaveData();
        saveData.SetFromString(ReadTextFile("", "playerData.txt"));
        return saveData;
    }
}
