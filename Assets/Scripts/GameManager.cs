using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] GameObject HomePanel;
    public GameObject GameoverPanel, GamePanel;
    [SerializeField] Text Level_no_txt;
    public int Levelno = 1;
    public void Awake()
    {
        Instance = this;
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }
    //public void Onbtnclick(string buttonname)
    //{
    //    switch (buttonname)
    //    {
    //        case "Play":
    //            HomePanel.SetActive(false);
    //            CubesGanerate.Instance.LevelLoad();
    //            //CubesGanerate.Instance.Ganerate_cubes();
    //            //GamePanel.SetActive(true);//Temprory commented
    //            Level_no_txt.text = "Level " + Levelno;
    //            break;
    //        case "Next":
    //            //GameoverPanel.gameObject.SetActive(false);
    //            GameoverPanel.SetActive(false);
    //            //GameObject.Find("Canvas/Game_Overpanel").gameObject.SetActive(false);
    //            CubesGanerate.Instance.Ganerate_cubes();
    //            break;
    //        case "Test":
    //            CubesGanerate.Instance.Ganerate_cubes();
    //            CubesGanerate.Instance.SaveLevel(1);
    //            break;
    //    }
    //}
    public void Gameover()
    {
        //if (CubesGanerate.Instance.AllCubes.Count == 0)
        //{
        //    Debug.Log("Game Over");
        //    Levelno++;
        //    Level_no_txt.text = "Level " + Levelno;
        //    GameoverPanel.SetActive(true);
        //    Destroy(GameObject.FindGameObjectWithTag("Level"));
        //}
    }   
}
