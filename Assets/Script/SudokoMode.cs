using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SudokoMode
{
    public static string RED_COLOR = "#E55A66";
    public static string BLUE_COLOR = "#8EE1E2";
    public static string TEXT_BLUE_COLOR = "#4287AB";
    public static string TEXT_BLACK_COLOR = "#000000";
    public static int CREAT_NUM_COUNT = 17;

    public enum CELL_STATE {
        NONE = 0,
        BLUE = 1,
        RED = 2,
     }
    public enum Difficulty
    {
        Easy = 22,
        Medium = 30,
        Hard = 40,
        Hell = 50,
    } 
}
