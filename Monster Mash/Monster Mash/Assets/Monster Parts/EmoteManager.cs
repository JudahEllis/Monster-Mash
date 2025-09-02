using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

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
        Boo,
        Hula,
        Vomit,
        Sleep,
        Explosive,
        Sneezing,
        Random,
    }

    public DefaultEmote[] defaultEmotes = new DefaultEmote[maxEmotes];
    private Dictionary<Emote, Action> emoteNameToActionRef;
    private static readonly int maxEmotes = 4;
    private Action<InputAction.CallbackContext>[] emoteHandlers = new Action<InputAction.CallbackContext>[maxEmotes];
    private playerController player;
    private monsterAttackSystem attackSystem;
    private bool isInitilised;

    public void Initilize(monsterAttackSystem attackSystem, playerController player)
    {
        if (isInitilised) { return; }

        this.player = player;
        this.attackSystem = attackSystem;
        LoadEmoteRebinds();
        InputRemapper.onRebind.AddListener(LoadEmoteRebinds);

        emoteNameToActionRef = new Dictionary<Emote, Action>()
        {
            { Emote.Fierce, attackSystem.fierceEmote },
            { Emote.Gas, attackSystem.gasEmote },
            { Emote.Mocking, attackSystem.mockingEmote },
            { Emote.Dance, attackSystem.danceEmote },
            { Emote.Jack, attackSystem.jackEmote },
            { Emote.Boo, attackSystem.booEmote },
            { Emote.Hula, attackSystem.hulaEmote },
            { Emote.Vomit, attackSystem.vomitEmote },
            { Emote.Sleep, attackSystem.sleepEmote },
            { Emote.Explosive, attackSystem.explosiveEmote },
            { Emote.Sneezing, attackSystem.sneezingEmote },
            { Emote.Random, PlayRandomEmote },
        };

        if (defaultEmotes == null || defaultEmotes.Length == 0) { return; }

        foreach (var emote in defaultEmotes)
        {
            SwapEmote(emote.EmoteSlot, emote.EmoteName);
        }

        isInitilised = true;
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

    /// <summary>
    /// Plays a randomly selected emote
    /// </summary>
    public void PlayRandomEmote()
    {
        if (attackSystem.emoteActive) { return; }

        var emoteFunctions = emoteNameToActionRef
            .Where(entry => entry.Key != Emote.Random)
            .Select(entry => entry.Value)
            .ToArray();

        int randIndex = Random.Range(0, emoteFunctions.Length);
        emoteFunctions[randIndex].Invoke();
    }

    // Converts the enum name to the InputAction so that it does not have to be passed in to SetEmote()
    private InputAction GetEmoteInputAction(EmoteSlot emoteIndex)
    {
        string actionName = emoteIndex.ToString();
        var emotesType = player.playerControlsMap.Emotes.GetType();
        var prop = emotesType.GetProperty(actionName);

        return prop == null ? null : (InputAction)prop.GetValue(player.playerControlsMap.Emotes);
    }

    private void LoadEmoteRebinds()
    {
        // Getting a ref to the action map through the first emote slot. This works with any slot
        InputActionMap emoteActionMap = GetEmoteInputAction(EmoteSlot.Emote1).actionMap;

        if (PlayerPrefs.HasKey(emoteActionMap.name))
        {
            string rebinds = PlayerPrefs.GetString(emoteActionMap.name);
            emoteActionMap.LoadBindingOverridesFromJson(rebinds);
        }
    }
}
