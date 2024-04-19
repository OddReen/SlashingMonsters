using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChangePlayer : MonoBehaviour
{
    [SerializeField] Button b_ChangePlayerType;
    private void Start()
    {
        b_ChangePlayerType.onClick.AddListener(ChangePlayerType);

    }
    public void ChangePlayerType()
    {
        GameManager.Instance.isPlaceholder = !GameManager.Instance.isPlaceholder;
        if (GameManager.Instance.isPlaceholder)
        {
            b_ChangePlayerType.GetComponentInChildren<TextMeshProUGUI>().text = "On death change to: Placeholder";
        }
        else
        {
            b_ChangePlayerType.GetComponentInChildren<TextMeshProUGUI>().text = "On death change to: Player";
        }
    }
}
