using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreElement : MonoBehaviour
{
    [SerializeField] private Image avatar_IMG;
    [SerializeField] private TextMeshProUGUI name_TXT;
    [SerializeField] private TextMeshProUGUI score_TXT;

    public void Initialize(Sprite avatar, string name, int score)
    {
        this.avatar_IMG.sprite = avatar;
        this.name_TXT.text = name;
        this.score_TXT.text = score.ToString();
    }
}
