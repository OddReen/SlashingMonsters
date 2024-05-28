using UnityEngine;

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
        CharacterBehaviour characterBehaviour = GetComponentInParent<CharacterBehaviour>();
        if (characterBehaviour != null)
        {
            characterBehaviour.atackingState = CharacterBehaviour.AttackingState.Attacking;
        }
    }
    public void Weapon_DisableHitBox()
    {
        if (weaponSlot.GetComponentInChildren<Weapon>() != null)
        {
            weaponSlot.GetComponentInChildren<Weapon>().DisableHitBox();
        }
        CharacterBehaviour characterBehaviour = GetComponentInParent<CharacterBehaviour>();
        if (characterBehaviour != null)
        {
            characterBehaviour.atackingState = CharacterBehaviour.AttackingState.Ending;
        }
    }
    public void Kick_EnableHitBox()
    {
        kickSlot.GetComponentInChildren<Weapon>().EnableHitBox();
        CharacterBehaviour characterBehaviour = GetComponentInParent<CharacterBehaviour>();
        if (characterBehaviour != null)
        {
            characterBehaviour.atackingState = CharacterBehaviour.AttackingState.Attacking;
        }
    }
    public void Kick_DisableHitBox()
    {
        kickSlot.GetComponentInChildren<Weapon>().DisableHitBox();
        CharacterBehaviour characterBehaviour = GetComponentInParent<CharacterBehaviour>();
        if (characterBehaviour != null)
        {
            characterBehaviour.atackingState = CharacterBehaviour.AttackingState.Ending;
        }
    }
    public void FootStep()
    {
        footSteps.FootStep();
    }
}