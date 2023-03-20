using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int dataOne;
    public float dataTwo;
    public string name;


    //initial values defined will be the default Values
    public GameData()
    {
        this.dataOne = 0;
        this.dataTwo = 0;
        this.name = "Player Name";
    }
}
