
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections;

namespace DialogueEditor
{
    public class DialogueManager : MonoBehaviour
    {
        //public NPCManager npcManager;
        //main conversation scene variables
        private NPCConversation currentConversation;
        private static int timesUsedCounter;
        private string sceneName;
        GameObject background;

        public string dialogueScene;

        [SerializeField] List<Image> characters;
        [SerializeField] List<NPCConversation> conversationNames;
        
        private List<string> characterNames = new List<string>();
        private List<Sprite> characterAssets = new List<Sprite>();


        // Start is called before the first frame update
        void Start()
        {
            background = GameObject.Find("Canvas");

            //Sets all conversations to list
            NPCConversationToList();

            //Last item of list
            timesUsedCounter = conversationNames.Count - 1;

            //Sets the default conversation
            SetNPCConversation(FindByName(conversationNames[timesUsedCounter].DefaultName));
            //SetNPCConversation(FindByName("Conv_Graduation"));

            //Gets the data associated with conversation for further edit
            GetConversationData(currentConversation);

            //Start conversation and Initializes addressable components to load resources
            StartCoroutine(LoadConversationAsync());
            

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

        /*Populates a list will all the conversations present
         * ConversationNames list has all conversation reversed from the order they appear in inspector
         * thus: last item in list == first conversation in inspector
         */
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
                image.gameObject.transform.localScale = new Vector3(1, 1);
            }

            switch (screenInter)
            {
                case "Conv_Opening":

                    //Change for setCharacterActive Method later
                    for (int x = 4; x <= 7; x++)
                    {
                        characters[x].gameObject.SetActive(true);
                    }

                    characters[0].sprite = characterAssets.Find(x => x.name.Equals("Gabriel")); //Gabriel
                    characters[1].sprite = characterAssets.Find(x => x.name.Equals("Faraday")); //Faraday
                    characters[2].sprite = characterAssets.Find(x => x.name.Equals("Auriga")); //Auriga
                    characters[3].sprite = characterAssets.Find(x => x.name.Equals("John")); //John
                    characters[4].sprite = characterAssets.Find(x => x.name.Equals("Dio")); //Dio
                    characters[5].sprite = characterAssets.Find(x => x.name.Equals("Jheera")); //Jheera
                    characters[6].sprite = characterAssets.Find(x => x.name.Equals("Neput")); //Neput
                    characters[7].sprite = characterAssets.Find(x => x.name.Equals("Azglor")); //Azglor
                    characters[8].sprite = characterAssets.Find(x => x.name.Equals("Tadria")); //Instructor
                    break;
                case "Conv_Intro":

                    for(int x = 4; x <= 7; x++)
                    {
                        characters[x].gameObject.SetActive(false);
                    }

                    characters[0].sprite = characterAssets.Find(x => x.name.Equals("Dad")); //dad
                    characters[1].sprite = characterAssets.Find(x => x.name.Equals("Catherine")); //sister
                    characters[1].transform.localScale = new Vector3(.8f, .8f);
                    characters[2].sprite = characterAssets.Find(x => x.name.Equals("Ari")); //brother
                    characters[2].transform.localScale = new Vector3(.8f, .8f);
                    characters[3].sprite = characterAssets.Find(x => x.name.Equals("Mom")); //mom
                    characters[8].sprite = characterAssets.Find(x => x.name.Equals("Tadria")); //instructor

                    //Sets background of intro conversation
                    background.GetComponent<Image>().sprite = Resources.Load<Sprite>("Background_Pics/housePX");
                    break;
                case "Conv_Academy":

                   for (int x = 4; x <= 7; x++)
                    {
                        characters[x].gameObject.SetActive(false);
                    }
                    break;
                    //Moved tutorial to dialogue manager so it can be loaded directly from dialogue
                case "Tutorial1":
                    characters[0].sprite = characterAssets.Find(x => x.name.Equals("Dio"));
                    characters[1].sprite = characterAssets.Find(x => x.name.Equals("Tadria"));
                    break;
                case "Tutorial2":
                    characters[0].sprite = characterAssets.Find(x => x.name.Equals("Dad"));
                    characters[1].sprite = characterAssets.Find(x => x.name.Equals("Tadria"));
                    break;
                case "Conv_Tour":

                    if (CampaignManager.dialogueEnd == false) {
                        characters[0].gameObject.SetActive(true);
                        for (int x = 1; x <= 8; x++)
                        {
                            characters[x].gameObject.SetActive(false);
                        }
                        characters[0].sprite = characterAssets.Find(x => x.name.Equals("Tadria"));
                        characters[1].sprite = characterAssets.Find(x => x.name.Equals("Azglor")); //Azglor
                        characters[2].sprite = characterAssets.Find(x => x.name.Equals("Dio")); //Dio
                        characters[3].sprite = characterAssets.Find(x => x.name.Equals("Jheera")); //Jheera
                        characters[4].sprite = characterAssets.Find(x => x.name.Equals("John")); //John
                        characters[5].sprite = characterAssets.Find(x => x.name.Equals("Auriga")); //Auriga
                        characters[6].sprite = characterAssets.Find(x => x.name.Equals("Gabriel")); //Gabriel
                        characters[7].sprite = characterAssets.Find(x => x.name.Equals("Neput")); //Neput
                        characters[8].sprite = characterAssets.Find(x => x.name.Equals("Faraday")); //Faraday
                    }

                    
                    break;

                case "Questionnaire":
                    characters[0].sprite = characterAssets.Find(x => x.name.Equals("Gabriel")); //Gabriel
                    characters[1].sprite = characterAssets.Find(x => x.name.Equals("Azglor")); //Azglor
                    characters[2].sprite = characterAssets.Find(x => x.name.Equals("Auriga")); //Auriga
                    characters[3].sprite = characterAssets.Find(x => x.name.Equals("John")); //John
                    characters[4].sprite = characterAssets.Find(x => x.name.Equals("Dio")); //Dio
                    characters[5].sprite = characterAssets.Find(x => x.name.Equals("Jheera")); //Jheera
                    characters[6].gameObject.SetActive(false);
                    characters[7].sprite = characterAssets.Find(x => x.name.Equals("Irvina")); //Irvina
                    characters[7].gameObject.transform.localScale = new Vector3(1.3f, 1.3f);
                    characters[8].sprite = characterAssets.Find(x => x.name.Equals("Thoth")); //Thoth
                    break;
                case "Conv_Graduation":
                    characters[0].sprite = characterAssets.Find(x => x.name.Equals("Tadria")); //Gabriel
                    for (int x = 1; x < 8; x++)
                    {
                        if(characters[x].gameObject != null)
                            characters[x].gameObject.SetActive(false);
                    }
                    characters[8].sprite = characterAssets.Find(x => x.name.Equals("Dio"));
                    break;
                default:

                    break;
            }
        }

