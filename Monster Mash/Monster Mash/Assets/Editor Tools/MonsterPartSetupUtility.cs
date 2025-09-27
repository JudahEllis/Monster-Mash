using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

public class MonsterPartSetupUtility : EditorWindow
{
    private GameObject monsterPartPrefab;
    private MonsterPartType monsterPartType;
    private NeutralAttack.AttackType neutralAttackType;
    private HeavyAttack.HeavyAttackType heavyAttackType;
    private string newAnimControllerPath = "";
    private string newMonsterPartAnimationPath = "";
    private AnimatorController newAnimationController;

    private bool animationControllerSetup;
    private bool mainScriptsAndComponentsSetup;
    private bool colliderTagsAndScriptsSetup;

    [MenuItem("Tools/Monster Part Setup Utility")]
    public static void ShowWindow()
    {
        GetWindow<MonsterPartSetupUtility>("Monster Part Setup Utility", true);
    }

    private void OnGUI()
    {
        monsterPartPrefab = (GameObject)EditorGUILayout.ObjectField(
        "Monster Part Prefab",
        monsterPartPrefab,
        typeof(GameObject),
        false
    );

        monsterPartType = (MonsterPartType)EditorGUILayout.EnumPopup("Monster Part Type", monsterPartType);
        neutralAttackType = (NeutralAttack.AttackType)EditorGUILayout.EnumPopup("Neutral Attack Type", neutralAttackType);
        heavyAttackType = (HeavyAttack.HeavyAttackType)EditorGUILayout.EnumPopup("Heavy Attack Type", heavyAttackType);

        EditorGUILayout.Space(5);

        EditorGUI.BeginDisabledGroup(animationControllerSetup);
        if (GUILayout.Button("Setup Animation Controller"))
        {
            if (CheckForMissingRefs()) { return; }
            animationControllerSetup = true;
            SetupAnimationController();
            UpdatePrefab();
        }
        EditorGUI.EndDisabledGroup();

        EditorGUI.BeginDisabledGroup(mainScriptsAndComponentsSetup);
        if (GUILayout.Button("Add Main Scripts and Components"))
        {
            if (CheckForMissingRefs()) { return; }
            mainScriptsAndComponentsSetup = true;

            AddScriptsAndTags();
            AddComponents();

            UpdatePrefab();
        }
        EditorGUI.EndDisabledGroup();

        EditorGUI.BeginDisabledGroup(colliderTagsAndScriptsSetup);
        if (GUILayout.Button("Add Tags and Scripts to Colliders"))
        {
            if (CheckForMissingRefs()) { return; }
            colliderTagsAndScriptsSetup = true;

            ColliderSetup();
            UpdatePrefab();
        }
        EditorGUI.EndDisabledGroup();
        DisplayHelpInfo();

    }

   

    #region Animation Controller Setup
    private void SetupAnimationController()
    {
        Dictionary<MonsterPartType, string> partTypeToPath = new Dictionary<MonsterPartType, string>
        {
            { MonsterPartType.Arm, "Assets/Monster Parts/Arms" },
            { MonsterPartType.Leg, "Assets/Monster Parts/Legs" },
            { MonsterPartType.Wing, "Assets/Monster Parts/Wings" },
            { MonsterPartType.Mouth, "Assets/Monster Parts/Mouths" },
            { MonsterPartType.Decor, "Assets/Monster Parts/Decor" },
            { MonsterPartType.Horn, "Assets/Monster Parts/Horns" },
            { MonsterPartType.Tail, "Assets/Monster Parts/Tails" }

        };

        if (partTypeToPath.TryGetValue(monsterPartType, out string monsterPartPath))
        {
            string templatePath = monsterPartPath + "/Template Assets";
            AnimationClip[] templateClips = GetAnimationClips(templatePath + "/Template Clips");

            if (!Directory.Exists(templatePath)) 
            {
                if (EditorUtility.DisplayDialog("Template Assets Missing", $"The template assets for {monsterPartType.ToString()}s is missing or the folder structure is not setup properly. " +
                    $"Look at how the template assets for arm 1 are setup if you need an example", "Ok"))
                {
                    animationControllerSetup = false;
                    return;
                }
            }

            CopyTemplateAssets(templatePath, templateClips, monsterPartPath);
            SwapAnimationControllerClips();
            AddNewAnimationController();
        }
        else
        {
            // Heads and torso use universal rigs
            string torsoControllerPath = "Assets/Monster Parts/Torsos/Torso 1/Torso 1.controller";
            string headControllerPath = "Assets/Monster Parts/Heads/Head 1/Animations/Head 1.controller";

            if (monsterPartType == MonsterPartType.Torso)
            {
                newAnimationController = AssetDatabase.LoadAssetAtPath<AnimatorController>(torsoControllerPath);
            }
            else if (monsterPartType == MonsterPartType.Head)
            {
                newAnimationController = AssetDatabase.LoadAssetAtPath<AnimatorController>(headControllerPath);
            }

            AddNewAnimationController();
        }
    }

