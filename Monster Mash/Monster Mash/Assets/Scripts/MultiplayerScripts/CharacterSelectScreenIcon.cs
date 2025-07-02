using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectScreenIcon : MonoBehaviour, ICursorSelectable
{
    public MonsterData buttonMonster;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ICursorSelectable.OnSelect(MultiplayerCursor cursor)
    {
        cursor.SelectCharacter(buttonMonster);
    }

    void ICursorSelectable.OnEnter(MultiplayerCursor cursor)
    {

    }

    void ICursorSelectable.OnExit(MultiplayerCursor cursor)
    {

    }
}
