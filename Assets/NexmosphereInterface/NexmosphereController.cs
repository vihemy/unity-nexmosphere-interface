using System;
using UnityEngine;

public class NexmosphereController : MonoBehaviour
{
    [SerializeField] private string portName = "COM3";
    [SerializeField] private int baudRate = 115200;
    private SerialPortManager serialPortManager;

    public event Action<string> GetCommand; // subscribe to this event to get commands from the Nexmosphere device

    private void OnEnable()
    {
        InitializeSerialPortManager(portName, baudRate);
    }

    // Initialize the controller with given serial port settings
    public void InitializeSerialPortManager(string portName, int baudRate)
    {
        serialPortManager = gameObject.AddComponent<SerialPortManager>();
        serialPortManager.Initialize(portName, baudRate);
        serialPortManager.OnDataReceived += HandleDataReceived;
    }

    // Forward incoming data from SerialPortManager to subscribers of GetCommand
    private void HandleDataReceived(string data)
    {
        GetCommand?.Invoke(data);
    }

    // Send a command to the Nexmosphere device
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
}
