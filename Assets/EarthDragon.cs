using DragonBones;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthDragon : MonoBehaviour
{
    UnityArmatureComponent armatureComponent;

    // Start is called before the first frame update
    void Start()
    {
        armatureComponent = GetComponent<UnityArmatureComponent>();
        armatureComponent.animation.FadeIn("Idle", 2f, -1);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            PlayAttackA();
        }
    }

    void PlayAttackA()
    {
        armatureComponent.animation.FadeIn("Attack A", 0, 1);
    }
}
