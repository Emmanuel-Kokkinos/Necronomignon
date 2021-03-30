using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DialogueEditor
{
    public class DialogueManager : MonoBehaviour
    {
        //public NPCManager npcManager;
        //main conversation scene variables
        private NPCConversation currentConversation;
        private static int timesUsedCounter = 0;
        private string sceneName;
        public DialogueManagerOnEnd conversationEnded;
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

            //SetNPCConversation(FindByName(conversationNames[timesUsedCounter].DefaultName));
            //SetNPCConversation(FindByName("Conv_Opening"));
            SetNPCConversation(FindByName(conversationNames[conversationNames.Count-1].DefaultName));
            GetConversationData(currentConversation);

            //Start conversation
            //BeginConversation(currentConversation, SceneManager.GetActiveScene().name);
            BeginConversation(currentConversation, "DialogScene");
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
            foreach(Image image in characters)
            {
                image.gameObject.SetActive(true);
            }
            switch (screenInter)
            {
                case "Conv_Opening":
                    characters[0].sprite = Resources.Load<Sprite>("Profile_Pictures/tribal_pix"); //Gabriel
                    characters[1].sprite = Resources.Load<Sprite>("Profile_Pictures/Faraday"); //Faraday
                    characters[2].sprite = Resources.Load<Sprite>("Profile_Pictures/Auriga"); //Auriga
                    characters[3].sprite = Resources.Load<Sprite>("Profile_Pictures/tribal_pix"); //John
                    characters[4].sprite = Resources.Load<Sprite>("Profile_Pictures/tribal_pix"); //Dio
                    characters[5].sprite = Resources.Load<Sprite>("Profile_Pictures/Jheera"); //Jheera
                    characters[6].sprite = Resources.Load<Sprite>("Profile_Pictures/Azglor"); //Azglor
                    characters[7].sprite = Resources.Load<Sprite>("Profile_Pictures/tribal_pix"); //Neput
                    characters[8].sprite = Resources.Load<Sprite>("Profile_Pictures/sea_soldier_pix"); //Tadria
                    break;
                case "Conv_Intro":
                    for(int x = 4; x <= 7; x++)
                    {
                        characters[x].gameObject.SetActive(false);
                    }

                    characters[0].sprite = Resources.Load<Sprite>("Profile_Pictures/Dad"); //dad
                    characters[1].sprite = Resources.Load<Sprite>("Profile_Pictures/Catherine"); //sister
                    characters[1].transform.localScale = new Vector3(.8f, .8f);
                    characters[2].sprite = Resources.Load<Sprite>("Profile_Pictures/Ari"); //brother
                    characters[2].transform.localScale = new Vector3(.8f, .8f);
                    characters[3].sprite = Resources.Load<Sprite>("Profile_Pictures/Mom"); //mom
                    characters[8].sprite = Resources.Load<Sprite>("Profile_Pictures/sea_soldier_pix"); //instructor

                    
                    background.GetComponent<Image>().sprite = Resources.Load<Sprite>("Background_Pics/housePX");
                    break;
                case "Conv_Academy":

                    // Can remove all of this code as nothing changes from the previous conversation
                    // But I kept it at least for now in case we want there to be a difference
                    for (int x = 4; x <= 7; x++)
                    {
                        characters[x].gameObject.SetActive(false);
                    }

                    characters[0].sprite = Resources.Load<Sprite>("Profile_Pictures/Dad"); //dad
                    characters[1].sprite = Resources.Load<Sprite>("Profile_Pictures/Catherine"); //sister
                    characters[1].transform.localScale = new Vector3(.8f, .8f);
                    characters[2].sprite = Resources.Load<Sprite>("Profile_Pictures/Ari"); //brother
                    characters[2].transform.localScale = new Vector3(.8f, .8f);
                    characters[3].sprite = Resources.Load<Sprite>("Profile_Pictures/Mom"); //mom
                    characters[8].sprite = Resources.Load<Sprite>("Profile_Pictures/sea_soldier_pix"); //instructor
                    break;
                default:
                    break;
            }
        }

        //On dialog end move to following scene or go back to prev scene --Requires implementation depending on the story

        public void ConversationEnd()
        {
            string convname = currentConversation.DefaultName;
            timesUsedCounter++;

            switch (convname)
            {
                case "Conv_Academy":
                    SceneManager.LoadScene("Menu");
                    break;
                case "IntroEnd": 
                    SetNPCConversation(FindByName(conversationNames[timesUsedCounter].DefaultName));
                    //SetNPCConversation(FindByName("Conv_Academy"));
                    break;
                case "Conv_Intro":
                    SetNPCConversation(FindByName(conversationNames[timesUsedCounter].DefaultName));
                    //BeginConversation(currentConversation, "DialogScene");
                    break;
                case "Conv_Opening":
                    SceneManager.LoadScene("Tutorial1");
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

            if (sceneName == "DialogScene")
                ConversationManager.Instance.StartConversation(conversation);
        }

        public void GetConversationData(NPCConversation conversation)
        {
            List<string> characterDupes = new List<string>();
            EditableConversation dataConv = conversation.DeserializeForEditor();

            dataConv.SpeechNodes.ForEach(node => characterDupes.Add(node.Name));
            foreach(string str in characterDupes)
            {
                if(!characterNames.Contains(str) && str != "")
                {
                    characterNames.Add(str);
                }
            }
        }
    }
}
