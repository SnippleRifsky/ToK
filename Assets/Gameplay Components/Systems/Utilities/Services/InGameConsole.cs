using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameConsole : MonoBehaviour
{
    [SerializeField] private Text consoleText;
    [SerializeField] private int maxMessages = 100;

    private Queue<string> messageQueue = new Queue<string>();

    private void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    private void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    private void HandleLog(string logString, string stackTrace, LogType type)
    {
        if (messageQueue.Count >= maxMessages)
        {
            messageQueue.Dequeue();
        }

        messageQueue.Enqueue(logString);
        consoleText.text = string.Join("\n", messageQueue.ToArray());
    }
}