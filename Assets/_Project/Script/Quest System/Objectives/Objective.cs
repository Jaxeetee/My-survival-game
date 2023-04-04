using UnityEngine;


[System.Serializable]
public class Objective
{
    protected string objectiveDescription;
    public bool isObjectiveComplete;

    protected virtual void InitializeObjective()
    {

    }

    protected virtual void Goal()
    {

    }
}
