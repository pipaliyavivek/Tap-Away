using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class one : MonoBehaviour
{
    public List<Transform> m_allTranform = new List<Transform>();
    private void OnEnable()
    {
        foreach (Transform item in transform.transform)
        {
            m_allTranform.Add(item);
        }
       //foreach (var item in m_allTranform)
       //{
       //    item.localPosition = new Vector3(Random.Range(0, 5), Random.Range(0, 5), 0);
       //}
    }
}
