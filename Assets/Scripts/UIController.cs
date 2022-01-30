using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class UIController : MonoBehaviour
{
    [SerializeField] private List<TMP_Text> playerScoreTexts;
    [SerializeField] private TMP_Text _winerText;
    [SerializeField] private TMP_Text _timerText;
    
    public void Refresh(List<PlayerController> players, TreasureController treasure, float time)
    {
        for (int i = 0; i<players.Count;i++)
        {
            float size = treasure.AttachedPlayer == players[i] ? 64 : 48;
            playerScoreTexts[i].text = $"<size={size:0}>P{players[i].PlayerNumber+1}: {players[i].Score:0.0}";
        }
        for (int i = players.Count; i<playerScoreTexts.Count;i++)
        {
            playerScoreTexts[i].text = "";
        }

        _timerText.text = time.ToString("00.0");
    }

    public void ShowWinner(PlayerController winPlayer)
    {
        _winerText.text = $"Player {winPlayer.PlayerNumber + 1}!";
    }
}
