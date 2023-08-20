using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameManager gameManager = null;
    [SerializeField] UICanvas[] uiState = new UICanvas[(int)GameState.MAX_STATE];

    private void Start()
    {
        if(gameManager == null)
            gameManager = FindObjectOfType<GameManager>();
    }

    public void OnInit()
    {
        return;
    }

    public void SetGameState(GameState gameState)
    {
        // Disable all canvase
        for(int i = 0; i < uiState.Length; i++)
            if(uiState[i] != null)  {
                uiState[i].gameObject.SetActive(false);
            }

        // Excluding this state
        uiState[(int)gameState].gameObject.SetActive(true);
    }

}
