using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    internal float orbitX = 0f;
    internal float orbitY = 0f;
    internal float orbitZ = 0f;
    public float orbitXSpeed = 50f;
    public float orbitYSpeed = 50f;
    public float orbitSmooth = 10f;
    public int depth;
    public LevelData _levelData;
    
    public void LateUpdate()
    {
        RoateCubes();
    }
    public void RoateCubes()
    {
        //CenterPoint.rotation = Quaternion.Lerp(CenterPoint.rotation, Quaternion.Euler(-orbitY, 0, orbitZ), orbitSmooth);        
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(-orbitY, 0, orbitZ), orbitSmooth);        
    }
    public void OnDrag(PointerEventData pointerData)
    {
        // Receiving drag input from UI.
        orbitX += pointerData.delta.x * orbitXSpeed / 1000f;
        orbitY -= pointerData.delta.y * orbitYSpeed / 1000f;//-
        orbitZ += pointerData.delta.x * orbitYSpeed / 1000f;//+        
    }
}
