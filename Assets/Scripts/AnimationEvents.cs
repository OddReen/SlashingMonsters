using UnityEngine;

public class AnimationEventHandler : MonoBehaviour
{
    [SerializeField] HitBox hitBox_Weapon;
    [SerializeField] HitBox hitBox_Kick;
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