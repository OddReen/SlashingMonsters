using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MusicManager;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject mainMenu;

    public static GameManager Instance;

    public GameObject[] interactables = null;

    [Header("Player")]
    [SerializeField] public GameObject player;
    [SerializeField] private GameObject playerPref;

    [Header("Interactable Spawner")]
    [SerializeField] Transform interactableSpawnerParent;
    [SerializeField] List<SpawnPrefab> interactableSpawnerList;
    [SerializeField] List<GameObject> interactableList;

    [Header("Enemy Spawner")]
    [SerializeField] Transform enemySpawnerParent;
    [SerializeField] List<SpawnPrefab> enemySpawnerList;
    [SerializeField] public List<GameObject> enemyList;

    [Header("Misc")]
    [SerializeField] public GameObject interactUI;
    public float respawnTime = 5;
    [SerializeField] public float nonActiveTime = 3;
    [SerializeField] public Transform checkpoint;

    [Header("Abyss Fall")]
    [SerializeField] Transform dieOnFallLimit;

    [SerializeField] bool isStarting = true;

    public void OnStartGame()
    {
        mainMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        player.GetComponent<CharacterBehaviour_Player>().animator.SetBool("GetUp", true);
        MusicManager.instance.TriggerMusic(MusicState.none);

        Invoke("OnPlayerGetUp", 3);
    }
    public void OnQuitGame()
    {
        Application.Quit();
    }

    public void OnPlayerGetUp()
    {
        player.GetComponent<CharacterBehaviour_Player>().UI.SetActive(true);
        Player_Input.Instance.EnableInput();
    }

    private void Start()
    {
        MusicManager.instance.TriggerMusic(MusicState.mainMenu);

        isStarting = true;

        if (Instance == null) Instance = this;

        SpawnPlayer();
        SpawnInteractables();
        SpawnEnemies();
        DieOnFall();

        player.GetComponent<CharacterBehaviour_Player>().animator.SetBool("GetUp", false);

        interactables = GameObject.FindGameObjectsWithTag("Interactable");
        interactUI = player.GetComponent<Character_Attack>().interactUI;

        player.GetComponent<CharacterBehaviour_Player>().UI.SetActive(false);

        Player_Input.Instance.DisableInput();

        Cursor.lockState = CursorLockMode.None;
        isStarting = false;
    }

    public void GetEnemyOutOfArray(GameObject gameObject)
    {
        enemyList.Remove(gameObject);
    }

    void DieOnFall() => StartCoroutine(C_DieOnFall());
    public void Restart() => StartCoroutine(C_Restart());
    public void SpawnPlayer()
    {
        GameObject newPlayer = Instantiate(playerPref, checkpoint.position, checkpoint.rotation);
        newPlayer.name = playerPref.name;
        player = newPlayer;
    }
    private IEnumerator C_DieOnFall()
    {
        while (true)
        {
            yield return null;
            if (player != null && player.transform.position.y < dieOnFallLimit.transform.position.y && !player.GetComponent<CharacterBehaviour_Player>().isDead)
            {
                player.GetComponent<HealthSystem>().Die();
            }
        }
    }
    private IEnumerator C_Restart()
    {
        yield return new WaitForSeconds(respawnTime);
        Fade.Instance.FadeOut();
        yield return new WaitForSeconds(respawnTime);
        Fade.Instance.FadeIn();
        // Delete
        Destroy(player);
        DeleteInteractables();
        DeleteEnemies();
        //Spawn
        SpawnPlayer();
        SpawnEnemies();
        SpawnInteractables();

        player.GetComponent<CharacterBehaviour_Player>().UI.SetActive(true);
        Player_Input.Instance.DisableInput();
        OnStartGame();

        interactables = GameObject.FindGameObjectsWithTag("Interactable");
    }

    public void SpawnInteractables()
    {
        for (int i = 0; i < interactableSpawnerParent.childCount; i++)
        {
            interactableSpawnerList.Add(interactableSpawnerParent.GetChild(i).GetComponent<SpawnPrefab>());
            interactableSpawnerList[i].Spawn();
            interactableList.Add(interactableSpawnerList[i].spawnedPref);
        }
    }
    public void SpawnEnemies()
    {
        for (int i = 0; i < enemySpawnerParent.childCount; i++)
        {
            enemySpawnerList.Add(enemySpawnerParent.GetChild(i).GetComponent<SpawnPrefab>());
            enemySpawnerList[i].Spawn();
            enemyList.Add(enemySpawnerList[i].spawnedPref);
        }
    }
    public void DeleteInteractables()
    {
        for (int i = 0; i < interactableList.Count; i++)
        {
            Destroy(interactableList[i]);
        }
        interactableList.Clear();
    }
    public void DeleteEnemies()
    {
        for (int i = 0; i < enemyList.Count; i++)
        {

            if (enemyList[i] != null)
            {
                Destroy(enemyList[i]);
            }
        }
        enemyList.Clear();
    }
}
