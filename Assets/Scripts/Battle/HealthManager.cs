using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DragonBones;

/*
* This Class Manages everything related to health, initialises 
*/
public class HealthManager : MonoBehaviour
{
    [SerializeField] Slider xpSlider;
    [SerializeField] Text xpText;
    [SerializeField] Text levelText;
    [SerializeField] UnityEngine.Transform damageOutputPrefab;
    public BattleManager battleManager;
    public LevelChecker levelChecker;

    public int playersLeft = 0;
    public int enemiesLeft = 0;

    public List<HealthBar> playerHealthBars = new List<HealthBar>();
    public List<HealthBar> enemyHealthBars = new List<HealthBar>();

    public List<Text> playerHealths = new List<Text>();
    public List<Text> playerHealthsSaved = new List<Text>();
    public List<Text> enemyHealths = new List<Text>();
    public List<Text> enemyHealthsSaved = new List<Text>();

    List<Beast> squad = new List<Beast>();
    List<Beast> enemies = new List<Beast>();

    public List<GameObject> winners = new List<GameObject>();

    public GameObject victoryScreen;

    //Get the health for each beast in play from BeastDatabase
    public void GetHealth(List<Beast> players, List<Beast> opposing, List<HealthBar> activePlayersHealth, List<HealthBar> activeEnemiesHealth)
    {
        foreach(Text text in playerHealths)
        {
            playerHealthsSaved.Add(text);
        }
        foreach (Text text in enemyHealths)
        {
            enemyHealthsSaved.Add(text);
        }

        for (int i = 10; i >= 0; i--)
        {
            if (activePlayersHealth[i] == null)
            {
                activePlayersHealth.RemoveAt(i);
                playerHealths[i].gameObject.SetActive(false);
                playerHealths.RemoveAt(i);
            }
        }

        for (int i = 10; i >= 0; i--)
        {
            if (activeEnemiesHealth[i] == null)
            {
                activeEnemiesHealth.RemoveAt(i);
                enemyHealths[i].gameObject.SetActive(false);
                enemyHealths.RemoveAt(i);
            }
        }

        squad = players;
        enemies = opposing;
        playerHealthBars = activePlayersHealth;
        enemyHealthBars = activeEnemiesHealth;     

        //checks if the player is not null and sets the max health of the health bar
        for(int x = 0; x < players.Count; x++)
        {
            if (players[x] != null)
            {
                playersLeft++;
                activePlayersHealth[x].SetMaxHealth(players[x].maxHP);
                playerHealths[x].text = players[x].maxHP.ToString();
            }
        }
        for(int x = 0; x< activeEnemiesHealth.Count; x++)
        {
            if (opposing[x] != null)
            {
                enemiesLeft += 1;
                activeEnemiesHealth[x].SetMaxHealth(opposing[x].maxHP);
                enemyHealths[x].text = enemies[x].maxHP.ToString();
            }
        }
    }

    //Subtract the damage from the target's health
    public void UpdateHealth(Beast target, int damage)
    {
        //removes the health from beasts that have been attacked 
        for(int x = 0; x< Values.SQUADMAX; x++)
        {  
            if (target == squad[x % squad.Count])
            {
                if(squad[x % squad.Count].hitPoints <= 0)
                {
                    battleManager.RemoveBeast(squad[x % squad.Count]);
                    break;
                }
                squad[x % squad.Count].hitPoints -= damage;
                playerHealths[x % squad.Count].text = squad[x % squad.Count].hitPoints.ToString();
                playerHealthBars[x % squad.Count].SetHealth(squad[x % squad.Count].hitPoints);

                if (squad[x % squad.Count].hitPoints <= 0)
                {
                    Debug.Log(target.name + " is knocked out.");
                    playerHealths[x % squad.Count].gameObject.SetActive(false);
                    if (!target.nonCombatant)
                    {
                        CheckRemainingPlayers();
                    }
                    battleManager.RemoveBeast(squad[x % squad.Count]);
                }
                else
                {
                    DisplayHealthLeft(target, squad[x % squad.Count].hitPoints);
                }
            }
            else if(target == enemies[x])
            {
                if(enemies[x].hitPoints < 0)
                {
                    battleManager.RemoveBeast(enemies[x]);
                    break;
                }
                enemies[x].hitPoints -= damage;
                enemyHealths[x].text = enemies[x].hitPoints.ToString();
                enemyHealthBars[x].SetHealth(enemies[x].hitPoints);

                if (enemies[x].hitPoints <= 0)
                {
                    Debug.Log(target.name + " is knocked out.");
                    enemyHealths[x].gameObject.SetActive(false);
                    if (!target.nonCombatant)
                    {
                        CheckRemainingOpposing();
                    }
                    battleManager.RemoveBeast(enemies[x]);
                }
                else
                {
                    DisplayHealthLeft(target, enemies[x].hitPoints);
                }
            }
        }
    }

