using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectArrow : MonoBehaviour, ICursorSelectable
{
    [SerializeField]
    bool isPositive;

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
        if (isPositive)
        {
            cursor.joinManager.IncreaseStageIndex();
        }

        else
        {
            cursor.joinManager.DecreaseStageIndex();
        }
    }

    void ICursorSelectable.OnEnter(MultiplayerCursor cursor)
    {

    }

    void ICursorSelectable.OnExit(MultiplayerCursor cursor)
    {

    }

}
