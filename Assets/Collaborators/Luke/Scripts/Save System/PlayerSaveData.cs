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

    public string GetString()
    {
        MemoryStream ms = new MemoryStream(NumberOfBytes());
        BinaryWriter bw = new BinaryWriter(ms);

        bw.Write(clearedSection1);
        bw.Write(pillsCollected);

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

        br.Close();
        ms.Close();

        return this;
    }

    public int NumberOfBytes()
    {
        // 4 bytes for pillcount and 1 byte for section1 bool
        return 4 + 1;
    }

    public int Version => 1;
}
