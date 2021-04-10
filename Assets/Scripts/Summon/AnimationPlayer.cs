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
    }

    //Front Row Attack
    public void FrontAttack()
    {
        if(image.name == "Terraos(Clone)")
        {
            armatureComponent.animation.FadeIn("Attack A", .5f, -1);
        }
        image.GetComponent<Animator>().SetTrigger("Front");
    }

    //Back Row Attack
    public void BackAttack()
    {
        image.GetComponent<Animator>().SetTrigger("Back");
    }

    //When a Beast Receives Damage
    public void Damaged()
    {
        image.GetComponent<Animator>().SetTrigger("GetHit");
    }

    //On Death
    public void Death()
    {
        image.GetComponent<Animator>().SetInteger("Health", 0);
        
    }

    // When Summoning a new beast into the field, or for the first time 
    public void Summon()
    {
        image.GetComponent<Animator>().SetInteger("Health", 100);
        //Summon animation will go here when we get them
        image.GetComponent<Animator>().Play("Base Layer.Idle", 0);
    }

    //Back Button(obviously) 
    public void BackButton()
    {
        SceneManager.LoadScene("SummonMain");
    }
}
