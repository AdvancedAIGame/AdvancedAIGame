using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    private string signature;
    private const string cell = "abcdefghijklmnopqrstuvwxyz";
    private System.Random random = new System.Random();
    // Start is called before the first frame update
    void Start()
    {
        signature = generate_string();
        PlayerPrefs.SetString("playerSignature", signature);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string generate_string()
    {
        return new string(Enumerable.Repeat(cell, 12).Select(s => s[random.Next(s.Length)]).ToArray());
    }
}
