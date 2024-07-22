using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class StaticInput
{
    public string command;
    public UnityEvent action;
}

[System.Serializable]
public class DynamicInput
{
    public string commandRoot; // Command root without the dynamic part
    public UnityEvent<int> action; // Action to invoke with the dynamic value
}

[System.Serializable]
public class OutputCommand
{
    public string command;
    public string triggerName;
}

public class NexmosphereManager : Singleton<NexmosphereManager>
{
    public List<StaticInput> staticInputs = new List<StaticInput>();
    public List<OutputCommand> outputCommands = new List<OutputCommand>();
    public List<OutputCommand> tearDownCommands = new List<OutputCommand>();

    private NexmosphereController nexmosphereController;

    private void Start()
    {

    }

    public void HandleDataReceived(string data)
    {
        Debug.Log($"Data received: {data}");

        // Handle static commands
        foreach (var staticInput in staticInputs)
        {
            if (data.Contains(staticInput.command))
            {
                Debug.Log($"Static command matched: {staticInput.command}");
                MainThreadDispatcher.RunOnMainThread(() => staticInput.action.Invoke());
                return;
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

    private void TearDown()
    {
        foreach (var tearDownCommand in tearDownCommands)
        {
            nexmosphereController.SendCommand(tearDownCommand.command);
        }
    }
}
