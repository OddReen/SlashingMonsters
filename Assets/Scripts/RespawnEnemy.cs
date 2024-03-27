using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnEnemy : MonoBehaviour
{
    [SerializeField] GameObject enemyPref;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Spawn();
        }
    }
    void Spawn()
    {
        Instantiate(enemyPref, transform.position, Quaternion.identity);
    }
}
