using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectIcon : MonoBehaviour, MultiplayerJoinManager.IQuickplayButtonable
{
    //This can be changed to a string if needed
    public int stageIndex;
    public void ButtonSelected(MultiplayerCursor cursor)
    {
       
        if (CharacterSelectManager.Instance.storedPlayerInformation.Count > 0)
        {
            CharacterSelectManager.Instance.storedPlayerInformation.Clear();
        }

        foreach (MultiplayerJoinManager.PlayerInformation info in cursor.joinManager.playerInfo)
        {

            CharacterSelectManager.Instance.storedPlayerInformation.Add(info);
        }

        cursor.SelectStage(stageIndex);
    }
}
