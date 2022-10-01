using UnityEngine;
using UnityEngine.UI;

public class ImageAlphaSetter : MonoBehaviour {
    Image image;

    private void Start() {
        image = GetComponent<Image>();
    }

    public void SetAlphaZero() {
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0f);
    }

    public void SetAlphaHalf() {
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0.5f);
    }

    public void SetAlphaFull() {
        image.color = new Color(image.color.r, image.color.g, image.color.b, 1f);
    }
}
