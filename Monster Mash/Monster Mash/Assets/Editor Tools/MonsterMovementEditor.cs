using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

public class MonsterMovementEditor : EditorWindow
{
    private AttackConfigList ruleset = new AttackConfigList();
    private Vector2 scrollPos;
    private readonly string jsonPath = "Assets/Resources/Data/attack_configs.json";

    private const int deleteButtonWidth = 90;
    private const int deleteButtonHeight = 25;
    private const int deleteButtonMargin = 5;

    private const int configLabelWidth = 233;

    [MenuItem("Tools/Monster Movement Editor")]
    public static void ShowWindow()
    {
        GetWindow<MonsterMovementEditor>("Monster Movement Editor");
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
        

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        int configToRemove = -1;
        for (int i = 0; i < ruleset.Configs.Count; i++)
        {
            var config = ruleset.Configs[i];
            EditorGUILayout.BeginVertical("box");
            DisplayConfig(config);
            GUILayout.Space(deleteButtonMargin);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
           
            if (GUILayout.Button("Delete Config", GUILayout.Width(deleteButtonWidth), GUILayout.Height(deleteButtonHeight)))
            {
                configToRemove = i;
            }
            
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.Space(deleteButtonMargin);

            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.EndScrollView();

        // if we try to remove an element during the loop it causes an error so we need to mark it and delete it later.
        if (configToRemove >= 0)
        {
            ruleset.Configs.RemoveAt(configToRemove);
        }

        if (GUILayout.Button("Add New Config", GUILayout.Width(110), GUILayout.Height(25)))
        {
            ruleset.Configs.Add(new AttackConfig());
        }

        EditorGUILayout.Space();

        GUILayout.Label("Warning: Saving overwrites all existing data. Do not save with an empty list.", EditorStyles.helpBox);
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Reload Data", GUILayout.Width(150),  GUILayout.Height(25)))
        {
            LoadJson();
        }

        if (GUILayout.Button("Save Changes", GUILayout.Width(150), GUILayout.Height(25)))
        {
            SaveJson();
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        
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
        float previousLabelWidth = EditorGUIUtility.labelWidth;
        EditorGUIUtility.labelWidth = configLabelWidth;

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
