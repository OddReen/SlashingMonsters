using System.Collections;
using UnityEngine;

public class PlayerHealth : HealthSystem
{
    public float respawnTime = 5;
    public override void Die()
    {
        base.Die();
        Fade.Instance.FadeOut();
        StartCoroutine(Respawn());
    }
    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnTime);
        Fade.Instance.FadeIn();
        GameManager.Instance.SpawnPlayerStart();
        Destroy(gameObject);
    }
}
