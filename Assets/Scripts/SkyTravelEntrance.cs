using UnityEngine;
using UnityEngine.SceneManagement;

public class SkyDestination : MonoBehaviour
{
    public string nextIsland;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("TRIGGER HIT BY: " + other.name + " TAG: " + other.tag);

        if (other.CompareTag("Player"))
        {
            Debug.Log("LOADING NEXT ISLAND");
            SceneManager.LoadScene(nextIsland);
        }
    }
}