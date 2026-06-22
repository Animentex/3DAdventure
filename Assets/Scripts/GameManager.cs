using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool inCombat;

    [Header("References")]
    public GameObject player;

    PlayerInputActions input;

    void Awake()
    {
        Instance = this;

        input = new PlayerInputActions();
    }

    void OnEnable()
    {
        input.Enable();

        input.Player.CombatTest.performed += _ =>
        {
            if (!inCombat)
                StartCombat();                
            else
                ExitCombat();
        };
    }

    void OnDisable()
    {
        input.Disable();
    }


    public GameObject exploration;
    public GameObject combatArena;


    public void StartCombat(GameObject enemy = null)
    {
        inCombat = true;


        exploration.SetActive(false);
        combatArena.SetActive(true);


        player.GetComponent<PlayerController>().enabled = false;


        CombatManager.Instance.StartBattle(enemy);

       //player.transform.position =
       //playerSpawn.position;

       //enemy.transform.position =
       //enemySpawn.position;
}


    public void ExitCombat()
   {
       inCombat = false;

       combatArena.SetActive(false);
       exploration.SetActive(true);

       player.GetComponent<PlayerController>().enabled = true;
}
}