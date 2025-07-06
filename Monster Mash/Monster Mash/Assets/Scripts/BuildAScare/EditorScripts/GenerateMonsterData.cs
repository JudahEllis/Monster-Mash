using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GenerateMonsterData : MonoBehaviour
{
    [SerializeField]
    MonsterData monsterInformation;
    public void GenerateData()
    {
        if(monsterInformation.monsterParts.Count == 0)
        {
            foreach (Transform child in transform)
            { 
                if(child.GetComponent<TempPartData>() != null)
                {
                    MonsterPartData part = new MonsterPartData();

                    part.partPrefabPath = child.GetComponent<TempPartData>().monsterPart.partPrefabPath;

                    part.partRotation = child.localRotation;

                    part.partScale = child.localScale;

                    part.partPosition = child.localPosition;

                    monsterInformation.monsterParts.Add(part);
                }
            }
        }
    }
}

#if UNITY_EDITOR

[CustomEditor(typeof(GenerateMonsterData))]
public class MonsterEditor : Editor
{
    private SerializedProperty monsterInfo;

    private void OnEnable()
    {
       
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GenerateMonsterData generate = (GenerateMonsterData)target;

        if (GUILayout.Button("Generate Data"))
        {
            generate.GenerateData();
        }
    }
}

#endif

