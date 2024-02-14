using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectIcon : MonoBehaviour, MultiplayerJoinManager.IQuickplayButtonable
{
    [SerializeField]
    private GameObject buttonCharacter;

    GameObject storedMonster;
    private void Start()
    {
        storedMonster = gameObject.transform.parent.GetChild(0).gameObject;
    }
    public void ButtonSelected(MultiplayerCursor cursor)
    {
        cursor.SelectCharacter(buttonCharacter, storedMonster);
    }
}
