using UnityEngine;
using System;

public class SOMission : ScriptableObject
{
    [SerializeField]
    private string _nameOfMission;

    public string nameOfMission
    {
        get => _nameOfMission;
        private set => _nameOfMission = value;
    }

    [SerializeField]
    private string _missionDescription;

    public string missionDescription
    {
        get => _missionDescription;
        private set => _missionDescription = value;
    }

    [SerializeField]
    private Objective _objectives;

    private bool _isComplete;
    public bool isComplete
    {
        get => _isComplete;
        private set {
            if (_objectives != null){
                _isComplete = _objectives.isObjectiveComplete;
            }
        }
    }

    struct Rewards{
        int experience;
        int currency;
        GameObject randomItem;
    }

    public event Action onMissionComplete;

    


}
