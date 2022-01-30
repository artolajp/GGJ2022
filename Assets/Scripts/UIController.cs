using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private List<TMP_Text> playerScoreTexts;
    
    public void Refresh(List<PlayerController> players)
    {
        for (int i = 0; i<players.Count;i++)
        {
            playerScoreTexts[i].text = $"P1: {players[i].Score:0.0}";
        }
        for (int i = players.Count; i<playerScoreTexts.Count;i++)
        {
            playerScoreTexts[i].text = "";
        }
    }
}
