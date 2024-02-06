using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Drawing;

public class UnicodeSupport : MonoBehaviour
{

    TextMeshProUGUI starDifficulty;
    string blackStar = "\u2605"; // Black Star
    string whiteStar = "\u2606"; // White Star


    // Start is called before the first frame update
    void Start()
    {
        starDifficulty = this.gameObject.GetComponent<TextMeshProUGUI>();
        starDifficulty.text = "<color=#FFD700FF>★</color>";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
