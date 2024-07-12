using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class InputCommand
{
    public string command;
    public UnityEvent action;
}

[System.Serializable]
public class OutputCommand
{
    public string command;
    public string triggerName;
}

public class NexmosphereManager : MonoBehaviour
{
    public string portName = "COM3";
    public int baudRate = 11520;
    public List<InputCommand> inputCommands = new List<InputCommand>();
    public List<OutputCommand> outputCommands = new List<OutputCommand>();
    public List<OutputCommand> teadDownCommands = new List<OutputCommand>();

    private NexmosphereController nexmosphereController;

    private void Start()
    {
        // Initialize NexmosphereController with appropriate settings
        nexmosphereController = gameObject.AddComponent<NexmosphereController>();
        nexmosphereController.Initialize(portName, baudRate);
        nexmosphereController.OnDataReceived += HandleDataReceived;
    }

    private void HandleDataReceived(string data)
    {
        foreach (var inputCommand in inputCommands)
        {
            if (data.Contains(inputCommand.command))
            {
                MainThreadDispatcher.RunOnMainThread(() => inputCommand.action.Invoke());
                break;
            }
        }
    }

    public void TriggerOutput(string triggerName)
    {
        foreach (var outputCommand in outputCommands)
        {
            if (outputCommand.triggerName == triggerName)
            {
                nexmosphereController.SendCommand(outputCommand.command);
                break;
            }
        }
    }

    private void TearDown()
    {
        foreach (var teadDownCommand in teadDownCommands)
        {
            nexmosphereController.SendCommand(teadDownCommand.command);
        }
    }

    private void OnDestroy()
    {
        if (nexmosphereController != null)
        {
            nexmosphereController.OnDataReceived -= HandleDataReceived;
        }
    }

    private void OnApplicationQuit()
    {
        TearDown();
    }

}
