using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectIcon : MonoBehaviour, MultiplayerJoinManager.IQuickplayButtonable
{
    //This can be changed to a string if needed
    public int stageIndex;
    public void ButtonSelected(MultiplayerCursor cursor)
    {
        cursor.SelectStage(stageIndex);
    }
}
