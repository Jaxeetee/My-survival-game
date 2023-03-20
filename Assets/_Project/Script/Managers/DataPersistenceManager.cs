using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class DataPersistenceManager : MonoBehaviour
{
    public static DataPersistenceManager instance { get; private set; }

    [Header("File Storage Config")]
    [SerializeField]
    private string _fileName;
    [SerializeField]
    private bool _useEncrpytion;


    private GameData _gameData;
    private List<IDataPersistence> _dataPersistenceObjects;
    private FileDataHandler _dataHandler;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Data Persistence Manager in hte scene");
        }

        instance = this;

    }

    private void Start()
    {
        this._dataHandler = new FileDataHandler(Application.persistentDataPath, _fileName, _useEncrpytion);
        this._dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }



    public void NewGame()
    {
        this._gameData = new GameData();
    }

    public void LoadGame()
    {
        this._gameData = _dataHandler.Load();
        if (this._gameData == null)
        {
            Debug.Log("No data was found. Initializing data to defaults");
            NewGame();
        }

        // TODO - push the loaded data to all other scripts that need it
        foreach(IDataPersistence dataPersistenceObj in _dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(_gameData);
        }
    }

    public void SaveGame()
    {
        // TODO - pass the data to other scripts so they can update it
        foreach (IDataPersistence dataPersistenceObj in _dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(ref _gameData);
        }
        // TODO - save that data to a file using the data handler
        _dataHandler.Save(_gameData);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }

}