    private AnimationClip[] GetAnimationClips(string folderPath)
    {
        string[] guids = AssetDatabase.FindAssets("t:AnimationClip", new[] { folderPath });
        List<AnimationClip> clips = new List<AnimationClip>();

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            AnimationClip clip = AssetDatabase.LoadAssetAtPath<AnimationClip>(path);
            if (clip != null)
            {
                clips.Add(clip);
            }
        }
        return clips.ToArray();
    }

    private void CopyTemplateAssets(string templatePath, AnimationClip[] templateClips, string partPath)
    {
        string templateAnimationControllerPath = templatePath + "/Arm Template.controller";
        newMonsterPartAnimationPath = $"{partPath}/{monsterPartPrefab.name}/Animations";
        newAnimControllerPath = $"{newMonsterPartAnimationPath}/{monsterPartPrefab.name}.controller";

        if (!Directory.Exists(newMonsterPartAnimationPath))
        {
            Directory.CreateDirectory(newMonsterPartAnimationPath);
        }

        AssetDatabase.CopyAsset(templateAnimationControllerPath, newAnimControllerPath);

        string allTemplateClipsPath = templatePath + "/Template Clips";

        foreach (AnimationClip clip in templateClips)
        {
            string templateClipPrefix = "Arm 11 ";
            string clipName = clip.name;
            string removedPrefixClipName = "";

            if (clipName.StartsWith(templateClipPrefix))
            {
                removedPrefixClipName = clipName.Substring(templateClipPrefix.Length);
                removedPrefixClipName = removedPrefixClipName.Trim();
            }

            string templateClipPath = $"{allTemplateClipsPath}/{clipName}.anim";
            string newClipPath = $"{newMonsterPartAnimationPath}/{monsterPartPrefab.name} {removedPrefixClipName}.anim";

            AssetDatabase.CopyAsset(templateClipPath, newClipPath);
        }
    }

    private void SwapAnimationControllerClips()
    {
        newAnimationController = AssetDatabase.LoadAssetAtPath<AnimatorController>(newAnimControllerPath);
        AnimationClip[] newAnimationClips = GetAnimationClips(newMonsterPartAnimationPath);

        foreach (var layer in newAnimationController.layers)
        {
            var stateMachine = layer.stateMachine;
            foreach (var currentState in stateMachine.states)
            {
                var motion = currentState.state.motion;
                if (motion is AnimationClip oldClip)
                {
                    string oldName = oldClip.name;
                    int sepIndex = oldName.IndexOf(" - ");
                    if (sepIndex < 0) continue;

                    string suffix = oldName.Substring(sepIndex);

                    AnimationClip newClip = Array.Find(newAnimationClips,
                        c =>
                        {
                            int newSepIndex = c.name.IndexOf(" - ");
                            return newSepIndex >= 0 && c.name.Substring(newSepIndex) == suffix;
                        });
                    if (newClip != null)
                    {
                        currentState.state.motion = newClip;
                    }
                }
            }
        }
    }

    private void AddNewAnimationController()
    {
        if (!monsterPartPrefab.TryGetComponent<Animator>(out var animator))
        {
            animator = monsterPartPrefab.AddComponent<Animator>();
        }

        animator.runtimeAnimatorController = newAnimationController;
    }
    #endregion

    #region Main Script and Component Setup
    private void AddScriptsAndTags()
    {
        if (!monsterPartPrefab.TryGetComponent<NewMonsterPart>(out var newMonsterPartScript))
        {
            newMonsterPartScript = monsterPartPrefab.AddComponent<NewMonsterPart>();
        }

        if (!monsterPartPrefab.TryGetComponent<MonsterPartVisual>(out var monsterPartVisualScript))
        {
            monsterPartPrefab.AddComponent<MonsterPartVisual>();
        }

        if (!monsterPartPrefab.TryGetComponent<TempPartData>(out var tempPartDataScript)) 
        {
            tempPartDataScript = monsterPartPrefab.AddComponent<TempPartData>();
        }

        if (!monsterPartPrefab.TryGetComponent<PartFlipped>(out var partFlippedScript))
        {
            monsterPartPrefab.AddComponent<PartFlipped>();
        }

        monsterPartPrefab.tag = "Connection - Monster Part";
        newMonsterPartScript.PartType = monsterPartType;
        newMonsterPartScript.neutralAttack.Attack = neutralAttackType;
        newMonsterPartScript.heavyAttack.Attack = heavyAttackType;


        string partTypeName = monsterPartType.ToString();
        string tempPartDataPath = $"Build-A-Scare Parts/{partTypeName}s/{monsterPartPrefab.name}";

        tempPartDataScript.monsterPart.partPrefabPath = tempPartDataPath;
    }

    private void AddComponents()
    {
        if (!monsterPartPrefab.TryGetComponent<Rigidbody>(out var rigidbody))
        {
            rigidbody = monsterPartPrefab.AddComponent<Rigidbody>();
        }

        if (!monsterPartPrefab.TryGetComponent<SphereCollider>(out var sphereCollider))
        {
            sphereCollider = monsterPartPrefab.AddComponent<SphereCollider>();
        }

        if (!monsterPartPrefab.TryGetComponent<AudioSource>(out var audioSource)) 
        {
            audioSource = monsterPartPrefab.AddComponent<AudioSource>();
        }

        rigidbody.useGravity = true;
        rigidbody.isKinematic = true;

        sphereCollider.radius = 0.1f;

        audioSource.volume = 0.1f;
        audioSource.spatialBlend = 0.5f;
    }
    #endregion

    #region Collider Setup
    private void ColliderSetup()
    {
        Collider[] colliders = monsterPartPrefab.GetComponentsInChildren<Collider>();

        foreach (Collider collider in colliders)
        {
            //Skip root of prefab because its getting the sphere collider
            if (collider.name.Equals(collider.transform.root.name)) { continue; }

            if (!collider.TryGetComponent<monsterPartReference>(out var monsterPartReference))
            {
                monsterPartReference = collider.AddComponent<monsterPartReference>();
            }

            monsterPartReference.partReference = collider.transform.root.GetComponent<NewMonsterPart>();
            CheckIfHitboxOrHurtBox(collider, monsterPartReference);
            HitboxSetup(collider, monsterPartReference);
        }
    }

    private void HitboxSetup(Collider collider, monsterPartReference monsterPartReference)
    {
        if ((collider.name.ToLower().Contains("neutral") || collider.name.ToLower().Contains("heavy")) && monsterPartReference.isHitbox)
        {
            switch ((neutralAttackType, heavyAttackType))
            {
                case (NeutralAttack.AttackType.Jab, HeavyAttack.HeavyAttackType.Jab):
                    monsterPartReference.isJabOrSlash = true;
                    break;
                case (NeutralAttack.AttackType.Slash, HeavyAttack.HeavyAttackType.Slash):
                    monsterPartReference.isJabOrSlash = true;
                    break;
                case (NeutralAttack.AttackType.Projectile, HeavyAttack.HeavyAttackType.Projectile):
                    monsterPartReference.isProjectile = true;
                    break;
                case (NeutralAttack.AttackType.Boomerang, HeavyAttack.HeavyAttackType.Boomerang):
                    monsterPartReference.isBoomerang = true;
                    break;
                case (_, HeavyAttack.HeavyAttackType.Reel):
                    monsterPartReference.isReel = true;
                    break;
                case (_, HeavyAttack.HeavyAttackType.Grapple):
                    monsterPartReference.isGrapple = true;
                    break;
            }
        }
    }

    private void CheckIfHitboxOrHurtBox(Collider collider, monsterPartReference monsterPartReference)
    {
        if (collider.name.ToLower().Contains("hitbox"))
        {
            collider.gameObject.tag = "Hitbox";
            monsterPartReference.isHitbox = true;
        }
        else if (collider.name.ToLower().Contains("hurtbox"))
        {
            collider.gameObject.tag = "Hurtbox";
            monsterPartReference.isHitbox = false;
        }
    }
    #endregion


    private void UpdatePrefab()
    {
        EditorUtility.SetDirty(monsterPartPrefab);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private void DisplayHelpInfo()
    {
        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Important Info:");
        EditorGUILayout.Space(5);
        EditorGUILayout.HelpBox("This tool aims to automate mostly all aspects of setting up a monster part but there are some things that can not be automated like placing colliders." +
            " So please look over the monster part after using the setup utility to make sure everything is correct.", MessageType.Info);
        EditorGUILayout.HelpBox("In order for the animation controller setup to work please make sure that the part type has template assets setup. " +
            "Please refer to the folder structure for arm 1 as an example.", MessageType.Info);
        EditorGUILayout.HelpBox("After clicking the button for \"setup animation controller\" it may look like the window has stalled. " +
            "This is because setting up the animation controller involves a lot of steps and it accesses the file system which can be slow. " +
            "While the setup process is executing please do not try to click anything or try to spam the button as this could cause a crash.", MessageType.Info);
        EditorGUILayout.HelpBox("In order for hitboxes and hurtboxes to be tagged correctly the game object they are attached to needs to have the word \"Hitbox\" or \"Hurtbox\" in the name. Not case sensitive", MessageType.Info);
        EditorGUILayout.HelpBox("In order for neutral and heavy hitboxes to have special attacks automatically enabled, the game object they are attached to needs to have the word \"Neutral\" or \"Heavy\" in the name. Not case sensitive", MessageType.Info);
        EditorGUILayout.HelpBox("After each step is completed the button will be disabled, this is to make it easier to know what steps have been completed. If you need to redo a step you can open and close the window to reset it.", MessageType.Info);
    }

    private bool CheckForMissingRefs()
    {
        if (monsterPartType == MonsterPartType.None)
        {
            if (EditorUtility.DisplayDialog("Monster Part Type Not Set", "The monster part type has not been assigned. This is required in order for the setup utility to work.", "Ok"))
            {
                return true;
            }
        }

        if (monsterPartPrefab == null)
        {
            if (EditorUtility.DisplayDialog("Monster Part Prefab Not Set", "The monster part prefab has not been assigned. This is required for the setup utility to work. you can't really setup a monster part without well... a monster part.", "Ok"))
            {
                return true;
            }
        }
        return false;
    }
}

