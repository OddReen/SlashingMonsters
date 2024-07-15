using UnityEngine;
using static BackgroundSoundsManager;

public class Crumble : MonoBehaviour
{
    [SerializeField] GameObject objectToCrumble;
    private void OnTriggerEnter(Collider other)
    {
        if (objectToCrumble != null)
        {
            BackgroundSoundsManager.instance.TriggerMusic(MusicState.roofColapse);
            BackgroundSoundsManager.instance.TriggerCrumble();
            Destroy(objectToCrumble);
        }
    }
}
