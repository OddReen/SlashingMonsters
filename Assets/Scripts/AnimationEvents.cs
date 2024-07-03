using UnityEngine;

public class AnimationEventHandler : MonoBehaviour
{
    [SerializeField] Transform kickSlot;
    [SerializeField] Transform weaponSlot;
    [SerializeField] Transform throwableSlot;
    [SerializeField] Sounds sounds;
    public void Throw()
    {
        GetComponentInParent<Character_Throw>().Throw(throwableSlot);
    }
    public void Interact()
    {
        GetComponentInParent<Character_Interact>().Interact();
    }
    public void Weapon_EnableHitBox()
    {
        weaponSlot.GetComponentInChildren<Weapon>().EnableHitBox();
        CharacterBehaviour characterBehaviour = GetComponentInParent<CharacterBehaviour>();
        if (characterBehaviour != null)
        {
            characterBehaviour.atackingState = CharacterBehaviour.AtackingState.Attacking;
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
            characterBehaviour.atackingState = CharacterBehaviour.AtackingState.Ending;
        }
    }
    public void Kick_EnableHitBox()
    {
        kickSlot.GetComponentInChildren<Weapon>().EnableHitBox();
        CharacterBehaviour characterBehaviour = GetComponentInParent<CharacterBehaviour>();
        if (characterBehaviour != null)
        {
            characterBehaviour.atackingState = CharacterBehaviour.AtackingState.Attacking;
        }
    }
    public void Kick_DisableHitBox()
    {
        kickSlot.GetComponentInChildren<Weapon>().DisableHitBox();
        CharacterBehaviour characterBehaviour = GetComponentInParent<CharacterBehaviour>();
        if (characterBehaviour != null)
        {
            characterBehaviour.atackingState = CharacterBehaviour.AtackingState.Ending;
        }
    }
    public void FootStep()
    {
        sounds.FootStep();
    }
    public void Hurt()
    {
        sounds.FootStep();
    }
    public void Noticed()
    {
        sounds.FootStep();
    }
}