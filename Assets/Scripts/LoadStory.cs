using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadStory : MonoBehaviour
{
    int beastIndex = 0;
    public Text txt;
    public Text title;
    int counter = 0;
    public LoadScenes scenes;
    // Start is called before the first frame update
    void Start()
    {

        if (!SummonBookLoader.hasAStory(SummonManager.name))
        {
            scenes.LoadSelect("BeastView");
        }
        else
        {
            List<Story> stoList = StoryManager.storyList.Stories;
            for (; beastIndex < stoList.Count; beastIndex++)
            {
                if(SummonManager.name != null && SummonManager.name == stoList[beastIndex].beast_name)
                {
                    break;
                }
            }
            txt.text = stoList[beastIndex].pages[0];
            title.text = SummonBookLoader.CamelCaseCorrection(stoList[beastIndex].title);
        }
    }

    // Update is called once per frame
    void Update()
    {
        txt.text = StoryManager.storyList.Stories[beastIndex].pages[counter];
    }
    public void counterUp()
    {

        if (counter + 1 >= StoryManager.storyList.Stories[beastIndex].pages.Count)
        {
            foreach(Beast b in BeastManager.beastsList.Beasts)
            {
                if(b.name == SummonManager.name && b.tier == 0)
                {
                    b.tier = 1;
                }
            }
            scenes.LoadSelect("BeastStats");
        }
        else
        {
            counter++;
        }
    }
    public void counterDown()
    {

        if (counter - 1 < 0)
        {
            scenes.LoadSelect("SummonMain");
        }
        else
        {
            counter--;
        }
    }
}
