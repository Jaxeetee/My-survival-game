using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileDataHandler
{
    private string _dataDirPath = "";
    private string _dataFileName = "";

    private bool _useEncryption = false;
    private readonly string _encryptionCodeWord = "word";

    public FileDataHandler(string dataDirPath, string dataFileName, bool useEncrpytion)
    {
        this._dataDirPath = dataDirPath;
        this._dataFileName = dataFileName;
        this._useEncryption = useEncrpytion;
    }

    public GameData Load()
    {
        string fullPath = Path.Combine(_dataDirPath, _dataFileName);

        GameData loadedData = null;
        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }


                //deserializing the data from Json back into the C# Object 
                if (_useEncryption)
                {
                    dataToLoad = EncryptDecrypt(dataToLoad);
                }
                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError("error occured when trying to save data to file: " + fullPath + "\n" + e);
            }
        }
        return loadedData;
    }

    public void Save(GameData data)
    {
        string fullPath = Path.Combine(_dataDirPath, _dataFileName);

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            // serializing game data to a json

            string dataToStore = JsonUtility.ToJson(data, true);

            if (_useEncryption)
            {
                dataToStore = EncryptDecrypt(dataToStore);
            }

            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("error occured when trying to save data to file: " + fullPath + "\n" + e);
        }
    }

    private string EncryptDecrypt(string data)
    {
        string modifiedData = "";
        for (int i = 0; i< data.Length; i++)
        {
            modifiedData += (char)(data[i] ^ _encryptionCodeWord[1 % _encryptionCodeWord.Length]);

        }

        return modifiedData;
    }
}
