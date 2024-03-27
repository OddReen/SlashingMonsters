using UnityEngine;

public class AnimationEventHandler : MonoBehaviour
{
    WeaponHit weaponHit;
    private void Start()
    {
        weaponHit = GetComponentInChildren<WeaponHit>();
    }
    public void EnableHitBox()
    {
        weaponHit.EnableHitBox();
    }
    public void DisableHitBox()
    {
        weaponHit.DisableHitBox();
    }
}