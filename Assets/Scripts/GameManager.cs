using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Player")]
    [SerializeField] public GameObject player;
    [SerializeField] private GameObject playerPref;

    [Header("Enemies")]
    [SerializeField] Transform enemySpawnerParent;
    [SerializeField] List<SpawnEnemy> enemySpawnerList;
    [SerializeField] List<GameObject> enemyList;

    [SerializeField] public GameObject interactUI;
    [SerializeField] public float nonActiveTime = 3;
    [SerializeField] public Transform checkpoint;

    private void Start()
    {
        if (Instance == null) Instance = this;
        for (int i = 0; i < enemySpawnerParent.childCount; i++)
        {
            enemySpawnerList.Add(enemySpawnerParent.GetChild(i).GetComponent<SpawnEnemy>());
        }
        SpawnPlayer();
        SpawnEnemies();
        interactUI = player.GetComponent<Character_Attack>().interactUI;
    }
    public void SpawnEnemies()
    {
        for (int i = 0; i < enemySpawnerList.Count; i++)
        {
            enemySpawnerList[i].Spawn();
            enemyList.Add(enemySpawnerList[i].spawnedEnemy);
        }
    }
    public void DeleteEnemies()
    {
        for (int i = 0; i < enemySpawnerList.Count; i++)
        {
            Destroy(enemyList[i]);
        }
        enemyList.Clear();
    }
    public void SpawnPlayer()
    {
        StartCoroutine(SpawnPlayerCoroutine());
    }
    private IEnumerator SpawnPlayerCoroutine()
    {
        GameObject newPlayer = Instantiate(playerPref, checkpoint.position - checkpoint.transform.forward, checkpoint.rotation);
        newPlayer.name = playerPref.name;
        player = newPlayer;

        CharacterBehaviour_Player characterBehaviour_Player = player.GetComponent<CharacterBehaviour_Player>();
        Rigidbody rb = player.GetComponent<Rigidbody>();
        rb.isKinematic = true;
        yield return new WaitForEndOfFrame();
        for (int i = 0; i < characterBehaviour_Player.actions.Count; i++)
        {
            characterBehaviour_Player.actions[i].enabled = false;
        }
        yield return new WaitForSeconds(nonActiveTime);
        rb.isKinematic = false;
        for (int i = 0; i < characterBehaviour_Player.actions.Count; i++)
        {
            characterBehaviour_Player.actions[i].enabled = true;
        }
    }
}
