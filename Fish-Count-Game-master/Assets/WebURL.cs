using UnityEngine;

public class WebURL : MonoBehaviour
{
    private string url = "https://babyapps.kids/";
    void OnMouseDown()
    {
        if (!string.IsNullOrEmpty(url))
        {
            Application.OpenURL(url);
        }
        else
        {
            Debug.LogWarning("URL is empty or null!");
        }
    }
}
