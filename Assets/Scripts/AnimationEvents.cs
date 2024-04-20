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
        weaponSlot.GetComponentInChildren<HitBox>().EnableHitBox();
    }
    public void Weapon_DisableHitBox()
    {
        weaponSlot.GetComponentInChildren<HitBox>().DisableHitBox();
    }
    public void Kick_EnableHitBox()
    {
        kickSlot.GetComponentInChildren<HitBox>().EnableHitBox();
    }
    public void Kick_DisableHitBox()
    {
        kickSlot.GetComponentInChildren<HitBox>().DisableHitBox();
    }
    public void FootStep()
    {
        footSteps.FootStep();
    }
}