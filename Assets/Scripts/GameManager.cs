using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] Transform spawner;
    [SerializeField] List<SpawnEnemy> enemySpawners;
    [SerializeField] List<GameObject> enemies;

    [SerializeField] public GameObject interactUI;
    [SerializeField] public float nonActiveTime = 3;
    [SerializeField] public GameObject player;
    [SerializeField] private GameObject playerPref;

    [SerializeField] public Transform checkpoint;

    private void Start()
    {
        if (Instance == null) Instance = this;
        for (int i = 0; i < spawner.childCount; i++)
        {
            enemySpawners.Add(spawner.GetChild(i).GetComponent<SpawnEnemy>());
        }
        SpawnPlayer();
        SpawnEnemies();
        interactUI = player.GetComponent<Character_Attack>().interactUI;
    }
    public void SpawnEnemies()
    {
        for (int i = 0; i < enemySpawners.Count; i++)
        {
            enemySpawners[i].Spawn();
            enemies.Add(enemySpawners[i].spawnedEnemy);
        }
    }
    public void DeleteEnemies()
    {
        for (int i = 0; i < enemySpawners.Count; i++)
        {
            Destroy(enemies[i]);
        }
        enemies.Clear();
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
