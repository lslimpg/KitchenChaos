using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Commented out to avoid creating a new asset in the project view
// because this is a list of recipes and not a single recipe.
//[CreateAssetMenu()]
public class RecipeListSO : ScriptableObject {
    public List<RecipeSO> recipeSOList;
}