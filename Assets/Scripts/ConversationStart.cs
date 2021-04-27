using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;

public class ConversationStart : MonoBehaviour
{
    public NPCConversation battleConvo;
    public static NPCConversation winLoseConvo;
    // Start is called before the first frame update
    void Start()
    {

        /*ConversationManager.Instance.StartConversation(conversation);
        if (ConversationManager.Instance != null)
        {
            print(CheckLevel.lvl);
            ConversationManager.Instance.SetInt("Levels", CheckLevel.lvl);
        }*/

        beginBattleConversation();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void beginBattleConversation()
    {
        ConversationManager.OnConversationEnded -= ConversationManager.OnConversationEnded;

        if (LevelChecker.lastClick == "John")
        {
            battleConvo = GameObject.Find("John_Battle").GetComponent<NPCConversation>();
        }
        else if (LevelChecker.lastClick == "DemonChick")
        {
            battleConvo = GameObject.Find("DemonChick_Battle").GetComponent<NPCConversation>();
        }
        else if (LevelChecker.lastClick == "Gabriel")
        {
            battleConvo = GameObject.Find("Gabriel_Battle").GetComponent<NPCConversation>();
        }

        if(battleConvo != null)
        ConversationManager.Instance.StartConversation(battleConvo);
    }

    public static void winBattleConvo()
    {
        if(CampaignManager.winTourBattle == 0)
        {
            if (LevelChecker.lastClick == "John")
            {
                winLoseConvo = GameObject.Find("John_win").GetComponent<NPCConversation>();
            }
            else if (LevelChecker.lastClick == "DemonChick")
            {
                winLoseConvo = GameObject.Find("Jheera_win").GetComponent<NPCConversation>();
            }
            else if (LevelChecker.lastClick == "Gabriel")
            {
                winLoseConvo = GameObject.Find("Gabriel_win").GetComponent<NPCConversation>();
            }
        }
        else
        {
            if (CampaignManager.winTourBattle == 1 && LevelChecker.lastClick == "John")
            {
                winLoseConvo = GameObject.Find("John_lose").GetComponent<NPCConversation>();
            }
            else if (CampaignManager.winTourBattle == 2 && LevelChecker.lastClick == "DemonChick")
            {
                winLoseConvo = GameObject.Find("Jheera_lose").GetComponent<NPCConversation>();
            }
            else if (CampaignManager.winTourBattle == 3 && LevelChecker.lastClick == "Gabriel")
            {
                winLoseConvo = GameObject.Find("Gabriel_lose").GetComponent<NPCConversation>();
            }
        }

        ConversationManager.Instance.StartConversation(winLoseConvo);
    }
}
