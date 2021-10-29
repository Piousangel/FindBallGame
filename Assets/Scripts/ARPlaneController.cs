using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Photon.Pun;

public class ARPlaneController : MonoBehaviour
{
    public ARRaycastManager aRRaycaster;

    ARSessionOrigin m_SessionOrigin;
    public GameObject placeObject;

    private GameObject spawnObject;

    GameObject aRPlane;

    public bool isSpwan = false;

    void Start()
    {
        //isSpwan = false;
        aRPlane = GameObject.FindGameObjectWithTag("Plane");
        StartCoroutine(PlaceObjectByTouch());
        m_SessionOrigin = FindObjectOfType<ARSessionOrigin>();
        m_SessionOrigin.transform.localScale = Vector3.one * 300;
    }

    [SerializeField]
    [Tooltip("The rotation the content should appear to have.")]
    Quaternion m_Rotation;

    /// <summary>
    /// The rotation the content should appear to have.
    /// </summary>
    public Quaternion rotation
    {
        get { return m_Rotation; }
        set
        {
            m_Rotation = value;
            if (m_SessionOrigin != null)
                m_SessionOrigin.MakeContentAppearAt(placeObject.transform, placeObject.transform.position, m_Rotation);
        }
    }

    public bool getIsSpwan()
    {
        return isSpwan;
    }
    private IEnumerator PlaceObjectByTouch()
    {
//#if UNITY_EDITOR
        //Instantiate(placeObject, new Vector3(0, -2, 0), Quaternion.identity);
        //yield return new WaitForFixedUpdate();
//#else
        while (!isSpwan)
        {
            
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                List<ARRaycastHit> hits = new List<ARRaycastHit>();
                if (aRRaycaster.Raycast(touch.position, hits, TrackableType.Planes))
                {
                    Pose hitPose = hits[0].pose;
                    //spawnObject = Instantiate(placeObject, hitPose.position, Quaternion.identity);
                    m_SessionOrigin.MakeContentAppearAt(placeObject.transform, hitPose.position,  Quaternion.Euler(m_Rotation.eulerAngles+ new Vector3(0,90f,0)));
                    //placeObject.SetActive(false);
          
                    isSpwan = true;
                    if (PhotonNetwork.IsMasterClient)
                    {
                        FindObjectOfType<MasterController>().isSpawn();
                       
                    }
                    else
                    {
                        FindObjectOfType<SlaveController>().isSpawn();
                    }
                    foreach(var plane in FindObjectsOfType<ARPlane>())
                    {
                        Destroy(plane.gameObject);
                    }
                    
                    
                }
                
            }
            yield return new WaitForFixedUpdate();
        }
//#endif
    }
}