using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class JournalInLv3 : MonoBehaviour
{
    public static JournalInLv3 Instance;

    [Header("UI Settings")]
    public TextMeshProUGUI taskText;
    public string completedColor = "green";

    [Header("Tasks List")]
    public List<JournalTask> tasks = new List<JournalTask>();

    private int currentTaskIndex = 0;

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Start()
    {
        RefreshJournal();
    }

    public void CompleteTask(int index)
    {
        if (index == currentTaskIndex && index < tasks.Count)
        {
            tasks[index].isDone = true;
            currentTaskIndex++; // unlock next task
            RefreshJournal();
        }
    }

    void RefreshJournal()
    {
        if (taskText == null) return;

        string display = "";

        for (int i = 0; i <= currentTaskIndex && i < tasks.Count; i++)
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