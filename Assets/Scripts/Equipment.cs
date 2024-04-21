using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    CharacterBehaviour_Player characterBehaviour_Player;
    [SerializeField] Transform throwableSlot;
    [SerializeField] Transform weaponSlot;
    private void Start()
    {
        characterBehaviour_Player = GetComponent<CharacterBehaviour_Player>();
    }
    public void EquipRight(Transform gameObject)
    {
        characterBehaviour_Player.hasThrowable = true;
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        gameObject.GetComponent<Collider>().enabled = false;
        gameObject.position = throwableSlot.position;
        gameObject.rotation = throwableSlot.rotation;
        gameObject.SetParent(throwableSlot);
    }
    public void EquipLeft(Transform gameObject)
    {
        characterBehaviour_Player.hasWeapon = true;
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        gameObject.GetComponent<Collider>().enabled = false;
        gameObject.position = weaponSlot.position;
        gameObject.rotation = weaponSlot.rotation;
        gameObject.SetParent(weaponSlot);
    }
}
