using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class GridEditor : MonoBehaviour
{
    public static GridEditor Instance;
    [SerializeField] public List<Cell> cells = new List<Cell>();

    public List<Vector3> m_Lvl = new List<Vector3>();

    public TextMeshProUGUI m_CurrentLvl;
    public GameObject CellPrefab;
    public Vector3 Distance;

    private int LvlNo;
    void Awake()
    {
        Instance = this;
    }
    [Button]
    void LvlAdd(int l_No)
    {
        m_Lvl.Clear();
        for (int i = 0; i < l_No; i++)
        {
            m_Lvl.Add(new Vector3(Random.Range(1, 5), Random.Range(1, 5), 0f));
        }
    }
    [Button]
    void Start()
    {
        LvlNo = 0;
        ChangeLvl();
        DOTween.SetTweensCapacity(2500, 100);
    }
    public void ChangeLvl()
    {
        m_CurrentLvl.text = "Level:" + LvlNo;
        LvlNo++;
        MakeGrid(m_Lvl[LvlNo]);
        Debug.Log("Current:" + m_Lvl[LvlNo]);
    }
    public void LateUpdate()
    {
        transform.localRotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(-orbitY, 0, orbitZ), orbitSmooth);
    }
    public Vector3 FindCenterOfTransforms(List<Transform> transforms)
    {
        var bound = new Bounds(transforms[0].position, Vector3.zero);
        for (int i = 1; i < transforms.Count; i++)
        {
            bound.Encapsulate(transforms[i].position);
        }
        return bound.center;
    }

    [Button]
    void MakeGrid(Vector3 l_V)
    {
        cells.Clear();
        foreach (Cell item in cells)
        {
            if (item == null) return;
            Destroy(item.gameObject);
        }
        StartCoroutine(Add3DGrid(l_V));
    }

    public IEnumerator Add3DGrid(Vector3 Grid)
    {
        cells.Clear();
        //transform.GetChild(0).position = new Vector3((Grid.x - 2) , (Grid.y - 2), (Grid.z - 2))*-1;
        for (int i = 0; i < Grid.x; i++)
        {
            for (int j = 0; j < Grid.y; j++)
            {
                for (int k = 0; k < Grid.z; k++)
                {
                    GameObject cell = PrefabUtility.InstantiatePrefab(CellPrefab, transform) as GameObject;
                    cell.transform.localPosition = new Vector3(Distance.x * k, Distance.y * j, Distance.z * i);
                    var CellScr = cell.GetComponent<Cell>();
                    CellScr.Finalquaternion = cell.transform.position;
                    CellScr.BeforeRotationPos = new Vector3(3 * k, 3 * j, 3 * i);
                    cells.Add(CellScr);
                    cell.name = "" + i + "" + j + "" + k;
                    //if (Grid.x - 2 == i && j == k && i == j) CellScr.IsMiddelCell = true;
                    CellScr.m_Visual.SetActive(false);
                    /*cell.transform.rotation = Quaternion.Euler(0f,Random.Range(0f,180f),0f);*/
                    FindCellsNeighber();
                    //  Effect(cell,k,j,i,new Vector3(3* k, 3*j,3*i),CellScr);
                }
            }
        }
        List<Transform> temp = new List<Transform>();
        cells.ForEach(x => temp.Add(x.transform));
        Vector3 centre = FindCenterOfTransforms(temp);
        temp.ForEach(x => x.parent = null);
        transform.position = centre;
        temp.ForEach(x => x.parent = transform);
        yield return null;
        ALLSETUP();
    }
    [Button]
    private void Effect(Cell l_Cell)
    {
        l_Cell.transform.localPosition = l_Cell.BeforeRotationPos;
        l_Cell.transform.rotation = Quaternion.Euler(0f, Random.Range(0f, 180f), 0f);
        l_Cell.m_Visual.SetActive(true);
        l_Cell.transform.DOMove(l_Cell.Finalquaternion, 1f).SetEase(Ease.OutCubic);
        l_Cell.transform.DORotateQuaternion(Quaternion.Euler(l_Cell.Direction * 90), 1f);        //1.3f
        //l_Cell.transform.DOMove(Vector3.zero, 1f).SetEase(Ease.InSine);
        //l_Cell.transform.DORotateQuaternion(Quaternion.Euler(l_Cell.Direction * 90),1f).SetEase(Ease.InBounce).OnComplete(()=> { ArrangeCells(); });       
    }
    [Button]
    private void RenameAll()
    {
        for (int i = 0; i < cells.Count; i++)
        {
            cells[i].gameObject.name = (i + 1).ToString();
            cells[i].Index = i;
        }
    }
    [Button]
    public void FillDate()
    {
        cells.Clear();
        for (int i = 0; i < transform.childCount; i++)
        {
            cells.Add(transform.GetChild(i).GetComponent<Cell>());
        }
    }
    public void ArrangeCells()
    {
        for (int i = 0; i < cells.Count; i++)
        {
            for (int j = 0; j < cells.Count; j++)
            {
                cells[j].Check();
                Effect(cells[j]);
            }
            if (!cells[i].isDirty)
            {
                if (cells[i].X_cells.Count > 1)
                {
                    for (int j = 0; j < cells[i].X_cells.Count; j++)
                    {
                        if (i > cells[i].X_cells[j].Index)
                        {
                            List<FACE_TO> facess = RandomFaces(cells[i].m_faceTo);
                            cells[i].SetDirection(facess[Random.Range(0, facess.Count)]);
                        }
                    }
                }
                if (cells[i].Y_cells.Count > 1)
                {
                    for (int j = 0; j < cells[i].Y_cells.Count; j++)
                    {
                        if (i > cells[i].Y_cells[j].Index)
                        {
                            List<FACE_TO> facess = RandomFaces(cells[i].m_faceTo);
                            cells[i].SetDirection(facess[Random.Range(0, facess.Count)]);
                        }
                    }
                }
                if (cells[i].Z_cells.Count > 1)
                {
                    for (int j = 0; j < cells[i].Z_cells.Count; j++)
                    {
                        if (i > cells[i].Z_cells[j].Index)
                        {
                            List<FACE_TO> facess = RandomFaces(cells[i].m_faceTo);
                            cells[i].SetDirection(facess[Random.Range(0, facess.Count)]);
                        }
                    }
                }
                if (cells[i].X_cells.Count > 1)
                {
                    for (int j = 0; j < cells[i].X_cells.Count; j++)
                    {
                        List<FACE_TO> facess = RandomFaces(cells[i].m_faceTo);
                        cells[i].X_cells[j].SetDirection(facess[Random.Range(0, facess.Count)]);
                        cells[i].X_cells[j].isDirty = true;
                        cells[i].X_cells[j].isDirty = true;
                    }
                }
                if (cells[i].Y_cells.Count > 1)
                {
                    for (int j = 0; j < cells[i].Y_cells.Count; j++)
                    {
                        List<FACE_TO> facess = RandomFaces(cells[i].m_faceTo);
                        cells[i].X_cells[j].SetDirection(facess[Random.Range(0, facess.Count)]);
                        cells[i].X_cells[j].isDirty = true;
                    }
                }
                if (cells[i].Z_cells.Count > 1)
                {
                    for (int j = 0; j < cells[i].Z_cells.Count; j++)
                    {
                        List<FACE_TO> facess = RandomFaces(cells[i].m_faceTo);
                        cells[i].X_cells[j].SetDirection(facess[Random.Range(0, facess.Count)]);
                        cells[i].X_cells[j].isDirty = true;
                    }
                }
            }
        }
    }
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
    public List<FACE_TO> RandomFaces(FACE_TO l_FaceTo)
    {
        l_FaceTo = GetOppsiteFace(l_FaceTo);
        List<FACE_TO> faces = new List<FACE_TO>();
        faces.Add(FACE_TO.LEFT);
        faces.Add(FACE_TO.RIGHT);
        faces.Add(FACE_TO.UP);
        faces.Add(FACE_TO.DOWN);
        faces.Add(FACE_TO.FORWORD);
        faces.Add(FACE_TO.BACKWORD);

        faces.Remove(l_FaceTo);
        return faces;
    }
    public FACE_TO Face(int face)
    {
        switch (face)
        {
            case 0:
                return FACE_TO.LEFT;
            case 1:
                return FACE_TO.RIGHT;
            case 2:
                return FACE_TO.UP;
            case 3:
                return FACE_TO.DOWN;
            case 4:
                return FACE_TO.FORWORD;
            case 5:
                return FACE_TO.BACKWORD;
            default:
                return FACE_TO.UP;
        }
    }
    [Button]
    public void FindCellsNeighber()
    {
        for (int i = 0; i < cells.Count; i++)
        {
            cells[i].X_cells.Clear();
            cells[i].Y_cells.Clear();
            cells[i].Z_cells.Clear();

            cells[i].X_cells.AddRange(cells.FindAll(x => (x.transform.position.y == cells[i].transform.position.y)
                                                && (x.transform.position.z == cells[i].transform.position.z)));
            cells[i].Y_cells.AddRange(cells.FindAll(x => (x.transform.position.x == cells[i].transform.position.x)
                                                && (x.transform.position.z == cells[i].transform.position.z)));
            cells[i].Z_cells.AddRange(cells.FindAll(x => (x.transform.position.x == cells[i].transform.position.x)
                                                && (x.transform.position.y == cells[i].transform.position.y)));
            cells[i].X_cells.Remove(cells[i]);
            cells[i].Y_cells.Remove(cells[i]);
            cells[i].Z_cells.Remove(cells[i]);
            //EditorUtility.SetDirty(cells[i]);
        }
    }
    [Button]
    void ALLSETUP()
    {
        ArrangeCells();
        //CALLCHECKALLCUBE();
        //DOVirtual.DelayedCall(0.2f, ()=>());
    }
    [Button]
    void CALLCHECKALLCUBE()
    {
        for (int i = 0; i < cells.Count; i++)
        {
            cells[i].Check();
            //Effect(cells[i]);
        }
        foreach (Cell item in cells)
        {
            Effect(item);
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
}
