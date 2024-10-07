using System;
using UnityEngine;

public class NexmosphereController : MonoBehaviour
{
    [SerializeField] private string portName = "COM3";
    [SerializeField] private int baudRate = 115200;
    private SerialPortManager serialPortManager;

    public event Action<string> GetCommand; // subscribe to this event to get the message from the Nexmosphere device

    private void OnEnable()
    {
        Initialize(portName, baudRate);
    }
    // Initialize the controller with given serial port settings
    public void Initialize(string portName, int baudRate)
    {
        serialPortManager = gameObject.AddComponent<SerialPortManager>();
        serialPortManager.Initialize(portName, baudRate);
        serialPortManager.OnDataReceived += HandleDataReceived;
    }

    // Forwards incoming data from the Nexmosphere SerialPortManager to subscribers of GetCommand
    private void HandleDataReceived(string data)
    {
        GetCommand?.Invoke(data);
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
}
