using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.IO;
using System.Runtime.InteropServices;

public class Converter : MonoBehaviour
{
    public Image img;
    public Canvas canvas;
    string path;
    string imgName;
    public void Apply()
    {
        path = EditorUtility.OpenFilePanel("Load your img", "", "png");

        Texture2D temp = new Texture2D(1, 1);
        temp.LoadImage(File.ReadAllBytes(path));
        img.sprite = Sprite.Create(temp, new Rect(0, 0, temp.width, temp.height), new Vector2(0, 0));

        imgName = path.Split('/')[path.Split('/').Length - 1].Split('.')[0];

        if (temp.width > temp.height)
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
        File.WriteAllBytes(desPath + "/" + imgName + "_converter.png", temp);
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
            //byte factorA = (byte)Random.Range(0, 255);
            pixels[i] = new Color32((byte)Loop(pixels[i].r + factorR), (byte)Loop(pixels[i].g + factorG), (byte)Loop(pixels[i].b + factorB), pixels[i].a);
            factors[i] = new Color32(factorR, factorG, factorB, 0);
        }

        temp.SetPixels32(pixels);

        img.sprite = Sprite.Create(temp, new Rect(0, 0, temp.width, temp.height), new Vector2(0, 0));
        temp.Apply();

        string desPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
        File.WriteAllBytes(desPath + "/" + imgName + "_key_" + Random.Range(0, 9999999) + ".txt", MemoryMarshal.Cast<Color32, byte>(factors).ToArray());
    }
    public void Decode()
    {
        string keyPath = EditorUtility.OpenFilePanel("Load your key", "", "txt");
        byte[] temp = File.ReadAllBytes(keyPath);
        Color32[] data = MemoryMarshal.Cast<byte, Color32>(temp).ToArray();

        Texture2D texture = img.sprite.texture;
        Color32[] pixels = texture.GetPixels32();

        for(int i = 0; i < pixels.Length; i++)
        {
            Color32 c = data[i];

            pixels[i] = new Color32((byte)Loop(pixels[i].r - c.r), (byte)Loop(pixels[i].g - c.g), (byte)Loop(pixels[i].b - c.b), pixels[i].a);
        }
        texture.SetPixels32(pixels);
        img.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));
        texture.Apply();
    }
    float Loop(float index)
    {
        if (index > 256)
        {
            return index - 256;
        }
        if(index < 0)
        {
            return index + 256;
        }
        return index;
    }
}
