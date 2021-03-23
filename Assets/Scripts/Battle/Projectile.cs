using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    BattleManager battleManager;
    Attack attack;

    private Vector3 shootDir;
    float moveSpeed = 150f;

    // Start is called before the first frame update
    void Start()
    {
        GameObject g = GameObject.Find("GameManager");

        if (g != null)
        {
            battleManager = g.GetComponent<BattleManager>();
            attack = g.GetComponent<Attack>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += shootDir * moveSpeed * Time.deltaTime;
    }

    public void Setup(Vector3 shootDir)
    {
        this.shootDir = shootDir;
        transform.eulerAngles = new Vector3(0, 0, GetAngleFromVectorFloat(shootDir));
    }

    float GetAngleFromVectorFloat(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;

        return n;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Parent_Beast>() != null)
        {
            Destroy(gameObject);
            collision.gameObject.GetComponent<Animator>().SetTrigger("GetHit");

            Beast attacker = battleManager.currentTurn;
            if(battleManager.turn != 0)
            {
                attacker = battleManager.roundOrder[battleManager.turn - 1];
            }

            if (battleManager.roundOrderTypes[battleManager.turn] == "Player")
            {
                attack.InitiateAttack(attacker, battleManager.targets, battleManager.inFront(), Player.summoner);
            }
            else
            {
                attack.InitiateAttack(attacker, battleManager.targets, battleManager.inFront(), battleManager.enemySummoner);
            }
        }
    }
}
