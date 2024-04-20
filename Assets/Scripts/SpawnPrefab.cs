using UnityEngine;

public class SpawnPrefab : MonoBehaviour
{
    [SerializeField] GameObject pref;
    public GameObject spawnedPref;

    public void Spawn()
    {
        spawnedPref = Instantiate(pref, transform.position, Quaternion.identity);
        spawnedPref.name = pref.name;
    }
}
