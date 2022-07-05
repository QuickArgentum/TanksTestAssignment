using UnityEngine;

[ExecuteInEditMode]
public class AnimateTransparency : MonoBehaviour
{
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
