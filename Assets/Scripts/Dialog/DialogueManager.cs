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
        private string sceneName;
        public DialogueManagerOnEnd conversationEnded;
        GameObject background;

        public string dialogueScene;

        [SerializeField] List<Image> characters;
        private List<string> characterNames = new List<string>();

        //Character dialogue system 
        //public List<NPC> characterList;

        public List<NPCConversation> storyConversations;

        // Start is called before the first frame update
        void Start()
        {

            background = GameObject.Find("Canvas");
            NPCConversationToList();

            SetNPCConversation(FindByName("Conv_Intro"));
            GetConversationData(currentConversation);

            //Start conversation
            BeginConversation(currentConversation, "DialogScene");

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
            NPCConversation sceneConvo = storyConversations.Find(x => x.DefaultName.Equals(npcConvName));

            if (sceneConvo != null)
                dialogueScene = npcConvName;

            return sceneConvo;
        }

        //Populates a list will all the conversations present
        private void NPCConversationToList()
        {
            NPCConversation[] convs = GameObject.FindObjectsOfType<NPCConversation>();
            storyConversations = new List<NPCConversation>(convs);
        }

        //Sets assets of current dialogue scene based on story 
        public void SceneInterface(string screenInter)
        {
            switch (screenInter)
            {
                case "Conv_Opening":
                    characters[0].sprite = Resources.Load<Sprite>("Profile_Pictures/tribal_pix"); //Gabriel
                    characters[1].sprite = Resources.Load<Sprite>("Profile_Pictures/Faraday"); //Faraday
                    characters[2].sprite = Resources.Load<Sprite>("Profile_Pictures/Auriga"); //Auriga
                    characters[3].sprite = Resources.Load<Sprite>("Profile_Pictures/tribal_pix"); //John
                    characters[4].sprite = Resources.Load<Sprite>("Profile_Pictures/tribal_pix"); //Dio
                    characters[5].sprite = Resources.Load<Sprite>("Profile_Pictures/Jheera"); //Jheera
                    characters[6].sprite = Resources.Load<Sprite>("Profile_Pictures/tribal_pix"); //Azglor
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
                    break;
            }
        }

        //On dialog end move to following scene or go back to prev scene --Requires implementation depending on the story
        /*public void ConversationEnd(string name)
        {
            switch (name)
            {
                case "OpeningEnd":
                    SceneManager.LoadScene("Menu");
                    break;
                case "IntroEnd": 
                    SetNPCConversation(FindByName("Conv_Academy"));
                    BeginConversation(currentConversation, "DialogScene");
                    break;
                case "StartTutorial":
                    SceneManager.LoadScene("Tutorial1");
                    break;
            }
        }*/

        public void ConversationEnd()
        {
            string convname = currentConversation.DefaultName;

            switch (convname)
            {
                case "Conv_Academy":
                    SceneManager.LoadScene("Menu");
                    break;
                case "Conv_Intro":
                    SetNPCConversation(FindByName("Conv_Academy"));
                    BeginConversation(currentConversation, "DialogScene");
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
