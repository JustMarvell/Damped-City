using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class DynamicPPController : MonoBehaviour
{
    [Header("Effect Settings")]
    public float minDistortionIntensity = 0f;
    public float maxDistortionIntensity = 50f;
    public float noiseSpeed = 5f;

    [Space]

    public float minVignetteIntensity = 0f;
    public float maxVignetteIntensity = .5f;

    // reffs

    private PostProcessVolume volume;
    private LensDistortion lensDistortion;
    private Vignette vignette;

    private Transform playerTransform;
    private bool isInside = false;
    private Collider col;

    void Start()
    {
        volume = GetComponent<PostProcessVolume>();

        volume.profile.TryGetSettings<LensDistortion>(out lensDistortion);
        volume.profile.TryGetSettings<Vignette>(out vignette);

        col = GetComponent<Collider>();
        if (!col.isTrigger)         // just to be safe
            col.isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInside = true;
            playerTransform = other.transform;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInside = true;
            playerTransform = other.transform;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInside = false;
            playerTransform = null;
            ResetEffect();
        }
    }

    void Update()
    {
        if (isInside)
        {
            Vector3 center = transform.position;
            float distance = Vector3.Distance(playerTransform.position, center);

            float maxDist = 1f;  // default if none
            if (col is SphereCollider sphere)
            {
                maxDist = sphere.radius;
            }
            else if (col is BoxCollider box)
            {
                Vector3 halfExtends = box.size / 2f;
                maxDist = halfExtends.magnitude;
            }

            float normDist = Mathf.Clamp(distance / maxDist, 0f, 1f);

            if (lensDistortion != null)
            {
                float amplitude = Mathf.Lerp(minDistortionIntensity, maxDistortionIntensity, 1f - normDist);
                float noise = Mathf.PerlinNoise(Time.time * noiseSpeed, 0f);
                lensDistortion.intensity.value = noise * amplitude;
                lensDistortion.intensity.overrideState = true;
            }

            if (vignette != null)
            {
                float intensity = Mathf.Lerp(minVignetteIntensity, maxVignetteIntensity, 1f - normDist);
                vignette.intensity.value = intensity;
                vignette.intensity.overrideState = true;
            }
        }
    }

    void ResetEffect()
    {
        if (lensDistortion != null)
        {
            lensDistortion.intensity.value = 0f;
            lensDistortion.intensity.overrideState = false;
        }

        if (vignette != null)
        {
            vignette.intensity.value = 0f;
            lensDistortion.intensity.overrideState = false;
        }
    }
}
