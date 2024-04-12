using FMOD;
using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootSteps : MonoBehaviour
{
    public EventReference footStep;
    EventInstance footStepInstance;

    public void FootStep()
    {
        footStepInstance = RuntimeManager.CreateInstance(footStep);
        RuntimeManager.AttachInstanceToGameObject(footStepInstance, transform, GetComponent<Rigidbody>());
        footStepInstance.start();
    }
}