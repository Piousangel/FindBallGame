using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Photon.Pun;
public class UIController : MonoBehaviour
{
    [SerializeField] private Text timeText;
    [SerializeField] private Text ScoreText;
    [SerializeField] private Text TotalScoreText;
    public GameObject TotalScreen;

    private float setTime = 60;
    private int _score;
    private float _time;

    GameSystem gameSystem;
    RandomInstant randomInstant;

    public bool gameStart = false;
    void Start()
    {
        gameSystem = FindObjectOfType<GameSystem>();
        randomInstant = FindObjectOfType<RandomInstant>();
        ScoreText.text = "Score : " + (_score).ToString();

        _time = setTime;
    }

    public Text atext;
    double start_time = 0;
    void Update()
    {
        if (gameStart)
        {
            atext.text = PhotonNetwork.Time.ToString() + " " + start_time.ToString();
            if (_time > 0)
            {
                
                _time = setTime - (float)(PhotonNetwork.Time - start_time);
            }
            if (_time <= 0)
            {
                gameStart = false;
                SetTotalScreen();
            }
        }
        timeText.text = "Time remaining : " + Mathf.Ceil(_time).ToString();
    }

    void SetTotalScreen()
    {
        if (TotalScreen.activeSelf == false)
        {
            TotalScreen.SetActive(true);

            //TotalScoreText.text = "Total Score : " + (score).ToString();
        }
    }

    public void ReStart()
    {
        TotalScreen.SetActive(false);
        _time = setTime;
        gameSystem.ScoreReset();
        if (PhotonNetwork.IsMasterClient) this.GetComponent<PhotonView>().RPC("RPC_LoadLevel", RpcTarget.All);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    [PunRPC]
    void RPC_LoadLevel()
    {
        PhotonNetwork.LoadLevel("SurroundFruits");
    }
    public void ScoreCheck(int score)
    {
        this.GetComponent<PhotonView>().RPC("RPC_Score", RpcTarget.All, score);
    }
    [PunRPC]
    void RPC_Score(int score)
    {

        ScoreText.text = "Score : " + (score).ToString();
        TotalScoreText.text = "Total Score : " + (score).ToString();
    }

    public void Set_Time()
    {
        start_time = PhotonNetwork.Time;
    }


}
