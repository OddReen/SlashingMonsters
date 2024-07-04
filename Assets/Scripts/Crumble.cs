using UnityEngine;
using static MusicManager;

public class Crumble : MonoBehaviour
{
    [SerializeField] GameObject objectToCrumble;
    private void OnTriggerEnter(Collider other)
    {
        if (objectToCrumble != null)
        {
            MusicManager.instance.TriggerMusic(MusicState.roofColapse);
            MusicManager.instance.TriggerCrumble();
            Destroy(objectToCrumble);
        }
    }
}
