//九宫格内每一个小格的UI功能
//11:49 星期六 2021年12月25日
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICell : MonoBehaviour
{
    private bool _vailed = true;
    private Button numBtn;
    private Text numText;
    private Transform guessTsm;
    private Image bgImg;
    public UISudoko UISudoko;
    private GameObject[] guessObjs = new GameObject[9];
    private bool _cellCanEdit = false;
    private bool _cellCanGuess = false;
    private int row = 0;
    private int col = 0; 

    public int Row { get => row; set => row = value; }
    public int Col { get => col; set => col = value; }
    public bool Vailed { get => _vailed; set => _vailed = value; }

    // Start is called before the first frame update
    private void Awake()
    {
        numBtn = this.gameObject.GetComponent<Button>();
        numText = this.transform.Find("confirm").GetComponent<Text>();
        numBtn.onClick.AddListener(OnNumBtnClick);
        guessTsm = this.transform.Find("guess");
        bgImg = this.transform.Find("bg").GetComponent<Image>();
        for (int i = 0; i < guessTsm.childCount; i++)
        {
            guessTsm.GetChild(i).GetComponent<Text>().text = (i+1).ToString();
            guessTsm.GetChild(i).gameObject.SetActive(false);
            guessObjs[i] = guessTsm.GetChild(i).gameObject;
        }
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnNumBtnClick()
    { 
        if(UISudoko)
        {
            UISudoko.CellBtnDown(row, col);
        }
    }
    public void SetUISudoko(UISudoko uiSudoko)
    {
        UISudoko = uiSudoko;
    }

    public string GetNumText()
    {
        return numText.text;
    }

    public void InitNumText(int num)
    {
        if(numText)
        {
            if(num == 0)
            {
                numText.text = "";
                numText.color = Utils.TryParseHtmlString(SudokoMode.TEXT_BLUE_COLOR);
            }
            else
            {
                numText.text = num.ToString();
                numText.color = Utils.TryParseHtmlString(SudokoMode.TEXT_BLACK_COLOR);
            } 
            _cellCanEdit = numText.color == Color.black ? false : true;
            _cellCanGuess = numText.text == "" ? true : false; 
        }
    }

    public void SetNumText(string num)
    {
        if (numText)
        {
            for (int i = 0; i < 9; i++)
            {
                guessObjs[i].SetActive(false);
            }
            numText.text = num; 
        }
    }
    public void SetGuessText(string num)
    {
        guessObjs[int.Parse(num) - 1].SetActive(!guessObjs[int.Parse(num) - 1].active);
    }

    public bool IsCellCanEditor()
    {
        return _cellCanEdit;
    }

    public bool IsCellCanGuess()
    {
        return _cellCanGuess;
    }

    //给单元格设置成解题正确的模式
    public void SetRightState()
    {
        _cellCanEdit = false;
        _cellCanGuess = false;
    }

    public void SetBgState(int state)
    {
        //if (!_vailed) return;
        switch (state)
        {
            case 0:
                bgImg.gameObject.SetActive(false);
                break;
            case 1:  
                bgImg.color = Utils.TryParseHtmlString(SudokoMode.BLUE_COLOR);
                bgImg.gameObject.SetActive(true);
                break;
            case 2:
                bgImg.color = Utils.TryParseHtmlString(SudokoMode.RED_COLOR); ;
                bgImg.gameObject.SetActive(true);
                _vailed = false;
                break;
            default:
                bgImg.gameObject.SetActive(false);
                break;
        } 
    }
}
