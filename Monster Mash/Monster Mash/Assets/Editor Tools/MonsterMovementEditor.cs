using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

public class MonsterMovementEditor : EditorWindow
{
    private AttackConfigList ruleset = new();
    private Vector2 scrollPos;
    private const string jsonPath = "Assets/Resources/Data/attack_configs.json";
    private int configToRemove = -1;

    private int startPageIndex = 0;
    private int endPageIndex = 5;
    private const int desiredPageCount = 5;

    private bool scrollToBottomNextFrame = false;

    [MenuItem("Tools/Monster Movement Editor")]
    public static void ShowWindow()
    {
        GetWindow<MonsterMovementEditor>("Monster Movement Editor", true);
    }

    private void OnEnable()
    {
        LoadJson();
    }

    private void OnGUI()
    {
        GUILayout.Label("Movement Rules", EditorStyles.largeLabel);
        if (ruleset == null)
        {
            GUILayout.Label("No Ruleset Loaded");
            return;
        }

        EditorGUILayout.Space();

        // Loops through the ruleset list and displays the configs
        DisplayConfigList();
        // if a config has been marked for deletion by pressing the delete button this is where the config is actually removed
        CheckRemoveConfig();

        AddButton();

        PageControl();

        EditorGUILayout.Space();

        SaveAndLoadButtons();
    }

    private void DisplayConfigList()
    {
        int safeEndIndex = Mathf.Min(endPageIndex, ruleset.Configs.Count);

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        
        for (int i = startPageIndex; i < safeEndIndex; i++)
        {
            var config = ruleset.Configs[i];

            EditorGUILayout.BeginVertical("box");
            DisplayConfig(config);
            DeleteButton(i);
            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.EndScrollView();

        if (scrollToBottomNextFrame)
        {
            scrollPos.y = float.MaxValue;
            scrollToBottomNextFrame = false;

            Repaint();
        }
    }

    private void PageControl()
    {
        int buttonWidth = 60;
        int buttonHeight = 25;

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("|<-", GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight)))
        {
            startPageIndex = 0;
            endPageIndex = desiredPageCount;
            ResetScroll();
        }

