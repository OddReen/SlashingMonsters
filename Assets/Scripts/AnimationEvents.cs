using UnityEngine;
using FMODUnity;

public class AnimationEventHandler : MonoBehaviour
{
    [SerializeField] Transform kickSlot;
    [SerializeField] Transform weaponSlot;
    [SerializeField] Transform throwableSlot;
    [SerializeField] FootSteps footSteps;
    public void Throw()
    {
        GetComponentInParent<Character_Throw>().Throw(throwableSlot);
    }
    public void Weapon_EnableHitBox()
    {
        weaponSlot.GetComponentInChildren<Weapon>().EnableHitBox();
        GetComponentInParent<CharacterBehaviour>().atackingState = CharacterBehaviour.AttackingState.Attacking;
    }
    public void Weapon_DisableHitBox()
    {
        weaponSlot.GetComponentInChildren<Weapon>().DisableHitBox();
        GetComponentInParent<CharacterBehaviour>().atackingState = CharacterBehaviour.AttackingState.Ending;
    }
    public void Kick_EnableHitBox()
    {
        //kickSlot.GetComponentInChildren<HitBox_Melee>().EnableHitBox();
        GetComponentInParent<CharacterBehaviour>().atackingState = CharacterBehaviour.AttackingState.Attacking;
    }
    public void Kick_DisableHitBox()
    {
        //kickSlot.GetComponentInChildren<HitBox_Melee>().DisableHitBox();
        GetComponentInParent<CharacterBehaviour>().atackingState = CharacterBehaviour.AttackingState.Ending;
    }
    public void FootStep()
    {
        footSteps.FootStep();
    }
}