using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public float respawnTime = 5;

    [Header("Player")]
    [SerializeField] public GameObject player;
    [SerializeField] private GameObject playerPref;
    [SerializeField] private GameObject playerPlaceholderPref;
    public bool isPlaceholder = false;

    [Header("Spawner")]
    [SerializeField] Transform spawnerParent;
    [SerializeField] List<SpawnPrefab> spawnerList;
    [SerializeField] List<GameObject> spawnList;

    [SerializeField] public GameObject interactUI;
    [SerializeField] public float nonActiveTime = 3;
    [SerializeField] public Transform checkpoint;

    private void Start()
    {
        if (Instance == null) Instance = this;
        for (int i = 0; i < spawnerParent.childCount; i++)
        {
            spawnerList.Add(spawnerParent.GetChild(i).GetComponent<SpawnPrefab>());
        }
        SpawnPlayer();
        SpawnPrefs();
        interactUI = player.GetComponent<Character_Attack>().interactUI;
    }

    #region SpawnPlayer
    public void SpawnPlayer() => StartCoroutine(C_SpawnPlayer());
    private IEnumerator C_SpawnPlayer()
    {
        GameObject whichPlayer = null;
        if (isPlaceholder)
        {
            whichPlayer = playerPref;
        }
        else
        {
            whichPlayer = playerPlaceholderPref;
        }
        GameObject newPlayer = Instantiate(whichPlayer, checkpoint.position - checkpoint.transform.forward, checkpoint.rotation);
        newPlayer.name = whichPlayer.name;
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
    #endregion

    #region Restart
    public void Restart() => StartCoroutine(C_Restart());
    private IEnumerator C_Restart()
    {
        yield return new WaitForSeconds(respawnTime);
        Fade.Instance.FadeOut();
        yield return new WaitForSeconds(respawnTime);
        Fade.Instance.FadeIn();
        Destroy(player);
        DeleteSpawned();
        SpawnPlayer();
        SpawnPrefs();
    }
    public void SpawnPrefs()
    {
        for (int i = 0; i < spawnerList.Count; i++)
        {
            spawnerList[i].Spawn();
            spawnList.Add(spawnerList[i].spawnedPref);
        }
    }
    public void DeleteSpawned()
    {
        for (int i = 0; i < spawnerList.Count; i++)
        {
            Destroy(spawnList[i]);
        }
        spawnList.Clear();
    }
    #endregion
}
