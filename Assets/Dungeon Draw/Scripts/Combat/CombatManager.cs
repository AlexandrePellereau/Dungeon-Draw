using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public List<Entity> enemies = new List<Entity>();
    
    /*
    Use StartFight() to start the fight
    It waits for the player to play first if PlayerPlayFirst is true
    Use the end turn button to end the player turn (EndTurn())
    Play the enemy turn with the coroutine EnemyTurn() (TODO: Replace with enemy turn logic)
    And wait for the player to play again
     */

    public bool PlayerPlayFirst { get; set; } = true;

    [HideInInspector]
    public static CombatManager Instance { get; private set; }
    
    private HandController _handController;
    private Deck _deck;

    private bool _isPlayerTurn;
    public bool IsPlayerTurn {
        get => _isPlayerTurn;
        private set { 
            _isPlayerTurn = value; 
            Debug.Log(_isPlayerTurn ? "Player turn" : "Enemy turn"); 
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start() //TODO: Remove (for testing purposes only)
    {
        _handController = GetComponent<HandController>();
        _deck = GetComponent<Deck>();
        StartFight();
    }


    public void StartFight()
    {
        Debug.Log("Fight started");
        IsPlayerTurn = PlayerPlayFirst;
        
        //TODO: Remove (for testing purposes only)
        // CardStats[] cards =
        // {
        //     new CardStats("Attack", CardType.Attack, CardRarity.Common, 1, new List<Effect> {new DealDamage(1, 5)}),
        //     
        // };
        GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag("enemy");
        foreach (GameObject gameObject in enemyObjects) 
        {
            enemies.Add(gameObject.GetComponent<Entity>());
        }
        
        PlayerTurn();
    }
    
    private void PlayerTurn()
    {
        // Debug.Log("Hand draw");
        StartCoroutine(_handController.DrawHand());
        // Debug.Log("Hand done drawing");
        // StopCoroutine(_handController.DrawHand());
        // Card targeting and functionality takes over
    }

    public void EndTurn()
    {
        foreach (Entity enemy in enemies)
        {
            Debug.Log("Enemy [" + enemy + "] health: " + enemy.currentHP);
        }
        IsPlayerTurn = !IsPlayerTurn;
        if (!IsPlayerTurn)
        {
            StartCoroutine(EnemyTurn());
        }
        else
        {
            PlayerTurn();
        }
    }
    

    private IEnumerator EnemyTurn()
    {
        yield return new WaitForSeconds(1f); //TODO: Replace with enemy turn logic
        EndTurn();
    }

}
