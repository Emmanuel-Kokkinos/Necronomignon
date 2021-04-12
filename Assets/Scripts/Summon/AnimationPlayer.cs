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
        beastPrefab.transform.localPosition = new Vector3(0, 100);
        beastPrefab.transform.localRotation = Quaternion.identity;
        beastPrefab.transform.localScale = new Vector3(100, 100);

        

        image = image.transform.GetChild(0).gameObject;

        armatureComponent = image.GetComponent<UnityArmatureComponent>();
        Summon();
    }

    //Front Row Attack
    public void FrontAttack()
    {
        if(image.name == "Terraos(Clone)")
        {
            armatureComponent.animation.Play("Attack A", 1);
            StartCoroutine(wait4me());
            
        }
        image.GetComponent<Animator>().SetTrigger("Front");
    }

    //Back Row Attack
    public void BackAttack()
    {
        if (image.name == "Terraos(Clone)")
        {
            armatureComponent.animation.Play("Attack B", 1);
            StartCoroutine(wait4me());
        }
        image.GetComponent<Animator>().SetTrigger("Back");
    }

    //When a Beast Receives Damage
    public void Damaged()
    {
        if (image.name == "Terraos(Clone)")
        {
            armatureComponent.animation.Play("Damage", 1);
            StartCoroutine(wait4me());
        }
        image.GetComponent<Animator>().SetTrigger("GetHit");
    }

    //On Death
    public void Death()
    {
        if (image.name == "Terraos(Clone)")
        {
            armatureComponent.animation.Play("Deathj", 1);
            StartCoroutine(wait4me());
        }
        image.GetComponent<Animator>().SetInteger("Health", 0);
    }

    // When Summoning a new beast into the field, or for the first time 
    public void Summon()
    {
        if (image.name == "Terraos(Clone)")
        {
            armatureComponent.animation.Play("Idle", 0);
        }
        image.GetComponent<Animator>().SetInteger("Health", 100);
        //Summon animation will go here when we get them
        image.GetComponent<Animator>().Play("Base Layer.Idle", 0);
    }

    //Back Button(obviously) 
    public void BackButton()
    {
        SceneManager.LoadScene("SummonMain");
    }

    IEnumerator wait4me()
    {
        yield return new WaitWhile(new System.Func<bool>(() => !armatureComponent.animation.isCompleted));
        Summon();
    }
}
