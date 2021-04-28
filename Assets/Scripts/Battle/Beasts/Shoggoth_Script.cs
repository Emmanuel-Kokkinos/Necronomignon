using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DragonBones;

public class Shoggoth_Script : Parent_Script, Parent_Beast
{
    public void checkStatusEffect() { }

    public void applyStatusEffect(string type) { }
    [SerializeField] AudioClip frontAttackSound, backAttackSound, damageSound, deathSound;
    [SerializeField] Texture tex;
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            //change texture color
        }
    }

    public void back_special()
    {

    }

    public void front_special()
    {

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

    public string Beast_Name()
    {
        return "Mandoro";
    }
}
