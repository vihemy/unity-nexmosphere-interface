using System;
using UnityEngine;

public class NexmosphereController : MonoBehaviour
{
    private SerialPortManager serialPortManager;

    // Event to handle incoming data
    public event Action<string> OnDataReceived;

    // Initialize the controller with given serial port settings
    public void Initialize(string portName, int baudRate)
    {
        serialPortManager = gameObject.AddComponent<SerialPortManager>();
        serialPortManager.Initialize(portName, baudRate);
        serialPortManager.OnDataReceived += HandleDataReceived; // forwards the event so that all comunication between the Nexmosphere device and the Unity application is handled by the NexmosphereController
    }

    // Handle incoming data from the Nexmosphere device
    private void HandleDataReceived(string data)
    {
        Debug.Log($"NexmosphereControllere has received data: {data}");
        // Forward the event
        OnDataReceived?.Invoke(data);
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
