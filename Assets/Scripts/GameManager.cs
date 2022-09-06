using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] GameObject HomePanel;
    public GameObject GameoverPanel, GamePanel;
    [SerializeField] Text Level_no_txt;
    public int Levelno = 1;
    public List<GameObject> m_allLevels = new List<GameObject>();
    public GameObject m_Drag;


    public void Awake() => Instance = this;

    private void Start()
    {
        Levelno = 0;
        StartGame();
    }
    void StartGame()
    {
        m_Drag = Instantiate(m_allLevels[Levelno], transform.position, Quaternion.identity);
        var temp = m_Drag.transform.position;
        temp.z = 0;
        m_Drag.transform.position = temp;
    }
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
    public void ChangeLvl()
    {
        Debug.Log(CheckNext(m_Drag.transform));
        if (CheckNext(m_Drag.transform) == true)
        {
            Levelno += 1;
            StartGame();
        }
    }
    internal float orbitX = 0f;
    internal float orbitY = 0f;
    internal float orbitZ = 0f;
    public float orbitXSpeed = 50f;
    public float orbitYSpeed = 50f;
    public float orbitSmooth = 10f;
    public void OnDrag(PointerEventData pointerData)
    {
        // Receiving drag input from UI.
        orbitX += pointerData.delta.x * orbitXSpeed / 1000f;
        orbitY -= pointerData.delta.y * orbitYSpeed / 1000f;
        orbitZ += pointerData.delta.x * orbitYSpeed / 1000f;
    }
    public void LateUpdate()
    {
        if (m_Drag)
            m_Drag.transform.localRotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(-orbitY, 0, orbitZ), orbitSmooth);
    }
    public bool CheckNext(Transform m_obj)
    {
        if (m_obj.transform.childCount <= 1)
        {
            Destroy(m_obj.gameObject);
            return true;
        }
        else
        {
            return false;
        }
    }

}
