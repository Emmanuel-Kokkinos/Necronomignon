using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Beast_Script : MonoBehaviour
{
    protected BattleManager battleManager;
    protected Attack attack;
    // Start is called before the first frame update
    void Start()
    {
        GameObject g = GameObject.Find("GameManager");

        if (g != null)
        {
            battleManager = g.GetComponent<BattleManager>();
            attack = g.GetComponent<Attack>();
        }
        /*GameObject go = GameObject.Find(this.name);
        print(go);
        if(go != null)
        {
            Image image = go.GetComponent<Image>();
            image.sprite = Resources.Load<Sprite>("Static_Images/" + removeClone(this.name));
        }*/
    }
    protected void start()
    {
        Start();
    }
    
    private string removeClone(string name)
    {
        string str = "";
        for(int x = 0; x< name.Length - 7; x++)
        {
            str += name.ToCharArray()[x];
        }
        return str;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
