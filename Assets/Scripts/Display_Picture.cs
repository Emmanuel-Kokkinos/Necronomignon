using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Display_Picture : MonoBehaviour
{
    [SerializeField] GameObject container;

    public List<Sprite> listOfPictures;
    public static Sprite displayPicture;
    public GameObject blurBackground;

    private void Start()
    {
        // Sets a default picture
        if (displayPicture == null)
        {
            displayPicture = listOfPictures[0]; // We can default to a random picture instead
        }

        if (blurBackground != null) blurBackground.SetActive(false);

        GameObject.Find("PictureButton").GetComponent<Image>().sprite = displayPicture;
    }

    // Sets current display picture to the chosen one and then updates the list
    public void ChangeDisplayPicture(int pictureNumber)
    {
        displayPicture = listOfPictures[pictureNumber];
        GameObject.Find("PictureButton").GetComponent<Image>().sprite = listOfPictures[pictureNumber];
        UpdatePictureList();
    }

    // Opens the list of possible display pictures to pick
    public void OpenAvailablePictures()
    {

        if (GameObject.Find("PictureContainer") != null) return;

        blurBackground.SetActive(true);

        GameObject parent = GameObject.Find("PicturePickHold");
        GameObject child = Instantiate(container, new Vector3(0, 0, 0), Quaternion.identity);

        child.name = "PictureContainer";
        child.transform.SetParent(parent.transform, false);

        Button closePick = (Button)GameObject.Find("closePickBtn").GetComponent<Button>();
        closePick.onClick.AddListener(ClosePick);

        //container.SetActive(!container.activeSelf);
        if (container.activeInHierarchy)
        {
            UpdatePictureList();
        }

    }

    // Updates the available pictures in the list to not include the currently chosen one
    void UpdatePictureList()
    {

        GameObject Content = GameObject.Find("Content");
        for (int x = 0; x < listOfPictures.Count; x++)
        {
            Content.transform.GetChild(x).gameObject.SetActive(true);
        }

        foreach(Sprite picture in listOfPictures)
        {
            GameObject picContain = new GameObject(); //Create the GameObject
            Image displayPicture = picContain.AddComponent<Image>(); //Add the Image Component script
            displayPicture.sprite = picture; //Set the Sprite of the Image Component on the new GameObject
            picContain.GetComponent<RectTransform>().SetParent(GameObject.Find("PicturePickHold").transform); //Assign the newly created Image GameObject as a Child of the Parent Panel.
            picContain.SetActive(true);
        }
        /*
        for (int x = 0; x < listOfPictures.Count; x++)
        {
            if (displayPicture == listOfPictures[x])
            {
                GameObject.Find("Content").transform.GetChild(x).gameObject.SetActive(false);
            }
        }
        */
    }

    public void ClosePick()
    {
        GameObject contain = GameObject.Find("PictureContainer");

        //if settings screen is not instantiated stop function
        if (contain == null) return;

        blurBackground.SetActive(false);

        Destroy(contain);
    }
}
