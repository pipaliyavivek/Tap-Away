using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

public class Cube : MonoBehaviour
{

    Tween m_shake;
    private bool Move;
    public Vector3 Position;
    public float Rotate;
    internal bool isvalid;
    public float RotationX, RotationY;
    public CubeFace mThisCubeFace;
    [SerializeField]public List<Collider> HitColliders = new List<Collider>();

    public List<Cube> m_OthersCube = new List<Cube>();

    private void FixedUpdate()
    {
        if (Move)
        {
            transform.position += transform.TransformDirection(Vector3.forward) * (Time.deltaTime * 20);
        }
      //  Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward), Color.red);
    }
    [Button]
    /*void FulFill()
    {
        CubesGanerate.Instance.m_AllCubeData.Add(new allCubeData
        {
            m_Cube = this,
            m_CubeFace = mThisCubeFace,
            m_RotationX = RotationX,
            m_RotationY= RotationY
        });  
    }*/
    void Start()
    {
     //   CubesGanerate.Instance.m_CallALLEvent.AddListener(RotateThisCube);
      //After  GetRotation();
        //RotateThisCube();
    }
   [Button]
   public void RotationSetManually(float Rotationx,float Rotationy,Vector2 ignored)
    {
        var tmp = transform.localEulerAngles;
        tmp.x = Rotationx;
        tmp.y = Rotationy;
        transform.localEulerAngles = tmp;       
        if(ignored.x==0)
        {
            int a = 0;
            while(transform.localEulerAngles.y == -ignored.y)
            {
                Rotaterandom();
                a++;
                if (a >= 100) { Debug.Log("NOT FOUND ROTATION"); break; };
            }
        }
        if(ignored.y == 0)
        {
            int a = 0;
            while (transform.localEulerAngles.x == -ignored.x)
            {
                Rotaterandom();
                a++;
                if (a >= 100) { Debug.Log("NOT FOUND ROTATION"); break; };
            }
        }
    }
    [Button]
    public void GetRotation()
    {
        RotationX = Clamp0360(transform.localEulerAngles.x);        
        RotationY = Clamp0360(transform.localEulerAngles.y);
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hit, Mathf.Infinity, 1<<6))
        {
            Debug.Log(hit.distance);
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward),Color.black,100f);
            if (hit.collider.gameObject.name == "UP")
            {
                mThisCubeFace = CubeFace.UP;
            }
            if (hit.collider.gameObject.name == "DOWN")
            {
                mThisCubeFace = CubeFace.DOWN;
            }
            if (hit.collider.gameObject.name == "RIGHT")
            {
                mThisCubeFace = CubeFace.RIGHT;
            }
            if (hit.collider.gameObject.name == "LEFT")
            {
                mThisCubeFace = CubeFace.LEFT;
            }
            if (hit.collider.gameObject.name == "FORWORD")
            {
                mThisCubeFace = CubeFace.FORWARD;
            }
            if (hit.collider.gameObject.name == "BACKWORD")
            {
                mThisCubeFace = CubeFace.BACKWARD;
            }
        }
        //if (RotationX == 0 && RotationY == 0) mThisCubeFace = CubeFace.FORWARD;
        //if ((RotationX == 180 && RotationY == 0 )||( RotationX == -180 && RotationY == 0 )|| (RotationX == 0 && RotationY == 180) || (RotationX == 180 && RotationY == -180)) mThisCubeFace = CubeFace.BACKWARD;
        //if ((RotationX == 90 && RotationY == 0 )||( RotationX == 90 && RotationY == 90 )|| (RotationX == 90 && RotationY == -90) || (RotationX == 90 && RotationY == -180) || (RotationX == 90 && RotationY == 180)) mThisCubeFace = CubeFace.DOWN;
        //if ((RotationX == -90 && RotationY == 0 )||( RotationX == -90 && RotationY == 90 )|| (RotationX == -90 && RotationY == -90) || (RotationX == -90 && RotationY == -180) || (RotationX == -90 && RotationY == 180)) mThisCubeFace = CubeFace.UP;
        //if ((RotationX == 0) && (RotationY == 90)) mThisCubeFace = CubeFace.RIGHT;
        //if ((RotationX == 0) && (RotationY == -90)) mThisCubeFace = CubeFace.LEFT;
    }
    [Button]
    public float Clamp0360(float angle)
    {   
        angle %= 360;
        if (angle > 180)
            return ClampAAngle(angle- 360);        
        return ClampAAngle(angle);
    }
    public float ClampAAngle(float angle)
    {
        if (angle > -10 && angle < 10) angle = 0;
        else if (angle > -100 && angle < -80) angle = -90;
        else if(angle > 80 && angle < 100) angle = 90;
        else if(angle > 170 && angle <= 180) angle = 180;
        else if(angle < -170 && angle >= -180) angle = -180;
        return angle;
    }
   
    public void OnMouseUpAsButton()
    { 
    //Debug.Log("Vector 3: " + Vector3.up+" Left: "+Vector3.left+" Right "+Vector3.right+" down "+Vector3.down+" forword "+Vector3.forward);
    RaycastHit hit;
    //Debug.Log("Vector 3 up" + Vector3.up);
    if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 3))
    {
        Debug.Log("Hit distance is :: " + hit.distance);
        if (hit.distance > 1)
        {
            //Move object in between till empty objects               
            Move_Cubes((int)hit.distance);
        }
        else
        {
            //Debug.Log((hit.distance));
            ShakeinDirection(transform.TransformDirection(Vector3.forward));
        }
    }
    else
    {
        Move = true;
     //   if(CubesGanerate.Instance.Allcubes.Contains(this)) CubesGanerate.Instance.Allcubes.RemoveAt(CubesGanerate.Instance.Allcubes.IndexOf(this));
        Destroy(gameObject, 1.5f);
        //CubesGanerate.Instance.AllCubes.Remove(transform);
        Debug.Log("Ok move");
        GameManager.Instance.Gameover();
        //Forword direction 
    }                
    }
    public void ShakeinDirection(Vector3 Dir)
    {
        Shake();
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Dir, out hit))
        {
            hit.collider.GetComponent<Cube>().ShakeinDirection(Dir);
        }
    }
    public void Shake()
    {
        m_shake?.Kill(true);
        m_shake = transform.DOShakePosition(0.5f, 0.1f, 1, 50, false);
    }
    public void Move_Cubes(int distance)
    {
        //Debug.Log("Distnace:  " + distance);
        transform.DOMove(transform.position + (transform.TransformDirection(Vector3.forward) * distance), 0.02f);
    }
    [Button]
    public bool Isvalid()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 3))
        {
            Debug.Log(transform.localEulerAngles + "  " + hit.collider.transform.localEulerAngles);
            float Opponent = hit.collider.transform.localEulerAngles.y;
            if (transform.localEulerAngles.y == 0 && Opponent == 180 || transform.localEulerAngles.y == 180 && Opponent == 0 || transform.localEulerAngles.y == 90 && Opponent == 270 || transform.localEulerAngles.y == 270 && Opponent == 90)
            {
                return false;
            }
            //CheckAgaincubes(t[i]);
        }
        return true;
    }
    public Cube GetFacingcube()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 100))
        {
            var lhitcube =hit.collider.GetComponent<Cube>();
            if(lhitcube) return lhitcube;
            return this;

        }
        return this;
    }
    [Button]
    void SetOtherCubes()
    {
        m_OthersCube.Clear();
        for (int i = 0; i < CubesGanerate.Instance.l_Cubes.x; i++)
        {
            //m_OthersCube.Add(CubesGanerate.Instance.Allcubes[i]);
        }
    }
    [Button]
    public void Rotaterandom()
    {
        //RotateThisCube();
        Debug.Log("Rotate random no is :: ");
        List<Vector3> Directions = new List<Vector3>();
        List<float> rotation = new List<float>();
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 1))
        {
            if (hit.collider.GetComponent<Cube>().GetFacingcube() != this)
            {
                Directions.Add(transform.TransformDirection(Vector3.forward));
                Debug.Log("Forward " + transform.localEulerAngles.y);
            }
        }
        else
        {
            Directions.Add(transform.TransformDirection(Vector3.forward));
            Debug.Log("Forward else" + transform.localEulerAngles.y);
        }
        hit = new RaycastHit();
        if (Physics.Raycast(transform.position, transform.TransformDirection(-Vector3.forward), out hit, 1))
        {
            if (hit.collider.GetComponent<Cube>().GetFacingcube() != this)
            {
                Directions.Add(transform.TransformDirection(-Vector3.forward));
              //  Debug.Log("Forward minus" + transform.localEulerAngles.y);
            }
        }
        else
        {
            Directions.Add(transform.TransformDirection(Vector3.forward));
            //Debug.Log("Forward else minus" + transform.localEulerAngles.y);
        }
        hit = new RaycastHit();
        if (Physics.Raycast(transform.position, transform.TransformDirection(-Vector3.up), out hit, 1))
        {
            if (hit.collider.GetComponent<Cube>().GetFacingcube() != this)
            {
                Directions.Add(transform.TransformDirection(-Vector3.up));
            }
        }
        else
        {
            Directions.Add(transform.TransformDirection(-Vector3.up));
        }
        hit = new RaycastHit();
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.up), out hit, 1))
        {
            if (hit.collider.GetComponent<Cube>().GetFacingcube() != this)
            {
                Directions.Add(transform.TransformDirection(Vector3.up));
            }
        } 
        else
        {
            Directions.Add(transform.TransformDirection(Vector3.up));
        }        
        //transform.rotation = Quaternion.Euler(0, CubesGanerate.Instance.Rotation_Cubes[Random.Range(0, CubesGanerate.Instance.Rotation_Cubes.Count)], 0);
        var lookdirection = transform.position - Directions[Random.Range(0, Directions.Count)];
        transform.LookAt(lookdirection);
       //after GetRotation();
    }
    #region Old_Script
    [SerializeField] public List<Cube> m_CubeItem = new List<Cube>();
    [Button]
    public void RotateThisCube()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hit))//Forword
        {
            var hitcube = hit.collider.GetComponent<Cube>();
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward), Color.blue, 100f, true);
            SetR(hitcube);
        }
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.back), out RaycastHit Backhit))//Backword
        {
            var hitcube = Backhit.collider.GetComponent<Cube>();
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.back), Color.blue, 100f, true);
            SetR(hitcube);
        }
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.up), out RaycastHit Uphit))//Up
        {
            var hitcube = Uphit.collider.GetComponent<Cube>();
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.up), Color.yellow, 100f, true);
            SetR(hitcube);
        }
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out RaycastHit downhit))//Down
        {
            var hitcube = downhit.collider.GetComponent<Cube>();
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down), Color.yellow, 100f, true);
            SetR(hitcube);
        }
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.left), out RaycastHit Lefthit))//Left
        {
            var hitcube = Lefthit.collider.GetComponent<Cube>();
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.left), Color.red, 100f, true);
            SetR(hitcube);
        }
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.right), out RaycastHit Righthit))//Right
        {
            var hitcube = Righthit.collider.GetComponent<Cube>();
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.right), Color.red, 100f, true);
            SetR(hitcube);
        }
        //transform.TransformDirection(Vector3.forward)
    }
    public void SetR(Cube hitcube)
    {
        m_CubeItem.Add(hitcube);
        if (RotationX == (-hitcube.RotationX))
        {
            Vector2 IgnorVector = new Vector2(0, -hitcube.RotationX);
            hitcube.RotationSetManually(RotationX,0, IgnorVector);
        }
        if (RotationY == (-hitcube.RotationY))
        {
            Vector2 IgnorVector = new Vector2(-hitcube.RotationY, 0);
            hitcube.RotationSetManually(0,RotationY, IgnorVector);
        }
    }
    #region Dust
    /* RaycastHit[] hitedde = Physics.RaycastAll(transform.position, Vector3.one*2 ,10f);
        HitColliders.Clear();
        for (int i = 0; i < hitedde.Length; i++)
        {
            var hitcube = hitedde[i].collider.GetComponent<Cube>();
            HitColliders.Add(hitedde[i].collider);
            if (RotationX == (-hitcube.RotationX))
            {
                Vector2 IgnorVector = new Vector2(0, -hitcube.RotationX);
                hitcube.RotationSetManually(RotationX, 0, IgnorVector);
            }
            if (RotationY == (-hitcube.RotationY))
            {
                Vector2 IgnorVector = new Vector2(-hitcube.RotationY, 0);
                hitcube.RotationSetManually(0, RotationY, IgnorVector);
            }
            //Debug.DrawRay(transform.position,hitedde[i].collider.transform.position, Color.red, 100f, true);
        }*/
    #endregion
    #endregion
    public void CheckThis(Vector3 i_OtherRotation)
    {
        var localangle = transform.localEulerAngles;
        if (localangle.x != 0 && localangle.x > 180) localangle.x = 180 - localangle.x;
        if (localangle.y != 0 && localangle.y > 180) localangle.y = 180 - localangle.y;
        if (localangle.z != 0 && localangle.z > 180) localangle.z = 180 - localangle.z;
        var rotationcubelist = CubesGanerate.Instance.Rotation_Cubes;        
        if(localangle.x !=0 ||localangle.x == -i_OtherRotation.x)
        {
            localangle.x = rotationcubelist[Random.Range(0, rotationcubelist.Count)];
        }
        else if(localangle.y !=0 || localangle.y == -i_OtherRotation.y)
        {
            localangle.y = rotationcubelist[Random.Range(0, rotationcubelist.Count)];
        }
        else if(localangle.z !=0)
        {
            /*localangle.x = rotationcubelist[Random.Range(0, rotationcubelist.Count)];*/
        }
        transform.eulerAngles = localangle;
        /*transform.eulerAngles.x = rotationcubelist[Random.Range(0, rotationcubelist.Count)];*/
    }
    [Button]
    public void Makevalid()
    {
        for (int i = 0; i < 4; i++)
        {
            if (Isvalid())
            {
                return;
            }
            Rotaterandom();
        }
        if (!Isvalid())
        {
            //if (CubesGanerate.Instance.Allcubes.Contains(this)) CubesGanerate.Instance.Allcubes.Remove(this);
            if (CubesGanerate.Instance.Allcubes.Contains(this)) CubesGanerate.Instance.Allcubes.RemoveAt(CubesGanerate.Instance.Allcubes.IndexOf(this));
            Destroy(gameObject);
        }
    }
}
public enum CubeFace
{
    UP,DOWN,LEFT,RIGHT,FORWARD,BACKWARD
}
[System.Serializable]
public class CubeData
{
    public CubeFace Key;
    public Cube m_Cube;

    public CubeData(CubeFace key, Cube cube)
    {
        Key = key;
        m_Cube = cube;
    }
}
