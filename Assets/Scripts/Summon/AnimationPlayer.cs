using DragonBones;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/*
 * This Script will help us handle animation calls and Triggers whenever it is needed. 
 */
public class AnimationPlayer : MonoBehaviour
{
    public GameObject image;
    UnityArmatureComponent armatureComponent;
    BeastManager beastManager;

    // Start is called before the first frame update
    void Start()
    {
        image.GetComponent<Image>().sprite = Resources.Load<Sprite>("Static_Images/EmptyRectangle");

        //Prefab setting
        GameObject beastPrefab = (GameObject)Instantiate(Resources.Load($"Prefabs/Beasts/{SummonBookLoader.beastName}"));
        beastPrefab.transform.SetParent(GameObject.Find($"Image").transform);
        beastPrefab.transform.localPosition = beastPrefab.transform.position;
        beastPrefab.transform.localRotation = Quaternion.identity;
        beastPrefab.transform.localScale = beastPrefab.transform.localScale * .85f;

        image = image.transform.GetChild(0).gameObject;

        armatureComponent = image.GetComponent<UnityArmatureComponent>();
        Summon();
    }

    //Front Row Attack
    public void FrontAttack()
    {
        image.GetComponent<Parent_Beast>().Play_SoundFX("front");
        armatureComponent.animation.Play("Front", 1);
        StartCoroutine(AnimationWaitTime());
    }

    //Back Row Attack
    public void BackAttack()
    {
        image.GetComponent<Parent_Beast>().Play_SoundFX("back");
        armatureComponent.animation.Play("Back", 1);
        StartCoroutine(AnimationWaitTime());
    }

    //When a Beast Receives Damage
    public void Damaged()
    {
        image.GetComponent<Parent_Beast>().Play_SoundFX("damage");
        armatureComponent.animation.Play("Damage", 1);
        StartCoroutine(AnimationWaitTime());
    }

    //On Death
    public void Death()
    {
        image.GetComponent<Parent_Beast>().Play_SoundFX("death");
        armatureComponent.animation.Play("Death", 1);
        StartCoroutine(AnimationWaitTime());
    }

    // When Summoning a new beast into the field, or for the first time 
    public void Summon()
    {
        armatureComponent.animation.Play("Idle", 0);
    }

    //Back Button(obviously) 
    public void BackButton()
    {
        SceneManager.LoadScene("SummonMain");
    }

    IEnumerator AnimationWaitTime()
    {
        yield return new WaitWhile(new System.Func<bool>(() => !armatureComponent.animation.isCompleted));
        Summon();
    }
}
