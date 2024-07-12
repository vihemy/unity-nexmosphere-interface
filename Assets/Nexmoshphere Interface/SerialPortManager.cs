using System;
using System.IO.Ports;
using System.Threading;
using UnityEngine;

public class SerialPortManager : MonoBehaviour
{
    private SerialPort serialPort;
    private Thread readThread;
    private bool isRunning;
    private string portName;
    private int baudRate;

    // Event to handle incoming data
    public event Action<string> OnDataReceived;

    // Initialize the serial port with given settings
    public void Initialize(string portName, int baudRate, Parity parity = Parity.None, int dataBits = 8, StopBits stopBits = StopBits.One)
    {
        this.portName = portName;
        this.baudRate = baudRate;

        InitializeSerialPort(parity, dataBits, stopBits);
        StartReadingThread();
    }

    private void InitializeSerialPort(Parity parity, int dataBits, StopBits stopBits)
    {
        serialPort = new SerialPort(portName, baudRate, parity, dataBits, stopBits)
        {
            ReadTimeout = 1000 // Set a timeout value
        };

        try
        {
            serialPort.Open();
            isRunning = true;
            Debug.Log("Serial port opened successfully.");
        }
        catch (Exception e)
        {
            MainThreadDispatcher.RunOnMainThread(() => Debug.LogError($"Failed to open serial port: {e.Message}"));
        }
    }

    private void StartReadingThread()
    {
        readThread = new Thread(Read);
        readThread.Start();
    }

    // Method to read data from the serial port
    private void Read()
    {
        while (isRunning && serialPort != null && serialPort.IsOpen)
        {
            try
            {
                string message = serialPort.ReadLine();
                MainThreadDispatcher.RunOnMainThread(() => OnDataReceived?.Invoke(message));
            }
            catch (TimeoutException) { }
            catch (Exception e)
            {
                MainThreadDispatcher.RunOnMainThread(() => Debug.LogError($"Error reading from serial port: {e.Message}"));
            }
        }
    }

    // Method to send data to the serial port
    public void SendSerialMessage(string message)
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            try
            {
                serialPort.WriteLine(message);
                MainThreadDispatcher.RunOnMainThread(() => Debug.Log($"Sent message: {message}"));
            }
            catch (Exception e)
            {
                MainThreadDispatcher.RunOnMainThread(() => Debug.LogError($"Error writing to serial port: {e.Message}"));
            }
        }
    }

    // Cleanup resources on destroy
    private void OnDestroy()
    {
        CleanUp();
    }

    private void OnApplicationQuit()
    {
        CleanUp();
    }

    private void CleanUp()
    {
        isRunning = false;
        StopReadingThread();
        CloseSerialPort();
    }

    private void StopReadingThread()
    {
        if (readThread != null && readThread.IsAlive)
        {
            readThread.Join();
        }
    }

    private void CloseSerialPort()
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
            MainThreadDispatcher.RunOnMainThread(() => Debug.Log("Serial port closed."));
        }
    }
}
