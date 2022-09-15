//九宫格的主体UI框架
//主要用于实现文本框和按钮的功能
//15:32 星期五 2021年12月24日

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UISudoko : MonoBehaviour
{
    //用户界面下方的操作按钮
    private Button[] editorBtns = new Button[9];
    private Button pencilbtn;
    private Button eraserbtn;
    private Button lightbtn;
    private Button gameBgbtn;
    private GameObject guessObj;
    //九宫格主题内的按钮  
    private UICell[,] sudokoCells = new UICell[9, 9];
    // Start is called before the first frame update
    private int _curCellRow = 0;
    private int _curCellCol = 0;
    private bool _isGuess = false;
    private void Awake()
    {
        for (int i = 0; i < 9; i++)
        {
            editorBtns[i] = this.transform.Find("munBtns/" + (i + 1)).GetComponent<Button>();
            Button editorBtn = editorBtns[i];
            editorBtns[i].onClick.AddListener(delegate ()
            {
                OnEditorBtnDown(editorBtn.gameObject);
            });
        }
        gameBgbtn = this.transform.Find("gameBg").GetComponent<Button>();
        pencilbtn = this.transform.Find("munBtns/pencil").GetComponent<Button>();
        eraserbtn = this.transform.Find("munBtns/eraser").GetComponent<Button>();
        lightbtn = this.transform.Find("munBtns/light").GetComponent<Button>();
        guessObj = pencilbtn.transform.Find("Guess").gameObject;

        guessObj.SetActive(false);
        pencilbtn.onClick.AddListener(OnPencilBtnDown);
        eraserbtn.onClick.AddListener(OnEraserBtnDown);
        lightbtn.onClick.AddListener(OnLightBtnDown);
        gameBgbtn.onClick.AddListener(() => { ResetCellBg(); });

        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                string index = "Sudoko/num/" + (i + 1).ToString() + (j + 1).ToString();
                sudokoCells[i, j] = this.transform.Find(index).GetComponent<UICell>();
                sudokoCells[i, j].SetUISudoko(this);
                sudokoCells[i, j].Row = i;
                sudokoCells[i, j].Col = j;
            }
        }
    }
    void Start()
    {
        int[,] nums = SudokoManager.Instance.GetNewSudoko((int)SudokoMode.Difficulty.Easy);
        if (nums == null) return;
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                sudokoCells[i, j].InitNumText(nums[i, j]);
            }
        }
        Debug.Log(Time.realtimeSinceStartup);
    }

    private void OnEditorBtnDown(GameObject gameObj)
    {
        if (sudokoCells[_curCellRow, _curCellCol].IsCellCanEditor())
        {
            if (_isGuess)
            {
                if (sudokoCells[_curCellRow, _curCellCol].IsCellCanGuess())
                {
                    sudokoCells[_curCellRow, _curCellCol].SetGuessText(gameObj.name);
                }
            }
            else
            {
                sudokoCells[_curCellRow, _curCellCol].SetNumText(gameObj.name);
                if (!SudokoManager.Instance.CheckPointValid(int.Parse(gameObj.name), _curCellRow, _curCellCol))
                {
                    sudokoCells[_curCellRow, _curCellCol].SetBgState((int)SudokoMode.CELL_STATE.RED);
                }
                else
                {
                    sudokoCells[_curCellRow, _curCellCol].SetBgState((int)SudokoMode.CELL_STATE.BLUE);
                    sudokoCells[_curCellRow, _curCellCol].SetRightState();
                }
            }
        }
    }

    private void OnEraserBtnDown()
    {
        if (sudokoCells[_curCellRow, _curCellCol].IsCellCanEditor())
        {
            sudokoCells[_curCellRow, _curCellCol].SetNumText("");
            sudokoCells[_curCellRow, _curCellCol].Vailed = true;
            sudokoCells[_curCellRow, _curCellCol].SetBgState((int)SudokoMode.CELL_STATE.NONE);
        }
    }

    //铅笔按钮
    private void OnPencilBtnDown()
    {
        _isGuess = !_isGuess;
        guessObj.SetActive(_isGuess);
    }

    private void OnLightBtnDown()
    {
        if (sudokoCells[_curCellRow, _curCellCol].IsCellCanEditor() && !_isGuess)
        {
            sudokoCells[_curCellRow, _curCellCol].SetNumText(SudokoManager.Instance.GetRightAnser(_curCellRow,_curCellCol).ToString());
            sudokoCells[_curCellRow, _curCellCol].SetBgState((int)SudokoMode.CELL_STATE.BLUE);
            sudokoCells[_curCellRow, _curCellCol].SetRightState();
        }
    }

    public void CellBtnDown(int row, int col)
    {

        ResetCellBg();
        _curCellRow = row;
        _curCellCol = col;
        if (sudokoCells[row, col].GetNumText() == "")
        {
            ShowCellPrompt(row, col);
        }
        else
        {
            ShowNumberPrompt(row, col);
        }
    }

    public void ResetCellBg()
    {
        _curCellRow = 0;
        _curCellCol = 0;
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                sudokoCells[i, j].SetBgState((int)SudokoMode.CELL_STATE.NONE);
            }
        }
    }

    /// <summary>
    ///当点击到数字时改变同行，同列和相同数字的颜色
    /// </summary>
    /// <param name="row"></param>
    /// <param name="col"></param>
    public void ShowNumberPrompt(int row, int col)
    {
        string curNum = sudokoCells[row, col].GetNumText();
        List<int> rowList = new List<int>();
        List<int> colList = new List<int>();
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (i == row || j == col || sudokoCells[i, j].GetNumText() == curNum)
                {
                    sudokoCells[i, j].SetBgState((int)SudokoMode.CELL_STATE.BLUE);
                }
            }
        }
        sudokoCells[row, col].SetBgState((int)SudokoMode.CELL_STATE.NONE);

    }
    /// <summary>
    /// 当点击到空格时改变同行，同列和所在小九宫的颜色
    /// </summary>
    /// <param name="row"></param>
    /// <param name="col"></param>
    public void ShowCellPrompt(int row, int col)
    {
        int box_i = (row / 3) * 3;
        int box_j = (col / 3) * 3;
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (i == row || j == col || ((i >= box_i && i <= box_i + 2) && (j >= box_j && j <= box_j + 2)))
                {
                    //Debug.Log("row--" + row + "\tcol--" + col + "\ti--" + i+ "\tj--" + j+ "\tbox_i--" + box_i+ "\tbox_j--" + box_j);
                    sudokoCells[i, j].SetBgState((int)SudokoMode.CELL_STATE.BLUE);
                }
                if (i == row && j == col)
                {
                    sudokoCells[i, j].SetBgState((int)SudokoMode.CELL_STATE.NONE);
                }
            }
        }
    }
}