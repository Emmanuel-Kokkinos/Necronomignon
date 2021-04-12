using DragonBones;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthDragon : MonoBehaviour
{
    public UnityArmatureComponent armatureComponent;
    enum State { IDLE, FRONT, BACK, DAMAGED };
    State state = State.FRONT;

    // Start is called before the first frame update
    void Start()
    {
        armatureComponent = GetComponent<UnityArmatureComponent>();
        //StartCoroutine(TestDeath());
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            PlayAttackA();
        }

        /*
        if (Input.GetKeyDown(KeyCode.Q))
        {
            PlayAttackA();
        }
        else
        {
            PlayIdle();
        }
        */
    }

    IEnumerator TestDeath()
    {
        GameObject.Find("Terraos(Clone)").GetComponent<Animator>().SetInteger("Health", 0);
        //armatureComponent.gameObject.SetActive(false);

        yield return new WaitForSeconds(2f);

        armatureComponent.gameObject.SetActive(true);
    }

     void PlayIdle()
    {
        if (state != State.IDLE)
        {
            armatureComponent.animation.FadeIn("Idle", .5f, -1);
            state = State.IDLE;
        }
    }

    void PlayAttackA()
    {
        armatureComponent.animation.FadeIn("Attack A", 0f, 1);
        state = State.FRONT;
    }
}
