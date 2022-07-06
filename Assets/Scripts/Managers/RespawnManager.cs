using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : Singleton<RespawnManager>
{
    [Header("Manages tank respawning. Children mark the spots.")]
    [Tooltip("Time it takes for a tank to respawn")]
    public float respawnTime = 1.0f;
    [Tooltip("Objects in this area around the respawn point will disallow spawning in it")]
    public Vector3 respawnBlockHalfExtents;
    [Tooltip("Kinds of objects that may block respawn points")]
    public LayerMask blockingMask;

    public void RequestRespawn(TankController tank)
    {
        StartCoroutine(OnRespawn(tank));
    }

    private IEnumerator OnRespawn(TankController tank)
    {
        yield return new WaitForSeconds(respawnTime);

        int startIndex = Random.Range(0, transform.childCount);
        int index = 0;
        for (; index < transform.childCount; index++)
        {
            int actualIndex = (startIndex + index) % transform.childCount;
            if (IsRespawnPointFree(actualIndex))
            {
                tank.Respawn(transform.GetChild(actualIndex).position);
                yield break;
            }
        }
        RequestRespawn(tank);
    }

    private bool IsRespawnPointFree(int index)
    {
        return !Physics.CheckBox
        (
            transform.GetChild(index).position + new Vector3(0, respawnBlockHalfExtents.y, 0),
            respawnBlockHalfExtents,
            Quaternion.identity,
            blockingMask
        );
    }
}
