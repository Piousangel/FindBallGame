using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class BracketMovements : MonoBehaviour
{
    RandomInstant randomInstant;
    GameObject selectionArea;
    GameObject selectionArea2;
    public GameObject bracket;

    public GameObject RaycastPointer;
  
    [HideInInspector]
    public bool isSelected = false;
    [HideInInspector]
    public int temp_postion_num;
    public List<KeyCode> code;
    public List<Transform> _position = new List<Transform>();
    public ARRaycastManager aRRaycaster;

    ARPlaneController aRPlaneController;
    

    GameObject obj;
   

    private void Start()
    {
        randomInstant = FindObjectOfType<RandomInstant>().GetComponent<RandomInstant>();
        selectionArea = GameObject.FindGameObjectWithTag("SelectionArea");
        //selectionArea2 = GameObject.FindGameObjectWithTag("SelectionArea2");
        _position = randomInstant.position;
        temp_postion_num = 0;
        aRPlaneController = FindObjectOfType<ARPlaneController>();

        
    }

    private void Update()
    {
        
            BracketMove();
        
      
    }

    void BracketMove()
    {

        //Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);

        BallInfo[] info;
        info = FindObjectsOfType<BallInfo>();

        if (Input.touchCount > 0 && aRPlaneController.isSpwan == true)
        {
            Touch touch = Input.GetTouch(0);
            List<ARRaycastHit> hits = new List<ARRaycastHit>();
            if (aRRaycaster.Raycast(touch.position, hits, TrackableType.All))
            {
                Pose hitPose = hits[0].pose;

                RaycastPointer.transform.localScale = new Vector3(5f, 5.0f,5f);
                RaycastPointer.transform.position = hitPose.position;

                //for (int i = 0; i < info.Length; i++)
                //{
                //    if (hitPose.position == info[i].position)
                //    {
                //        this.transform.position = _position[info[i].index].position;
                //    }
                //}

            }
        }

    }
    
    

    public void CheckComplete()
    {
        bracket.SetActive(false);
        isSelected = false;
    }
    //public void ResetSelectionArea()
    //{
    //    selectionArea.transform.position = new Vector3(0, -2, 0);
    //}

}