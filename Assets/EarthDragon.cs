using DragonBones;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthDragon : MonoBehaviour
{
    UnityArmatureComponent armatureComponent;
    bool idle = false;

    // Start is called before the first frame update
    void Start()
    {
        armatureComponent = GetComponent<UnityArmatureComponent>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!idle)
        {
            PlayIdle();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            PlayAttackA();
        }
    }

     void PlayIdle()
    {
        armatureComponent.animation.FadeIn("Idle", .5f, -1);
        idle = true;
    }

    void PlayAttackA()
    {
        armatureComponent.animation.FadeIn("Attack A", 0f, 1);
        idle = false;
    }
}
