using UnityEngine;

[ExecuteInEditMode]
public class AnimateTransparency : MonoBehaviour
{
    [Header("Allows to change object's material opacity from the inspector")]
    [Range(0, 1)]
    public float transparency;

    private Material material;

    private void Awake()
    {
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        material = renderer.sharedMaterial;
    }

    void Update()
    {
        Color color = material.color;
        color.a = transparency;
        material.color = color;
    }
}
