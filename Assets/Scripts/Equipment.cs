using System;
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
    public void EquipThrowable(Transform gameObject)
    {
        gameObject.GetComponent<Throwable>().canInteract = false;
        characterBehaviour_Player.hasThrowable = true;
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        gameObject.GetComponent<Collider>().enabled = false;
        gameObject.position = throwableSlot.position;
        gameObject.rotation = throwableSlot.rotation;
        gameObject.SetParent(throwableSlot);
    }
    public void EquipWeapon(Transform gameObject)
    {
        GameObject parent = gameObject.GetComponentInParent<CharacterBehaviour>().gameObject;
        gameObject.GetComponent<Weapon>().canInteract = false;
        gameObject.GetComponent<Weapon>().targetTypeTag = "Enemy";
        gameObject.GetComponentInChildren<Animator>().applyRootMotion = false;
        gameObject.GetComponentInChildren<Animator>().Play("isWeapon");
        //gameObject.GetComponentInChildren<Animator>().enabled = false;
        characterBehaviour_Player.hasWeapon = true;
        gameObject.GetComponent<Collider>().enabled = false;
        gameObject.position = weaponSlot.position;
        gameObject.rotation = weaponSlot.rotation;
        gameObject.SetParent(weaponSlot);
        Destroy(parent);
    }
}
