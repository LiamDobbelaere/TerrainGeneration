using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureScroll : MonoBehaviour
{
    private Renderer rend;
    public float xScroll = 0.075f;
    public float yScroll = 0.075f;

    // Start is called before the first frame update
    void Start()
    {
        rend = this.GetComponent<Renderer>();

        Vector2 offset = rend.material.mainTextureOffset;

        offset.x = Random.Range(0.0f, 1.0f);
        offset.y = Random.Range(0.0f, 1.0f);

        rend.material.mainTextureOffset = offset;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 offset = rend.material.mainTextureOffset;

        offset.x += xScroll * Time.deltaTime;
        offset.y += yScroll * Time.deltaTime;

        rend.material.mainTextureOffset = offset;
    }
}
