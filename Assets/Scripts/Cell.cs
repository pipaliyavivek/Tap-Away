using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FACE_TO { LEFT, RIGHT, UP, DOWN, FORWORD, BACKWORD }
public class Cell : MonoBehaviour
{
    public bool isDirty;
    public FACE_TO m_faceTo;
    public int Index;
    private bool Move;
    Tween m_shake;
    internal Vector3 Finalquaternion;
    internal Vector3 BeforeRotationPos;

    public GameObject m_Visual;
    public bool IsMiddelCell = false;
    [SerializeField] public List<Cell> X_cells = new List<Cell>();
    [SerializeField] public List<Cell> Y_cells = new List<Cell>();
    [SerializeField] public List<Cell> Z_cells = new List<Cell>();

    public Vector3 Direction;
    private void FixedUpdate()
    {
        if (Move)
        {
            transform.position += transform.TransformDirection(Vector3.forward) * (Time.deltaTime * 20);
        }
    }
    public void OnMouseUpAsButton()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 3))
        {
            if (hit.distance > 1)
            {
                Move_Cubes((int)hit.distance);
            }
            else
            {
                ShakeinDirection(transform.TransformDirection(Vector3.forward));
            }
        }
        else
        {
            Move = true;

            if (GridEditor.Instance.cells.Contains(this))
            {
                GridEditor.Instance.cells.Remove(this);
            }
            if (GridEditor.Instance.cells.Count == 0)
            {
                //Win
                Debug.Log("You Win!!!");
                DOVirtual.DelayedCall(0.7f, () =>
                {
                    GridEditor.Instance.ChangeLvl();
                });
            }

            Destroy(gameObject, 1f);
        }
    }
    public void ShakeinDirection(Vector3 Dir)
    {
        Shake();
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Dir, out hit))
        {
            hit.collider.GetComponent<Cell>().ShakeinDirection(Dir);
        }
    }
    public void Shake()
    {
        m_shake?.Kill(true);
        m_shake = transform.DOShakePosition(0.5f, 0.1f, 1, 50, false);
    }
    public void Move_Cubes(int distance)
    {
        transform.DOMove(transform.position + (transform.TransformDirection(Vector3.forward) * distance), 0.02f);
    }
    public void SetDirection(FACE_TO i_face)
    {
        m_faceTo = i_face;
        switch (i_face)
        {
            case FACE_TO.LEFT:
                Direction = -Vector3.up;
                break;
            case FACE_TO.RIGHT:
                Direction = Vector3.up;
                break;
            case FACE_TO.UP:
                Direction = -Vector3.right;
                break;
            case FACE_TO.DOWN:
                Direction = Vector3.right;
                break;
            case FACE_TO.FORWORD:
                Direction = Vector3.forward;
                break;
            case FACE_TO.BACKWORD:
                Direction = new Vector3(0, -2, 0);
                break;
            default:
                break;
        }
        transform.rotation = Quaternion.Euler(Direction * 90);
    }
    [Button]
    public void Check()
    {
        /* if (Physics.Raycast(transform.position + (Vector3.one * 0.5f), transform.TransformDirection(Vector3.forward), out RaycastHit hit))//Forword
         {
             Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward), Color.blue, 200f, true);
             var m_OtherCell = hit.collider.GetComponent<Cell>();
             if (m_OtherCell.m_faceTo == GetOppsiteFace(m_faceTo))
             {
                 m_OtherCell.SetDirection(m_faceTo);
                 m_OtherCell.Check();
             }
         }*/
        RaycastHit[] hit = Physics.RaycastAll(transform.position, transform.TransformDirection(Vector3.forward), 50);//forward
        if (hit.Length > 0)
        {
            objects = new GameObject[hit.Length];
            for (int i = 0; i < hit.Length; i++)
            {
                objects[i] = hit[i].collider.gameObject;
                if (objects[i].GetComponent<Cell>().m_faceTo == GetOppsiteFace(m_faceTo))
                {
                    Debug.Log(objects[i].name);
                    // objects[i].GetComponent<Cell>().SetDirection(m_faceTo);
                    // objects[i].GetComponent<Cell>().Check();
                    //m_OtherCell.SetDirection(m_faceTo);
                    //m_OtherCell.Check();
                }
                else
                {
                   // Debug.Log(objects[i].name);
                }
                //  objects[i].GetComponent<Cell>().SetDirection(GetOppsiteFace(m_faceTo));
            }
        }
    }
    [ShowInInspector] public GameObject[] objects;
    public FACE_TO GetOppsiteFace(FACE_TO l_FaceTo)
    {
        switch (l_FaceTo)
        {
            case FACE_TO.LEFT:
                l_FaceTo = FACE_TO.RIGHT;
                break;
            case FACE_TO.RIGHT:
                l_FaceTo = FACE_TO.LEFT;
                break;
            case FACE_TO.UP:
                l_FaceTo = FACE_TO.DOWN;
                break;
            case FACE_TO.DOWN:
                l_FaceTo = FACE_TO.UP;
                break;
            case FACE_TO.FORWORD:
                l_FaceTo = FACE_TO.BACKWORD;
                break;
            case FACE_TO.BACKWORD:
                l_FaceTo = FACE_TO.FORWORD;
                break;
            default:
                break;
        }
        return l_FaceTo;
    }
    //public void CheckAndChange()
    //{
    //    if (X_cells.Count > 0)
    //    {
    //        for (int i = 0; i < X_cells.Count; i++)
    //        {
    //            if (X_cells[i].m_faceTo == m_faceTo)
    //            {
    //                X_cells[i].RandomSides(m_faceTo);
    //            }
    //        }
    //    }
    //    if (Y_cells.Count > 0)
    //    {
    //        for (int j = 0; j < Y_cells.Count; j++)
    //        {
    //            if (Y_cells[j].m_faceTo == m_faceTo)
    //            {
    //                Y_cells[j].RandomSides(m_faceTo);
    //            }
    //        }
    //    }
    //    if (Z_cells.Count > 0)
    //    {
    //        for (int k = 0; k < Z_cells.Count; k++)
    //        {
    //            if (Z_cells[k].m_faceTo == m_faceTo)
    //            {
    //                Z_cells[k].RandomSides(m_faceTo);
    //            }
    //        }
    //    }
    //}
}
