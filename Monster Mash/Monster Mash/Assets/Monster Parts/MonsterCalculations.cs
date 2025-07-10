using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.IO;

public class MonsterCalculations
{
    #region return these vars
    public bool requiresBackwardStance;
    public bool requiresForwardStance;
    public bool requiresRightStance;
    public bool requiresLeftStance;

    public bool hasTorsoCommand;
    public string forwardInputTorsoCommand;
    public string backwardInputTorsoCommand;
    public string upwardInputTorsoCommand;
    public string downwardInputTorsoCommand;

    public bool hasHeadCommand;
    public string forwardInputHeadCommand;
    public string backwardInputHeadCommand;
    public string upwardInputHeadCommand;
    public string downwardInputHeadCommand;

    public bool hasNeutralMovementCommand;
    public string forwardNeutralMovementCommand;
    public string upwardNeutralMovementCommand;
    public string backwardNeutralMovementCommand;
    public string downwardNeutralMovementCommand;

    public bool hasHeavyMovementCommand;
    public string forwardHeavyMovementCommand;
    public string upwardHeavyMovementCommand;
    public string backwardHeavyMovementCommand;
    public string downwardHeavyMovementCommand;
    #endregion

    private AttackConfigList attackConfigList;

    public void AttackCalculationSetUp(NewMonsterPart part)
    {
        LoadJSON();

        foreach (AttackConfig config in attackConfigList.Configs)
        {
            if (part.PartType == config.PartType)
            {
                if (config.PartConnections.Matches(part.connectionPoint))
                {
                    ApplyConfig(config);
                }
            }
        }
    }

    private void ApplyConfig(AttackConfig config)
    {
        requiresBackwardStance = false;
        requiresForwardStance = false;
        requiresRightStance = true;
        requiresLeftStance = false;


        hasTorsoCommand = true;
        forwardInputTorsoCommand = ConvertEnumToAnimationString(config.forwardInputTorsoCommand.ToString());
        backwardInputTorsoCommand = ConvertEnumToAnimationString(config.backwardInputTorsoCommand.ToString());
        upwardInputTorsoCommand = ConvertEnumToAnimationString(config.upwardInputTorsoCommand.ToString());
        downwardInputTorsoCommand = ConvertEnumToAnimationString(config.downwardInputTorsoCommand.ToString());

        hasHeadCommand = false;
        forwardInputHeadCommand = ConvertEnumToAnimationString(config.forwardInputHeadCommand.ToString());
        backwardInputHeadCommand = ConvertEnumToAnimationString(config.backwardInputHeadCommand.ToString());
        upwardInputHeadCommand = ConvertEnumToAnimationString(config.upwardInputHeadCommand.ToString());
        downwardInputHeadCommand = ConvertEnumToAnimationString(config.downwardInputHeadCommand.ToString());

        hasNeutralMovementCommand = true;
        forwardNeutralMovementCommand = ConvertEnumToAnimationString(config.forwardNeutralMovementCommand.ToString());
        upwardNeutralMovementCommand = ConvertEnumToAnimationString(config.upwardNeutralMovementCommand.ToString());
        backwardNeutralMovementCommand = ConvertEnumToAnimationString(config.backwardNeutralMovementCommand.ToString());
        downwardNeutralMovementCommand = ConvertEnumToAnimationString(config.downwardNeutralMovementCommand.ToString());

        hasHeavyMovementCommand = true;
        forwardHeavyMovementCommand = ConvertEnumToAnimationString(config.forwardHeavyMovementCommand.ToString());
        upwardHeavyMovementCommand = ConvertEnumToAnimationString(config.upwardHeavyMovementCommand.ToString());
        backwardHeavyMovementCommand = ConvertEnumToAnimationString(config.backwardHeavyMovementCommand.ToString());
        downwardHeavyMovementCommand = ConvertEnumToAnimationString(config.downwardHeavyMovementCommand.ToString());
    }

    private void LoadJSON()
    {
        string jsonPath = "Assets/Resources/Data/attack_configs.json";

        if (File.Exists(jsonPath))
        {
            string jsonText = File.ReadAllText(jsonPath);
            attackConfigList = JsonUtility.FromJson<AttackConfigList>(jsonText);
        }
    }

    // You cant have spaces in enum names so we need to split the enum string and rearange it so that it matches the animation string
    private string ConvertEnumToAnimationString(string enumString)
    {
        if (enumString.Equals("None"))
        {
            return "";
        }

        for (int i = 0; i < enumString.Length; i++)
        {
            if (char.IsDigit(enumString[i]))
            {
                // Split into: prefix, digits, and possible suffix
                string prefix = enumString.Substring(0, i);
                string digitsAndSuffix = enumString[i..];

                for (int j = 0; j < digitsAndSuffix.Length; j++)
                {
                    if (char.IsUpper(digitsAndSuffix[j]))
                    {
                        string digits = digitsAndSuffix.Substring(0, j);
                        string suffix = digitsAndSuffix[j..];

                        return $"{prefix} {digits} {suffix}";
                    }
                }

                return $"{prefix} {digitsAndSuffix}";
            }

            if (char.IsUpper(enumString[i]) && i != 0)
            {
                string prefix = enumString.Substring(0, i);
                string suffix = enumString[i..];

                return $"{prefix} {suffix}";
            }
        }


        return enumString;
    }
}
