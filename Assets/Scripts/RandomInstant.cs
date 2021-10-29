using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RandomInstant : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> prefab_item;
    public List<Transform> position;

    [HideInInspector]
    public List<Transform> temp_pattern = new List<Transform>();
    
    

    public List<Transform> position_pattern;

    private PhotonView PV;
    private void Awake()
    {
        PV = this.GetComponent<PhotonView>();
    }

    public void MapReset()
    {
        foreach (var pos in position)
        {
            if (pos.transform.childCount > 0)
            {
                Destroy(pos.transform.GetChild(0).gameObject);
            }
        }

        for(int i = 0; i < 30; i++)
        {
            int random_number = Random.Range(0, prefab_item.Count);
            if (PhotonNetwork.IsMasterClient) PV.RPC("RPC_MapReset", RpcTarget.All, random_number, i);
        }
        Random_Pattern();
    }
    [PunRPC]
    public void RPC_MapReset(int num, int index)
    {
        GameObject instant_item = Instantiate(prefab_item[num], Vector3.zero, Quaternion.identity);
        instant_item.name = prefab_item[num].name;
        instant_item.transform.SetParent(position[index], false);
        instant_item.transform.localPosition = Vector3.zero;
    }

    public void Random_Pattern()
    {
        int random_number = Random.Range(0, 20);
        random_number = (random_number / 5) + random_number;
        Debug.Log(random_number);

        PV.RPC("RPC_Random_Pattern", RpcTarget.All, random_number);
        
    }
    [PunRPC]
    void RPC_Random_Pattern(int pattern_num)
    {
        foreach (var pattern in temp_pattern)
        {
            Destroy(pattern.gameObject);
        }
        temp_pattern.Clear();

        List<GameObject> instant_pattern = new List<GameObject>();

        instant_pattern.Add(Instantiate(position[pattern_num + 0].GetChild(position[pattern_num + 0].childCount - 1).gameObject, Vector3.zero, Quaternion.identity));
        instant_pattern.Add(Instantiate(position[pattern_num + 1].GetChild(position[pattern_num + 1].childCount - 1).gameObject, Vector3.zero, Quaternion.identity));
        instant_pattern.Add(Instantiate(position[pattern_num + 6].GetChild(position[pattern_num + 6].childCount - 1).gameObject, Vector3.zero, Quaternion.identity));
        instant_pattern.Add(Instantiate(position[pattern_num + 7].GetChild(position[pattern_num + 7].childCount - 1).gameObject, Vector3.zero, Quaternion.identity));
        
        instant_pattern[0].name = position[pattern_num + 0].GetChild(position[pattern_num + 0].childCount - 1).name;
        instant_pattern[1].name = position[pattern_num + 1].GetChild(position[pattern_num + 1].childCount - 1).name;
        instant_pattern[2].name = position[pattern_num + 6].GetChild(position[pattern_num + 6].childCount - 1).name;
        instant_pattern[3].name = position[pattern_num + 7].GetChild(position[pattern_num + 7].childCount - 1).name;

        int index = 0;
        foreach (var pattern in instant_pattern)
        {
            pattern.transform.SetParent(position_pattern[index++], false);
            pattern.transform.localPosition = Vector3.zero;
            temp_pattern.Add(pattern.transform);
        }

    }
}
