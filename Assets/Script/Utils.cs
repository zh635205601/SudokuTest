using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils
{  
    public static Color TryParseHtmlString(string colorStr)
    {
        Color color = new Color();
        ColorUtility.TryParseHtmlString(colorStr, out color);
        return color;
    }
}
