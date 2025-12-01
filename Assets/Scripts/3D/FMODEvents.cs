using UnityEngine;
using FMODUnity;
using System;

public class FMODEvents : MonoBehaviour
{
    public static FMODEvents instance;

    void Awake()
    {
        instance = this;
    }

    [field : Header("UI Sounds")]
    [field : SerializeField] public EventReference UI_ButtonClick { get; private set; }
    [field : SerializeField] public EventReference UI_ButtonHover { get; private set; }
    [field: SerializeField] public EventReference UI_SliderChanged { get; private set; }
    
    [field: Header("Atmosphere / Que Sounds")]
    [field: SerializeField] public EventReference QUE_GameStartQue { get; private set; }
    [field : SerializeField] public EventReference ATMOSPHERE_1 { get; private set; }
    
    [field : Header("Music Sounds")]
    [field : SerializeField] public EventReference MUSIC_MainMenu { get; private set; }


    [field : Header("Interactable Sound")]
    [field : SerializeField] public EventReference INTERACTABLE_DefaultInteractingSound { get; private set; }
    [field : SerializeField] public EventReference INTERACTABLE_FailedInteractingSound { get; private set; }
    [field : SerializeField] public EventReference INTERACTABLE_MoneyBagPickup { get; private set; }
    
    [field : Header("Dialogue Sounds")]
    [field : SerializeField] public EventReference DIALOGUE_EnterDialogue { get; private set; }
    [field : SerializeField] public EventReference DIALOGUE_CloseDialogue { get; private set; }
    [field : SerializeField] public EventReference DIALOGUE_DialogueTyping { get; private set; }
    [field : SerializeField] public EventReference DIALOGUE_SkipDialogue { get; private set; }

    [field : Header("Player Sounds")]
    [field : SerializeField] public EventReference PLAYER_Footsteps { get; private set; }
    [field : SerializeField] public EventReference PLAYER_Sprintsteps { get; private set; }

    [field : Header("Enemy Sounds")]
    [field : SerializeField] public EventReference ENEMY_Chasing { get; private set; }
    [field : SerializeField] public EventReference ENEMY_Attacking { get; private set; }
}