        //On dialog end move to following scene or go back to prev scene --Requires implementation depending on the story
        public void ConversationEnd()
        {
            string convname = currentConversation.DefaultName;
            timesUsedCounter--;

            switch (convname)
            {
                case "Conv_Academy":
                    SetNPCConversation(FindByName("Conv_Opening"));
                    BeginConversation(currentConversation, "DialogScene");
                    break;
                case "Conv_Intro":
                    SetNPCConversation(FindByName(conversationNames[timesUsedCounter].DefaultName));
                    BeginConversation(currentConversation, "DialogScene");
                    break;
                case "Conv_Opening":
                    SetNPCConversation(FindByName("Conv_Tour"));
                    BeginConversation(currentConversation, "Tournament");
                    break;
                case "Tutorial1":
                    SceneManager.LoadScene("Menu");
                    break;
                case "Conv_Tour":
                    for(int x = 0; x <= 8; x++)
                    {
                        characters[x].gameObject.SetActive(false);
                    }
                    CampaignManager.firstMission.SetActive(true);
                    CampaignManager.dialogueEnd = true;
                    break;
                case "Conv_Graduation":
                    LoadScenes load = gameObject.AddComponent<LoadScenes>();
                    load.LoadSelect("Tournament");
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

            if (sceneName != "DialogScene")
                SceneManager.LoadScene(sceneName);

            if(conversation != null)
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

        //Import characters assets and begin conversation
        public IEnumerator LoadConversationAsync()
        {
            //Gets all the addressable character sprites
            AsyncOperationHandle<IList<Sprite>> characterSprites = Addressables.LoadAssetsAsync<Sprite>("GameNPC", spr => { Debug.Log(spr.name); });

            yield return new WaitUntil(() => characterSprites.IsDone);

            //Sets character assets list to set the sprites in scene
            characterAssets = (List<Sprite>)characterSprites.Result;

            print(characterAssets.Count);

            //Begins conversation at end of corroutine
            BeginConversation(currentConversation, "DialogScene");
        }

        public void CharacterOutOfScene(int charId)
        {
            characters[charId].gameObject.SetActive(false);
        }

        public void CharacterReplace(string character)
        {
            characters[0].sprite = characterAssets.Find(x => x.name.Equals(character));
        }

        public void CharacterOnScene(int charId)
        {
            characters[charId].gameObject.SetActive(true);
        }

    }
}
