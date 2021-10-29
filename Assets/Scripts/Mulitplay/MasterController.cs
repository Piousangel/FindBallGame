using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class MasterController : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject bracket;
    private PhotonView PV;

    public TextMeshPro MasterID;

    private void Awake()
    {
        bracket = FindObjectOfType<GameSystem>().bracketMaster;
        PV = this.GetComponent<PhotonView>();
    }

    RandomInstant randomInstant;
    [HideInInspector]
    public int temp_position_num;
    public List<KeyCode> code;
    public List<Transform> _position = new List<Transform>();
    GameObject selectionArea;

    private void Start()
    {
        randomInstant = FindObjectOfType<RandomInstant>().GetComponent<RandomInstant>();
        _position = randomInstant.position;
        isSelected = false;
        gd_Spawned = false;
        bracket.transform.position = _position[0].position;
        selectionArea = bracket.transform.Find("GameObject").Find("SelectionArea").gameObject;
        MasterID = FindObjectOfType<GameSystem>().MasterNickName;
    }

    public bool isSelected = false;

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (isSelected == false)
            {
                BracketMove();
            }
            SelectionArea();
        }
        bracket.transform.position = _position[temp_position_num].position;
    }



    public void SelectionArea()
    {
        if (Input.GetKeyDown(code[4]))
        {
            PV.RPC("RPC_Send_Select", RpcTarget.All);
        }
    }

    public void SelectionArea_AR()
    {
        PV.RPC("RPC_Send_Select", RpcTarget.All);
    }

    void BracketMove()
    {
        if (Input.GetKeyUp(code[0])) PV.RPC("RPC_Send_Index", RpcTarget.All, "Up");
        else if (Input.GetKeyUp(code[1])) PV.RPC("RPC_Send_Index", RpcTarget.All, "Left");
        else if (Input.GetKeyUp(code[2])) PV.RPC("RPC_Send_Index", RpcTarget.All, "Down");
        else if (Input.GetKeyUp(code[3])) PV.RPC("RPC_Send_Index", RpcTarget.All, "Right");
        else return;
    }

    [PunRPC]
    void RPC_Send_Index(string dir)
    {
        int temp_position = 0;
        temp_position = temp_position_num;

        switch (dir)
        {
            case "Up":
                if (temp_position / 6 > 0) temp_position -= 6;
                break;
            case "Left":
                if (temp_position % 6 > 0) temp_position -= 1;
                break;
            case "Down":
                if (temp_position / 6 < 3) temp_position += 6;
                break;
            case "Right":
                if (temp_position % 6 < 5) temp_position += 1;
                break;
        }
        temp_position_num = temp_position;
    }

    public void BracketMoveAR(int index)
    {
        PV.RPC("RPC_Send_Index_AR", RpcTarget.All, index);
    }

    [PunRPC]
    void RPC_Send_Index_AR(int index)
    {
        temp_position_num = index;
        
        //MasterId.transform.SetParent(bracket.transform);
        //MasterId.transform.localPosition = new Vector3(0, 0, 0);
    }

    public void Send_NickName()
    {
        PV.RPC("RPC_Send_NickName", RpcTarget.All, PhotonNetwork.NickName.ToString());
    }

    [PunRPC]
    void RPC_Send_NickName(string name)
    {
        MasterID.text = name + "Player";
        //MasterId.text = PhotonNetwork.NickName.ToString() + "Player";
    }

    [PunRPC]
    void RPC_Send_Select()
    {
        selectionArea.SetActive(!selectionArea.activeSelf);
        isSelected = !isSelected;
    }


    public void CheckComplete()
    {
        PV.RPC("ResetSelect", RpcTarget.All);
    }

    [PunRPC]
    private void ResetSelect()
    {
        selectionArea.SetActive(false);
        isSelected = false;
        FindObjectOfType<RayCastPointerMovement>().resetArea();
    }

    public bool gd_Spawned = false;
    public void isSpawn()
    {
        PV.RPC("RPC_isSpawn", RpcTarget.All);
    }
    [PunRPC]
    private void RPC_isSpawn()
    {
        gd_Spawned = true;
    }
}
