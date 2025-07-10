using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TorsoCommand
{
    None,
    ForwardAttack,
    UpperAttack,
    DownwardAttack,
    LowerAttack,
    ButtAttack,
}

public enum HeadCommand
{
    None,
    UpwardAttack,
    ForwardAttack,
    FaceAttack,
}

public enum NeutralMovementCommand
{
    None,
    ForwardStrike,
    UpwardStrike,
    DownwardStrike,
    BackwardStrike,
    Quick180,
}

public enum HeavyMovementCommand
{
    None,
    ForwardLeap,
    UpwardLeap,
    DownwardLeap,
    BackwardLeap,
    Quick180Heavy,
}