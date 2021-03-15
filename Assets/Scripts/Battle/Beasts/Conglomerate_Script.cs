using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conglomerate_Script : MonoBehaviour, Parent_Beast
{
    BattleManager battleManager;
    [SerializeField] GameObject backPrefab;
    int currentSlot;
    void Start()
    {
        GameObject g = GameObject.Find("GameManager");

        if (g != null)
        {
            battleManager = g.GetComponent<BattleManager>();
        }
    }

    public void back_special()
    {
        ProjectileAnimation();
        battleManager.PlayDamagedAnimation(battleManager.targets[0]);
    }

    public void front_special()
    {
        battleManager.PlayDamagedAnimation(battleManager.targets[0]);
    }

    void ProjectileAnimation()
    {
        GameObject target = battleManager.getSlot(battleManager.targets[0]);

        GameObject movePrefab = Instantiate(backPrefab);
        movePrefab.transform.SetParent(target.transform);
        movePrefab.transform.localPosition = new Vector3(0, 0);
        movePrefab.transform.localRotation = Quaternion.identity;
        movePrefab.transform.localScale = new Vector3(5, 5);
    }
}
