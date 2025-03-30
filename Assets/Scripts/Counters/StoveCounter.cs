using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounter : BaseCounter {
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public class OnStateChangedEventArgs : EventArgs {
        public State state;
    }
    public enum State {
        Idle,
        Frying,
        Fried,
        Burnt,
    }

    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] private BurningRecipeSO[] burningRecipeSOArray;
    private float fryingTimer;
    private float burningTimer;
    private FryingRecipeSO fryingRecipeSO;
    private BurningRecipeSO burningRecipeSO;
    private State state;

    private void Start() {
        state = State.Idle;
    }

    private void Update() {
        if (HasKitchenObject()) {
            switch (state) {
                case State.Idle:
                    break;
                case State.Frying:
                    fryingTimer += Time.deltaTime;
                    if (fryingTimer >= fryingRecipeSO.fryingTimerMax) {
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);

                        burningTimer = 0f;
                        burningRecipeSO = GetBurningRecipeSOForInput(fryingRecipeSO.output);
                        state = State.Fried;

                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });
                    }
                    break;
                case State.Fried:
                    burningTimer += Time.deltaTime;
                    if (burningTimer >= burningRecipeSO.burningTimerMax) {
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(burningRecipeSO.output, this);

                        state = State.Burnt;

                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });
                    }
                    break;
                case State.Burnt:
                    break;
            }
        }
    }

    public override void Interact(Player player) {
        if (!HasKitchenObject()) {
            if (player.HasKitchenObject() && HasOutputRecipe(player.GetKitchenObject().GetKitchenObjectSO())) {
                player.GetKitchenObject().SetKitchenObjectParent(this);
                fryingRecipeSO = GetFryingRecipeSOForInput(GetKitchenObject().GetKitchenObjectSO());
                state = State.Frying;
                fryingTimer = 0f;

                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });
            }
        } else {
            if (!player.HasKitchenObject()) {
                // Give the object to the player
                GetKitchenObject().SetKitchenObjectParent(player);
                state = State.Idle;

                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });
            }
        }
    }

    private bool HasOutputRecipe(KitchenObjectSO input) {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOForInput(input);
        return fryingRecipeSO != null;
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO input) {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOForInput(input);
        if (fryingRecipeSO != null) {
            return fryingRecipeSO.output;
        }
        return null;
    }

    private FryingRecipeSO GetFryingRecipeSOForInput(KitchenObjectSO input) {
        foreach (FryingRecipeSO fryingRecipeSO in fryingRecipeSOArray) {
            if (fryingRecipeSO.input == input) {
                return fryingRecipeSO;
            }
        }
        return null;
    }

    private BurningRecipeSO GetBurningRecipeSOForInput(KitchenObjectSO input) {
        foreach (BurningRecipeSO burningRecipeSO in burningRecipeSOArray) {
            if (burningRecipeSO.input == input) {
                return burningRecipeSO;
            }
        }
        return null;
    }
}