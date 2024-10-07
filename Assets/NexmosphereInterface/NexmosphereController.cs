using System;
using UnityEngine;

public class NexmosphereController : MonoBehaviour
{
    [SerializeField] private string portName = "COM3";
    [SerializeField] private int baudRate = 115200;

    private SerialPortManager serialPortManager;

    private void OnEnable()
    {
        Initialize(portName, baudRate);
    }
    // Initialize the controller with given serial port settings
    public void Initialize(string portName, int baudRate)
    {
        serialPortManager = gameObject.AddComponent<SerialPortManager>();
        serialPortManager.Initialize(portName, baudRate);
        serialPortManager.OnDataReceived += HandleDataReceived; // Receives the event so that all comunication between the Nexmosphere device and the Unity application is handled by the NexmosphereController
    }

    // Handle incoming data from the Nexmosphere device
    private void HandleDataReceived(string data)
    {
        Debug.Log($"NexmosphereController has received serial message: {data}");
    }

    // Send a message to the Nexmosphere device
    public void SendCommand(string command)
    {
        serialPortManager.SendSerialMessage(command);
    }

    private void OnDestroy()
    {
        if (serialPortManager != null)
        {
            serialPortManager.OnDataReceived -= HandleDataReceived;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SendCommand("X003A[255]");
        }
    }
}
