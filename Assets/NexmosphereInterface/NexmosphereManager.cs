// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.Events;

// public class NexmosphereManager : MonoBehaviour
// {
//     public string portName = "COM3";
//     public int baudRate = 11520;


//     private NexmosphereController nexmosphereController;

//     private void Start()
//     {
//         // Initialize NexmosphereController with appropriate settings
//         nexmosphereController = gameObject.AddComponent<NexmosphereController>();
//         nexmosphereController.Initialize(portName, baudRate);
//         nexmosphereController.OnDataReceived += HandleDataReceived;
//     }

//     private void HandleDataReceived(string data)
//     {
//         foreach (var inputCommand in inputCommands)
//         {
//             if (data.Contains(inputCommand.command))
//             {
//                 MainThreadDispatcher.RunOnMainThread(() => inputCommand.action.Invoke());
//                 break;
//             }
//         }
//     }

//     private void OnDestroy()
//     {
//         if (nexmosphereController != null)
//         {
//             nexmosphereController.OnDataReceived -= HandleDataReceived;
//         }
//     }


// }
