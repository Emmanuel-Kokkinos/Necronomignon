﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DragonBones;

//Script used to show the beast
public class TrainPrep : MonoBehaviour
{
 
    public Button img;
    public new Text name;
    public LoadScenes loadScenes;
    public List<Image> pentagrams;

    Beast b;

    // Start is called before the first frame update
    void Start()
    {
        b = BeastManager.getFromNameS(name.text);

        for (int x = 0; x <= b.tier; x++)
        {
            pentagrams[x].gameObject.SetActive(true);
        }

        img.GetComponent<Image>().sprite = Resources.Load<Sprite>("Static_Images/EmptyRectangle");

        name.text = SummonBookLoader.CamelCaseCorrection(SummonManager.name);
        GameObject beastPrefab = (GameObject)Instantiate(Resources.Load($"Prefabs/Beasts/{SummonManager.name}"));
        beastPrefab.transform.SetParent(GameObject.Find($"BeastImage").transform);
        beastPrefab.transform.localPosition = beastPrefab.transform.position;
        beastPrefab.transform.localRotation = Quaternion.identity;
        beastPrefab.transform.localScale = beastPrefab.transform.localScale * .8f;
        beastPrefab.GetComponent<UnityArmatureComponent>().animation.Play("Idle", 1);
        beastPrefab.GetComponent<UnityArmatureComponent>().animation.Stop();

        
    }

    public void CheckToTrain()
    {

        if (b.tier < 5)
        {
            loadScenes.LoadSelect("BeastQuiz");
        }

    }
}
