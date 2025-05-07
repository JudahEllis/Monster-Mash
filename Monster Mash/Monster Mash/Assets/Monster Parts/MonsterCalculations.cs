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
                requiresBackwardStance = false;
                requiresForwardStance = false;
                requiresRightStance = true;
                requiresLeftStance = false;

                hasTorsoCommand = true;
                forwardInputTorsoCommand = "Forward Attack";
                backwardInputTorsoCommand = "Forward Attack"; //make sure monster flips before attacking
                upwardInputTorsoCommand = "Upper Attack";
                downwardInputTorsoCommand = "Downward Attack";

                hasHeadCommand = false;
                forwardInputHeadCommand = "";
                backwardInputHeadCommand = "";
                upwardInputHeadCommand = "Upward Attack";
                downwardInputHeadCommand = "Forward Attack"; // Always forward. Value does not change and can be removed

                hasNeutralMovementCommand = true;
                forwardNeutralMovementCommand = "Forward Strike";
                upwardNeutralMovementCommand = "Upward Strike";
                backwardNeutralMovementCommand = "Quick 180";
                downwardNeutralMovementCommand = "Downward Strike";

                hasHeavyMovementCommand = true;
                forwardHeavyMovementCommand = "Forward Leap";
                upwardHeavyMovementCommand = "Upward Leap"; // is empty most of the time, occurences can be reduced by having a default value
                backwardHeavyMovementCommand = "Quick 180 Heavy"; // is empty most of the time, occurences can be reduced by having a default value
                downwardHeavyMovementCommand = "Downward Leap"; // is empty most of the time, occurences can be reduced by having a default value

                //ApplyConfig("ArmShoulderConfig");


            }
            else if (part.isRightPelvisLimb || part.isLeftPelvisLimb || part.isBellyLimb || part.isShoulderBladeLimb)
            {
                /*requiresBackwardStance = false;
                requiresForwardStance = false;
                requiresRightStance = true;
                requiresLeftStance = false;

                hasTorsoCommand = true;
                forwardInputTorsoCommand = "Lower Attack";
                backwardInputTorsoCommand = "Lower Attack"; //make sure monster flips before attacking
                upwardInputTorsoCommand = "Lower Attack";
                downwardInputTorsoCommand = "Lower Attack";

                hasHeadCommand = false;
                forwardInputHeadCommand = "";
                backwardInputHeadCommand = "";
                upwardInputHeadCommand = "Upward Attack";
                downwardInputHeadCommand = "Forward Attack";

                hasNeutralMovementCommand = true;
                forwardNeutralMovementCommand = "Forward Strike";
                upwardNeutralMovementCommand = "Upward Strike";
                backwardNeutralMovementCommand = "Quick 180";
                downwardNeutralMovementCommand = "Downward Strike";

                hasHeavyMovementCommand = true;
                forwardHeavyMovementCommand = "Forward Leap";
                upwardHeavyMovementCommand = "Upward Leap";
                backwardHeavyMovementCommand = "Quick 180 Heavy";
                downwardHeavyMovementCommand = "Downward Leap";*/

                ApplyConfig("ArmPelvisConfig");
            }
            else if (part.isTailLimb)
            {
                /*requiresBackwardStance = false;
                requiresForwardStance = false;
                requiresRightStance = true;
                requiresLeftStance = false;

                hasTorsoCommand = true;
                forwardInputTorsoCommand = "Butt Attack";
                backwardInputTorsoCommand = "Butt Attack"; //make sure monster flips before attacking
                upwardInputTorsoCommand = "Forward Attack";
                downwardInputTorsoCommand = "Lower Attack";

                hasHeadCommand = false;
                forwardInputHeadCommand = "";
                backwardInputHeadCommand = "";
                upwardInputHeadCommand = "Upward Attack";
                downwardInputHeadCommand = "Forward Attack";

                hasNeutralMovementCommand = true;
                forwardNeutralMovementCommand = "Quick 180";
                upwardNeutralMovementCommand = "Upward Strike";
                backwardNeutralMovementCommand = "Backward Strike";
                downwardNeutralMovementCommand = "Downward Strike";

                hasHeavyMovementCommand = true;
                forwardHeavyMovementCommand = "Quick 180 Heavy";
                upwardHeavyMovementCommand = "Upward Leap";
                backwardHeavyMovementCommand = "Backward Leap";
                downwardHeavyMovementCommand = "Downward Leap";*/

                ApplyConfig("ArmTailConfig");

            }
            else if (part.isRightEarLimb || part.isLeftEarLimb || part.isFacialLimb)
            {
                /*requiresBackwardStance = false;
                requiresForwardStance = false;
                requiresRightStance = true;
                requiresLeftStance = false;

                hasTorsoCommand = true;
                forwardInputTorsoCommand = "Forward Attack";
                backwardInputTorsoCommand = "Forward Attack"; //make sure monster flips before attacking
                upwardInputTorsoCommand = "Upper Attack";
                downwardInputTorsoCommand = "Downward Attack";

                hasHeadCommand = false;
                forwardInputHeadCommand = "Face Attack";
                backwardInputHeadCommand = "Face Attack";
                upwardInputHeadCommand = "Upward Attack";
                downwardInputHeadCommand = "Forward Attack";

                hasNeutralMovementCommand = true;
                forwardNeutralMovementCommand = "Forward Strike";
                upwardNeutralMovementCommand = "Upward Strike";
                backwardNeutralMovementCommand = "Quick 180";
                downwardNeutralMovementCommand = "Downward Strike";

                hasHeavyMovementCommand = true;
                forwardHeavyMovementCommand = "Forward Leap";
                upwardHeavyMovementCommand = "Upward Leap";
                backwardHeavyMovementCommand = "Quick 180 Heavy";
                downwardHeavyMovementCommand = "Downward Leap";*/

                ApplyConfig("ArmEarConfig");

            }
            else if (part.isTopHeadLimb)
            {
                /*requiresBackwardStance = false;
                requiresForwardStance = false;
                requiresRightStance = true;
                requiresLeftStance = false;

                hasTorsoCommand = true;
                forwardInputTorsoCommand = "Forward Attack";
                backwardInputTorsoCommand = "Forward Attack"; //make sure monster flips before attacking
                upwardInputTorsoCommand = "Upper Attack";
                downwardInputTorsoCommand = "Downward Attack";

                hasHeadCommand = false;
                forwardInputHeadCommand = "Forward Attack";
                backwardInputHeadCommand = "Forward Attack";
                upwardInputHeadCommand = "Upward Attack";
                downwardInputHeadCommand = "Forward Attack";

                hasNeutralMovementCommand = true;
                forwardNeutralMovementCommand = "Forward Strike";
                upwardNeutralMovementCommand = "Upward Strike";
                backwardNeutralMovementCommand = "Quick 180";
                downwardNeutralMovementCommand = "Downward Strike";

                hasHeavyMovementCommand = true;
                forwardHeavyMovementCommand = "Forward Leap";
                upwardHeavyMovementCommand = "Upward Leap";
                backwardHeavyMovementCommand = "Quick 180 Heavy";
                downwardHeavyMovementCommand = "Downward Leap";*/

                ApplyConfig("ArmTopHeadConfig");
            }
            else if (part.isBacksideHeadLimb)
            {
                /*requiresBackwardStance = false;
                requiresForwardStance = false;
                requiresRightStance = true;
                requiresLeftStance = false;

                hasTorsoCommand = true;
                forwardInputTorsoCommand = "Lower Attack";
                backwardInputTorsoCommand = "Lower Attack"; //make sure monster flips before attacking
                upwardInputTorsoCommand = "Upper Attack";
                downwardInputTorsoCommand = "Lower Attack";

                hasHeadCommand = false;
                forwardInputHeadCommand = "Face Attack";
                backwardInputHeadCommand = "Face Attack";
                upwardInputHeadCommand = "Forward Attack";
                downwardInputHeadCommand = "Upward Attack";

                hasNeutralMovementCommand = true;
                forwardNeutralMovementCommand = "Quick 180";
                upwardNeutralMovementCommand = "Upward Strike";
                backwardNeutralMovementCommand = "Backward Strike";
                downwardNeutralMovementCommand = "Downward Strike";

                hasHeavyMovementCommand = true;
                forwardHeavyMovementCommand = "Quick 180 Heavy";
                upwardHeavyMovementCommand = "Upward Leap";
                backwardHeavyMovementCommand = "Backward Leap";
                downwardHeavyMovementCommand = "Downward Leap";*/

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
                requiresBackwardStance = false;
                requiresForwardStance = false;
                requiresRightStance = true;
                requiresLeftStance = false;

                hasTorsoCommand = true;
                forwardInputTorsoCommand = "Forward Attack";
                backwardInputTorsoCommand = "Forward Attack"; //make sure monster flips before attacking
                upwardInputTorsoCommand = "Upper Attack";
                downwardInputTorsoCommand = "Downward Attack";

                hasHeadCommand = false;
                forwardInputHeadCommand = "";
                backwardInputHeadCommand = "";
                upwardInputHeadCommand = "Upward Attack";
                downwardInputHeadCommand = "Forward Attack";

                hasNeutralMovementCommand = true;
                forwardNeutralMovementCommand = "";
                upwardNeutralMovementCommand = "";
                backwardNeutralMovementCommand = "";
                downwardNeutralMovementCommand = "";

                hasHeavyMovementCommand = true;
                forwardHeavyMovementCommand = "";
                upwardHeavyMovementCommand = "";
                backwardHeavyMovementCommand = "";
                downwardHeavyMovementCommand = "";

            }
            else if (part.isRightPelvisLimb || part.isLeftPelvisLimb)
            {
                requiresBackwardStance = false;
                requiresForwardStance = false;
                requiresRightStance = true;
                requiresLeftStance = false;

                hasTorsoCommand = true;
                forwardInputTorsoCommand = "Lower Attack";
                backwardInputTorsoCommand = "Butt Attack"; //make sure monster flips before attacking
                upwardInputTorsoCommand = "Lower Attack";
                downwardInputTorsoCommand = "Lower Attack";

                hasHeadCommand = false;
                forwardInputHeadCommand = "";
                backwardInputHeadCommand = "";
                upwardInputHeadCommand = "Upward Attack";
                downwardInputHeadCommand = "Forward Attack";

                hasNeutralMovementCommand = true;
                forwardNeutralMovementCommand = "";
                upwardNeutralMovementCommand = "";
                backwardNeutralMovementCommand = "";
                downwardNeutralMovementCommand = "";

                hasHeavyMovementCommand = true;
                forwardHeavyMovementCommand = "";
                upwardHeavyMovementCommand = "";
                backwardHeavyMovementCommand = "";
                downwardHeavyMovementCommand = "";
            }
            else if (part.isBellyLimb || part.isShoulderBladeLimb)
            {
                requiresBackwardStance = false;
                requiresForwardStance = false;
                requiresRightStance = true;
                requiresLeftStance = false;

                hasTorsoCommand = true;
                forwardInputTorsoCommand = "Lower Attack";
                backwardInputTorsoCommand = "Lower Attack"; //make sure monster flips before attacking
                upwardInputTorsoCommand = "Lower Attack";
                downwardInputTorsoCommand = "Lower Attack";

                hasHeadCommand = false;
                forwardInputHeadCommand = "";
                backwardInputHeadCommand = "";
                upwardInputHeadCommand = "Upward Attack";
                downwardInputHeadCommand = "Forward Attack";

                hasNeutralMovementCommand = true;
                forwardNeutralMovementCommand = "";
                upwardNeutralMovementCommand = "";
                backwardNeutralMovementCommand = "";
                downwardNeutralMovementCommand = "";

                hasHeavyMovementCommand = true;
                forwardHeavyMovementCommand = "";
                upwardHeavyMovementCommand = "";
                backwardHeavyMovementCommand = "";
                downwardHeavyMovementCommand = "";
            }
            else if (part.isTailLimb)
            {
                requiresBackwardStance = false;
                requiresForwardStance = false;
                requiresRightStance = true;
                requiresLeftStance = false;

                hasTorsoCommand = true;
                forwardInputTorsoCommand = "Butt Attack";
                backwardInputTorsoCommand = "Butt Attack"; //make sure monster flips before attacking
                upwardInputTorsoCommand = "Forward Attack";
                downwardInputTorsoCommand = "Lower Attack";

                hasHeadCommand = false;
                forwardInputHeadCommand = "";
                backwardInputHeadCommand = "";
                upwardInputHeadCommand = "Upward Attack";
                downwardInputHeadCommand = "Forward Attack";

                hasNeutralMovementCommand = true;
                forwardNeutralMovementCommand = "";
                upwardNeutralMovementCommand = "";
                backwardNeutralMovementCommand = "";
                downwardNeutralMovementCommand = "";

                hasHeavyMovementCommand = true;
                forwardHeavyMovementCommand = "";
                upwardHeavyMovementCommand = "";
                backwardHeavyMovementCommand = "";
                downwardHeavyMovementCommand = "";

            }
            else if (part.isRightEarLimb || part.isLeftEarLimb || part.isFacialLimb)
            {
                requiresBackwardStance = false;
                requiresForwardStance = false;
                requiresRightStance = true;
                requiresLeftStance = false;

                hasTorsoCommand = true;
                forwardInputTorsoCommand = "Forward Attack";
                backwardInputTorsoCommand = "Forward Attack"; //make sure monster flips before attacking
                upwardInputTorsoCommand = "Upper Attack";
                downwardInputTorsoCommand = "Downward Attack";

                hasHeadCommand = false;
                forwardInputHeadCommand = "Face Attack";
                backwardInputHeadCommand = "Face Attack";
                upwardInputHeadCommand = "Upward Attack";
                downwardInputHeadCommand = "Forward Attack";

                hasNeutralMovementCommand = true;
                forwardNeutralMovementCommand = "";
                upwardNeutralMovementCommand = "";
                backwardNeutralMovementCommand = "";
                downwardNeutralMovementCommand = "";

                hasHeavyMovementCommand = true;
                forwardHeavyMovementCommand = "";
                upwardHeavyMovementCommand = "";
                backwardHeavyMovementCommand = "";
                downwardHeavyMovementCommand = "";
            }
            else if (part.isTopHeadLimb)
            {
                requiresBackwardStance = false;
                requiresForwardStance = false;
                requiresRightStance = true;
                requiresLeftStance = false;

                hasTorsoCommand = true;
                forwardInputTorsoCommand = "Forward Attack";
                backwardInputTorsoCommand = "Forward Attack"; //make sure monster flips before attacking
                upwardInputTorsoCommand = "Upper Attack";
                downwardInputTorsoCommand = "Downward Attack";

                hasHeadCommand = false;
                forwardInputHeadCommand = "Forward Attack";
                backwardInputHeadCommand = "Forward Attack";
                upwardInputHeadCommand = "Upward Attack";
                downwardInputHeadCommand = "Forward Attack";

                hasNeutralMovementCommand = true;
                forwardNeutralMovementCommand = "";
                upwardNeutralMovementCommand = "";
                backwardNeutralMovementCommand = "";
                downwardNeutralMovementCommand = "";

                hasHeavyMovementCommand = true;
                forwardHeavyMovementCommand = "";
                upwardHeavyMovementCommand = "";
                backwardHeavyMovementCommand = "";
                downwardHeavyMovementCommand = "";

            }
            else if (part.isBacksideHeadLimb)
            {
                requiresBackwardStance = false;
                requiresForwardStance = false;
                requiresRightStance = true;
                requiresLeftStance = false;

                hasTorsoCommand = true;
                forwardInputTorsoCommand = "Lower Attack";
                backwardInputTorsoCommand = "Lower Attack"; //make sure monster flips before attacking
                upwardInputTorsoCommand = "Upper Attack";
                downwardInputTorsoCommand = "Lower Attack";

                hasHeadCommand = false;
                forwardInputHeadCommand = "Face Attack";
                backwardInputHeadCommand = "Face Attack";
                upwardInputHeadCommand = "Forward Attack";
                downwardInputHeadCommand = "Upward Attack";

                hasNeutralMovementCommand = true;
                forwardNeutralMovementCommand = "";
                upwardNeutralMovementCommand = "";
                backwardNeutralMovementCommand = "";
                downwardNeutralMovementCommand = "";

                hasHeavyMovementCommand = true;
                forwardHeavyMovementCommand = "";
                upwardHeavyMovementCommand = "";
                backwardHeavyMovementCommand = "";
                downwardHeavyMovementCommand = "";
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
                requiresBackwardStance = false;
                requiresForwardStance = false;
                requiresRightStance = true;
                requiresLeftStance = false;

                hasTorsoCommand = true;
                forwardInputTorsoCommand = "Forward Attack";
                backwardInputTorsoCommand = "Forward Attack"; //make sure monster flips before attacking
                upwardInputTorsoCommand = "Upper Attack";
                downwardInputTorsoCommand = "Downward Attack";

                hasHeadCommand = false;
                forwardInputHeadCommand = "";
                backwardInputHeadCommand = "";
                upwardInputHeadCommand = "Upward Attack";
                downwardInputHeadCommand = "Forward Attack";

                hasNeutralMovementCommand = true;
                forwardNeutralMovementCommand = "";
                upwardNeutralMovementCommand = "";
                backwardNeutralMovementCommand = "";
                downwardNeutralMovementCommand = "";

                hasHeavyMovementCommand = true;
                forwardHeavyMovementCommand = "";
                upwardHeavyMovementCommand = "";
                backwardHeavyMovementCommand = "";
                downwardHeavyMovementCommand = "";


            }
            else if (part.isRightPelvisLimb || part.isLeftPelvisLimb)
            {
                requiresBackwardStance = false;
                requiresForwardStance = false;
                requiresRightStance = true;
                requiresLeftStance = false;

                hasTorsoCommand = true;
                forwardInputTorsoCommand = "Lower Attack";
                backwardInputTorsoCommand = "Butt Attack"; //make sure monster flips before attacking
                upwardInputTorsoCommand = "Lower Attack";
                downwardInputTorsoCommand = "Lower Attack";

                hasHeadCommand = false;
                forwardInputHeadCommand = "";
                backwardInputHeadCommand = "";
                upwardInputHeadCommand = "Upward Attack";
                downwardInputHeadCommand = "Forward Attack";

                hasNeutralMovementCommand = true;
                forwardNeutralMovementCommand = "";
                upwardNeutralMovementCommand = "";
                backwardNeutralMovementCommand = "";
                downwardNeutralMovementCommand = "";

                hasHeavyMovementCommand = true;
                forwardHeavyMovementCommand = "";
                upwardHeavyMovementCommand = "";
                backwardHeavyMovementCommand = "";
                downwardHeavyMovementCommand = "";
            }
            else if (part.isBellyLimb || part.isShoulderBladeLimb)
            {
                requiresBackwardStance = false;
                requiresForwardStance = false;
                requiresRightStance = true;
                requiresLeftStance = false;

                hasTorsoCommand = true;
                forwardInputTorsoCommand = "Lower Attack";
                backwardInputTorsoCommand = "Lower Attack"; //make sure monster flips before attacking
                upwardInputTorsoCommand = "Lower Attack";
                downwardInputTorsoCommand = "Lower Attack";

                hasHeadCommand = false;
                forwardInputHeadCommand = "";
                backwardInputHeadCommand = "";
                upwardInputHeadCommand = "Upward Attack";
                downwardInputHeadCommand = "Forward Attack";

                hasNeutralMovementCommand = true;
                forwardNeutralMovementCommand = "";
                upwardNeutralMovementCommand = "";
                backwardNeutralMovementCommand = "";
                downwardNeutralMovementCommand = "";

                hasHeavyMovementCommand = true;
                forwardHeavyMovementCommand = "";
                upwardHeavyMovementCommand = "";
                backwardHeavyMovementCommand = "";
                downwardHeavyMovementCommand = "";
            }
            else if (part.isTailLimb)
            {
                requiresBackwardStance = false;
                requiresForwardStance = false;
                requiresRightStance = true;
                requiresLeftStance = false;

                hasTorsoCommand = true;
                forwardInputTorsoCommand = "Butt Attack";//make sure monster flips before attacking
                backwardInputTorsoCommand = "Butt Attack";
                upwardInputTorsoCommand = "Forward Attack";
                downwardInputTorsoCommand = "Lower Attack";

                hasHeadCommand = false;
                forwardInputHeadCommand = "";
                backwardInputHeadCommand = "";
                upwardInputHeadCommand = "Upward Attack";
                downwardInputHeadCommand = "Forward Attack";

                hasNeutralMovementCommand = true;
                forwardNeutralMovementCommand = "";
                upwardNeutralMovementCommand = "";
                backwardNeutralMovementCommand = "";
                downwardNeutralMovementCommand = "";

                hasHeavyMovementCommand = true;
                forwardHeavyMovementCommand = "";
                upwardHeavyMovementCommand = "";
                backwardHeavyMovementCommand = "";
                downwardHeavyMovementCommand = "";

            }
            else if (part.isRightEarLimb || part.isLeftEarLimb || part.isFacialLimb)
            {
                requiresBackwardStance = false;
                requiresForwardStance = false;
                requiresRightStance = true;
                requiresLeftStance = false;

                hasTorsoCommand = true;
                forwardInputTorsoCommand = "Forward Attack";
                backwardInputTorsoCommand = "Forward Attack"; //make sure monster flips before attacking
                upwardInputTorsoCommand = "Upper Attack";
                downwardInputTorsoCommand = "Downward Attack";

                hasHeadCommand = false;
                forwardInputHeadCommand = "Face Attack";
                backwardInputHeadCommand = "Face Attack";
                upwardInputHeadCommand = "Upward Attack";
                downwardInputHeadCommand = "Forward Attack";

                hasNeutralMovementCommand = true;
                forwardNeutralMovementCommand = "";
                upwardNeutralMovementCommand = "";
                backwardNeutralMovementCommand = "";
                downwardNeutralMovementCommand = "";

                hasHeavyMovementCommand = true;
                forwardHeavyMovementCommand = "";
                upwardHeavyMovementCommand = "";
                backwardHeavyMovementCommand = "";
                downwardHeavyMovementCommand = "";
            }
            else if (part.isTopHeadLimb)
            {
                requiresBackwardStance = false;
                requiresForwardStance = false;
                requiresRightStance = true;
                requiresLeftStance = false;

                hasTorsoCommand = true;
                forwardInputTorsoCommand = "Forward Attack";
                backwardInputTorsoCommand = "Forward Attack"; //make sure monster flips before attacking
                upwardInputTorsoCommand = "Upper Attack";
                downwardInputTorsoCommand = "Downward Attack";

                hasHeadCommand = false;
                forwardInputHeadCommand = "Forward Attack";
                backwardInputHeadCommand = "Forward Attack";
                upwardInputHeadCommand = "Upward Attack";
                downwardInputHeadCommand = "Forward Attack";

                hasNeutralMovementCommand = true;
                forwardNeutralMovementCommand = "";
                upwardNeutralMovementCommand = "";
                backwardNeutralMovementCommand = "";
                downwardNeutralMovementCommand = "";

                hasHeavyMovementCommand = true;
                forwardHeavyMovementCommand = "";
                upwardHeavyMovementCommand = "";
                backwardHeavyMovementCommand = "";
                downwardHeavyMovementCommand = "";

            }
            else if (part.isBacksideHeadLimb)
            {
                requiresBackwardStance = false;
                requiresForwardStance = false;
                requiresRightStance = true;
                requiresLeftStance = false;

                hasTorsoCommand = true;
                forwardInputTorsoCommand = "Lower Attack";
                backwardInputTorsoCommand = "Lower Attack"; //make sure monster flips before attacking
                upwardInputTorsoCommand = "Upper Attack";
                downwardInputTorsoCommand = "Lower Attack";

                hasHeadCommand = false;
                forwardInputHeadCommand = "Face Attack";
                backwardInputHeadCommand = "Face Attack";
                upwardInputHeadCommand = "Forward Attack";
                upwardInputHeadCommand = "Upward Attack";

                hasNeutralMovementCommand = true;
                forwardNeutralMovementCommand = "";
                upwardNeutralMovementCommand = "";
                backwardNeutralMovementCommand = "";
                downwardNeutralMovementCommand = "";

                hasHeavyMovementCommand = true;
                forwardHeavyMovementCommand = "";
                upwardHeavyMovementCommand = "";
                backwardHeavyMovementCommand = "";
                downwardHeavyMovementCommand = "";
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
                requiresBackwardStance = false;
                requiresForwardStance = false;
                requiresRightStance = true;
                requiresLeftStance = false;

                hasTorsoCommand = true;
                forwardInputTorsoCommand = "Forward Attack";
                backwardInputTorsoCommand = "Forward Attack"; //make sure monster flips before attacking
                upwardInputTorsoCommand = "Upper Attack";
                downwardInputTorsoCommand = "Downward Attack";

                hasHeadCommand = false;
                forwardInputHeadCommand = "";
                backwardInputHeadCommand = "";
                upwardInputHeadCommand = "Upward Attack";
                downwardInputHeadCommand = "Forward Attack";

                hasNeutralMovementCommand = true;
                forwardNeutralMovementCommand = "";
                upwardNeutralMovementCommand = "";
                backwardNeutralMovementCommand = "";
                downwardNeutralMovementCommand = "";

                hasHeavyMovementCommand = true;
                forwardHeavyMovementCommand = "";
                upwardHeavyMovementCommand = "";
                backwardHeavyMovementCommand = "";
                downwardHeavyMovementCommand = "";

            }
            else if (part.isRightPelvisLimb || part.isLeftPelvisLimb || part.isBellyLimb || part.isShoulderBladeLimb)
            {
                requiresBackwardStance = false;
                requiresForwardStance = false;
                requiresRightStance = true;
                requiresLeftStance = false;

                hasTorsoCommand = true;
                forwardInputTorsoCommand = "Lower Attack";
                backwardInputTorsoCommand = "Lower Attack"; //make sure monster flips before attacking
                upwardInputTorsoCommand = "Lower Attack";
                downwardInputTorsoCommand = "Lower Attack";

                hasHeadCommand = false;
                forwardInputHeadCommand = "";
                backwardInputHeadCommand = "";
                upwardInputHeadCommand = "Upward Attack";
                downwardInputHeadCommand = "Forward Attack";

                hasNeutralMovementCommand = true;
                forwardNeutralMovementCommand = "";
                upwardNeutralMovementCommand = "";
                backwardNeutralMovementCommand = "";
                downwardNeutralMovementCommand = "";

                hasHeavyMovementCommand = true;
                forwardHeavyMovementCommand = "";
                upwardHeavyMovementCommand = "";
                backwardHeavyMovementCommand = "";
                downwardHeavyMovementCommand = "";
            }
            else if (part.isTailLimb)
            {
                requiresBackwardStance = false;
                requiresForwardStance = false;
                requiresRightStance = true;
                requiresLeftStance = false;

                hasTorsoCommand = true;
                forwardInputTorsoCommand = "Butt Attack";
                backwardInputTorsoCommand = "Butt Attack"; //make sure monster flips before attacking
                upwardInputTorsoCommand = "Forward Attack";
                downwardInputTorsoCommand = "Lower Attack";
                hasHeadCommand = false;

                forwardInputHeadCommand = "";
                backwardInputHeadCommand = "";
                upwardInputHeadCommand = "Upward Attack";
                downwardInputHeadCommand = "Forward Attack";

                hasNeutralMovementCommand = true;
                forwardNeutralMovementCommand = "";
                upwardNeutralMovementCommand = "";
                backwardNeutralMovementCommand = "";
                downwardNeutralMovementCommand = "";

                hasHeavyMovementCommand = true;
                forwardHeavyMovementCommand = "";
                upwardHeavyMovementCommand = "";
                backwardHeavyMovementCommand = "";
                downwardHeavyMovementCommand = "";
            }
            else if (part.isRightEarLimb || part.isLeftEarLimb || part.isFacialLimb)
            {
                requiresBackwardStance = false;
                requiresForwardStance = false;
                requiresRightStance = true;
                requiresLeftStance = false;

                hasTorsoCommand = true;
                forwardInputTorsoCommand = "Forward Attack";
                backwardInputTorsoCommand = "Forward Attack"; //make sure monster flips before attacking
                upwardInputTorsoCommand = "Upper Attack";
                downwardInputTorsoCommand = "Downward Attack";

                hasHeadCommand = false;
                forwardInputHeadCommand = "Face Attack";
                backwardInputHeadCommand = "Face Attack";
                upwardInputHeadCommand = "Upward Attack";
                downwardInputHeadCommand = "Forward Attack";

                hasNeutralMovementCommand = true;
                forwardNeutralMovementCommand = "";
                upwardNeutralMovementCommand = "";
                backwardNeutralMovementCommand = "";
                downwardNeutralMovementCommand = "";

                hasHeavyMovementCommand = true;
                forwardHeavyMovementCommand = "";
                upwardHeavyMovementCommand = "";
                backwardHeavyMovementCommand = "";
                downwardHeavyMovementCommand = "";
            }
            else if (part.isTopHeadLimb)
            {
                requiresBackwardStance = false;
                requiresForwardStance = false;
                requiresRightStance = true;
                requiresLeftStance = false;

                hasTorsoCommand = true;
                forwardInputTorsoCommand = "Forward Attack";
                backwardInputTorsoCommand = "Forward Attack"; //make sure monster flips before attacking
                upwardInputTorsoCommand = "Upper Attack";
                downwardInputTorsoCommand = "Downward Attack";

                hasHeadCommand = false;
                forwardInputHeadCommand = "Forward Attack";
                backwardInputHeadCommand = "Forward Attack";
                upwardInputHeadCommand = "Upward Attack";
                downwardInputHeadCommand = "Forward Attack";

                hasNeutralMovementCommand = true;
                forwardNeutralMovementCommand = "";
                upwardNeutralMovementCommand = "";
                backwardNeutralMovementCommand = "";
                downwardNeutralMovementCommand = "";

                hasHeavyMovementCommand = true;
                forwardHeavyMovementCommand = "";
                upwardHeavyMovementCommand = "";
                backwardHeavyMovementCommand = "";
                downwardHeavyMovementCommand = "";
            }
            else if (part.isBacksideHeadLimb)
            {
                requiresBackwardStance = false;
                requiresForwardStance = false;
                requiresRightStance = true;
                requiresLeftStance = false;

                hasTorsoCommand = true;
                forwardInputTorsoCommand = "Lower Attack";
                backwardInputTorsoCommand = "Lower Attack"; //make sure monster flips before attacking
                upwardInputTorsoCommand = "Upper Attack";
                downwardInputTorsoCommand = "Lower Attack";

                hasHeadCommand = false;
                forwardInputHeadCommand = "Face Attack";
                backwardInputHeadCommand = "Face Attack";
                upwardInputHeadCommand = "Forward Attack";
                upwardInputHeadCommand = "Upward Attack";

                hasNeutralMovementCommand = true;
                forwardNeutralMovementCommand = "";
                upwardNeutralMovementCommand = "";
                backwardNeutralMovementCommand = "";
                downwardNeutralMovementCommand = "";

                hasHeavyMovementCommand = true;
                forwardHeavyMovementCommand = "";
                upwardHeavyMovementCommand = "";
                backwardHeavyMovementCommand = "";
                downwardHeavyMovementCommand = "";
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
                requiresBackwardStance = false;
                requiresForwardStance = false;
                requiresRightStance = true;
                requiresLeftStance = false;

                hasTorsoCommand = true;
                forwardInputTorsoCommand = "Forward Attack";
                backwardInputTorsoCommand = "Forward Attack"; //make sure monster flips before attacking
                upwardInputTorsoCommand = "Upper Attack";
                downwardInputTorsoCommand = "Downward Attack";

                hasHeadCommand = false;
                forwardInputHeadCommand = "";
                backwardInputHeadCommand = "";
                upwardInputHeadCommand = "Upward Attack";
                downwardInputHeadCommand = "Forward Attack";

                hasNeutralMovementCommand = true;
                forwardNeutralMovementCommand = "";
                upwardNeutralMovementCommand = "";
                backwardNeutralMovementCommand = "";
                downwardNeutralMovementCommand = "";

                hasHeavyMovementCommand = true;
                forwardHeavyMovementCommand = "";
                upwardHeavyMovementCommand = "";
                backwardHeavyMovementCommand = "";
                downwardHeavyMovementCommand = "";

            }
            else if (part.isRightPelvisLimb || part.isLeftPelvisLimb || part.isBellyLimb || part.isShoulderBladeLimb)
            {
                requiresBackwardStance = false;
                requiresForwardStance = false;
                requiresRightStance = true;
                requiresLeftStance = false;

                hasTorsoCommand = true;
                forwardInputTorsoCommand = "Lower Attack";
                backwardInputTorsoCommand = "Lower Attack"; //make sure monster flips before attacking
                upwardInputTorsoCommand = "Lower Attack";
                downwardInputTorsoCommand = "Lower Attack";

                hasHeadCommand = false;
                forwardInputHeadCommand = "";
                backwardInputHeadCommand = "";
                upwardInputHeadCommand = "Upward Attack";
                downwardInputHeadCommand = "Forward Attack";

                hasNeutralMovementCommand = true;
                forwardNeutralMovementCommand = "";
                upwardNeutralMovementCommand = "";
                backwardNeutralMovementCommand = "";
                downwardNeutralMovementCommand = "";

                hasHeavyMovementCommand = true;
                forwardHeavyMovementCommand = "";
                upwardHeavyMovementCommand = "";
                backwardHeavyMovementCommand = "";
                downwardHeavyMovementCommand = "";
            }
            else if (part.isTailLimb)
            {
                requiresBackwardStance = false;
                requiresForwardStance = false;
                requiresRightStance = true;
                requiresLeftStance = false;

                hasTorsoCommand = true;
                forwardInputTorsoCommand = "Butt Attack";
                backwardInputTorsoCommand = "Butt Attack"; //make sure monster flips before attacking
                upwardInputTorsoCommand = "Forward Attack";
                downwardInputTorsoCommand = "Lower Attack";

                hasHeadCommand = false;
                forwardInputHeadCommand = "";
                backwardInputHeadCommand = "";
                upwardInputHeadCommand = "Upward Attack";
                downwardInputHeadCommand = "Forward Attack";

                hasNeutralMovementCommand = true;
                forwardNeutralMovementCommand = "";
                upwardNeutralMovementCommand = "";
                backwardNeutralMovementCommand = "";
                downwardNeutralMovementCommand = "";

                hasHeavyMovementCommand = true;
                forwardHeavyMovementCommand = "";
                upwardHeavyMovementCommand = "";
                backwardHeavyMovementCommand = "";
                downwardHeavyMovementCommand = "";
            }
            else if (part.isRightEarLimb || part.isLeftEarLimb || part.isFacialLimb)
            {
                requiresBackwardStance = false;
                requiresForwardStance = false;
                requiresRightStance = true;
                requiresLeftStance = false;

                hasTorsoCommand = true;
                forwardInputTorsoCommand = "Forward Attack";
                backwardInputTorsoCommand = "Forward Attack"; //make sure monster flips before attacking
                upwardInputTorsoCommand = "Upper Attack";
                downwardInputTorsoCommand = "Downward Attack";

                hasHeadCommand = false;
                forwardInputHeadCommand = "Face Attack";
                backwardInputHeadCommand = "Face Attack";
                upwardInputHeadCommand = "Upward Attack";
                downwardInputHeadCommand = "Forward Attack";

                hasNeutralMovementCommand = true;
                forwardNeutralMovementCommand = "";
                upwardNeutralMovementCommand = "";
                backwardNeutralMovementCommand = "";
                downwardNeutralMovementCommand = "";

                hasHeavyMovementCommand = true;
                forwardHeavyMovementCommand = "";
                upwardHeavyMovementCommand = "";
                backwardHeavyMovementCommand = "";
                downwardHeavyMovementCommand = "";
            }
            else if (part.isTopHeadLimb)
            {
                requiresBackwardStance = false;
                requiresForwardStance = false;
                requiresRightStance = true;
                requiresLeftStance = false;

                hasTorsoCommand = true;
                forwardInputTorsoCommand = "Forward Attack";
                backwardInputTorsoCommand = "Forward Attack"; //make sure monster flips before attacking
                upwardInputTorsoCommand = "Upper Attack";
                downwardInputTorsoCommand = "Downward Attack";

                hasHeadCommand = false;
                forwardInputHeadCommand = "Forward Attack";
                backwardInputHeadCommand = "Forward Attack";
                upwardInputHeadCommand = "Upward Attack";
                downwardInputHeadCommand = "Forward Attack";

                hasNeutralMovementCommand = true;
                forwardNeutralMovementCommand = "";
                upwardNeutralMovementCommand = "";
                backwardNeutralMovementCommand = "";
                downwardNeutralMovementCommand = "";

                hasHeavyMovementCommand = true;
                forwardHeavyMovementCommand = "";
                upwardHeavyMovementCommand = "";
                backwardHeavyMovementCommand = "";
                downwardHeavyMovementCommand = "";
            }
            else if (part.isBacksideHeadLimb)
            {
                requiresBackwardStance = false;
                requiresForwardStance = false;
                requiresRightStance = true;
                requiresLeftStance = false;

                hasTorsoCommand = true;
                forwardInputTorsoCommand = "Lower Attack";
                backwardInputTorsoCommand = "Lower Attack"; //make sure monster flips before attacking
                upwardInputTorsoCommand = "Upper Attack";
                downwardInputTorsoCommand = "Lower Attack";

                hasHeadCommand = false;
                forwardInputHeadCommand = "Face Attack";
                backwardInputHeadCommand = "Face Attack";
                upwardInputHeadCommand = "Forward Attack";
                upwardInputHeadCommand = "Upward Attack";

                hasNeutralMovementCommand = true;
                forwardNeutralMovementCommand = "";
                upwardNeutralMovementCommand = "";
                backwardNeutralMovementCommand = "";
                downwardNeutralMovementCommand = "";

                hasHeavyMovementCommand = true;
                forwardHeavyMovementCommand = "";
                upwardHeavyMovementCommand = "";
                backwardHeavyMovementCommand = "";
                downwardHeavyMovementCommand = "";
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
                requiresBackwardStance = false;
                requiresForwardStance = false;
                requiresRightStance = true;
                requiresLeftStance = false;

                hasTorsoCommand = true;
                forwardInputTorsoCommand = "Forward Attack";
                backwardInputTorsoCommand = "Forward Attack"; //make sure monster flips before attacking
                upwardInputTorsoCommand = "Upper Attack";
                downwardInputTorsoCommand = "Downward Attack";

                hasHeadCommand = false;
                forwardInputHeadCommand = "";
                backwardInputHeadCommand = "";
                upwardInputHeadCommand = "Upward Attack";
                downwardInputHeadCommand = "Forward Attack";

                hasNeutralMovementCommand = true;
                forwardNeutralMovementCommand = "";
                upwardNeutralMovementCommand = "";
                backwardNeutralMovementCommand = "";
                downwardNeutralMovementCommand = "";

                hasHeavyMovementCommand = true;
                forwardHeavyMovementCommand = "";
                upwardHeavyMovementCommand = "";
                backwardHeavyMovementCommand = "";
                downwardHeavyMovementCommand = "";

            }
            else if (part.isRightPelvisLimb || part.isLeftPelvisLimb || part.isBellyLimb || part.isShoulderBladeLimb)
            {
                requiresBackwardStance = false;
                requiresForwardStance = false;
                requiresRightStance = true;
                requiresLeftStance = false;

                hasTorsoCommand = true;
                forwardInputTorsoCommand = "Lower Attack";
                backwardInputTorsoCommand = "Lower Attack"; //make sure monster flips before attacking
                upwardInputTorsoCommand = "Lower Attack";
                downwardInputTorsoCommand = "Lower Attack";

                hasHeadCommand = false;
                forwardInputHeadCommand = "";
                backwardInputHeadCommand = "";
                upwardInputHeadCommand = "Upward Attack";
                downwardInputHeadCommand = "Forward Attack";

                hasNeutralMovementCommand = true;
                forwardNeutralMovementCommand = "";
                upwardNeutralMovementCommand = "";
                backwardNeutralMovementCommand = "";
                downwardNeutralMovementCommand = "";

                hasHeavyMovementCommand = true;
                forwardHeavyMovementCommand = "";
                upwardHeavyMovementCommand = "";
                backwardHeavyMovementCommand = "";
                downwardHeavyMovementCommand = "";
            }
            else if (part.isTailLimb)
            {
                requiresBackwardStance = false;
                requiresForwardStance = false;
                requiresRightStance = true;
                requiresLeftStance = false;

                hasTorsoCommand = true;
                forwardInputTorsoCommand = "Butt Attack";
                backwardInputTorsoCommand = "Butt Attack"; //make sure monster flips before attacking
                upwardInputTorsoCommand = "Forward Attack";
                downwardInputTorsoCommand = "Lower Attack";

                hasHeadCommand = false;
                forwardInputHeadCommand = "";
                backwardInputHeadCommand = "";
                upwardInputHeadCommand = "Upward Attack";
                downwardInputHeadCommand = "Forward Attack";

                hasNeutralMovementCommand = true;
                forwardNeutralMovementCommand = "";
                upwardNeutralMovementCommand = "";
                backwardNeutralMovementCommand = "";
                downwardNeutralMovementCommand = "";

                hasHeavyMovementCommand = true;
                forwardHeavyMovementCommand = "";
                upwardHeavyMovementCommand = "";
                backwardHeavyMovementCommand = "";
                downwardHeavyMovementCommand = "";
            }
            else if (part.isRightEarLimb || part.isLeftEarLimb || part.isFacialLimb)
            {
                requiresBackwardStance = false;
                requiresForwardStance = false;
                requiresRightStance = true;
                requiresLeftStance = false;

                hasTorsoCommand = true;
                forwardInputTorsoCommand = "Forward Attack";
                backwardInputTorsoCommand = "Forward Attack"; //make sure monster flips before attacking
                upwardInputTorsoCommand = "Upper Attack";
                downwardInputTorsoCommand = "Downward Attack";

                hasHeadCommand = false;
                forwardInputHeadCommand = "Face Attack";
                backwardInputHeadCommand = "Face Attack";
                upwardInputHeadCommand = "Upward Attack";
                downwardInputHeadCommand = "Forward Attack";

                hasNeutralMovementCommand = true;
                forwardNeutralMovementCommand = "";
                upwardNeutralMovementCommand = "";
                backwardNeutralMovementCommand = "";
                downwardNeutralMovementCommand = "";

                hasHeavyMovementCommand = true;
                forwardHeavyMovementCommand = "";
                upwardHeavyMovementCommand = "";
                backwardHeavyMovementCommand = "";
                downwardHeavyMovementCommand = "";
            }
            else if (part.isTopHeadLimb)
            {
                requiresBackwardStance = false;
                requiresForwardStance = false;
                requiresRightStance = true;
                requiresLeftStance = false;

                hasTorsoCommand = true;
                forwardInputTorsoCommand = "Forward Attack";
                backwardInputTorsoCommand = "Forward Attack"; //make sure monster flips before attacking
                upwardInputTorsoCommand = "Upper Attack";
                downwardInputTorsoCommand = "Downward Attack";

                hasHeadCommand = false;
                forwardInputHeadCommand = "Forward Attack";
                backwardInputHeadCommand = "Forward Attack";
                upwardInputHeadCommand = "Upward Attack";
                downwardInputHeadCommand = "Forward Attack";

                hasNeutralMovementCommand = true;
                forwardNeutralMovementCommand = "";
                upwardNeutralMovementCommand = "";
                backwardNeutralMovementCommand = "";
                downwardNeutralMovementCommand = "";

                hasHeavyMovementCommand = true;
                forwardHeavyMovementCommand = "";
                upwardHeavyMovementCommand = "";
                backwardHeavyMovementCommand = "";
                downwardHeavyMovementCommand = "";
            }
            else if (part.isBacksideHeadLimb)
            {
                requiresBackwardStance = false;
                requiresForwardStance = false;
                requiresRightStance = true;
                requiresLeftStance = false;

                hasTorsoCommand = true;
                forwardInputTorsoCommand = "Lower Attack";
                backwardInputTorsoCommand = "Lower Attack"; //make sure monster flips before attacking
                upwardInputTorsoCommand = "Upper Attack";
                downwardInputTorsoCommand = "Lower Attack";

                hasHeadCommand = false;
                forwardInputHeadCommand = "Face Attack";
                backwardInputHeadCommand = "Face Attack";
                upwardInputHeadCommand = "Forward Attack";
                upwardInputHeadCommand = "Upward Attack";

                hasNeutralMovementCommand = true;
                forwardNeutralMovementCommand = "";
                upwardNeutralMovementCommand = "";
                backwardNeutralMovementCommand = "";
                downwardNeutralMovementCommand = "";

                hasHeavyMovementCommand = true;
                forwardHeavyMovementCommand = "";
                upwardHeavyMovementCommand = "";
                backwardHeavyMovementCommand = "";
                downwardHeavyMovementCommand = "";
            }

        }
        #endregion
    }

    private void LoadConfigs()
    {
        TextAsset jsonText = Resources.Load<TextAsset>("Data/attack_configs");

        if (jsonText == null)
        {
            Debug.LogError("Could not load attack_configs.json");
            return;
        }

        AttackConfigList configList = JsonUtility.FromJson<AttackConfigList>(jsonText.text);
        configMap = configList.configs.ToDictionary(config => config.ConfigName);
    }

    private void ApplyConfig(string configName)
    {
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