    //Displays the damage output
    public void DisplayDamageOutput(Beast target, string damage, Color color)
    {
        print(target.name);
        if (target.name != "Target")
        {
            GameObject slot = battleManager.getSlot(target);
            Vector3 location = new Vector3(0, 0);

            if (slot != null)
            {
                location = new Vector3(slot.transform.localPosition.x, slot.transform.localPosition.y);
            }

            if (damage.Equals("MISS!") || damage.Equals("GUARD!"))
            {
                location.x -= 25;
                location.y -= 25;
            }

            if (damage.Equals("CRIT!"))
            {
                location.x -= 25;
                location.y -= 50;
            }

            UnityEngine.Transform damagePopup = Instantiate(damageOutputPrefab);
            damagePopup.transform.SetParent(GameObject.Find("Canvas").transform);
            damagePopup.localPosition = location;
            damagePopup.localRotation = Quaternion.identity;

            DamageOutput damageOutput = damagePopup.GetComponent<DamageOutput>();
            damageOutput.Create(damage, color);
        }
    }

    //adds health to the given beast upto the beasts maxHP
    public void heal(Beast target, double heal)
    {
        bool isPlayer = false;
        int y = -1;

        //looks for the beast that needs to be healed and heals it up to it's max hp
        for (int x = 0; x < Values.SQUADMAX; x++)
        {
            if (target.Equals(squad[x]))
            {
                isPlayer = true;
                y = x;
                break;
            }
            else if (target.Equals(enemies[x]))
            {
                isPlayer = false;
                y = x;
                break;
            }
        }

        if (heal > target.maxHP - target.hitPoints)
        {
            heal = target.maxHP - target.hitPoints;
        }

        target.hitPoints += (int)heal;

        if (isPlayer)
        {
            playerHealths[y % squad.Count].text = squad[y % squad.Count].hitPoints.ToString();
            playerHealthBars[y % squad.Count].SetHealth(squad[y % squad.Count].hitPoints);
        }
        else
        {
            enemyHealths[y % enemies.Count].text = enemies[y % enemies.Count].hitPoints.ToString();
            enemyHealthBars[y % enemies.Count].SetHealth(enemies[y % enemies.Count].hitPoints);
        }

        DisplayDamageOutput(target, Math.Floor(heal).ToString(), new Color(93f / 255f, 245f / 255f, 66f / 255f));
    }

    //prints the health left, needs to be updated to implement ui
    void DisplayHealthLeft(Beast target, int healthLeft)
    {
        Debug.Log(target.name + " has " + healthLeft + " health left.");
    }

    //Check to see if there are any players left, if not end game
    void CheckRemainingPlayers()
    {
        playersLeft--;
        if (playersLeft <= 0)
        {
            Debug.Log("Opposing Team Wins. Better Luck Next Time.");
            Player.summoner.addXP(battleManager.enemySummoner.xp/50);
            StartCoroutine(LoadMap());
        }
    }
    //Check to see if there are any enemies left, if not end game
    void CheckRemainingOpposing()
    {
        enemiesLeft--;
        if (enemiesLeft <= 0)
        {
            Debug.Log("Congratulations! You Win!");
            levelChecker.Progess(SceneManager.GetActiveScene().name);
            StartCoroutine(displayVictoryScreen());
        }
    }

