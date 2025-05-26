using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICursorSelectable
{
    void OnSelect(MultiplayerCursor cursor);

    void OnEnter(MultiplayerCursor cursor);

    void OnExit(MultiplayerCursor cursor);
}
