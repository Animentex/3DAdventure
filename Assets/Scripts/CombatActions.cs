using UnityEngine;

public class CombatActions : MonoBehaviour
{
    public static CombatActions Instance;


    void Awake()
    {
        Instance = this;
    }


    public void BasicAttack()
    {
        Debug.Log("Player used Basic Attack!");

        CombatManager.Instance.ResolvePlayerAction();
    }
}