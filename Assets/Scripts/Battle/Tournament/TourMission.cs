using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*This class will define the missions in the tournament in a similar fashion to the missions in the main battle*/
public class TourMission : MonoBehaviour
{ 
        public string mission;

        public BeastManager beastManager;

        public List<Beast> enemies = new List<Beast>();

        public Summoner summoner;

        public int totalEnemies = 11;

    // Start is called before the first frame update
    void Start()
    {
        BuildMission();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BuildMission()
    {
        mission = TourChecker.lastClick;
        summoner = new Summoner();

        // Kitsune in a random position
        if (mission == "first")
        {
            Beast kitsune = beastManager.getFromName("Kitsune");

            int ran = -1;
            //ran = Random.Range(-1, Values.SMALLSLOT - 1);
            ran = (kitsune.size == 0) ? Random.Range(-1, Values.SMALLSLOT - 1) : Random.Range(Values.SMALLSLOT, totalEnemies - 1);

            while (ran >= 0)
            {
                enemies.Add(null);
                ran--;
            }

            enemies.Add(kitsune);

            while (enemies.Count < totalEnemies)
            {
                enemies.Add(null);
            }

            foreach (Beast b in enemies)
            {
                if (b != null)
                {
                    b.setTierUpper(2);
                }
            }

            summoner.xp = 2;
        }
    }
}
