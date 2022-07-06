using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecalManager : Singleton<DecalManager>
{
    [Header("Manages tank explosion decals on the floor")]
    public GameObject prefab;
    public int maximumDecals;

    public void CreateDecal(Vector3 position)
    {
        GameObject decal = Instantiate(prefab);
        decal.transform.parent = transform;
        decal.transform.position = position;
        decal.transform.rotation = Quaternion.Euler(0, Random.Range(-180, 180), 0);

        if (transform.childCount > maximumDecals)
        {
            Destroy(transform.GetChild(0).gameObject);
        }
    }
}
