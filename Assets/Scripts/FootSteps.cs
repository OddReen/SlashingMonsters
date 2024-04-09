using FMOD;
using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Profiling.RawFrameDataView;
using static UnityEngine.Rendering.DebugUI;

public class FootSteps : MonoBehaviour
{
    CharacterBehaviour_Player characterBehaviour_Player;

    public EventReference footStep;
    EventInstance footStepInstance;

    private void Start()
    {
        characterBehaviour_Player = GetComponent<CharacterBehaviour_Player>();
    }
    public void FootStep()
    {
        //if (characterBehaviour_Player.player_Movement.currentSpeed <= characterBehaviour_Player.player_Movement.walkingSpeed)
        //{
        //    footStepInstance.setParameterByName("run", 0);
        //}
        //else
        //{
        //    footStepInstance.setParameterByName("run", 1);
        //}
        footStepInstance = RuntimeManager.CreateInstance(footStep);
        RuntimeManager.AttachInstanceToGameObject(footStepInstance, transform, GetComponent<Rigidbody>());
        footStepInstance.start();
        //RuntimeManager.PlayOneShot(footStep, transform.position);
    }
}