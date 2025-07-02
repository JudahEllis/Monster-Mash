using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

public class MonsterMovementEditor : EditorWindow
{
    private AttackConfigList ruleset = new();
    private Vector2 scrollPos;
    private const string jsonPath = "Assets/Resources/Data/attack_configs.json";
    private AttackConfig configToRemove = null;

    private int startPageIndex = 0;
    private int endPageIndex = 5;
    private const int desiredPageCount = 5;

    private bool scrollToBottomNextFrame = false;

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

    private void TopBarLayout()
    {
        int searchBarWidth = 200;
        float searchLabelWidth = 45;

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Movement Rules", EditorStyles.largeLabel);
        GUILayout.FlexibleSpace();
        EditorGUIUtility.labelWidth = searchLabelWidth;
        searchText = EditorGUILayout.TextField("Search", searchText, GUILayout.Width(searchBarWidth));
        EditorGUILayout.EndHorizontal();
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

    private void DeleteButton(AttackConfig config)
    {
        int buttonWidth = 90;
        int buttonHeight = 25;
        int margin = 5;

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
            string jsonText = JsonUtility.ToJson(ruleset, true);
            File.WriteAllText(jsonPath, jsonText);
            AssetDatabase.Refresh();

            // Show success popup only after successful write
            EditorUtility.DisplayDialog(
                "Saved",
                "Changes have been successfully saved to the JSON file.",
                "OK"
            );
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error saving JSON: " + ex.Message);

            // Optional: show an error dialog to the user
            EditorUtility.DisplayDialog(
                "Save Failed",
                "An error occurred while saving the JSON file:\n" + ex.Message,
                "OK"
            );
        }
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
