using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class RayCastPointerMovement : MonoBehaviour
{

	MasterController bracket1;
	SlaveController bracket2;

	

	public Text atext;
	public List<Transform> _position = new List<Transform>();
	//BallInfo ballInfo;
	RandomInstant randomInstant;
	public GameObject selectionArea;
	bool isSelected;


	bool gameStart = false;
	public ARRaycastManager aRRaycaster;
	ARPlaneController aRPlaneController;
	private void Start()
    {
		//_position = randomInstant.position;
		//ballInfo = FindObjectOfType<BallInfo>();
		gameStart = false;
		aRPlaneController = FindObjectOfType<ARPlaneController>();
		StartCoroutine(Check_Online()); 
	}

	IEnumerator Check_Online()
	{
		while (true)
		{
			bracket1 = FindObjectOfType<MasterController>();
			bracket2 = FindObjectOfType<SlaveController>();
			if (bracket1 != null && bracket2 != null)
				break;
			yield return new WaitForFixedUpdate();
		}
		if (PhotonNetwork.IsMasterClient) isSelected = bracket1.isSelected;
		else isSelected = bracket2.isSelected;
		gameStart = true;
	}

    public void FixedUpdate()
    {
		BallTracking();
    }

	RaycastHit[] my_hit;
	void BallTracking()
    {
		
		if (gameStart)
		{
			BallInfo[] info = FindObjectsOfType<BallInfo>();
			if (Input.touchCount > 0 && aRPlaneController.isSpwan == true)
			{
				Touch touch = Input.GetTouch(0);
				List<ARRaycastHit> hits = new List<ARRaycastHit>();
				if (aRRaycaster.Raycast(touch.position, hits, TrackableType.All))
				{
					Pose hitPose = hits[0].pose;

					int layerMask = 1 << LayerMask.NameToLayer("Ground");
					my_hit = Physics.RaycastAll(Camera.main.transform.position, hitPose.position - Camera.main.transform.position);
					if (my_hit.Length > 0)
					{
						foreach(var hit in my_hit)
						if(hit.collider.gameObject.name == "Plane")
						{
							this.transform.localScale = new Vector3(5f, 5.0f, 5f);
							this.transform.position = hit.point;
						}
                    }
				}
			}
		}
	}

    void OnTriggerEnter(Collider other)
	{
		if (gameStart && other.transform.tag == "Ball" && isSelected == false)
		{
			if(other.GetComponent<BallInfo>().index % 6 < 6 && other.GetComponent<BallInfo>().index / 6 < 4)
            {
				if (PhotonNetwork.IsMasterClient) bracket1.BracketMoveAR(other.GetComponent<BallInfo>().index);
				else bracket2.BracketMoveAR(other.GetComponent<BallInfo>().index);
				//selectArea(other);
			}
			
			//atext.text = "Bracket.transform.position" + Bracket.transform.position.ToString() + " /n " + "gameObject.transform" + other.gameObject.transform.position.ToString();
			//atext.text = _position[other.GetComponent<BallInfo>().index].position.ToString();
			
		}
	}

	public void selectArea()
    {
        if (gameStart)
		{
			if (PhotonNetwork.IsMasterClient)
			{
				bracket1.SelectionArea_AR();
				isSelected = bracket1.isSelected;
			}
			else
			{
				bracket2.SelectionArea_AR();
				isSelected = bracket2.isSelected;
			}


		}
		//selectionArea.transform.position = ball.GetComponent<BallInfo>().position;
		//FindObjectOfType<GameSystem>().Check_Pattern();

	}

	public void resetArea()
    {
		if(gameStart)
        {
			if (PhotonNetwork.IsMasterClient)
			{
				isSelected = bracket1.isSelected;
			}
			else
			{
				isSelected = bracket2.isSelected;
			}
		}
    }
}
