using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialDialogManager : MonoBehaviour
{
    //public NPCManager npcManager;
    //main conversation scene variables
    private NPCConversation currentConversation;
    private static int tutorialCounter = 0;
    private string sceneName;
    GameObject background;

    public string dialogueScene;

    [SerializeField] List<Image> characters;
    [SerializeField] List<NPCConversation> conversationNames;

    private List<string> characterNames = new List<string>();


    // Start is called before the first frame update
    void Start()
    {
        background = GameObject.Find("Canvas");
        NPCConversationToList();

        SetNPCConversation(FindByName(conversationNames[tutorialCounter].DefaultName));

        GetConversationData(currentConversation);

        //Start conversation
        BeginConversation(currentConversation, "Tutorial1");

        //Add conversation End Events
        ConversationManager.OnConversationEnded = new ConversationManager.ConversationEndEvent(ConversationEnd);
    }

    // ---SETS THE CONVERSATION OBJECTS IN SCENE

    //Sets the conversation progression in the scene 
    public void SetNPCConversation(NPCConversation dialogue)
    {
        currentConversation = dialogue;
    }
    //Gets NPC conversation by the name
    private NPCConversation FindByName(string npcConvName)
    {
        NPCConversation sceneConvo = conversationNames.Find(x => x.DefaultName.Equals(npcConvName));

        if (sceneConvo != null)
            dialogueScene = npcConvName;

        return sceneConvo;
    }

    //Populates a list will all the conversations present
    private void NPCConversationToList()
    {
        NPCConversation[] convs = GameObject.FindObjectsOfType<NPCConversation>();
        conversationNames = new List<NPCConversation>(convs);
    }

    //Sets assets of current dialogue scene based on story 
    public void SceneInterface(string screenInter)
    {
        switch (screenInter)
        {
            case "Tutorial1":
                characters[0].sprite = Resources.Load<Sprite>("Profile_Pictures/Dio");
                characters[1].sprite = Resources.Load<Sprite>("Profile_Pictures/Tadria");
                break;
            case "Tutorial2":
                characters[0].sprite = Resources.Load<Sprite>("Profile_Pictures/Dad");
                characters[1].sprite = Resources.Load<Sprite>("Profile_Pictures/Tadria");
                break;
            default:
                break;
        }
    }

    //On dialog end move to following scene or go back to prev scene --Requires implementation depending on the story
    public void ConversationEnd()
    {
        string convname = currentConversation.DefaultName;
        tutorialCounter++;

        switch (convname)
        {
            case "Tutorial1":
                SetNPCConversation(FindByName(conversationNames[tutorialCounter].DefaultName));
                BeginConversation(conversationNames[tutorialCounter], "Tutorial1");
                break;
            case "Tutorial2":
                SceneManager.LoadScene("Menu");
                break;
        }
    }

    //Conversation begins on scene
    public void BeginConversation(NPCConversation conversation, string setSceneName)
    {

        //Sets the scene Interface
        SceneInterface(dialogueScene);

        //sceneName = SceneManager.GetActiveScene().name;
        sceneName = setSceneName;

        if (sceneName == "Tutorial1")
            ConversationManager.Instance.StartConversation(conversation);
    }

    public void GetConversationData(NPCConversation conversation)
    {
        List<string> characterDupes = new List<string>();
        EditableConversation dataConv = conversation.DeserializeForEditor();

        dataConv.SpeechNodes.ForEach(node => characterDupes.Add(node.Name));
        foreach (string str in characterDupes)
        {
            if (!characterNames.Contains(str) && str != "")
            {
                characterNames.Add(str);
            }
        }
    }
}
