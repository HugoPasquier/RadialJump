using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public Vector3 CurrentCheckpoint;
    
    private void Awake()
    { 
        var objs = FindObjectsOfType<CheckpointManager>();

        if (objs.Length > 1)
        {
            foreach (var current in objs)
            {
                if (current != this)
                {
                    var player = FindObjectOfType<PlayerMovement>();
                    player.transform.position = current.CurrentCheckpoint;
                }
            }
            
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);   
    }
}
