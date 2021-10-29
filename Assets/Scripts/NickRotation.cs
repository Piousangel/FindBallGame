using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NickRotation : MonoBehaviour
{


    private void FixedUpdate()
    {
        this.transform.rotation = Camera.main.transform.rotation;
    }


}
