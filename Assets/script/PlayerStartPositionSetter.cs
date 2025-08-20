using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStartPositionSetter : MonoBehaviour
{
    public GameObject player;
    public Transform fromFieldStartPos;
    public Transform fromInsideTentAStartPos;
    public Transform fromCampStartPos;

    void Start()
    {
        switch (SceneTransitionData.entranceID)
        {
            case "fromField":
                player.transform.position = fromFieldStartPos.position;
                break;
            case "fromInsideTentA":
                player.transform.position = fromInsideTentAStartPos.position;
                break;
            case "fromCamp":
                player.transform.position = fromCampStartPos.position;
                break;
        }
    }
}

