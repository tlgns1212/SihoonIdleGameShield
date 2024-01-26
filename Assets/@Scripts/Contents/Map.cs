using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    private int _index;
    private int _maxIndex;
    public void Init()
    {
        _maxIndex = transform.childCount - 1;
        _index = 0;
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        transform.GetChild(_index++).position += new Vector3(57.6f, 0, 0);
        if (_index > _maxIndex) _index = 0;
    }
}
