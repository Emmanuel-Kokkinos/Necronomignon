using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class Kitsune_Script : MonoBehaviour, Parent_Beast
{
    BattleManager battleManager;
    Attack attack;
    [SerializeField] GameObject backPrefab;
    float shootFrame;

    void Start()
    {
        
        GameObject g = GameObject.Find("GameManager");
        GameObject au = GameObject.Find("Music");
        if (g != null)
        {
            battleManager = g.GetComponent<BattleManager>();
            attack = g.GetComponent<Attack>();
        }

    }

    public void back_special()
    {
        RuntimeAnimatorController animCont = this.GetComponent<Animator>().runtimeAnimatorController;

        AnimationClip kitsuneBack = animCont.animationClips[2];

        //Calculate the time at which the frame of the animation is called
        float frame = 34 * (1 / 30);

        print("frame " + frame);
        shootFrame = kitsuneBack.length - frame;
        //shootFrame += Time.time;
        print("shoot :" + shootFrame);
        ProjectileAnimation();
    }

    public void front_special() 
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

    void ProjectileAnimation()
    {
        
        GameObject player = battleManager.getSlot(battleManager.currentTurn);
        GameObject target = battleManager.getSlot(battleManager.targets[0]);

        GameObject movePrefab = Instantiate(backPrefab);
        
        movePrefab.transform.SetParent(player.transform);
        movePrefab.transform.localPosition = new Vector3(0, 0);
        movePrefab.transform.localRotation = Quaternion.identity;
        movePrefab.transform.localScale = new Vector3(30, 30);



        StartCoroutine(ShootCoroutine(target, player, movePrefab));
        
    }

    public void Play_SoundFX()
    {
        throw new System.NotImplementedException();
    }

    IEnumerator ShootCoroutine(GameObject target, GameObject player, GameObject movePrefab)
    {
        yield return new WaitForSecondsRealtime(5);

        Vector3 shootDir = ((target.transform.localPosition) - (player.transform.localPosition)).normalized;


        movePrefab.GetComponent<Projectile>().Setup(shootDir);
    }
}
