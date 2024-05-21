using UnityEngine;
using UnityEngine.UI;

public class ButtonAni : MonoBehaviour
{
    Text buttonText;
    Image buttonImg;
    bool lerp;
    float lerpSpeed = 0;
    public Color32 c1, c2;

    private void Start()
    {
        buttonImg = GetComponent<Image>();
        buttonText = GetComponentInChildren<Text>();

        c2 = buttonImg.color;
        c1 = buttonText.color;
    }
    private void Update()
    {
        if (!lerp) return;
        lerpSpeed += Time.deltaTime / 0.2f;
        buttonText.color = Color.Lerp(c1, c2, lerpSpeed);
        buttonImg.color = Color.Lerp(c2, c1, lerpSpeed);
        transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 0.98f, lerpSpeed);
    }
    public void OnHover(bool enter)
    {
        if (enter)
        {
            lerp = true;
            transform.localScale = Vector3.one * 0.98f;
        }
        else
        {
            lerpSpeed = 0;
            lerp = false;
            buttonText.color = c1;
            buttonImg.color = c2;
            transform.localScale = Vector3.one;
        }
    }
}
