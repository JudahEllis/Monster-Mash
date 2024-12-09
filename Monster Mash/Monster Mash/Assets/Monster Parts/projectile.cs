using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectile : MonoBehaviour
{
    [Header("VFX and Objects")]
    public GameObject mainVisual;
    public GameObject trailVisual;
    public GameObject impactVisual;
    public Collider baseCollider;
    public Transform projectileHolder;
    [Header("Main VFX Properties")]
    public float speed = 10f;
    public float timeFromImpactToDisable = 2;
    public float timeUntilAutoDisable = 20;
    private int movementModifier = 1;
    public bool fadesAway;
    [Header("Additional VFX Properties")]
    public bool isCenterOfSpray;
    public bool needsUpwardDraft;
    public float timeUntilUpwardDraft = 0;
    public float upwardDraftRotationSpeed = 0;
    public Quaternion leftRotation;
    public Quaternion rightRotation;

    [Header("Data")]
    public bool facingRight;
    private bool upwardDraftActivated = false;
    private Quaternion intendedRotation;
    private Vector3 startingPoint = new Vector3(0, 0, 0);
    private Vector3 startingSize = new Vector3(1, 1, 1);
    private Quaternion startingRotation;
    private bool impactCalled;

    [Header("Boomerang")]
    [SerializeField] public bool needsReload = false;
    [HideInInspector] public bool isBoomerang = false;
    [HideInInspector] public float height;
    [HideInInspector] public float distance;
    [HideInInspector] public float autoEnd;
    [HideInInspector] public float turnAround = 0f;
    [SerializeField] private bool isReloaded = false;
    [HideInInspector] public int neutralOrHeavy;
    private monsterPart myMonsterPart;
    private bool hasRequiredAsset = false;
    private GameObject requiredAsset;


    private bool keepGoing = true;
    private Vector3 homeStretch;
    private bool boomStart = true;
    private bool boomEnd = false;

    public void resetPosition()
    {
        transform.parent = projectileHolder;
        transform.localPosition = startingPoint;
        transform.localScale = startingSize;
        transform.localRotation = startingRotation;

        if (facingRight)
        {
            intendedRotation = rightRotation;
        }
        else
        {
            intendedRotation = leftRotation;
        }
    }

    private void OnEnable()
    {
        isReloaded = false;
        keepGoing = false;
        StopAllCoroutines();
        movementModifier = 1;
        impactCalled = false;
        upwardDraftActivated = false;
        startingRotation = transform.localRotation;
        if (mainVisual != null)
        {
            mainVisual.SetActive(true);
        }
        if (impactVisual != null)
        {
            impactVisual.SetActive(false);
        }
        if (trailVisual != null)
        {
            trailVisual.SetActive(true);
        }
        baseCollider.enabled = true;
        /*
        if (trailVisual.GetComponent<ParticleSystem>() != null)
        {
            trailVisual.GetComponent<ParticleSystem>().Play();
        }
        */
        resetPosition();
        updateVelocity();

        if (!isBoomerang)
        {
            StartCoroutine(autoDisable());
            StartCoroutine(updraftTimer());
        }
        else
        {
            boomStart = true;
            boomEnd = false;
        }

        if (needsReload)
        {
            myMonsterPart = projectileHolder.gameObject.GetComponent<vfxHolder>().myMonsterPart;

            hasRequiredAsset = projectileHolder.GetComponent<vfxHolder>().GetHasRequiredAsset();

            if (neutralOrHeavy == 0) //is neutral attack
            {
                myMonsterPart.SetReloadNeutral(false);
            }
            else if (neutralOrHeavy == 1) //is heavy attack
            {
                myMonsterPart.SetReloadHeavy(false);
            }
        }
    }

    private void updateVelocity()
    {
        transform.parent = null;
        GetComponent<Rigidbody>().velocity = this.transform.forward * speed * movementModifier;
    }
    public void impact()
    {
        if (!keepGoing)
        {
            StartCoroutine(impactEffect());
        }
        else
        {
            BoomerangTurnaround();
        }
    }

    IEnumerator impactEffect()
    {
        if (!keepGoing)
        {
            movementModifier = 0;
            upwardDraftActivated = false;
            updateVelocity();
            if (mainVisual != null && fadesAway == false)
            {
                mainVisual.SetActive(false);
                print("main visual off 1");

                if (needsReload)
                {
                    projectileHolder.gameObject.GetComponent<vfxHolder>().isReloaded = true;

                    if (neutralOrHeavy == 0) //is neutral attack
                    {
                        myMonsterPart.SetReloadNeutral(true);
                    }
                    else if (neutralOrHeavy == 1) //is heavy attack
                    {
                        myMonsterPart.SetReloadHeavy(true);
                    }
                }

                isReloaded = true;
            }
            else
            {
                if (mainVisual.GetComponent<ParticleSystem>() != null)
                {
                    mainVisual.GetComponent<ParticleSystem>().Stop();
                }
            }
        }

        if (impactVisual != null)
        {
            impactVisual.SetActive(true);
            
            if (impactVisual.GetComponent<ParticleSystem>() != null)
            {
                impactVisual.GetComponent<ParticleSystem>().Play();

                if (keepGoing)
                {
                    yield return new WaitForSeconds(1f);

                    impactVisual.GetComponent<ParticleSystem>().Stop();
                    impactVisual.SetActive(false);

                    yield break;
                }
            }
            
        }

        if (!keepGoing)
        {

            baseCollider.enabled = false;
            if (trailVisual != null)
            {
                if (trailVisual.GetComponent<ParticleSystem>() != null)
                {
                    trailVisual.GetComponent<ParticleSystem>().Stop();
                }
            }
            yield return new WaitForSeconds(timeFromImpactToDisable);
            if (impactVisual != null)
            {
                impactVisual.SetActive(false);

            }

            if (trailVisual != null)
            {
                trailVisual.SetActive(false);
            }
            resetPosition();
            yield return new WaitForSeconds(1);

            this.gameObject.SetActive(false);
        }
    }

    IEnumerator autoDisable()
    {
        yield return new WaitForSeconds(timeUntilAutoDisable);
        if (impactCalled == false)
        {
            impact();
        }
    }

    public void Update()
    {
        if (upwardDraftActivated && impactCalled == false)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, intendedRotation, upwardDraftRotationSpeed * Time.deltaTime);
            //transform.Rotate(-Vector3.forward * 100 * Time.deltaTime, Space.World);
            //transform.rotation = Quaternion.Lerp(transform.rotation, rotation, upwardDraftRotationSpeed * Time.deltaTime);
            if (transform.rotation.x == intendedRotation.x)
            {
                upwardDraftActivated = false;
            }

            updateVelocity();
        }

        if (hasRequiredAsset)
        {
            projectileHolder.GetComponent<vfxHolder>().SetBoomerangAsset(isReloaded);
        }
    }

    IEnumerator updraftTimer()
    {
        yield return new WaitForSeconds(timeUntilUpwardDraft);
        if (needsUpwardDraft)
        {
            upwardDraftActivated = true;
        }
    }

    private void FixedUpdate()
    {
        if (isBoomerang)
        {
            if (boomStart)
            {
                projectileHolder.gameObject.GetComponent<vfxHolder>().isReloaded = false;

                keepGoing = true;
                
                GetComponent<Rigidbody>().velocity = (transform.forward + new Vector3(0f, height)) * speed * movementModifier;

                if (Vector2.Distance(startingPoint, transform.position) >= distance)
                {
                    BoomerangTurnaround();
                }
            }

            if (boomEnd)
            {
                //print(Vector2.Distance(homeStretch, transform.position));

                if (Vector2.Distance(homeStretch, transform.position) < 2)
                {
                    boomEnd = false;
                    keepGoing = false;
                    impactCalled = false;
                    //print("here we go boys");
                    StartCoroutine(autoDisable());
                }
            }
        }
    }

    private void BoomerangTurnaround()
    {
        boomStart = false;
        StartCoroutine(Boomerang());
        StartCoroutine(BoomerangEnd());
    }
    IEnumerator Boomerang()
    {
        GetComponent<Rigidbody>().velocity *= 0f;

        yield return new WaitForSeconds(turnAround);

        homeStretch = projectileHolder.position;

        Vector2 direction = (homeStretch - transform.position).normalized;

        GetComponent<Rigidbody>().velocity = direction * speed;

        boomEnd = true;

        yield break;
    }

    IEnumerator BoomerangEnd()
    {
        yield return new WaitForSeconds(autoEnd);

        keepGoing = false;
        impactCalled = false;
        boomEnd = false;

        StartCoroutine(autoDisable());

        yield break;
    }

    public bool GetReload()
    {
        return isReloaded;
    }

    public void SetReload(bool reload)
    {
        isReloaded = reload;
    }
}
