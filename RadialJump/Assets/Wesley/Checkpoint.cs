using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        FindObjectOfType<CheckpointManager>().CurrentCheckpoint = transform.position;
        Destroy(this);
    }
}