        if (GUILayout.Button("<-", GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight)))
        {
            if (startPageIndex > 0)
            {
                endPageIndex = startPageIndex;
                startPageIndex -= desiredPageCount;
                startPageIndex = Mathf.Clamp(startPageIndex, 0, ruleset.Configs.Count);
                ResetScroll();
            }
        }

        if (GUILayout.Button("->", GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight)))
        {
            if (endPageIndex < ruleset.Configs.Count)
            {
                startPageIndex = endPageIndex;
                endPageIndex += desiredPageCount;
                endPageIndex = Mathf.Clamp(endPageIndex, 0, ruleset.Configs.Count);
                ResetScroll();
            }
        }

        if (GUILayout.Button("->|", GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight)))
        {
            GoToLastPage();
            ResetScroll();
        }
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
    }

    private void GoToLastPage()
    {
        int count = ruleset.Configs.Count;

        startPageIndex = Mathf.Max(0, count - desiredPageCount);
        endPageIndex = count;
    }

    private void ResetScroll()
    {
        scrollPos.y = 0;
    }

    private void AddButton()
    {
        int buttonWidth = 110;
        int buttonHeight = 25;

        if (GUILayout.Button("Add New Config", GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight)))
        {
            ruleset.Configs.Add(new AttackConfig());
            GoToLastPage();
            scrollToBottomNextFrame = true;
        }
    }

    private void DeleteButton(int index)
    {
        int buttonWidth = 90;
        int buttonHeight = 25;
        int margin = 5;

        GUILayout.Space(margin);
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Delete Config", GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight)))
        {
            configToRemove = index;
        }

        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.Space(margin);
    }

    private void SaveAndLoadButtons()
    {
        int buttonWidth = 150;
        int buttonHeight = 25;

        GUILayout.Label("Warning: Saving overwrites all existing data. Do not save with an empty list.", EditorStyles.helpBox);
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Reload Data", GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight)))
        {
            LoadJson();
        }

        if (GUILayout.Button("Save Changes", GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight)))
        {
            SaveJson();
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }

    private void CheckRemoveConfig()
    {
        // if we try to remove an element during the loop it causes an error so we need to mark it and delete it later.
        if (configToRemove >= 0)
        {
            ruleset.Configs.RemoveAt(configToRemove);
            // reset index so it doesent try to repeatedly remove
            configToRemove = -1;
        }
    }

    private void LoadJson()
    {
        if (File.Exists(jsonPath))
        {
            string jsonText = File.ReadAllText(jsonPath);
            ruleset = JsonUtility.FromJson<AttackConfigList>(jsonText);
        }
        else
        {
            ruleset = new AttackConfigList();
        }

        if (ruleset.Configs == null)
        {
            ruleset.Configs = new List<AttackConfig>();
        }
    }

    private void SaveJson()
    {
        string jsonText = JsonUtility.ToJson(ruleset, true);
        File.WriteAllText(jsonPath, jsonText);
        AssetDatabase.Refresh();
    }

    private void DisplayConfig(AttackConfig config)
    {
        int labelWidth = 233;
        float previousLabelWidth = EditorGUIUtility.labelWidth;
        EditorGUIUtility.labelWidth = labelWidth;

        config.ConfigName = EditorGUILayout.TextField("Config Name", config.ConfigName);
        config.forwardInputTorsoCommand = EditorGUILayout.TextField("Forward Input Torso Command", config.forwardInputTorsoCommand);
        config.backwardInputTorsoCommand = EditorGUILayout.TextField("Backward Input Torso Command", config.backwardInputTorsoCommand);
        config.upwardInputTorsoCommand = EditorGUILayout.TextField("Upward Input Torso Command", config.upwardInputTorsoCommand);
        config.downwardInputTorsoCommand = EditorGUILayout.TextField("Downward Input Torso Command", config.downwardInputTorsoCommand);

        config.forwardInputHeadCommand = EditorGUILayout.TextField("Forward Input Head Command", config.forwardInputHeadCommand);
        config.backwardInputHeadCommand = EditorGUILayout.TextField("Backward Input Head Command", config.backwardInputHeadCommand);
        config.upwardInputHeadCommand = EditorGUILayout.TextField("Upward Input Head Command", config.upwardInputHeadCommand);
        config.downwardInputHeadCommand = EditorGUILayout.TextField("Downward Input Head Command", config.downwardInputHeadCommand);

        config.forwardNeutralMovementCommand = EditorGUILayout.TextField("Forward Neutral Movement Command", config.forwardNeutralMovementCommand);
        config.backwardNeutralMovementCommand = EditorGUILayout.TextField("Backward Neutral Movement Command", config.backwardNeutralMovementCommand);
        config.upwardNeutralMovementCommand = EditorGUILayout.TextField("Upward Neutral Movement Command", config.upwardNeutralMovementCommand);
        config.downwardNeutralMovementCommand = EditorGUILayout.TextField("Downward Neutral Movement Command", config.downwardNeutralMovementCommand);

        config.forwardHeavyMovementCommand = EditorGUILayout.TextField("Forward Heavy Movement Command", config.forwardHeavyMovementCommand);
        config.backwardHeavyMovementCommand = EditorGUILayout.TextField("Backward Heavy Movement Command", config.backwardHeavyMovementCommand);
        config.upwardHeavyMovementCommand = EditorGUILayout.TextField("Upward Heavy Movement Command", config.upwardHeavyMovementCommand);
        config.downwardHeavyMovementCommand = EditorGUILayout.TextField("Downward Heavy Movement Command", config.downwardHeavyMovementCommand);

        EditorGUIUtility.labelWidth = previousLabelWidth;
    }
}
