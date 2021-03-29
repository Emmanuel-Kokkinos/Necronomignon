using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using System.Runtime.Versioning;
using UnityEngine.UI;

/*
 *  This class handles the creation of a NPC Object from the Json File    
 *  it also links NPCDialogue objects to the NPC object
 */

[System.Serializable]
public class NPCManager : MonoBehaviour
{

    static int givenId = 100;
    string path;
    string jsonString;
    public NPCDialogueManager npcDialogueManager;

    //Sprite for the character
    public Image npcImage;

    public static NPCList npcList = new NPCList();

    // Start is called before the first frame update
    void Start()
    {
        ParseJsonData("/Scripts/Data/NPC_Dialogue/NPC.json");
    }

    //Parse data from json file
    void ParseJsonData(string dataPath)
    {
        path = Application.dataPath + dataPath;

        jsonString = File.ReadAllText(path);

        if (jsonString != null && npcList.NPCs.Count <= 0)
        {
            npcList = JsonUtility.FromJson<NPCList>(jsonString);
            foreach (NPC npc in npcList.NPCs)
            {
                // gives each npc a unique id
                npc.Npc_id = givenId;
                givenId++;
            }
        }

    }

    public bool isLoaded()
    {
        if (npcList.NPCs.Count == 0)
        {
            return false;
        }

        return true;
    }



    //Methods to get NPC
    public NPC GetNPC(string npcname)
    {
        if (!isLoaded())
            Start();

        NPC npc = new NPC();

        foreach(NPC chr in npcList.NPCs)
        {
            if (npcname.Equals(chr.Name))
            {
                npc = new NPC(chr);
                break;
            }
        }

        return npc;
    }

    public NPC GetNPC(NPC getNPC)
    {
        if (!isLoaded())
            Start();

        NPC npc = new NPC();

        foreach (NPC chr in npcList.NPCs)
        {
            if (getNPC.Equals(chr))
            {
                npc = new NPC(chr);
                break;
            }
        }

        return npc;
    }
}
