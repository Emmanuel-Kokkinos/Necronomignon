using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 
public class SovereignDragon_Script : Parent_Script, Parent_Beast
{
    [SerializeField] GameObject frontPrefab;
    [SerializeField] AudioClip frontAttackSound, backAttackSound, damageSound, deathSound;


    private void Update()
    {
        
    }

    public void back_special()
    {
        battleManager.PlayDamagedAnimation(battleManager.targets[0]);

        if (battleManager.roundOrderTypes[battleManager.turn] == "Player")
        {
            attack.InitiateAttack(battleManager.currentTurn, battleManager.targets, battleManager.inFront(), Player.summoner);
        }
        else
        {
            attack.InitiateAttack(battleManager.currentTurn, battleManager.targets, battleManager.inFront(), battleManager.enemySummoner);
        }
    }

    public void front_special() 
    {
        ProjectileAnimation();
    }

    void ProjectileAnimation()
    {
        GameObject player = battleManager.getSlot(battleManager.currentTurn);
        GameObject target = battleManager.getSlot(battleManager.targets[0]);

        GameObject movePrefab = Instantiate(frontPrefab);
        movePrefab.transform.SetParent(player.transform);
        movePrefab.transform.localPosition = new Vector3(0, 0);
        movePrefab.transform.localRotation = Quaternion.identity;
        movePrefab.transform.localScale = new Vector3(30, 30);

        Vector3 shootDir = ((target.transform.localPosition) - (player.transform.localPosition)).normalized;
        movePrefab.GetComponent<Projectile>().Setup(shootDir);
    }

    public void Play_SoundFX(string sound)
    {
        switch (sound)
        {
            case "front": audioSrc.PlayOneShot(frontAttackSound); break;
            case "back": audioSrc.PlayOneShot(backAttackSound); break;
            case "damage": audioSrc.PlayOneShot(damageSound); break;
            case "death": audioSrc.PlayOneShot(deathSound); break;
        }
    }
}
