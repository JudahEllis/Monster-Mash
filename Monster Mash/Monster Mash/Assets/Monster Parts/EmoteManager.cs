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
    [HideInInspector] public EmoteManager.EmoteSlot EmoteSlot;
    public EmoteManager.Emote EmoteName;
    public InputActionReference emoteInputAction;
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

    [Tooltip("Only Assign 4 emotes")] public DefaultEmote[] defaultEmotes = new DefaultEmote[4];
    private Dictionary<Emote, Action> emoteNameToActionRef;
    private Action<InputAction.CallbackContext>[] emoteHandlers = new Action<InputAction.CallbackContext>[4];
    private monsterAttackSystem attackSystem;
    private bool isInitilised;

    public void Initilize(monsterAttackSystem attackSystem, playerController player)
    {
        if (isInitilised) { return; }

        this.attackSystem = attackSystem;

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

        for (int i = 0; i < defaultEmotes.Length; i++)
        {
            defaultEmotes[i].EmoteSlot = (EmoteSlot)i;
            SwapEmote(defaultEmotes[i].EmoteSlot, defaultEmotes[i].EmoteName);
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
        int slotIndex = (int)emoteSlot;
        DefaultEmote defaultEmoteForSlot = Array.Find(defaultEmotes, e => e.EmoteSlot == emoteSlot);

        if (defaultEmoteForSlot == null ||
            defaultEmoteForSlot.emoteInputAction == null ||
            defaultEmoteForSlot.emoteInputAction.action == null)
            return;

        var inputAction = attackSystem.myPlayer.playerInput.actions.FindAction(defaultEmoteForSlot.emoteInputAction.name);
        if (!inputAction.enabled)
            inputAction.Enable();

        Action emoteAction = emoteNameToActionRef[emoteName];

        // Unsubscribe previous function if there is one
        if (emoteHandlers[slotIndex] != null)
            inputAction.performed -= emoteHandlers[slotIndex];

        // Store and subscribe the new handler
        emoteHandlers[slotIndex] = ctx => emoteAction();
        inputAction.performed += emoteHandlers[slotIndex];

        // Update the emote name for this slot
        defaultEmoteForSlot.EmoteName = emoteName;
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
}
