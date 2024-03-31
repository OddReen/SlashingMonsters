using UnityEngine;

public class AnimationEventHandler : MonoBehaviour
{
    HitBox_Weapon hitBox_Weapon;
    HitBox_Kick hitBox_Kick;
    private void Start()
    {
        hitBox_Kick = GetComponentInChildren<HitBox_Kick>();
        hitBox_Weapon = GetComponentInChildren<HitBox_Weapon>();
    }
    public void Weapon_EnableHitBox()
    {
        hitBox_Weapon.EnableHitBox();
    }
    public void Weapon_DisableHitBox()
    {
        hitBox_Weapon.DisableHitBox();
    }
    public void Kick_EnableHitBox()
    {
        hitBox_Kick.EnableHitBox();
    }
    public void Kick_DisableHitBox()
    {
        hitBox_Kick.DisableHitBox();
    }
}