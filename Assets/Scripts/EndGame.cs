using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour
{

    public GameObject player;
    private Vector3 playerStart;
    public GameObject[] enemies;

    public GameObject gameResetText;

    public GameObject[] healthPickups;

    public GameObject healthS;

    public GameObject Monsters;
    // Start is called before the first frame update
    void Start()
    {
        playerStart = player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        EndHelper();
    }

    public void EndHelper()
    {
        //reset the position, show a text saying game reset and maybe points, delay
        player.GetComponent<CharacterController>().enabled = false;
        player.transform.position = playerStart;
        player.GetComponent<CharacterController>().enabled = true;
        player.GetComponent<PlayerInventory>().playerHealth = 50;
        TMPro.TextMeshProUGUI playerHealth = healthS.GetComponent<TMPro.TextMeshProUGUI>();
        playerHealth.text = "50";

        Monsters.GetComponent<AffinityCalculation>().resetExpansion(enemies);

        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i].activeSelf == true)
            {
                enemies[i].GetComponent<EnemyController>().enemyReset();
            }
        }

        foreach(GameObject hp in healthPickups)
        {
            hp.SetActive(true);
        }

        StartCoroutine(gameResetDelay());
        //gameResetText.SetActive(false);
    }

    IEnumerator gameResetDelay()
    {
        player.GetComponent<CharacterController>().enabled = false;
        player.GetComponent<PlayerMovement>().enabled = false;
        gameResetText.SetActive(true);
        //resetText.CrossFadeAlpha(0.0f, 5f, true);
        yield return new WaitForSeconds(5);

        gameResetText.SetActive(false);
        player.GetComponent<CharacterController>().enabled = true;
        player.GetComponent<PlayerMovement>().enabled = true;
    }
}
