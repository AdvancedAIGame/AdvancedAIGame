using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    // Start is called before the first frame update
    private TextMeshProUGUI health_score;
    void Start()
    {
        health_score = GetComponent<TextMeshProUGUI>();
    }

   public void UpdateHealthScore(PlayerInventory playerInventory)
    {
        //update game score
        health_score.text = playerInventory.playerHealth.ToString();
    }
}
