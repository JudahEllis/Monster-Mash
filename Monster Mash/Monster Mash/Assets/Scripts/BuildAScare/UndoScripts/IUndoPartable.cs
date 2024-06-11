using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUndoPartable
{
    void UndoAction(UndoData undo);
}
