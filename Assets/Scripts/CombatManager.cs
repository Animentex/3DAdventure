using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance;


    public enum CombatState
    {
        None,
        PlayerTurn,
        EnemyTurn,
        Victory,
        Defeat
    }


    public CombatState currentState;


    GameObject currentEnemy;


    void Awake()
    {
        Instance = this;
    }


    public void StartBattle(GameObject enemy)
    {
        currentEnemy = enemy;

        Debug.Log("Battle Started!");

        BeginPlayerTurn();
    }



    public void BeginPlayerTurn()
    {
        currentState = CombatState.PlayerTurn;

        Debug.Log("Player Turn");
    }



    public void BeginEnemyTurn()
    {
        currentState = CombatState.EnemyTurn;

        Debug.Log("Enemy Turn");
    }



    public void ProgressTurn()
    {
        if (currentState == CombatState.PlayerTurn)
        {
            BeginEnemyTurn();
        }
        else if (currentState == CombatState.EnemyTurn)
        {
            BeginPlayerTurn();
        }
    }



    public void Victory()
    {
        currentState = CombatState.Victory;

        Debug.Log("Victory!");
    }



    public void Defeat()
    {
        currentState = CombatState.Defeat;

        Debug.Log("Defeat!");
    }



    public void EndBattle()
    {
        Debug.Log("Combat Ended");

        currentState = CombatState.None;

        currentEnemy = null;

        GameManager.Instance.ExitCombat();
    }
}