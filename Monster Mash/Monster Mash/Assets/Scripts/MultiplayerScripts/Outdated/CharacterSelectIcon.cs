using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectIcon : MonoBehaviour, MultiplayerJoinManager.IQuickplayButtonable
{
    [SerializeField]
    private GameObject buttonCharacter;

    [SerializeField]
    private GameObject monsterVisual;

    [SerializeField]
    MonsterData monsterToSelect;

    GameObject storedMonster;
    private void Start()
    {
        storedMonster = gameObject.transform.parent.GetChild(0).gameObject;
    }
    public void ButtonSelected(MultiplayerCursor cursor)
    {
        //cursor.joinManager.playerInfo[cursor.cursorIndex].characterModel = monsterVisual;
        //cursor.SelectCharacter(monsterToSelect, storedMonster);
    }
}
