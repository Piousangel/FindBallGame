using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class GameSystem : MonoBehaviour
{
    public GameObject bracketMaster;
    public GameObject bracketSlave;
    MasterController bracket1;
    SlaveController bracket2;

    public TextMeshPro MasterNickName;
    public TextMeshPro SlaveNickName;


    private List<Transform> pattern = new List<Transform>();
    RandomInstant RI;
    //BallPosition BP;

    UIController uIController;
    private int score;

    private void Start()
    {
        RI = FindObjectOfType<RandomInstant>();
        //BP = FindObjectOfType<BallPosition>();
        uIController = FindObjectOfType<UIController>();
        score = 0;

        if (PhotonNetwork.IsMasterClient) bracket1 = PhotonNetwork.Instantiate("MasterController", Vector3.zero, Quaternion.identity).GetComponent<MasterController>();
        else bracket2 = PhotonNetwork.Instantiate("SlaveController", Vector3.zero, Quaternion.identity).GetComponent<SlaveController>();
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

#if UNITY_EDITOR
        if (PhotonNetwork.IsMasterClient) bracket1.isSpawn();
        else bracket2.isSpawn();
#endif
        if (PhotonNetwork.IsMasterClient) bracket1.Send_NickName();
        else bracket2.Send_NickName();

        while (true)
        {
            bool mastergd = bracket1.gd_Spawned;
            bool slavegd = bracket2.gd_Spawned;
            Debug.Log(mastergd + " " + slavegd);
            if (mastergd && slavegd)
                break;
            yield return new WaitForFixedUpdate();
        }

        if (PhotonNetwork.IsMasterClient) RI.MapReset();
        uIController.Set_Time();
        uIController.gameStart = true;

        
    }


    private void FixedUpdate()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (bracket1.isSelected && bracket2.isSelected)
            {
                Check_Pattern();
            }
        }
    }


    int debug_index = 0;
    public void Check_Pattern()
    {
        pattern = RI.temp_pattern;
        if(bracket1.temp_position_num %6 + 1 == bracket2.temp_position_num % 6 && bracket1.temp_position_num + 1 == bracket2.temp_position_num)
        {
            
            int pos = bracket1.temp_position_num;
            List<string> names = new List<string>();
            names.Add(RI.position[pos].GetChild(0).name);
            names.Add(RI.position[pos + 1].GetChild(0).name);
            names.Add(RI.position[pos + 6].GetChild(0).name);
            names.Add(RI.position[pos + 7].GetChild(0).name);

            int index = 0;
            bool check = true;
            foreach(var pat in pattern)
            {
                Debug.Log(pat.name + " " + names[index] + " " + debug_index++);
                if (pat.name != names[index++])
                {
                    check = false;
                    break;
                }
            }
            if (check)
            {
                RI.Random_Pattern();
                uIController.ScoreCheck(++score);
            }
            
            Debug.Log(check);
            bracket1.CheckComplete();
            bracket2.CheckComplete();
        }
        
    }

    public void ScoreReset()
    {
        score = 0;
        uIController.ScoreCheck(0);
    }
}
