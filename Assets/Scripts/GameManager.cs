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


    public void StartCombat()
    {
        inCombat = true;

        Debug.Log("Entering Combat");

        player.GetComponent<PlayerController>().enabled = false;
    }


    public void ExitCombat()
    {
        inCombat = false;

        Debug.Log("Leaving Combat");

        player.GetComponent<PlayerController>().enabled = true;
    }
}