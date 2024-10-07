using System;
using UnityEngine;

public class ExampleActor : MonoBehaviour
{
    [SerializeField] private NexmosphereController nexmosphereController;
    [SerializeField] private string sendCommand = "X003A[255]";

    private void OnEnable()
    {
        nexmosphereController.GetCommand += HandleCommand;
    }

    private void HandleCommand(string command)
    {
        Debug.Log($"ExampleActor has received command: {command}");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            nexmosphereController.SendCommand(sendCommand);
        }
    }
}
