using System.Collections;
using UnityEngine;

public class PlayerHealth : HealthSystem
{
    public float respawnTime = 5;
    public override void Die()
    {
        base.Die();
        StartCoroutine(Respawn());
    }
    private IEnumerator Respawn()
    {
        Fade.Instance.FadeOut();
        yield return new WaitForSeconds(respawnTime);
        Fade.Instance.FadeIn();
        GameManager.Instance.SpawnPlayer();
        GameManager.Instance.DeleteEnemies();
        GameManager.Instance.SpawnEnemies();
        Destroy(gameObject);
    }
}
