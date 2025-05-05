using UnityEngine;

[RequireComponent(typeof(Camera))]
public class FogHandler : MonoBehaviour
{
    [SerializeField] bool enableFog = true;

    bool previousFogState;

    void OnPreRender()
    {
        previousFogState = RenderSettings.fog;
        RenderSettings.fog = enableFog;
    }

    void OnPostRender()
    {
        RenderSettings.fog = previousFogState;
    }
}
