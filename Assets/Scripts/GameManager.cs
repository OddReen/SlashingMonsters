using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float nonActiveTime = 3;
    public static GameManager Instance;
    public GameObject player;
    [SerializeField] private GameObject playerPref;
    [SerializeField] private Transform checkpoint;

    private void Start()
    {
        if (Instance == null)
            Instance = this;
        SpawnPlayerStart();
    }
    public void SpawnPlayerStart()
    {
        StartCoroutine(SpawnPlayer());
    }
    private IEnumerator SpawnPlayer()
    {
        GameObject newPlayer = Instantiate(playerPref, checkpoint.position, checkpoint.rotation);
        newPlayer.name = playerPref.name;
        player = newPlayer;
        HealthSystem healthSystem = player.GetComponent<HealthSystem>();
        Rigidbody rb = player.GetComponent<Rigidbody>();
        rb.isKinematic = true;
        yield return new WaitForEndOfFrame();
        for (int i = 0; i < healthSystem.scripts.Length; i++)
        {
            healthSystem.scripts[i].enabled = false;
        }
        yield return new WaitForSeconds(nonActiveTime);
        rb.isKinematic = false;
        for (int i = 0; i < healthSystem.scripts.Length; i++)
        {
            healthSystem.scripts[i].enabled = true;
        }
    }
}
