using System.Collections;
using UnityEngine;
using static BackgroundSoundsManager;

public class CharacterBehaviour_Player : CharacterBehaviour
{
    public int weaponHash;

    [SerializeField] public bool inCombat = false;

    [SerializeField] public bool isPerformingAction = false;
    [SerializeField] public bool canInteract = false;
    [SerializeField] public bool hasWeapon = false;
    [SerializeField] public bool hasThrowable = false;
    [SerializeField] public float interactRadius = 1f;

    [Header("GameObject References")]
    [SerializeField] public GameObject UI;
    [SerializeField] public GameObject menu;
    [SerializeField] public GameObject interactUI;

    [Header("Script References"), HideInInspector]
    public Character_Movement player_Movement;
    public Player_CameraController player_CameraController;

    [Header("LayerMasks")]
    [SerializeField] public LayerMask playerMask;
    [SerializeField] public LayerMask enemyMask;
    [SerializeField] public LayerMask interactableMask;

    IEnumerator InCombat()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.25f);
            inCombat = false;
            for (int i = 0; i < GameManager.Instance.enemyList.Count; i++)
            {
                if (GameManager.Instance.enemyList[i].GetComponent<CharacterBehaviour_Enemy>() != null && GameManager.Instance.enemyList[i].GetComponent<CharacterBehaviour_Enemy>().isSighted)
                {
                    inCombat = true;
                    break;
                }
            }
            animator.SetBool("inCombat", inCombat);
            Player_CameraController.Instance.InCombat(inCombat);
        }
    }
    public override void Awake()
    {
        base.Awake();
        StartCoroutine(InCombat());
        player_Movement = GetComponent<Character_Movement>();
        player_CameraController = GetComponent<Player_CameraController>();

        CharacterActions[] player = GetComponents<CharacterActions>();
        for (int i = 0; i < player.Length; i++)
        {
            actions.Add(player[i]);
        }
    }
    private void Update()
    {
        for (int i = 0; i < actions.Count; i++)
        {
            actions[i].UpdateAction();
        }
    }
    private void LateUpdate()
    {
        animator.transform.position = transform.position;
    }
}
