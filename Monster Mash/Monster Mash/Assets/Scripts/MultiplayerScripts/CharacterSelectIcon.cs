using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectIcon : MonoBehaviour, MultiplayerJoinManager.IQuickplayButtonable
{
    [SerializeField]
    private GameObject buttonCharacter;
    public void ButtonSelected(MultiplayerCursor cursor)
    {
        cursor.SelectCharacter(buttonCharacter);
    }
}
