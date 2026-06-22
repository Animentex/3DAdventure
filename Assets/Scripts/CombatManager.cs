using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance;


   public enum CombatState
   {
    None,

    PlayerTurn,
    PlayerAction,

    EnemyTurn,
    EnemyAction,

    ReactionWindow,

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

    public void ResolvePlayerAction()
    {
        currentState = CombatState.PlayerAction;

        Debug.Log("Resolving Player Action");


        // Damage later
        Debug.Log("Enemy takes damage");


        BeginEnemyTurn();
    }



    public void BeginEnemyTurn()
    {
        currentState = CombatState.EnemyTurn;

        Debug.Log("Enemy Turn");

        StartEnemyAttack();
    }

    public void StartEnemyAttack()
    {
        currentState = CombatState.EnemyAction;

        Debug.Log("Enemy prepares attack");


        // Later:
        // Play enemy animation
        // Start timing

        OpenReactionWindow();
    }

    public void OpenReactionWindow()
    {
        currentState = CombatState.ReactionWindow;

        Debug.Log("React now!");
    }

    public void EndEnemyAttack()
    {
        Debug.Log("Enemy attack resolved");

        BeginPlayerTurn();
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

    public void Dodge()
    {
        Debug.Log("Dodged!");

        EndEnemyAttack();
    }


    public void Counter()
    {
        Debug.Log("Counter attack!");

        EndEnemyAttack();
    }


    public void Parry()
    {
        Debug.Log("Perfect Parry!");

        EndEnemyAttack();
    }
}