using FMOD;
using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sounds : MonoBehaviour
{
    public EventReference footStep;
    EventInstance footStepInstance;

    public EventReference hurt;
    EventInstance hurtInstance;

    public EventReference noticed;
    EventInstance noticedInstance;

    public EventReference swing;
    EventInstance swingInstance;

    public void Swing()
    {
        swingInstance = RuntimeManager.CreateInstance(swing);
        RuntimeManager.AttachInstanceToGameObject(swingInstance, transform, GetComponent<Rigidbody>());
        swingInstance.start();
    }
    public void FootStep()
    {
        footStepInstance = RuntimeManager.CreateInstance(footStep);
        RuntimeManager.AttachInstanceToGameObject(footStepInstance, transform, GetComponent<Rigidbody>());
        footStepInstance.start();
    }
    public void Hurt()
    {
        hurtInstance = RuntimeManager.CreateInstance(hurt);
        RuntimeManager.AttachInstanceToGameObject(hurtInstance, transform, GetComponent<Rigidbody>());
        hurtInstance.start();
    }
    public void Noticed()
    {
        noticedInstance = RuntimeManager.CreateInstance(noticed);
        RuntimeManager.AttachInstanceToGameObject(noticedInstance, transform, GetComponent<Rigidbody>());
        noticedInstance.start();
    }
}