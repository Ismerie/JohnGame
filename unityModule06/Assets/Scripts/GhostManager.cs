using System.Collections.Generic;
using UnityEngine;

public class GhostManager : MonoBehaviour
{
    public static GhostManager Instance { get; private set; }
    private readonly List<GhostAI> ghosts = new();

    void Awake()
    {
        if (Instance != null && Instance != this) { 
            Destroy(gameObject); 
            return; 
        }
        Instance = this;
    }

    public void Register(GhostAI g)
    {
        if (!ghosts.Contains(g)) 
            ghosts.Add(g);
    }

    public void Unregister(GhostAI g)
    {
        ghosts.Remove(g);
    }

    public void AlertAll(Transform player)
    {
        Debug.Log("Alert all");
        foreach (var g in ghosts)
            g.Alert(player);
    }
}
