using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyCustom.NetworkServiceFw;

public class PlayerControllerCube : MonoBehaviour
{
    private byte eventCode = 3;
    private Vector3 goVector;

    void OnEnable()
    {
        // Sync Vector3就好
        NetworkServiceFw.BindEvent<Vector3>(eventCode, OnCubeSync);

    }
    void Start()
    {
        //cubeDemo = FindObjectOfType<CubeDemoShowCaseManager>();
    }

    void Update()
    {

        if (Input.GetKey(KeyCode.A))
        {
            gameObject.transform.position += Vector3.left * 0.01f;
            NetworkServiceFw.TriggerTCPToOthers(eventCode, goVector);
        }
        if (Input.GetKey(KeyCode.D))
        {
            gameObject.transform.position += Vector3.right * 0.01f;
            NetworkServiceFw.TriggerTCPToOthers(eventCode, goVector);
        }
        if (Input.GetKey(KeyCode.W))
        {
            gameObject.transform.position += Vector3.forward * 0.01f;
            NetworkServiceFw.TriggerTCPToOthers(eventCode, goVector);
        }
        if (Input.GetKey(KeyCode.S))
        {
            gameObject.transform.position += Vector3.back * 0.01f;
            NetworkServiceFw.TriggerTCPToOthers(eventCode, goVector);
        }
        goVector = transform.position;
    }
    public void OnCubeSync(Vector3 pos)
    {

        Vector3 receivedPosition = pos;

        transform.position = receivedPosition;
    }
}
