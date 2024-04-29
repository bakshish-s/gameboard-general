using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ArmedPlayer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI playerName;

    public void OnArmed(string playerName)
    {
        this.playerName.text = playerName;
        gameObject.SetActive(true);
    }
}
