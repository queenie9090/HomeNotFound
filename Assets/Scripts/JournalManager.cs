using UnityEngine;
using TMPro;
using System.Collections.Generic; // Needed for Lists

[System.Serializable]
public class JournalTask
{
    public string taskDescription;
    public bool isDone = false;
}

public class JournalManager : MonoBehaviour
{
    public static JournalManager Instance;

    [Header("UI Settings")]
    public TextMeshProUGUI taskText;
    public string completedColor = "green";

    [Header("Tasks List")]
    // You can now add as many tasks as you want in the Inspector!
    public List<JournalTask> tasks = new List<JournalTask>();

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Start()
    {
        RefreshJournal();
    }

    // Call this using: JournalManager.Instance.CompleteTask(0); 
    // (Note: Lists start at 0, so Task 1 is index 0)
    public void CompleteTask(int index)
    {
        if (index >= 0 && index < tasks.Count)
        {
            tasks[index].isDone = true;
            RefreshJournal();
        }
    }

    void RefreshJournal()
    {
        if (taskText == null) return;

        string display = "";

        for (int i = 0; i < tasks.Count; i++)
        {
            string line = (i + 1) + ". " + tasks[i].taskDescription;

            if (tasks[i].isDone)
            {
                display += $"<color={completedColor}>{line}</color>\n\n";
            }
            else
            {
                display += line + "\n\n";
            }
        }

        taskText.text = display;
    }
}