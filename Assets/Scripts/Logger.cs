using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Logger : MonoBehaviour
{
    public static Logger Instance { get; private set; }

    private List<LogEntry> logEntries = new List<LogEntry>();
    private string logFilePath;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            logFilePath = Path.Combine(Application.persistentDataPath, "game_logs.json");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LogEvent(string eventType, string message)
    {
        LogEntry entry = new LogEntry
        {
            eventType = eventType,
            message = message,
            timestamp = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")
        };
        logEntries.Add(entry);
    }

    public void SaveLogsOnExit()
    {
        WriteLogToFile();
    }

    private void WriteLogToFile()
    {
        try
        {
            LogData logData = new LogData { entries = logEntries };
            string json = JsonUtility.ToJson(logData, true);
            File.WriteAllText(logFilePath, json);
            Debug.Log($"Logs saved to {logFilePath}");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to save logs: " + e.Message);
        }
    }

    private void OnApplicationQuit()
    {
        WriteLogToFile();
    }

    [System.Serializable]
    private class LogEntry
    {
        public string eventType;
        public string message;
        public string timestamp;
    }

    [System.Serializable]
    private class LogData
    {
        public List<LogEntry> entries;
    }
}
