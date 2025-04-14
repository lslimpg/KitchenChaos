using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour {

    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeCompleted;

    public static DeliveryManager Instance { get; private set; }
    [SerializeField] private RecipeListSO recipeListSO;
    private List<RecipeSO> waitingRecipeSOList;
    private float spawnRecipeTimer;
    private float spawnRecipeTimerMax = 4f;
    private int waitingRecipeMax = 4;

    private void Awake() {
        Instance = this;
        waitingRecipeSOList = new List<RecipeSO>();
    }
    private void Update() {
        spawnRecipeTimer -= Time.deltaTime;
        if (spawnRecipeTimer <= 0f) {
            spawnRecipeTimer = spawnRecipeTimerMax;

            if (waitingRecipeSOList.Count <= waitingRecipeMax) { 
                RecipeSO waitingRecipeSO = recipeListSO.recipeSOList[UnityEngine.Random.Range(0, recipeListSO.recipeSOList.Count)];
                // Debug.Log("Waiting Recipe: " + waitingRecipeSO.recipeName);
                OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
                waitingRecipeSOList.Add(waitingRecipeSO);
            }
        }
    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject) {
        List<KitchenObjectSO> plateKitchenObjectSOList = plateKitchenObject.GetKitchenObjectSOList();

        foreach (RecipeSO waitingRecipeSO in waitingRecipeSOList) {
            if (waitingRecipeSO.kitchenObjectSOList.Count == plateKitchenObjectSOList.Count) {
                bool plateContentsMatchesRecipe = true;
                foreach (KitchenObjectSO recipeKitchenObjectSO in waitingRecipeSO.kitchenObjectSOList) {
                    if (!plateKitchenObjectSOList.Contains(recipeKitchenObjectSO)) {
                        plateContentsMatchesRecipe = false;
                        break;
                    }
                }
                if (plateContentsMatchesRecipe) {
                    Debug.Log("Delivered: " + waitingRecipeSO.recipeName);
                    waitingRecipeSOList.Remove(waitingRecipeSO);
                    OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
                    return;
                }
            }
        }
    }

    public List<RecipeSO> GetWaitingRecipeSOList() {
        return waitingRecipeSOList;
    }
}
