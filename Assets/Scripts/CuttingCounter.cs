using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter
{
    public event EventHandler OnCut;
    public event EventHandler<OnCuttingProgressChangedEventArgs> OnCuttingProgressChanged;
    public class OnCuttingProgressChangedEventArgs : EventArgs {
        public float cuttingProgressNormalized;
    }
    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;

    private int cuttingProgress;
    public override void Interact(Player player) {
        if (!HasKitchenObject()) {
            if (player.HasKitchenObject() && HasOutputRecipe(player.GetKitchenObject().GetKitchenObjectSO())) {
                player.GetKitchenObject().SetKitchenObjectParent(this);
                cuttingProgress = 0;
                CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOForInput(GetKitchenObject().GetKitchenObjectSO());

                OnCuttingProgressChanged?.Invoke(this, new OnCuttingProgressChangedEventArgs {
                    cuttingProgressNormalized = ((float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax),
                });
            }
        } else {
            if (!player.HasKitchenObject()) {
                // Give the object to the player
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }

    public override void InteractAlternate(Player player) {
        if (HasKitchenObject() && HasOutputRecipe(GetKitchenObject().GetKitchenObjectSO())) {
            KitchenObject kitchenObject = GetKitchenObject();
            CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOForInput(kitchenObject.GetKitchenObjectSO());

            if (++cuttingProgress >= cuttingRecipeSO.cuttingProgressMax) {
                kitchenObject.DestroySelf();
                KitchenObject.SpawnKitchenObject(GetOutputForInput(kitchenObject.GetKitchenObjectSO()), this);
            }

            OnCut?.Invoke(this, EventArgs.Empty);

            OnCuttingProgressChanged?.Invoke(this, new OnCuttingProgressChangedEventArgs {
                cuttingProgressNormalized = ((float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax),
            });
        }
    }

    private bool HasOutputRecipe(KitchenObjectSO input) {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOForInput(input);
        return cuttingRecipeSO != null;
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO input) {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOForInput(input);
        if (cuttingRecipeSO != null) {
            return cuttingRecipeSO.output;
        }
        return null;
    }

    private CuttingRecipeSO GetCuttingRecipeSOForInput(KitchenObjectSO input) {
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray) {
            if (cuttingRecipeSO.input == input) {
                return cuttingRecipeSO;
            }
        }
        return null;
    }
}
