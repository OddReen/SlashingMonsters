using UnityEngine;

public class Crumble : MonoBehaviour
{
    [SerializeField] GameObject objectToCrumble;
    private void OnTriggerEnter(Collider other)
    {
        if (objectToCrumble != null)
            Destroy(objectToCrumble);
    }
}
