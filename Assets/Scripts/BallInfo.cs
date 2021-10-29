using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BallInfo : MonoBehaviour
{
    public int index;
    public Vector3 position;

    BracketMovements bracketMovements;



    private void Start()
    {
        
        position = this.transform.parent.position;
        int row = Int32.Parse(this.transform.parent.name.Substring(15, 1));
        int col = Int32.Parse(this.transform.parent.name.Substring(17, 1));
        Debug.Log("row :"+row);
        Debug.Log("Col:" + col);

        index = row * 6 + col;
        Debug.Log("Index:" + index);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
