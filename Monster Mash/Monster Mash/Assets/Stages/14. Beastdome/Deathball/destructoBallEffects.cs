using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destructoBallEffects : MonoBehaviour
{

    public Animation[] jumpers;
    #region Jumpers
    public void playJumper1()
    {
        jumpers[0].Play();
    }

    public void playJumper2()
    {
        jumpers[1].Play();
    }

    public void playJumper3()
    {
        jumpers[2].Play();
    }

    public void playJumper4()
    {
        jumpers[3].Play();
    }

    #endregion

    public ParticleSystem dirtEffect;
    #region Dirt Effects

    public void playDirtEffect()
    {
        dirtEffect.Stop();
        dirtEffect.Play();
    }

    public void stopDirtEffect()
    {
        dirtEffect.Stop();
    }

    #endregion

    public ParticleSystem leftMovingSparks;
    public ParticleSystem rightMovingSparks;
    public ParticleSystem leftSparkImpact;
    public ParticleSystem rightSparkImpact;
    public ParticleSystem bottomSparkImpact;
    #region Spark Effects
    public void playLeftMovingSparks()
    {
        rightMovingSparks.Stop();
        leftMovingSparks.Play();
    }

    public void playRightMovingSparks()
    {
        leftMovingSparks.Stop();
        rightMovingSparks.Play();
    }
    public void stopMovingSparks()
    {
        rightMovingSparks.Stop();
        leftMovingSparks.Stop();
    }

    public void playLeftSparkImpact()
    {
        leftSparkImpact.Stop();
        leftSparkImpact.Play();
    }

    public void playRightSparkImpact()
    {
        rightSparkImpact.Stop();
        rightSparkImpact.Play();
    }

    public void playBottomSparkImpact()
    {
        bottomSparkImpact.Stop();
        bottomSparkImpact.Play();
    }
    #endregion

}
