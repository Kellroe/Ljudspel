using UnityEngine;
using System.Collections;

public class uv_scroll : MonoBehaviour {

    #region Members

    public Vector3 direction = Vector3.zero;
    public float scrollSpeed = 3.0f;

    #endregion

    void Update()
    {
        float offset = Time.time * scrollSpeed;
        renderer.material.SetTextureOffset("_MainTex", offset * direction);
    }
}
