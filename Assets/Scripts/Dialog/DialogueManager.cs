using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;
using UnityEngine.SceneManagement;

namespace DialogueEditor
{
    public class DialogueManager : MonoBehaviour
    {

        //main conversation scene variables
        private NPCConversation currentConversation;
        private string sceneName;
        public DialogueManagerOnEnd conversationEnded;


        public string dialogueScene;

        //Character dialogue system 
        //public List<NPC> characterList;

        public List<NPCConversation> storyConversations;

        // Start is called before the first frame update
        void Start()
        {
            NPCConversationToList();

            SetNPCConversation(FindByName("Conv_Intro"));

            //Start conversation
            BeginConversation(currentConversation, "DialogScene");
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
            print(screenInter);

            switch (screenInter)
            {

            }
        }
        //On dialog end move to following scene or go back to prev scene --Requires implementation depending on the story
        public void ConversationEnd(string name)
        {
            switch (name)
            {
                case "IntroEnd": 
                    SetNPCConversation(FindByName("Conv_Academy"));
                    BeginConversation(currentConversation, "DialogScene");
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

            Debug.Log(sceneName);

            if (sceneName == "DialogScene")
                ConversationManager.Instance.StartConversation(conversation);
        }
    }
}
