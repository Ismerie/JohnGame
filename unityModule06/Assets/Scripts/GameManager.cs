using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int keys = 0;

    void Awake()
    {
        if (Instance == null) 
            Instance = this;
        else Destroy(gameObject);
    }

    public void AddKey()
    {
        keys++;
        Debug.Log("Keys: " + keys);
    }
}
