using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CookingPot : MonoBehaviour
{
    [Header("Configurações")]
    public float cookingTime = 30f;
    public SpriteRenderer visualIndicator;

    [Header("Prefabs das Sopas")]
    public CollectableItem sopaBatataCarnePrefab;
    public CollectableItem sopaCenouraCarnePrefab;
    public CollectableItem sopaTomateCarnePrefab;

    private bool isCooking = false;
    private bool isReady = false;

    private List<IngredientType> currentIngredients = new List<IngredientType>();
    private CollectableItem resultSoupPrefab;

    private void Start()
    {
        if (visualIndicator != null)
            visualIndicator.enabled = false;
    }

    public void Interact(PlayerCarrying playerCarry)
    {
        Debug.Log("Interagindo com a panela");

        if (playerCarry == null)
            return;

        if (isReady)
        {
            SpawnSoup(playerCarry);
            return;
        }

        if (isCooking)
        {
            Debug.Log("Já está cozinhando...");
            return;
        }

        if (playerCarry.IsCarryingItem())
        {
            CollectableItem heldItem = playerCarry.GetCarriedItem();
            if (heldItem != null)
                TryAddIngredient(heldItem, playerCarry);
        }
        else
        {
            Debug.Log("Jogador não está carregando nada.");
        }
    }

    private void TryAddIngredient(CollectableItem item, PlayerCarrying playerCarry)
    {
        IngredientType ingredient = item.itemType;

        Debug.Log("Tentando adicionar: " + ingredient);

        if (!IsValidIngredient(ingredient))
        {
            Debug.Log("Ingrediente inválido.");
            return;
        }

        if (currentIngredients.Contains(ingredient))
        {
            Debug.Log("Ingrediente repetido.");
            return;
        }

        if (IsVegetable(ingredient))
        {
            foreach (var ing in currentIngredients)
            {
                if (IsVegetable(ing))
                {
                    Debug.Log("Já existe um vegetal na panela.");
                    return;
                }
            }
        }

        currentIngredients.Add(ingredient);

        Debug.Log("Ingrediente adicionado com sucesso.");

        playerCarry.DropItem();
        Destroy(item.gameObject);

        if (currentIngredients.Count == 3)
            CheckRecipe();
    }

    private void CheckRecipe()
    {
        Debug.Log("=== Verificando Receita ===");

        foreach (var ing in currentIngredients)
            Debug.Log("Ingrediente: " + ing);

        bool hasCarne = currentIngredients.Contains(IngredientType.Carne);
        bool hasSal = currentIngredients.Contains(IngredientType.Sal);

        Debug.Log("Tem Carne? " + hasCarne);
        Debug.Log("Tem Sal? " + hasSal);

        if (!hasCarne || !hasSal)
        {
            Debug.Log("Receita incompleta. Precisa de Carne e Sal.");
            return;
        }

        if (currentIngredients.Contains(IngredientType.Batata))
        {
            resultSoupPrefab = sopaBatataCarnePrefab;
            Debug.Log("Receita válida: Sopa de Batata com Carne");
        }
        else if (currentIngredients.Contains(IngredientType.Cenoura))
        {
            resultSoupPrefab = sopaCenouraCarnePrefab;
            Debug.Log("Receita válida: Sopa de Cenoura com Carne");
        }
        else if (currentIngredients.Contains(IngredientType.Tomate))
        {
            resultSoupPrefab = sopaTomateCarnePrefab;
            Debug.Log("Receita válida: Sopa de Tomate com Carne");
        }
        else
        {
            Debug.Log("Nenhum vegetal válido encontrado.");
            return;
        }

        StartCoroutine(CookSoup());
    }

    private IEnumerator CookSoup()
    {
        isCooking = true;

        if (visualIndicator != null)
        {
            visualIndicator.enabled = true;
            visualIndicator.color = Color.yellow;
        }

        Debug.Log("Cozinhando...");

        yield return new WaitForSeconds(cookingTime);

        isCooking = false;
        isReady = true;

        if (visualIndicator != null)
            visualIndicator.color = Color.green;

        Debug.Log("Sopa pronta!");
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

        Debug.Log("Sopa entregue ao jogador.");

        isReady = false;
        resultSoupPrefab = null;
        currentIngredients.Clear();

        if (visualIndicator != null)
            visualIndicator.enabled = false;
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