using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;
using UnityEngine.EventSystems;
using UnityEditor;
using UnityEngine.Events;
using System.Linq;

public class CubesGanerate : MonoBehaviour
{
    public static CubesGanerate Instance;

    public Transform cubePrefab, Root;

    public List<float> Rotation_Cubes = new List<float>();

    public Camera Maincamera;

    public List<LevelData> AllLevels;

    public LevelData _templevel;
    public List<Cube> Allcubes;
    public List<Transform> AllcubesTransform;
    public Vector3 l_Cubes;
    internal float orbitX = 0f;
    internal float orbitY = 0f;
    internal float orbitZ = 0f;
    public float orbitXSpeed = 50f;
    public float orbitYSpeed = 50f;
    public float orbitSmooth = 10f;

    public List<allCubeData> m_AllCubeData = new List<allCubeData>();
    public UnityEvent m_CallALLEvent = new UnityEvent();


    public Transform mAllWall;
    public void Awake()
    {
        Instance = this;
    }
    public void LateUpdate()
    {
        RoateCubes();
    }
    [Button]
    void INVOKEVENT()
    {
        m_CallALLEvent?.Invoke();
    }
    [Button]
    void FullFill()
    {
        m_AllCubeData.Clear();
        foreach (var item in Allcubes)
        {
            m_AllCubeData.Add(new allCubeData
            {
                m_Cube = item,
                m_CubeFace = item.mThisCubeFace,
                m_RotationX = item.RotationX,
                m_RotationY = item.RotationY
            });
        }
    }
    void Start()
    {
        StartCoroutine(Ganerate_cubes(3, 3, 3));
    }
    [Button]
    public IEnumerator Ganerate_cubes(int row, int column, int depth)//Ganerate_cubes
    {
        row = (int)l_Cubes.x;
        column = (int)l_Cubes.y;
        depth = (int)l_Cubes.z;
        Allcubes.ForEach(x => DestroyImmediate(x.gameObject));
        Allcubes.Clear();
        AllcubesTransform.Clear();
        _templevel = new LevelData();
        _templevel.Size.x = row;
        _templevel.Size.y = column;
        _templevel.Size.z = depth;
        List<Transform> tempRefs = new List<Transform>();
        Bounds l_Bounds = new Bounds();
        for (int i = 0; i < depth; i++)
        {
            yield return new WaitForSeconds(0.2f);
            for (int j = 0; j < column; j++)
            {
                yield return new WaitForSeconds(0.2f);
                for (int k = 0; k < row; k++)
                {
                    yield return new WaitForSeconds(0.2f);
                    Vector3 pos = new Vector3(k, j, i);
                    Transform foo = (Transform)Instantiate(cubePrefab, pos, Quaternion.Euler(0, Rotation_Cubes[Random.Range(0, Rotation_Cubes.Count)], 0) /*Quaternion.Euler(0, Rotation_Cubes[Random.Range(0, Rotation_Cubes.Count)], 0)*/);
                    Cube CubeClass = foo.GetComponent<Cube>();
                    Allcubes.Add(CubeClass);
                    AllcubesTransform.Add(foo);
                    foo.name = "" + k + "" + j + "" + i;
                    tempRefs.Add(foo);
                  //  l_Bounds.Encapsulate(foo.GetComponent<BoxCollider>().bounds);
                    //CubeClass.TwoSideRay();
                    /*if (CubeClass.RotationX == -CubeClass.RotationX)
                    {
                        CubeClass.RotationSetManually(0, CubeClass.RotationX, Vector2.zero);
                       //CubeClass.RotationX = Rotation_Cubes[Random.Range(0, 4)];
                    }
                    if (CubeClass.RotationY == -CubeClass.RotationY)
                    {
                        CubeClass.RotationSetManually(0, CubeClass.RotationY, Vector2.zero);
                        //CubeClass.RotationY = Rotation_Cubes[Random.Range(0, Rotation_Cubes.Count)];
                    }
                    if (k > 0)
                    {
                        if (Allcubes[Allcubes.IndexOf(CubeClass) - 1].RotationX == -CubeClass.RotationX)
                        {
                            CubeClass.RotationSetManually(0,0,Vector2.zero);
                            //CubeClass.Rotaterandom();
                        } 
                    }*/
                    //CubeClass.Rotaterandom();
                }
            }
        }
        transform.position = l_Bounds.center;
        for (int i = 0; i < tempRefs.Count; i++)
        {
            tempRefs[i].SetParent(transform);
            //tempRefs[i].name = "" + i;
            _templevel.Position.Add(tempRefs[i].localPosition);
            _templevel.Rotation.Add(tempRefs[i].localEulerAngles);
        }
        transform.position = Vector3.zero;
        //MakeValid();
        //CheckCubes(tempRefs);
    }
    [Button]
    public void LoadLevel(int Levelno)
    {
        Levelno = Levelno - 1;
        LevelData Currentlevel = AllLevels[Levelno];

        for (int i = 0; i < Currentlevel.Position.Count; i++)
        {
            Transform foo = (Transform)Instantiate(cubePrefab, Currentlevel.Position[i], Quaternion.identity, transform);
            foo.localEulerAngles = Currentlevel.Rotation[i];
            //foo.name = "" + i;
        }
    }
    [Button]
    public void SaveLevel(int Levelno)
    {
        Levelno = Levelno - 1;
        AllLevels[Levelno].Position = _templevel.Position;
        AllLevels[Levelno].Rotation = _templevel.Rotation;
        AllLevels[Levelno].Size = _templevel.Size;
#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(AllLevels[Levelno]);
#endif
    }
    public void MakeValid()
    {
        for (int i = 0; i < Allcubes.Count; i++)
        {
            Allcubes[i].Rotaterandom();
        }
    }
    [Button]
    public void ClearLevel()
    {
        foreach (Transform item in transform)
        {
            Destroy(item.gameObject);
        }
    }
    public void CheckCubes(List<Transform> t)
    {
        int count = 0;
        Debug.Log("Check cubes..");
        for (int i = 0; i < t.Count; i++)
        {
            RaycastHit hit;
            //Debug.Log("Vector 3 up" + Vector3.up);
            if (Physics.Raycast(t[i].position, t[i].TransformDirection(Vector3.forward), out hit, 1))
            {
                count++;
                Debug.Log("not valid:: " + i);
                t[i].GetComponent<Cube>().isvalid = false;
                t[i].rotation = Quaternion.Euler(0, Rotation_Cubes[Random.Range(0, Rotation_Cubes.Count)], 0);
             // CheckAgaincubes(t[i]);
            }
            else
            {
                t[i].GetComponent<Cube>().isvalid = true;
                Debug.Log("Done cube ganerate:: " + i);
            }
        }
    }
    public void CheckAgaincubes(Transform t)
    {
        if (Physics.Raycast(t.position, t.TransformDirection(Vector3.forward), out RaycastHit hit, 1))
        {
            t.GetComponent<Cube>().isvalid = false;
            t.rotation = Quaternion.Euler(0, Rotation_Cubes[Random.Range(0, Rotation_Cubes.Count)], 0);
            CheckAgaincubes(t);
        }
        else
        {
            t.GetComponent<Cube>().isvalid = true;
        }
        /* for (int i = 0; i < t.Count; i++)
         {
             if (Physics.Raycast(t[i].position, t[i].TransformDirection(Vector3.forward), out hit, 1))
             {
                 t[i].GetComponent<Cube>().isvalid = false;
                 t[i].rotation = Quaternion.Euler(0, Rotation_Cubes[Random.Range(0, Rotation_Cubes.Count)], 0);
                 //CheckAgaincubes(t);
             }
             else
             {
                 t[i].GetComponent<Cube>().isvalid = true;
             }
         }*/
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
    public void RoateCubes()
    {
        //CenterPoint.rotation = Quaternion.Lerp(CenterPoint.rotation, Quaternion.Euler(-orbitY, 0, orbitZ), orbitSmooth);        
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(-orbitY, 0, orbitZ), orbitSmooth);
        //mAllWall.transform.rotation = transform.rotation;
    }
    public void OnDrag(PointerEventData pointerData)
    {
        // Receiving drag input from UI.
        orbitX += pointerData.delta.x * orbitXSpeed / 1000f;
        orbitY -= pointerData.delta.y * orbitYSpeed / 1000f;
        orbitZ += pointerData.delta.x * orbitYSpeed / 1000f;
    }
    //Level Load
    public void LevelLoad()
    {
        //GameObject _level = Instantiate(Current_level);//For testing
        ////GameObject _level = (GameObject)Instantiate(Resources.Load("Level" + GameManager.Instance.Levelno));//right way
        //_leveldata = _level.GetComponent<LevelManager>();
        //_leveldata._levelData = AllLevels[GameManager.Instance.Levelno - 1];
        //for (int i = 0; i < _leveldata.depth; i++)
        //{
        //    for (int j = 0; j < _leveldata.columns; j++)
        //    {
        //        for (int k = 0; k < _leveldata.rows; k++)
        //        {
        //            Transform foo = (Transform)Instantiate(cubePrefab);
        //            AllCubes.Add(foo);
        //            //foo.transform.position = _leveldata._levelData.position[0];
        //        }
        //    }
        //}
        //Root = _level.transform;       
        //for (int i = 0; i < AllCubes.Count; i++)
        //{
        //    AllCubes[i].localPosition = _leveldata._levelData.Position[i];
        //    AllCubes[i].localRotation = Quaternion.Euler(0, _leveldata._levelData.Rotation[i], 0);
        //}
        //for (int i = 0; i < AllCubes.Count; i++)
        //{
        //    AllCubes[i].SetParent(Root);
        //}
        //Maincamera.transform.position = new Vector3(Root.transform.position.x, Root.transform.position.y, -10);
    }
}
[System.Serializable]
public class allCubeData
{
    public Cube m_Cube;
    public float m_RotationX;
    public float m_RotationY;
    public CubeFace m_CubeFace;
}