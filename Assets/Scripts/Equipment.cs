using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    CharacterBehaviour_Player characterBehaviour_Player;
    [SerializeField] Transform throwableSlot;
    [SerializeField] Transform weaponSlot;

    [Header("Weapon Durability Bar")]
    [SerializeField] public GameObject durabilityBarBackground;
    [SerializeField] public GameObject durabilityBarLoss;
    [SerializeField] public GameObject durabilityBar;
    [SerializeField] public float durabilityBarLossSpeed = 1;
    [SerializeField] public float durabilityBarLossAmount = 1;
    [SerializeField] public float durabilityBarLossTime = 2;

    private void Start()
    {
        characterBehaviour_Player = GetComponent<CharacterBehaviour_Player>();
    }
    public void EquipThrowable(GameObject gameObject)
    {
        gameObject.GetComponent<Throwable>().canInteract = false;
        characterBehaviour_Player.hasThrowable = true;
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        gameObject.GetComponent<Collider>().enabled = false;
        gameObject.transform.position = throwableSlot.position;
        gameObject.transform.rotation = throwableSlot.rotation;
        gameObject.transform.SetParent(throwableSlot);
    }
    public void EquipWeapon(GameObject weaponGameObject)
    {
        durabilityBarBackground.SetActive(true);

        GameObject parent = weaponGameObject.GetComponentInParent<CharacterBehaviour>().gameObject;
        Weapon weapon = weaponGameObject.GetComponent<Weapon>();
        Animator animator = weaponGameObject.GetComponentInChildren<Animator>();
        weapon.equipment = this;

        weapon.canInteract = false;
        weapon.targetTypeTag = "Enemy";
        weapon.DurabilityBarUpdate();


        animator.applyRootMotion = false;
        animator.Play("isWeapon");

        characterBehaviour_Player.hasWeapon = true;
        weaponGameObject.GetComponent<Collider>().enabled = false;

        //Position
        weaponGameObject.transform.position = weaponSlot.position;
        weaponGameObject.transform.rotation = weaponSlot.rotation;
        weaponGameObject.transform.SetParent(weaponSlot);
        Destroy(parent);
    }
}
