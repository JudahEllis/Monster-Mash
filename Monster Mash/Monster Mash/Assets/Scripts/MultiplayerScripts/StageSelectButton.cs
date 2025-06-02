using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectButton : MonoBehaviour, ICursorSelectable
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ICursorSelectable.OnSelect(MultiplayerCursor cursor)
    {
        //Temp When Moved to StageManager script will grab manager on Start

        cursor.joinManager.SelectStage();
    }

    void ICursorSelectable.OnEnter(MultiplayerCursor cursor)
    {

    }

    void ICursorSelectable.OnExit(MultiplayerCursor cursor)
    {

    }
}
