using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.IO;

public class Converter : MonoBehaviour
{
    public Image img;
    public Canvas canvas;
    string path;
    public void Apply()
    {
        path = EditorUtility.OpenFilePanel("Load your img", "", "png");

        Texture2D temp = new Texture2D(1, 1);
        temp.LoadImage(File.ReadAllBytes(path));
        img.sprite = Sprite.Create(temp, new Rect(0, 0, temp.width, temp.height), new Vector2(0, 0));

        if(temp.width > temp.height)
        {
            float factor = (canvas.pixelRect.width/2) / temp.width;
            img.rectTransform.sizeDelta = new Vector2(canvas.pixelRect.width/2, temp.height*factor);
        }
        else
        {
            float factor = canvas.pixelRect.height / temp.height;
            img.rectTransform.sizeDelta = new Vector2(temp.width * factor, canvas.pixelRect.height);
        }

        img.rectTransform.position = new Vector2(img.rectTransform.sizeDelta.x/2, img.rectTransform.position.y);
    }
    public void Download()
    {
        byte[] temp;
        temp = img.sprite.texture.EncodeToPNG();
        string desPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyPictures);
        File.WriteAllBytes(desPath + "/" + path.Split('/')[path.Split('/').Length - 1].Split('.')[0] + ".png", temp);
    }
    public void Encode()
    {
        Texture2D temp = img.sprite.texture;
        Color32[] factors = new Color32[temp.width * temp.height];
        Color32[] pixels = temp.GetPixels32();

        for(int i = 0; i < pixels.Length; i++)
        {
            byte factorR = (byte)Random.Range(0, 255);
            byte factorG = (byte)Random.Range(0, 255);
            byte factorB = (byte)Random.Range(0, 255);
            byte factor4 = (byte)Random.Range(0, 255);
            pixels[i] = new Color32((byte)Loop(pixels[i].r + factorR), (byte)Loop(pixels[i].g + factorG), (byte)Loop(pixels[i].g + factorB), (byte)Loop(pixels[i].a+factor4));
            factors[i] = new Color32(factorR, factorG, factorB, factor4);
        }

        temp.SetPixels32(pixels);

        img.sprite = Sprite.Create(temp, new Rect(0, 0, temp.width, temp.height), new Vector2(0, 0));
        temp.Apply();
    }
    public void Decode()
    {
        
    }
    float Loop(float index)
    {
        if (index > 255)
        {
            return index - 255;
        }
        if(index < 0)
        {
            return index + 255;
        }
        return index;
    }
}
