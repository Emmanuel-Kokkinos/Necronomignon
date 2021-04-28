using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DragonBones;

//This script manages the whole tutorial. Each tutorial scene is hard coded in order
//to keep the whole battle algorithm intact

public class TutorialManager : MonoBehaviour
{
    Scene scene;
    public GameObject pawn;
    public GameObject knight;
    public GameObject rook;
    public GameObject king;
    public GameObject mando;
    public GameObject dream;
    public GameObject wyvernSpot;
    public GameObject wyvern;
    public GameObject exit;
    
    void Start()
    {
        scene = SceneManager.GetActiveScene();
        dream.GetComponent<UnityArmatureComponent>().animation.Play("Idle", 0);
        mando.GetComponent<UnityArmatureComponent>().animation.Play("Idle", 0);
        exit.SetActive(false);
        if (scene.name == "Tutorial3")
        {
            StartCoroutine(Tutorial3Start());
        }
    }

    public void Tutorial1Click()
    {
        Debug.Log("Behemoth does Bite Attack.");
        StartCoroutine(WaitAndLoad("Tutorial2"));
    }

    int tutorial2Clicks = 0;

    public void Tutorial2Click1()
    {
        Debug.Log("Behemoth does fireball attack.");
        GameObject.Find("btnKnight").SetActive(false);
        tutorial2Clicks += 1;
        if (tutorial2Clicks == 2) StartCoroutine(WaitAndLoad("Tutorial3"));
    }

    public void Tutorial2Click2()
    {
        Debug.Log("Trogdor does melee attack.");
        GameObject.Find("btnPawn").SetActive(false);
        tutorial2Clicks += 1;
        if (tutorial2Clicks == 2) StartCoroutine(WaitAndLoad("Tutorial3"));
    }

    public void Tutorial3Click()
    {
        Debug.Log("Air Spector does powerful attack!");
        StartCoroutine(WaitAndLoad("Tutorial4"));
    }

    IEnumerator Tutorial3Start()
    {
        yield return new WaitForSeconds(1);
        Debug.Log("Rook melee attacked Behemoth.");
        GameObject.Find("btnPawn").GetComponent<Button>().interactable = true;
        GameObject.Find("btnKnight").GetComponent<Button>().interactable = true;
        GameObject.Find("btnRook").GetComponent<Button>().interactable = true;
    }

    public void Tutorial4Click()
    {
        Debug.Log("Gaia uses healing powers to heal it's teammates!");
        StartCoroutine(WaitAndLoad("TutorialEnd"));
    }

    IEnumerator WaitAndLoad(string nextScene)
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(nextScene);
    }

    public void mandoAttack()
    {

        UnityArmatureComponent animator = new UnityArmatureComponent() ;
        if (mando != null)
        {
            animator = mando.GetComponent<UnityArmatureComponent>();
        }
        animator.animation.Play("Front", 1);
        pawn.SetActive(false);
    }
    public void dreamAttack()
    {

        UnityArmatureComponent animator = new UnityArmatureComponent();
        if (dream != null)
        {
            animator = dream.GetComponent<UnityArmatureComponent>();
        }
        animator.animation.Play("Front",1);
        
        StartCoroutine(waitAttack());
    }
    public void wyvernAttack()
    {

        UnityArmatureComponent animator = new UnityArmatureComponent();
        if (dream != null)
        {
            animator = wyvern.GetComponent<UnityArmatureComponent>();
        }
        animator.animation.Play("Front",1);

        StartCoroutine(waitAttackW());
    }

    IEnumerator waitAttack()
    {
        //yield return new WaitUntil(() => dream.GetComponent<UnityArmatureComponent>().animation.isCompleted);
        yield return new WaitForSeconds(3f);
        dream.GetComponent<UnityArmatureComponent>().animation.Play("Idle", 0);
        knight.SetActive(false);
        UnityEngine.Transform transform = rook.GetComponent<UnityEngine.Transform>();
        for (int x = 10; x > 0; x--)
        {
            transform.position += Vector3.left * x;
            yield return new WaitForSeconds(.03f);
            transform.position += (Vector3.right * (x * 2));
            yield return new WaitForSeconds(.03f);
            transform.position += Vector3.left * x;
            yield return new WaitForSeconds(.03f);
        }
        
    }
    IEnumerator waitAttackW()
    {
        yield return new WaitForSeconds(.5f);
        rook.SetActive(false);
        king.SetActive(false);

    }

    public void unlockExit()
    {
        exit.SetActive(true);
    }

    public void PlaceWyvern()
    {
        wyvernSpot.gameObject.SetActive(true);
    }
}
