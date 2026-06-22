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


    [Header("Combat References")]
    public Transform playerBattlePosition;
    public Transform enemyBattlePosition;


    GameObject currentEnemy;



    void Awake()
    {
        Instance = this;
    }



    public void StartBattle(GameObject enemy)
    {
        currentEnemy = enemy;

        Debug.Log("Battle Started!");

        currentState = CombatState.PlayerTurn;


        MoveCombatants();


        BeginPlayerTurn();
    }



    void MoveCombatants()
    {
        // Temporary movement
        // Later we replace this with animations

        Debug.Log("Moving fighters into arena");
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



    public void EndBattle()
    {
        currentState = CombatState.None;

        currentEnemy = null;


        Debug.Log("Battle Ended");
    }
}