using RandomStrings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AffinityCalculation : MonoBehaviour
{
    private string playerSignature;
    private int resetCount = 0;
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

    public List<string> clonalExpansion()
    {
        Antibody at = new Antibody(12);
        List<string> p = at.population(120);
        List<string> pop = at.select(p, playerSignature, 120);
        List<string> clone = new List<string>();
        List<string> pop_clone = new List<string>();
        int iteration = 0;
        

        print("player signature "+ playerSignature);

        while (iteration < 100)
        {
            print("Iteration: " + iteration);
            foreach (string population in pop)
            {
               // print(population + "  " + at.ComputeAffinity(population, playerSignature));

                if (at.ComputeAffinity(population, playerSignature) != 12)
                {
                    clone = at.clone(population, at.ComputeAffinity(population, playerSignature));
                    ///print(clone[0]);
                    ///
                    foreach (string c in clone)
                    {
                        pop_clone.Add(c);
                    }
                }

                //print(population + "  " + at.ComputeAffinity(population, playerSignature));

            }



         

            for (int j = 0; j < pop_clone.Count; ++j)
            {
                string hypermutate_pop = at.hypermutate(pop_clone[j], at.ComputeAffinity(pop_clone[j], playerSignature));

                pop_clone[j] = hypermutate_pop;

                int af = at.ComputeAffinity(hypermutate_pop, playerSignature);

                //print("hyper: " + hypermutate_pop + "  " + "affinity: " + af);
            }



            pop_clone.ForEach(item => p.Add(item));
            p = at.select(p, playerSignature, 120);

          

            List<string> sel = at.select(p, playerSignature, 80);
            List<string> rep = at.replace(p, sel);
          
      
            p = rep;
            print(p[0] + " " + at.ComputeAffinity(p[0], playerSignature));
            iteration++;
        }

        assignSignatures(p);

        return p;

    }

    public void assignSignatures(List<string> signatures)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Monster");
        int index = 0;
        foreach(GameObject e in enemies)
        {
            e.GetComponent<EnemyController>().signature = signatures[index];
            e.GetComponent<EnemyController>().clonalExpansion();
            index++;
        }
    }

    public List<string> resetExpansion(GameObject[] enemies)
    {
        resetCount++;
        Antibody at = new Antibody(12);
        List<string> p = new List<string>();//at.population(120);
        foreach(GameObject e in enemies)
        {
            p.Add(e.GetComponent<EnemyController>().signature);
        }
        
        List<string> pop = at.select(p, playerSignature, 3);
        List<string> clone = new List<string>();
        List<string> pop_clone = new List<string>();
        int iteration = 0;


        print("player signature: " + playerSignature);

        while (iteration < 100)
        {
            print("Iteration: " + iteration);
            foreach (string population in pop)
            {
                // print(population + "  " + at.ComputeAffinity(population, playerSignature));

                if (at.ComputeAffinity(population, playerSignature) != 12)
                {
                    clone = at.clone(population, at.ComputeAffinity(population, playerSignature));
                    ///print(clone[0]);
                    ///
                    foreach (string c in clone)
                    {
                        pop_clone.Add(c);
                    }
                }
                //print(population + "  " + at.ComputeAffinity(population, playerSignature));
            }

            for (int j = 0; j < pop_clone.Count; ++j)
            {
                string hypermutate_pop = at.hypermutate(pop_clone[j], at.ComputeAffinity(pop_clone[j], playerSignature));

                pop_clone[j] = hypermutate_pop;

                int af = at.ComputeAffinity(hypermutate_pop, playerSignature);

                //print("hyper: " + hypermutate_pop + "  " + "affinity: " + af);
            }

            pop_clone.ForEach(item => p.Add(item));
            p = at.select(p, playerSignature, 3);

            //List<string> sel = at.select(p, playerSignature, 80);
            //List<string> rep = at.replace(p, sel);
            //p = rep;

            print("Affinity after reset #" + resetCount + ": " + p[0] + " " + at.ComputeAffinity(p[0], playerSignature));
            iteration++;
        }

        assignSignatures(p);

        return p;

    }

}
