using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoggoth_Script : Parent_Script, Parent_Beast
{
    [SerializeField] AudioClip frontAttackSound, backAttackSound, damageSound, deathSound;
    AudioSource audioSrc;

    void Start()
    {
        audioSrc = GetComponent<AudioSource>();

        base.start();
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
}
