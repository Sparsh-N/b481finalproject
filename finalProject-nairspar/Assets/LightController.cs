using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{

    // serfields are for basically making sure we can modifuy it
    private Material sphereMaterial;
    [SerializeField]
    private Color emissionColor = Color.white;
    [SerializeField]
    private float emissionStrength = 1.0f;

    void Start() {
        sphereMaterial = GetComponent<Renderer>().material;
        // track light posn. sphereMaterial.setVector (vec4)
        // float4x4 spherePosn = transform.position;
        // UpdateEmission();
    }

    void Update() {
        // if (Input.GetKeyDown(KeyCode.L))
        // {
        //     isLightOn = !isLightOn;
        //     UpdateEmission();
        // }
        Vector4 spherePosn = new Vector4(transform.position.x, transform.position.y, transform.position.z, 1.0f);
    }

    void UpdateEmission()
    {
        // if (isLightOn) {
        //     sphereMaterial.SetColor("_EmissionColor", emissionColor);
        //     sphereMaterial.SetFloat("_EmissionStrength", emissionStrength);
        //     sphereMaterial.EnableKeyword("_EMISSION");
        // }
        // else {
        //     sphereMaterial.SetColor("_EmissionColor", Color.black);
        //     sphereMaterial.SetFloat("_EmissionStrength", 0.0f);
        //     sphereMaterial.DisableKeyword("_EMISSION");
        // }
    }
}
