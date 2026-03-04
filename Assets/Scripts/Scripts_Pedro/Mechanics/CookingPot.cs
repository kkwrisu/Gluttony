using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CookingPot : MonoBehaviour
{
    [Header("Configurações")]
    public float cookingTime = 15f;

    [Header("Relógio Visual")]
    [SerializeField] private SpriteRenderer clockRenderer;
    [SerializeField] private Sprite[] clockSprites;

    [Header("Sopa Pronta")]
    [SerializeField] private Sprite[] steamFrames;
    [SerializeField] private float steamFrameRate = 0.3f;

    [Header("Áudio")]
    public AudioClip cookingLoopSound;
    public AudioClip soupReadySound;
    private AudioSource audioSource;

    private static bool isAnyPotPlayingLoop = false;

    [Header("Prefabs das Sopas")]
    public CollectableItem sopaBatataCarnePrefab;
    public CollectableItem sopaCenouraCarnePrefab;
    public CollectableItem sopaTomateCarnePrefab;

    [Header("Diálogo")]
    [SerializeField] private DialogueManager dialogueManager;

    private bool isCooking = false;
    private bool isReady = false;
    private bool isThisPotPlayingLoop = false;

    private List<IngredientType> currentIngredients = new List<IngredientType>();
    private CollectableItem resultSoupPrefab;

    private Coroutine steamCoroutine;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (clockRenderer != null)
            clockRenderer.gameObject.SetActive(false);
    }

    public void Interact(PlayerCarrying playerCarry)
    {
        if (playerCarry == null)
            return;

        if (dialogueManager != null && dialogueManager.IsActive)
        {
            dialogueManager.DisplayNextSentence();
            return;
        }

        if (isReady)
        {
            SpawnSoup(playerCarry);
            return;
        }

        if (isCooking)
            return;

        if (!playerCarry.IsCarryingItem())
        {
            ShowCurrentIngredients();
            return;
        }

        CollectableItem heldItem = playerCarry.GetCarriedItem();
        if (heldItem != null)
            TryAddIngredient(heldItem, playerCarry);
    }

    private void ShowCurrentIngredients()
    {
        if (dialogueManager == null)
            return;

        string message;

        if (currentIngredients.Count == 0)
        {
            message = "A panela está vazia.";
        }
        else
        {
            message = "Ingredientes colocados na panela:\n";

            foreach (var ingredient in currentIngredients)
            {
                message += "- " + ingredient.ToString() + "\n";
            }
        }

        dialogueManager.StartDialogue(new string[] { message });
    }

    private void TryAddIngredient(CollectableItem item, PlayerCarrying playerCarry)
    {
        IngredientType ingredient = item.itemType;

        if (!IsValidIngredient(ingredient))
            return;

        if (currentIngredients.Contains(ingredient))
            return;

        if (IsVegetable(ingredient))
        {
            foreach (var ing in currentIngredients)
            {
                if (IsVegetable(ing))
                    return;
            }
        }

        currentIngredients.Add(ingredient);

        playerCarry.DropItem();
        Destroy(item.gameObject);

        if (currentIngredients.Count == 3)
            CheckRecipe();
    }

    private void CheckRecipe()
    {
        bool hasCarne = currentIngredients.Contains(IngredientType.Carne);
        bool hasSal = currentIngredients.Contains(IngredientType.Sal);

        if (!hasCarne || !hasSal)
            return;

        if (currentIngredients.Contains(IngredientType.Batata))
            resultSoupPrefab = sopaBatataCarnePrefab;
        else if (currentIngredients.Contains(IngredientType.Cenoura))
            resultSoupPrefab = sopaCenouraCarnePrefab;
        else if (currentIngredients.Contains(IngredientType.Tomate))
            resultSoupPrefab = sopaTomateCarnePrefab;
        else
            return;

        StartCoroutine(CookSoup());
    }

    private IEnumerator CookSoup()
    {
        isCooking = true;
        isReady = false;

        if (!isAnyPotPlayingLoop && audioSource != null && cookingLoopSound != null)
        {
            audioSource.clip = cookingLoopSound;
            audioSource.loop = true;
            audioSource.Play();

            isAnyPotPlayingLoop = true;
            isThisPotPlayingLoop = true;
        }

        if (clockRenderer == null || clockSprites.Length == 0)
            yield break;

        clockRenderer.gameObject.SetActive(true);

        float timePerSprite = cookingTime / clockSprites.Length;

        for (int i = 0; i < clockSprites.Length; i++)
        {
            clockRenderer.sprite = clockSprites[i];
            yield return new WaitForSeconds(timePerSprite);
        }

        isCooking = false;
        isReady = true;

        if (isThisPotPlayingLoop && audioSource != null)
        {
            audioSource.Stop();
            audioSource.loop = false;

            isAnyPotPlayingLoop = false;
            isThisPotPlayingLoop = false;
        }

        PlayReadySound();

        if (steamFrames.Length > 0)
            steamCoroutine = StartCoroutine(AnimateSteam());
    }

    private IEnumerator AnimateSteam()
    {
        int index = 0;

        while (isReady)
        {
            clockRenderer.sprite = steamFrames[index];
            index = (index + 1) % steamFrames.Length;
            yield return new WaitForSeconds(steamFrameRate);
        }
    }

    private void SpawnSoup(PlayerCarrying playerCarry)
    {
        if (resultSoupPrefab == null)
            return;

        CollectableItem soup = Instantiate(
            resultSoupPrefab,
            transform.position + Vector3.up,
            Quaternion.identity
        );

        playerCarry.TryCollectSpecific(soup);

        isReady = false;
        resultSoupPrefab = null;
        currentIngredients.Clear();

        if (steamCoroutine != null)
            StopCoroutine(steamCoroutine);

        if (clockRenderer != null)
            clockRenderer.gameObject.SetActive(false);
    }

    private void PlayReadySound()
    {
        if (audioSource != null && soupReadySound != null)
            AudioManager.Instance.PlaySFX(soupReadySound); ;
    }

    private bool IsValidIngredient(IngredientType type)
    {
        return type == IngredientType.Batata ||
               type == IngredientType.Cenoura ||
               type == IngredientType.Tomate ||
               type == IngredientType.Carne ||
               type == IngredientType.Sal;
    }

    private bool IsVegetable(IngredientType type)
    {
        return type == IngredientType.Batata ||
               type == IngredientType.Cenoura ||
               type == IngredientType.Tomate;
    }
}