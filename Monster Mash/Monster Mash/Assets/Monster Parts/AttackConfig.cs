using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Data model for the json config objects. Do not touch this class unless you need to add new varables to the json
[Serializable]
public class AttackConfig
{
    public string ConfigName;
    public MonsterPartType PartType;
    public MonsterPartConnectionMask PartConnections;
    public TorsoCommand forwardInputTorsoCommand;
    public TorsoCommand backwardInputTorsoCommand;
    public TorsoCommand upwardInputTorsoCommand;
    public TorsoCommand downwardInputTorsoCommand;
    public HeadCommand forwardInputHeadCommand;
    public HeadCommand backwardInputHeadCommand;
    public HeadCommand upwardInputHeadCommand;
    public HeadCommand downwardInputHeadCommand;
    public NeutralMovementCommand forwardNeutralMovementCommand;
    public NeutralMovementCommand upwardNeutralMovementCommand;
    public NeutralMovementCommand backwardNeutralMovementCommand;
    public NeutralMovementCommand downwardNeutralMovementCommand;
    public HeavyMovementCommand forwardHeavyMovementCommand;
    public HeavyMovementCommand upwardHeavyMovementCommand;
    public HeavyMovementCommand backwardHeavyMovementCommand;
    public HeavyMovementCommand downwardHeavyMovementCommand;
}

// You cant load json directly into a list so we need a wrapper class
[Serializable]
public class AttackConfigList
{
    public List<AttackConfig> Configs;
}
