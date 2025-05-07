using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

    private Dictionary<string, AttackConfig> configMap;

    public void AttackCalculationSetUp(monsterPart part)
    {
        LoadConfigs();

        #region Arms
        if (part.isArm)
        {
            //arms can move in forwards, upwards, and downwards motions. We should make it so backwards moves will flip your character and perform a forwards
            //pretty straight forward with forward attacks, simply move the torso and associated pieces forward
            //upwards can be complicated, current plan is to have upwards neutrals do a little bounce while the heavies launch you up
            //downwards have not been touched at all, lots of issues with what to do if you're grounded or on a semisolid. While in the air, heavies will send you downward superhero landing style
            //it would actually be cool to have down attacks while grounded damage the floor (destructible floors?). We already need to redo downward torso animations to make it do more like a scorpion 

            if (part.isRightShoulderLimb || part.isLeftShoudlerLimb || part.isChestLimb)
            {
                ApplyConfig("ArmShoulderConfig");
            }
            else if (part.isRightPelvisLimb || part.isLeftPelvisLimb || part.isBellyLimb || part.isShoulderBladeLimb)
            {
                ApplyConfig("ArmPelvisConfig");
            }
            else if (part.isTailLimb)
            {
                ApplyConfig("ArmTailConfig");
            }
            else if (part.isRightEarLimb || part.isLeftEarLimb || part.isFacialLimb)
            {
                ApplyConfig("ArmEarConfig");
            }
            else if (part.isTopHeadLimb)
            {
                ApplyConfig("ArmTopHeadConfig");
            }
            else if (part.isBacksideHeadLimb)
            {
                ApplyConfig("ArmBackSideHeadConfig");
            }

        }
        #endregion

        #region Legs
        if (part.isLeg)
        {
            //legs can attack forwards, backwards, and downwards. We should come up with a way to do an upwards kick by spinning the character like fox
            //downwards will be just like with arms where it can damage platforms (small aoe with stomps)
            //we should make it so that kicks dont brace the other leg, just do a high kick like chun-li

            if (part.isRightShoulderLimb || part.isLeftShoudlerLimb || part.isChestLimb)
            {
                ApplyConfig("LegShoulderConfig");
            }
            else if (part.isRightPelvisLimb || part.isLeftPelvisLimb)
            {
                ApplyConfig("LegPelvisConfig");
            }
            else if (part.isBellyLimb || part.isShoulderBladeLimb)
            { 
                ApplyConfig("LegBellyShoulderBladeConfig");
            }
            else if (part.isTailLimb)
            {
                ApplyConfig("LegTailConfig");
            }
            else if (part.isRightEarLimb || part.isLeftEarLimb || part.isFacialLimb)
            {
                ApplyConfig("LegEarConfig");
            }
            else if (part.isTopHeadLimb)
            {
                ApplyConfig("LegTopHeadConfig");
            }
            else if (part.isBacksideHeadLimb)
            {
                ApplyConfig("LegBackSideHeadConfig");
            }

        }
        #endregion

        #region Tails
        if (part.isTail)
        {
            //tails can move backwards, upwards, and downwards. Upwards and downwards will spin the character in order to make sure contact is made
            //forwards attack should flip character around or potentially do spin manuevers with the non shooty ones
            //for anywhere thats not the hind quarters, treat the tail you would like a leg with body movements

            if (part.isRightShoulderLimb || part.isLeftShoudlerLimb || part.isChestLimb)
            {
                ApplyConfig("TailShoulderConfig");
            }
            else if (part.isRightPelvisLimb || part.isLeftPelvisLimb)
            {
                ApplyConfig("TailPelvisConfig");
            }
            else if (part.isBellyLimb || part.isShoulderBladeLimb)
            {
                ApplyConfig("TailBellyShoulderBladeConfig");
            }
            else if (part.isTailLimb)
            {
                ApplyConfig("TailTailConfig");
            }
            else if (part.isRightEarLimb || part.isLeftEarLimb || part.isFacialLimb)
            {
                ApplyConfig("TailEarConfig");
            }
            else if (part.isTopHeadLimb)
            {
                ApplyConfig("TailTopHeadConfig");
            }
            else if (part.isBacksideHeadLimb)
            {
                ApplyConfig("TailBackSideHeadConfig");
            }
        }
        #endregion

        #region Horns

        if (part.isHorn)
        {
            //horns just attack in forwards direction with torso or head sending attack upwards or downwards. Backwards will flip character. Downwards is impossible
            //limited capabilities when attached not to top head, face, or upper torso. Basically it can just go in singular direction

            if (part.isRightShoulderLimb || part.isLeftShoudlerLimb || part.isChestLimb)
            {
                ApplyConfig("HornShoulderConfig");
            }
            else if (part.isRightPelvisLimb || part.isLeftPelvisLimb || part.isBellyLimb || part.isShoulderBladeLimb)
            {
                ApplyConfig("HornPelvisConfig");
            }
            else if (part.isTailLimb)
            {
                ApplyConfig("HornTailConfig");
            }
            else if (part.isRightEarLimb || part.isLeftEarLimb || part.isFacialLimb)
            {
                ApplyConfig("HornEarConfig");
            }
            else if (part.isTopHeadLimb)
            {
                ApplyConfig("HornTopHeadConfig");
            }
            else if (part.isBacksideHeadLimb)
            {
                ApplyConfig("HornBackSideHeadConfig");
            }
        }
        #endregion

        #region Eyes
        if (part.isEye)
        {
            //eyes just attack in forwards direction with torso or head sending attack upwards or downwards. Backwards will flip character. Downwards is impossible
            //limited capabilities when attached not to face, chest, or lower torso. Basically it can just go in singular direction

            if (part.isRightShoulderLimb || part.isLeftShoudlerLimb || part.isChestLimb)
            {
                ApplyConfig("EyeShoulderConfig");
            }
            else if (part.isRightPelvisLimb || part.isLeftPelvisLimb || part.isBellyLimb || part.isShoulderBladeLimb)
            {
                ApplyConfig("EyePelvisConfig");
            }
            else if (part.isTailLimb)
            {
                ApplyConfig("EyeTailConfig");
            }
            else if (part.isRightEarLimb || part.isLeftEarLimb || part.isFacialLimb)
            {
                ApplyConfig("EyeEarConfig");
            }
            else if (part.isTopHeadLimb)
            {
                ApplyConfig("EyeTopHeadConfig");
            }
            else if (part.isBacksideHeadLimb)
            {
                ApplyConfig("EyeBackSideHeadConfig");
            }
        }
        #endregion

        #region Mouths
        if (part.isMouth)
        {
            //eyes just attack in forwards direction with torso or head sending attack upwards or downwards. Backwards will flip character. Downwards is impossible
            //limited capabilities when attached not to face, chest, or lower torso. Basically it can just go in singular direction

            if (part.isRightShoulderLimb || part.isLeftShoudlerLimb || part.isChestLimb)
            {
                ApplyConfig("MouthShoulderConfig");
            }
            else if (part.isRightPelvisLimb || part.isLeftPelvisLimb || part.isBellyLimb || part.isShoulderBladeLimb)
            {
                ApplyConfig("MouthPelvisConfig");
            }
            else if (part.isTailLimb)
            {
                ApplyConfig("MouthTailConfig");
            }
            else if (part.isRightEarLimb || part.isLeftEarLimb || part.isFacialLimb)
            {
                ApplyConfig("MouthEarConfig");
            }
            else if (part.isTopHeadLimb)
            {
                ApplyConfig("MouthTopHeadConfig");
            }
            else if (part.isBacksideHeadLimb)
            {
                ApplyConfig("MouthBackSideHeadConfig");
            }

        }
        #endregion
    }

    private void LoadConfigs()
    {
        // Loads the json file from Assets/Resources/Data/attack_configs.json 
        TextAsset jsonText = Resources.Load<TextAsset>("Data/attack_configs");

        if (jsonText == null)
        {
            Debug.LogError("Could not load attack_configs.json");
            return;
        }

        // Builds a Dictionary that maps the config object with the name of the config so that it can be retrived by name elsewhere in the code.
        AttackConfigList configList = JsonUtility.FromJson<AttackConfigList>(jsonText.text);
        configMap = configList.configs.ToDictionary(config => config.ConfigName);
    }

    private void ApplyConfig(string configName)
    {
        // Searches the config map for the config object that is paired with the passed in name.
        if (configMap.TryGetValue(configName, out var config))
        {
            requiresBackwardStance = false;
            requiresForwardStance = false;
            requiresRightStance = true;
            requiresLeftStance = false;


            hasTorsoCommand = true;
            forwardInputTorsoCommand = config.forwardInputTorsoCommand;
            backwardInputTorsoCommand = config.backwardInputTorsoCommand;
            upwardInputTorsoCommand = config.upwardInputTorsoCommand;
            downwardInputTorsoCommand = config.downwardInputTorsoCommand;

            hasHeadCommand = false;
            forwardInputHeadCommand = config.forwardInputHeadCommand;
            backwardInputHeadCommand = config.backwardInputHeadCommand;
            upwardInputHeadCommand = config.upwardInputHeadCommand;
            downwardInputHeadCommand = config.downwardInputHeadCommand;

            hasNeutralMovementCommand = true;
            forwardNeutralMovementCommand = config.forwardNeutralMovementCommand;
            upwardNeutralMovementCommand = config.upwardNeutralMovementCommand;
            backwardNeutralMovementCommand = config.backwardNeutralMovementCommand;
            downwardNeutralMovementCommand = config.downwardNeutralMovementCommand;

            hasHeavyMovementCommand = true;
            forwardHeavyMovementCommand = config.forwardHeavyMovementCommand;
            upwardHeavyMovementCommand = config.upwardHeavyMovementCommand;
            backwardHeavyMovementCommand = config.backwardHeavyMovementCommand;
            downwardHeavyMovementCommand = config.downwardHeavyMovementCommand;
        }
        else
        {
            Debug.LogError(configName + " could not be found in attack_configs.json");
        }
    }
}
