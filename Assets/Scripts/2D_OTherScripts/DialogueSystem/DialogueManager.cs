using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using FMOD.Studio;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;

    [Space]

    public float dialogueTypingDelay = .3f;
    public DialogueInputManager dialogueInputManager;
    public Animator animator;
    public static bool isInDialogue;

    public bool reloadSceneOnFinalGameDialogue = true;

    Queue<string> sentences;
    Dialogue dialogue;
    AudioManager audioManager;
    FMODEvents fMODEvents;
    
    public EventInstance typingSound;

    public static DialogueManager instance;
    PlayerInteraction_3D playerInteraction;
    Transform cameraTransform;

    PLAYBACK_STATE typingPlaybackState;

    private int index = 0;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        sentences = new Queue<string>();
        playerInteraction = FindObjectOfType<PlayerInteraction_3D>();
        audioManager = AudioManager.instance;
        fMODEvents = FMODEvents.instance;
        cameraTransform = Camera.main.transform;

        typingSound = audioManager.CreateEventInstance(fMODEvents.DIALOGUE_DialogueTyping);
    }

    public void StartDialogue(Dialogue _dialogue)
    {
        index = 0;

        dialogueInputManager.enabled = true;

        if (playerInteraction != null)
            PlayerInteraction_3D.OnInteraction = true;

        isInDialogue = true;
        dialogue = _dialogue;

        audioManager.PlayOneShot(fMODEvents.DIALOGUE_EnterDialogue, cameraTransform.position);
        GameMaster.instance.PauseGame_nst();

        animator.SetBool("IsOpen", true);

        sentences.Clear();   

        foreach (Sentences sentence in _dialogue.sentences)
        {
            sentences.Enqueue(sentence.sentences);
        }

        DisplayNextSentences();
    }

    public void DisplayNextSentences()
    {

        if (!isInDialogue)
        {
            typingSound.getPlaybackState(out typingPlaybackState);
            if (typingPlaybackState.Equals(PLAYBACK_STATE.PLAYING))
                typingSound.stop(STOP_MODE.IMMEDIATE);
            return;           }


        if (sentences.Count == 0)
        {
            typingSound.stop(STOP_MODE.IMMEDIATE);
            EndDialogue();
            return;
        }

        nameText.text = dialogue.sentences[index].speaker;

        if (dialogue.sentences[index].lookTarget != null)
            C_CameraController.instance.LookAtTarget(dialogue.sentences[index].lookTarget);

        index++;

        string sentence = sentences.Dequeue();

        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";    
        typingSound.stop(STOP_MODE.IMMEDIATE);

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            
            typingSound.start();
            
            yield return null;
        }
    }

    public void EndDialogue()
    {
        index = 0;

        StopAllCoroutines();

        typingSound.stop(STOP_MODE.IMMEDIATE);

        animator.SetBool("IsOpen", false);
        isInDialogue = false;

        GameMaster.instance.ResumeGame_nst();

        audioManager.PlayOneShot(fMODEvents.DIALOGUE_CloseDialogue, cameraTransform.position);

        if (dialogue.isEndGameDialogue)
        {
            // trigger apa kek di sini yg berkaitan dng end game
        }

        if (dialogue.canActivateObject)
        {
            foreach (GameObject neuron in dialogue.objectToActivate)
            {
                neuron.SetActive(true);
            }
        }

        if (dialogue.haveNextDialogue)
        {
            StartCoroutine(nameof(TriggerNextDialogue), dialogue);
        }

        if (dialogue.canDeleteObstacle)
        {
            foreach (GameObject yopussi in dialogue.ObstaclesToDelete)
            {
                Destroy(yopussi);
            }
        }
        if (dialogue.isFinalGameDialogue)
        {
            GameMaster.instance.WinGame();
        }

        if (playerInteraction != null)
            PlayerInteraction_3D.OnInteraction = false;

        dialogue = null;
        dialogueInputManager.enabled = false;
    }

    IEnumerator TriggerNextDialogue(Dialogue dialogue)
    {
        Dialogue yapping = dialogue;

        yield return new WaitForSeconds(yapping.nextDialogueCooldown);

        yapping.nextDialogue.TriggerDialogue();
    }
}
