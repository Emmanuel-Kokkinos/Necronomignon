using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
//using UnityEditorInternal;

public class LoadAssetBundles : MonoBehaviour
{

    static AssetBundle myAssetBundleBeast;
    string path = Application.streamingAssetsPath + "/assetBundles/beasts";
    string pathstatimg = Application.streamingAssetsPath + "/assetBundles/staticimages";
    string pathAnimation = Application.streamingAssetsPath + "/AssetBundles/animation";
    static AssetBundle myAssetBundleImg;
    static AssetBundle myAssetBundleAnimation;

    void Start()
    {
        if (myAssetBundleBeast == null)
        {
            LoadAssetBundle(path, pathstatimg, pathAnimation);
        }
    }

    void LoadAssetBundle(string bundleURL, string imgURL, string aniURL)
    {
        //myAssetBundleBeast = AssetBundle.LoadFromFile(bundleURL);
        myAssetBundleImg = AssetBundle.LoadFromFile(imgURL);
        //myAssetBundleAnimation = AssetBundle.LoadFromFile(aniURL);


        if(myAssetBundleImg == null)
        {
            print("myAssetBundle failed");
        }
    }

    public static GameObject getObj(string str)
    {
        GameObject go = Resources.Load<GameObject>("Prefabs/Beasts/" + str);
        if(go != null)
        {
            print(go.GetComponent<Image>());
            print(go.name + "_Idle_00.png");
            print(myAssetBundleImg);
            //Loading asset bundle causing crash in load squad. Reverted to previous work for functionality until myassetbundle is fixed
            //go.GetComponent<Image>().sprite = /*myAssetBundleImg.LoadAsset<Sprite>($"{go.name}_Idle_00");*/Resources.Load<Sprite>("Static_Images/" + go.name+ "_Idle_00");
            //print(Resources.Load<RuntimeAnimatorController>("Animations/"+go.name + "/" +go.name + "_Controller")/*myAssetBundleAnimation.LoadAsset<RuntimeAnimatorController>( go.name + "_Controller"*/);
            //byte[] filedata = File.ReadAllBytes(Application.streamingAssetsPath+"Animations/"+go.name+"/"+go.name+"_Controller.controller");
            //go.GetComponent<Animator>().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Animations/" + go.name + "/" + go.name + "_Controller");
            //go.GetComponent<Animator>().runtimeAnimatorController = myAssetBundleAnimation.LoadAsset<RuntimeAnimatorController>(go.name);
        }
        return go;
    }
}
