using UnityEngine;

public class CharacterBehaviour_Player : CharacterBehaviour
{
    [SerializeField] public bool isPerformingAction = false;
    [SerializeField] public bool canInteract = false;
    [SerializeField] public bool hasWeapon = false;
    [SerializeField] public bool hasThrowable = false;
    [SerializeField] public float interactRadius = 1f;

    [Header("GameObject References")]
    [SerializeField] public GameObject menu;
    [SerializeField] public GameObject interactUI;

    [Header("Script References"), HideInInspector]
    public Character_Movement player_Movement;
    public Player_CameraController player_CameraController;

    [Header("LayerMasks")]
    [SerializeField] public LayerMask playerMask;
    [SerializeField] public LayerMask enemyMask;
    [SerializeField] public LayerMask interactableMask;

    public override void Awake()
    {
        base.Awake();
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
