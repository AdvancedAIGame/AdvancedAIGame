using RandomStrings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AffinityCalculation : MonoBehaviour
{
    private string playerSignature;
    // Start is called before the first frame update
    void Start()
    {
        playerSignature = PlayerPrefs.GetString("playerSignature");
        clonalExpansion();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void clonalExpansion()
    {
        Antibody at = new Antibody(12);
        List<string> p = at.population(120);
        List<string> pop = at.select(p, playerSignature, 4);

        print(" "+ playerSignature);
        foreach(string population in pop)
        {
            print(population + "  " + at.ComputeAffinity(population,playerSignature));
        }
    }
}
