using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System;

public class MonsterMovementEditor : EditorWindow
{
    private AttackConfigList ruleset = new();
    private Vector2 scrollPos;
    private const string jsonPath = "Assets/Resources/Data/attack_configs.json";
    private AttackConfig configToRemove = null;

    private int startPageIndex = 0;
    private int endPageIndex = 5;
    private const int desiredPageCount = 5;


    private string searchText = "";

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
        TopBarLayout();

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

        if (string.IsNullOrWhiteSpace(searchText))
        {
            AddButton();
            PageControl();
        }

        EditorGUILayout.Space();

        SaveAndLoadButtons();
    }

    private void DisplayConfigList()
    {
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        List<AttackConfig> listToDisplay;

        bool isSearching = !string.IsNullOrWhiteSpace(searchText);
        string normalizedSearch = searchText?.Trim().ToLower() ?? "";

        //if searching get the entire list and filter it by the search text and config name
        if (isSearching)
        {
            listToDisplay = ruleset.Configs.FindAll(config =>
                !string.IsNullOrEmpty(config.ConfigName) &&
                config.ConfigName.ToLower().Contains(normalizedSearch)
            );
        }
        // if not searching then get a subset of the list that only includes the current page
        else
        {
            int safeStartIndex = Mathf.Clamp(startPageIndex, 0, Mathf.Max(0, ruleset.Configs.Count - 1));
            int safeEndIndex = Mathf.Clamp(endPageIndex, 0, ruleset.Configs.Count);
            int count = Mathf.Max(0, safeEndIndex - safeStartIndex);

            listToDisplay = ruleset.Configs.GetRange(safeStartIndex, count);
        }

        // Display the contents of the list and hide the delete button if searching.
        for (int i = 0; i < listToDisplay.Count; i++)
        {
            var config = listToDisplay[i];
            EditorGUILayout.BeginVertical("box");

            DisplayConfig(config);
            DeleteButton(config);

            EditorGUILayout.EndVertical();
        }

        if (listToDisplay.Count == 0)
        {
            EditorGUILayout.HelpBox("No configs match your search.", MessageType.Info);
        }

        EditorGUILayout.EndScrollView();
    }


    private void PageControl()
    {
        const int buttonWidth = 60;
        const int buttonHeight = 25;

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

    private void TopBarLayout()
    {
        const int searchBarWidth = 200;
        const float searchLabelWidth = 45;

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Movement Rules", EditorStyles.largeLabel);
        GUILayout.FlexibleSpace();
        EditorGUIUtility.labelWidth = searchLabelWidth;
        searchText = EditorGUILayout.TextField("Search", searchText, GUILayout.Width(searchBarWidth));
        EditorGUILayout.EndHorizontal();
    }

    private void AddButton()
    {
        const int buttonWidth = 110;
        const int buttonHeight = 25;

        if (GUILayout.Button("Add New Config", GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight)))
        {
            ruleset.Configs.Add(new AttackConfig());
            GoToLastPage();
            scrollPos.y = int.MaxValue;
        }
    }

    private void DeleteButton(AttackConfig config)
    {
        const int buttonWidth = 90;
        const int buttonHeight = 25;
        const int margin = 5;

        GUILayout.Space(margin);
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Delete Config", GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight)))
        {
            configToRemove = config;
        }

        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.Space(margin);
    }

    private void SaveAndLoadButtons()
    {
        const int buttonWidth = 150;
        const int buttonHeight = 25;

        GUILayout.Label("Warning: Saving overwrites all existing data. Do not save with an empty list.", EditorStyles.helpBox);
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Reload Data", GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight)))
        {
            LoadJson();
        }

        if (GUILayout.Button("Save Changes", GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight)))
        {
            bool confirm = EditorUtility.DisplayDialog(
                "Confirm Save",
                "This will overwrite all data in the JSON file. \nAre You sure you want to continue?",
                "Yes",
                "Cancel");

            if (confirm)
            {
                SaveJson();
            }
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }

    private void CheckRemoveConfig()
    {
        // if we try to remove an element during the loop it causes an error so we need to mark it and delete it later.
        if (configToRemove != null)
        {
            ruleset.Configs.Remove(configToRemove);
            // reset index so it doesent try to repeatedly remove
            configToRemove = null;
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
        try
        {
            // create a backup just incase
            if (File.Exists(jsonPath))
            {
                string folder = Path.GetDirectoryName(jsonPath);
                string backupFileName = $"attack_configs_backup_{DateTime.Now:MM-dd-yyyy}.json";
                string backupPath = Path.Combine(folder, backupFileName);
                const int maxBackups = 3;

                
                File.Copy(jsonPath, backupPath, overwrite: true);

                string[] backupFiles = Directory.GetFiles(folder, "attack_configs_backup_*.json");

                var datedBackups = backupFiles
                    .Select(path =>
                    {
                        string filename = Path.GetFileNameWithoutExtension(path);
                        string datePart = filename.Replace("attack_configs_backup_", "");
                        if (DateTime.TryParseExact(datePart, "MM-dd-yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime date))
                            return new { Path = path, Date = date };
                        return null;
                    })
                    .Where(entry => entry != null)
                    .OrderBy(entry => entry.Date)
                    .ToList();

                if (datedBackups.Count > maxBackups)
                {
                    for (int i = 0; i < datedBackups.Count - maxBackups; i++)
                    {
                        File.Delete(datedBackups[i].Path);
                    }
                }
            }

            // save the data to the main json file

            string jsonText = JsonUtility.ToJson(ruleset, true);
            File.WriteAllText(jsonPath, jsonText);
            AssetDatabase.Refresh();

            
            EditorUtility.DisplayDialog(
                "Saved",
                "Changes have been successfully saved to the JSON file. A backup has been created",
                "OK"
            );
        }
        catch (Exception ex)
        {
            Debug.LogError("Error saving JSON: " + ex.Message);

            EditorUtility.DisplayDialog(
                "Save Failed",
                "An error occurred while saving the JSON file:\n" + ex.Message,
                "OK"
            );
        }
    }



    private void DisplayConfig(AttackConfig config)
    {
        const int labelWidth = 233;
        const int contentSpacing = 10;
        float previousLabelWidth = EditorGUIUtility.labelWidth;

        EditorGUIUtility.labelWidth = labelWidth;

        config.ConfigName = EditorGUILayout.TextField("Config Name", config.ConfigName);
        config.PartType = (MonsterPartType)EditorGUILayout.EnumPopup("Monster Part Type", config.PartType);
        config.PartConnections = (MonsterPartConnectionMask)EditorGUILayout.EnumFlagsField("Targeted Connection Points", config.PartConnections);
        GUILayout.Space(contentSpacing);

        GUILayout.Label("Movement Commands", EditorStyles.boldLabel);

        config.forwardInputTorsoCommand = (TorsoCommand)EditorGUILayout.EnumPopup("Forward Input Torso Command", config.forwardInputTorsoCommand);
        config.backwardInputTorsoCommand = (TorsoCommand)EditorGUILayout.EnumPopup("Backward Input Torso Command", config.backwardInputTorsoCommand);
        config.upwardInputTorsoCommand = (TorsoCommand)EditorGUILayout.EnumPopup("Upward Input Torso Command", config.upwardInputTorsoCommand);
        config.downwardInputTorsoCommand = (TorsoCommand)EditorGUILayout.EnumPopup("Downward Input Torso Command", config.downwardInputTorsoCommand);

        config.forwardInputHeadCommand = (HeadCommand)EditorGUILayout.EnumPopup("Forward Input Head Command", config.forwardInputHeadCommand);
        config.backwardInputHeadCommand = (HeadCommand)EditorGUILayout.EnumPopup("Backward Input Head Command", config.backwardInputHeadCommand);
        config.upwardInputHeadCommand = (HeadCommand)EditorGUILayout.EnumPopup("Upward Input Head Command", config.upwardInputHeadCommand);
        config.downwardInputHeadCommand = (HeadCommand)EditorGUILayout.EnumPopup("Downward Input Head Command", config.downwardInputHeadCommand);

        config.forwardNeutralMovementCommand = (NeutralMovementCommand)EditorGUILayout.EnumPopup("Forward Neutral Movement Command", config.forwardNeutralMovementCommand);
        config.backwardNeutralMovementCommand = (NeutralMovementCommand)EditorGUILayout.EnumPopup("Backward Neutral Movement Command", config.backwardNeutralMovementCommand);
        config.upwardNeutralMovementCommand = (NeutralMovementCommand)EditorGUILayout.EnumPopup("Upward Neutral Movement Command", config.upwardNeutralMovementCommand);
        config.downwardNeutralMovementCommand = (NeutralMovementCommand)EditorGUILayout.EnumPopup("Downward Neutral Movement Command", config.downwardNeutralMovementCommand);

        config.forwardHeavyMovementCommand = (HeavyMovementCommand)EditorGUILayout.EnumPopup("Forward Heavy Movement Command", config.forwardHeavyMovementCommand);
        config.backwardHeavyMovementCommand = (HeavyMovementCommand)EditorGUILayout.EnumPopup("Backward Heavy Movement Command", config.backwardHeavyMovementCommand);
        config.upwardHeavyMovementCommand = (HeavyMovementCommand)EditorGUILayout.EnumPopup("Upward Heavy Movement Command", config.upwardHeavyMovementCommand);
        config.downwardHeavyMovementCommand = (HeavyMovementCommand)EditorGUILayout.EnumPopup("Downward Heavy Movement Command", config.downwardHeavyMovementCommand);

        EditorGUIUtility.labelWidth = previousLabelWidth;
    }

}
