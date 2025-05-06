using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackConfig
{
    public string ConfigName;
    public string forwardInputTorsoCommand;
    public string backwardInputTorsoCommand;
    public string upwardInputTorsoCommand;
    public string downwardInputTorsoCommand;
    public string forwardInputHeadCommand;
    public string backwardInputHeadCommand;
    public string upwardInputHeadCommand;
    public string downwardInputHeadCommand;
    public string forwardNeutralMovementCommand;
    public string upwardNeutralMovementCommand;
    public string backwardNeutralMovementCommand;
    public string downwardNeutralMovementCommand;
    public string forwardHeavyMovementCommand;
    public string upwardHeavyMovementCommand;
    public string backwardHeavyMovementCommand;
    public string downwardHeavyMovementCommand;
}

[System.Serializable]
public class AttackConfigList
{
    public List<AttackConfig> configs;
}
