using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class monsterPartReference : MonoBehaviour
{
    public monsterAttackSystem mainSystem;
    public monsterPart partReference;
    public List<monsterPartReference> referencesToIgnore = new List<monsterPartReference>();
    private List<monsterAttackSystem> attackHistory = new List<monsterAttackSystem>();
    public bool isHitbox;
    public bool isHurtbox;
    public bool isStomp;
    public bool isProjectile;
    public bool isJabOrSlash;
    public bool isReel;
    public bool isGrapple;
    public bool isBoomerang;
    public bool isChargingJab;

    #region Box Cast vars
    private enum orientation
    {
        Forward,
        Back,
        Left,
        Right,
        Up,
        Down
    }

    /// <summary>
    /// Maps an orientation enum to the corisponding vector
    /// </summary>
    private Dictionary<orientation, Vector3> orientationToVector;

    [Header("Box Cast Positioning Info")]
    [SerializeField] private Vector3 boxSize;
    [SerializeField] private Vector3 boxRotation;
    [Tooltip("Please note that the max distance is not where the box cast is curentely located. When activated the box cast will start at the begining of the ray and travel to the end of the ray, " +
        "the location of the box gizmo indicates the ending position of the box cast.")]
    [SerializeField] private float boxMaxDistance;
    [Tooltip("Orientations are in world space so the selected orientation label might not be corect depending on the objects rotation. " +
        "The orientation will still work but may be reversed, just pick the orientation that puts the box cast in the direction you want and ignore the labels if this happens, you can also use negative values as a work around.")]
    [SerializeField] private orientation boxOrientation;
    [SerializeField] private bool showDebug;

    #endregion

    [Header("Damage, Direction, and Status Effects")]
    public int damage = 0;
    public bool hasTickDamage;
    public int tickDamage = 0;
    public float tickTiming = 0;
    public float tickDamageDuration = 0;
    public bool markedHeavy;
    public bool isFullyChargedHeavy;
    public int directionOfAttack = 1;
    private Vector3 pointOfContact;
    public bool hasStatusEffect;
    public bool burnedStatusEffect;
    public bool electrifiedStatusEffect;
    public bool poisonedStatusEffect;
    public bool stinkyStatusEffect;
    public bool cursedStatusEffect;
    public bool confusedStatusEffect;
    public bool slimedStatusEffect;
    public bool frozenStatusEffect;
    public bool squashedStatusEffect;
    public bool slowedStatusEffect;
    public bool grabbedStatusEffect;


    private void Awake()
    {
        orientationToVector = new Dictionary<orientation, Vector3>
        {
            {orientation.Forward, transform.forward },
            {orientation.Back, -transform.forward },
            {orientation.Left, -transform.right },
            {orientation.Right, transform.right },
            {orientation.Up, transform.up },
            {orientation.Down, -transform.up }
        };
    }

    public void resetAttackHistory()
    {
        attackHistory.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isHitbox)
        {
            if (other.GetComponent<monsterPartReference>() != null)
            {
                monsterAttackSystem damagedMonster = other.GetComponent<monsterPartReference>().mainSystem;

                if (damagedMonster != null)
                {
                    if (attackHistory.Contains(damagedMonster) == false && damagedMonster != mainSystem && damagedMonster.myPlayer != null)
                    {
                        //attack
                        pointOfContact = other.ClosestPointOnBounds(transform.position);
                        damagedMonster.myPlayer.damaged(damage, markedHeavy, directionOfAttack, pointOfContact);

                        if (hasStatusEffect && markedHeavy)
                        {
                            damagedMonster.myPlayer.statusGiven(burnedStatusEffect, electrifiedStatusEffect, confusedStatusEffect, stinkyStatusEffect, cursedStatusEffect, slowedStatusEffect,
                                                                poisonedStatusEffect, frozenStatusEffect, slimedStatusEffect, tickDamage, tickTiming, tickDamageDuration);
                        }

                        #region VFX & Misc Response to Successful Hit
                        if (isProjectile || isBoomerang)
                        {
                            GetComponent<projectile>().impact();
                        }

                        if (isJabOrSlash)
                        {
                            partReference.triggerJabOrSlashHitDetect();
                            mainSystem.myPlayer.forceStopLeap();

                            if (isChargingJab && isFullyChargedHeavy)
                            {
                                mainSystem.myPlayer.endChargeForward();
                                partReference.endChargeForward();
                                isFullyChargedHeavy = false;
                            }
                        }

                        if (isGrapple)
                        {
                            //mainSystem.myPlayer.leapAttackForward();

                            if (directionOfAttack == 1 || directionOfAttack == -1)
                            {
                                //grapple into a body smash
                                mainSystem.myPlayer.playerGrapple(damagedMonster.myPlayer);
                            }
                            else if (directionOfAttack == 2)
                            {
                                //grapple into a body smash upwards
                            }
                            else if (directionOfAttack == 0)
                            {
                                //grapple into a body smash & jump
                            }
                        }
                        #endregion

                        attackHistory.Add(damagedMonster);
                    }
                    else
                    {
                        return;
                    }
                }
            }
        }

        if (other.gameObject.tag == "Solid")
        {
            pointOfContact = other.ClosestPointOnBounds(transform.position);

            if (isGrapple)
            {
                //mainSystem.myPlayer.leapAttackForward();

                if (directionOfAttack == 1 || directionOfAttack == -1)
                {
                    //mainSystem.myPlayer.leftRightGrapple(false, pointOfContact);
                    //grapple into wall
                    mainSystem.myPlayer.wallGrapple(pointOfContact);
                    print("wall detected");
                }
                else if (directionOfAttack == 2)
                {
                    //grapple into ceiling
                }
                else if (directionOfAttack == 0)
                {
                    //grapple into floor
                }
            }

            if (isJabOrSlash)
            {
                partReference.triggerJabOrSlashHitDetect();
                mainSystem.myPlayer.forceStopLeap();

                if (isChargingJab && isFullyChargedHeavy)
                {
                    mainSystem.myPlayer.endChargeForward();
                    partReference.endChargeForward();
                    isFullyChargedHeavy = false;
                }
            }
        }

        if (other.GetComponent<collisionMaterial>() != null)
        {
            if (isStomp)
            {
                Vector3 storedCollisionPoint = other.ClosestPoint(transform.position);
                other.GetComponent<collisionMaterial>().spawnVFX(storedCollisionPoint);
                partReference.triggerStompDetectionOff();
            }
        }
    }

    private void TriggerBoxCast()
    {
        Vector3 orientation;
        orientationToVector.TryGetValue(boxOrientation, out orientation);

        RaycastHit hitinfo;
        if (Physics.BoxCast(transform.position, boxSize / 2, transform.TransformDirection(orientation), out hitinfo, Quaternion.Euler(boxRotation), boxMaxDistance))
        {
            Debug.Log(hitinfo.transform.name);
        }
    }

    private void OnDrawGizmos()
    {
        if (!showDebug) { return; }

        // OnDrawGizmos is run in the editor so we need to call awake manualy
        if (orientationToVector == null)
        {
            Awake();
        }
        

        Vector3 orientation;
        orientationToVector.TryGetValue(boxOrientation, out orientation);

        Gizmos.color = Color.red;

        Gizmos.DrawRay(transform.position, transform.TransformDirection(orientation) * boxMaxDistance);

        // Rotating a gizmo is not straightforward so we need to apply the rotation to the transform matrix
        Vector3 position = transform.position + transform.TransformDirection(orientation) * boxMaxDistance;
        Quaternion rotation = Quaternion.Euler(boxRotation);
        Gizmos.matrix = Matrix4x4.TRS(position, rotation, Vector3.one);

        Gizmos.DrawWireCube(Vector3.zero, boxSize);
    }

}
