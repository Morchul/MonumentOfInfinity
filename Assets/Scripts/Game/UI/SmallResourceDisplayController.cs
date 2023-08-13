using UnityEngine;
using UnityEngine.UI;

public class SmallResourceDisplayController : MonoBehaviour
{
    private Text text;
    private SpriteController image;

    // Start is called before the first frame update
    private void Awake()
    {
        text = GetComponentInChildren<Text>();
        image = GetComponentInChildren<SpriteController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetResource(Resource resource)
    {
        if (text != null)
            text.text = resource.Amount + "x";
        Sprite sprite = GameAssets.GetInstance().GetResourceSprite(resource.Type);
        image.SetSprite(sprite);
    }
}
