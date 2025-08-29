using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.InputSystem;

[Serializable]
public class DefaultEmote
{
    public EmoteManager.EmoteSlot EmoteSlot;
    public EmoteManager.Emote EmoteName;
}

[Serializable]
public class EmoteManager
{
    public enum EmoteSlot
    {
        Emote1 = 0,
        Emote2 = 1,
        Emote3 = 2,
        Emote4 = 3,
    }

    public enum Emote
    {
        Fierce,
        Gas,
        Mocking,
        Dance,
        Jack,
        Thinking,
        Boo,
        Excercise,
        Hula,
        Vomit,
        Brian,
        Sleep,
        Explosive,
        Laughing,
        Sneezing
    }

    public DefaultEmote[] defaultEmotes = new DefaultEmote[maxEmotes];
    private Dictionary<Emote, Action> emoteNameToActionRef;
    private static readonly int maxEmotes = 4;
    private Action<InputAction.CallbackContext>[] emoteHandlers = new Action<InputAction.CallbackContext>[maxEmotes];
    private playerController player;

    public void Initilize(monsterAttackSystem attackSystem, playerController player)
    {
        this.player = player;

        emoteNameToActionRef = new Dictionary<Emote, Action>()
        {
            { Emote.Fierce, attackSystem.fierceEmote },
            { Emote.Gas, attackSystem.gasEmote },
            { Emote.Mocking, attackSystem.mockingEmote },
            { Emote.Dance, attackSystem.danceEmote },
            { Emote.Jack, attackSystem.jackEmote },
            { Emote.Thinking, attackSystem.thinkingEmote },
            { Emote.Boo, attackSystem.booEmote },
            { Emote.Excercise, attackSystem.excerciseEmote },
            { Emote.Hula, attackSystem.hulaEmote },
            { Emote.Vomit, attackSystem.vomitEmote },
            { Emote.Brian, attackSystem.brianEmote },
            { Emote.Sleep, attackSystem.sleepEmote },
            { Emote.Explosive, attackSystem.explosiveEmote },
            { Emote.Laughing, attackSystem.laughingEmote },
            { Emote.Sneezing, attackSystem.sneezingEmote },
        };

        if (defaultEmotes == null || defaultEmotes.Length == 0) { return; }

        foreach (var emote in defaultEmotes)
        {
            SwapEmote(emote.EmoteSlot, emote.EmoteName);
        }
    }
    

    /// <summary>
    /// Swaps the passed in emote slot's current emote 
    /// </summary>
    /// <param name="emoteSlot">The slot you want to swap</param>
    /// <param name="emoteName">The Enum name of the emote you want to set the slot to</param>
    public void SwapEmote(EmoteSlot emoteSlot, Emote emoteName)
    {
        InputAction emoteInputAction = GetEmoteInputAction(emoteSlot);
        Action emoteAction = emoteNameToActionRef[emoteName];

        // Unsubscribe previous function if there is one
        if (emoteHandlers[(int)emoteSlot] != null)
        {
            emoteInputAction.performed -= emoteHandlers[(int)emoteSlot];
        }

        // store the function that we are subscribing to so that we can unsubscribe it later
        emoteHandlers[(int)emoteSlot] = ctx => emoteAction();
        // subscribe the input action to the passed in function
        emoteInputAction.performed += emoteHandlers[(int)emoteSlot];
    }

    // Converts the enum name to the InputAction so that it does not have to be passed in to SetEmote()
    private InputAction GetEmoteInputAction(EmoteSlot emoteIndex)
    {
        string actionName = emoteIndex.ToString();
        var emotesType = player.playerControlsMap.Emotes.GetType();
        var prop = emotesType.GetProperty(actionName);

        return prop == null ? null : (InputAction)prop.GetValue(player.playerControlsMap.Emotes);
    }
}