    //Display the victory popup with the winning squad and rewards for winning the battle.
    IEnumerator displayVictoryScreen()
    {
        yield return new WaitForSeconds(2.5f);
        victoryScreen.SetActive(true);
        int unlock = UnityEngine.Random.Range(0,100);
        for (int x = 0; x < Values.SQUADMAX; x++)
        {
            if (squad[x] != null && squad[x].tier != -2)
            {
                winners[x].GetComponent<Image>().sprite = Resources.Load<Sprite>("Static_Images/EmptyRectangle");

                GameObject beastPrefab = (GameObject)Instantiate(Resources.Load("Prefabs/Beasts/" + squad[x].name));
                beastPrefab.transform.SetParent(winners[x].transform);
                beastPrefab.transform.localPosition = new Vector3(0, -30);
                beastPrefab.transform.localRotation = Quaternion.identity;
                beastPrefab.transform.localScale = new Vector3(25f, 25f);
            }
            else
            {
                winners[x].SetActive(false);
            }
        }

        UpdateXpBar(unlock <1);
        StartCoroutine(winnersAnimations());
    }

    // Updates xp bar and text
    private void UpdateXpBar(bool specialUnlock)
    {   
        int xp = (int)Mathf.Round(battleManager.enemySummoner.getLevel() / Player.summoner.getLevel() * (battleManager.enemySummoner.xp / 5));
        if (xp < 1) xp = 1;
        xpText.text = "XP Gained: " + xp;

        Player.summoner.addXP((int)Mathf.Round((battleManager.enemySummoner.getLevel() / Player.summoner.getLevel()) * (battleManager.enemySummoner.xp / 5)));
        Player.summoner.updateLevel();
        xpSlider.maxValue = Player.summoner.xpForLevel(Player.summoner.level);
        xpSlider.value = Player.summoner.xp;
        levelText.text = Player.summoner.level.ToString();

        if (specialUnlock && BeastManager.getFromNameS("SovereignDragon").tier < 0)
        {
            xpText.text += "\n Speacial Unlock:\n SovereignDragon";
            LevelChecker.unlock("SovereignDragon");
        }
        else if (specialUnlock && BeastManager.getFromNameS("Thanatos").tier < 0)
        {
            xpText.text += "\n Speacial Unlock:\n Thanatos";
            LevelChecker.unlock("Thanatos");
        }
        else if (specialUnlock && BeastManager.getFromNameS("Nage").tier < 0)
        {
            xpText.text += "\n Speacial Unlock:\n Nage";
            LevelChecker.unlock("Nage");
        }
        else if (specialUnlock && BeastManager.getFromNameS("Mandoro").tier < 0)
        {
            xpText.text += "\n Speacial Unlock:\n Mandoro";
            LevelChecker.unlock("Mandoro");
        }
    }

    //Play the 'roaring' animation for the winning team.
    IEnumerator winnersAnimations()
    {
        yield return new WaitForSeconds(2f);
        foreach (GameObject g in winners)
        {
            if(g.transform.childCount > 0)
            {
                g.transform.GetChild(0).GetComponent<UnityArmatureComponent>().animation.Play("Front");
            }
        }
    }

    //Collect rewards after winning a battle.
    public void onCollect()
    {
        StartCoroutine(LoadMap());
    }

    //After 1 second load the Map scene
    IEnumerator LoadMap()
    {
        yield return new WaitForSeconds(2);
        victoryScreen.SetActive(false);
        LoadScenes load = new LoadScenes();

        if (TournamentManager.sceneInterface == "Campaign")
            load.LoadSelect("Map");
        else if (TournamentManager.sceneInterface == "Tournament")
            load.LoadSelect("Tournament");
    }
}