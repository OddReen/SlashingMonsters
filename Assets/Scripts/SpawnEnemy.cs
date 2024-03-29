using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    [SerializeField] GameObject enemyPref;
    public GameObject spawnedEnemy;

    public void Spawn()
    {
        spawnedEnemy = Instantiate(enemyPref, transform.position, Quaternion.identity);
        spawnedEnemy.name = enemyPref.name;
    }
}
