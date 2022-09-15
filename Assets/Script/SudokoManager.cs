using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SudokoManager
{
    private static SudokoManager instance = null; 
    private int[,] _curSudoko = new int[9, 9];
    private int[,] _answerSudoko = new int[9, 9];
    //9行，每行有9个布尔值，如果[2,3]为True，则表示3行存在3
    private bool[,] _row = new bool[9, 9];
    //9列，每列有9个布尔值，如果[2,3]为True，则表示3列存在3
    private bool[,] _col = new bool[9, 9];
    //9个小九宫格，每个小九宫格有9个布尔值，如果[2,3]为True，则表示第3个小九宫格存在3
    private bool[,] _box = new bool[9, 9];

    private SudokoManager() { }
    public static SudokoManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new SudokoManager();
            }
            return instance;
        }
    }
    public int[,] CurSudoko { get => _curSudoko; set => _curSudoko = value; }
    public bool[,] Row { get => _row; set => _row = value; }
    public bool[,] Col { get => _col; set => _col = value; }
    public bool[,] Box { get => _box; set => _box = value; }

    public int[,] GetNewSudoko(int difficulty)
    { 
        while(!CheckSudokoCompliance())
        {
            InitSudoko();
            _answerSudoko = (int[,])_curSudoko.Clone();
            SolveSudoko(_answerSudoko);
        }
        int[,] tempSudoko = new int[9,9];
        while (!CheckSudokoEqual(tempSudoko,_answerSudoko))
        {
            tempSudoko = (int[,])_answerSudoko.Clone();
            DigSudoko(tempSudoko, difficulty);
            _curSudoko = (int[,])tempSudoko.Clone();
            SolveSudoko(tempSudoko);
        }  
        Debug.Log("初始生成完毕");

        return _curSudoko;
    }

    /// <summary>
    /// 生成一个随机的九宫格
    /// </summary>
    private void InitSudoko()
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                _curSudoko[i, j] = 0;
                _row[i, j] = false;
                _col[i, j] = false;
                _box[i, j] = false;
            }
        }
        for (int n = 0; n < SudokoMode.CREAT_NUM_COUNT; n++)
        {
            int i = Random.Range(0, 9);
            int j = Random.Range(0, 9);
            if (_curSudoko[i, j] != 0)
            {
                n--;
                continue;
            }
            int num = Random.Range(0, 9);
            int k = (i / 3) * 3 + j / 3;
            if (_row[i, num] || _col[j, num] || _box[k, num])
            {
                n--;
                continue;
            }
            _curSudoko[i, j] = num + 1;
            _row[i, num] = _col[j, num] = _box[k, num] = true;
        } 
    }

    private void DigSudoko(int[,] sudoko ,int diffdifficulty)
    {
        for (int n = 0; n < diffdifficulty; n++)
        {
            int i = Random.Range(0, 9);
            int j = Random.Range(0, 9);
            if (sudoko[i, j] != 0)
            {
                sudoko[i, j] = 0;
            }
            else
            {
                n--;
            }
        } 
    }

    private void InitSudokoHelper(int[,] sudoko, bool[,] row, bool[,] col, bool[,] box)
    { 
        int i = Random.Range(0, 8);
        int j = Random.Range(0, 8);
        if (sudoko[i, j] != 0)
            InitSudokoHelper(_curSudoko, row, col, box);
        int num = Random.Range(0, 8); 
        int k = (i / 3) * 3 + j / 3;
        if (row[i, num]||col[j, num]||box[k, num])
            InitSudokoHelper(_curSudoko, row, col, box);
        sudoko[i, j] = num+1;
        row[i, num] = col[j, num] = box[k, num] = true;
        return;
    }

    /// <summary>
    /// 解决已有的九宫格问题
    /// </summary>
    /// <param name="sudoko">已有的九宫格按照9x9二维数组编写，空的位置用0替代</param>
    /// <returns>整理好的九宫格</returns>
    public void SolveSudoko(int[,] sudoko)
    {
        if (sudoko == null) return;
        //9行，每行有9个布尔值，如果[2,3]为True，则表示3行存在3
        //bool[,] row = new bool[9, 9];
        //9列，每列有9个布尔值，如果[2,3]为True，则表示3列存在3
        //bool[,] col = new bool[9, 9];
        //9个小九宫格，每个小九宫格有9个布尔值，如果[2,3]为True，则表示第3个小九宫格存在3
        //bool[,] box = new bool[9, 9];
        //遍历初始九宫格，把现有的数字标记好
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                _row[i, j] = false;
                _col[i, j] = false;
                _box[i, j] = false;
            }
        }
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (sudoko[i, j] == 0) continue;
                int num = sudoko[i, j] - 1;
                int k = (i / 3) * 3 + j / 3;
                _row[i, num] = _col[j, num] = _box[k, num] = true;
            }
        }
        //开始运行解题
        SolveSudokoHelper(sudoko, 0, _row, _col, _box); 
    }

    /// <summary>
    /// 九宫格枚举解题
    /// </summary>
    /// <param name="sudoko">已有的九宫格</param>
    /// <param name="index">在试第几个数字</param>
    /// <param name="row">一个9x9的二维bool数组，ture的位置代表行里已经存在的数字</param>
    /// <param name="col">一个9x9的二维bool数组，ture的位置代表列里已经存在的数字</param>
    /// <param name="box">一个9x9的二维bool数组，ture的位置代表小九宫格里已经存在的数字</param>
    /// <returns>判断九宫格单个格子是否填好</returns>
    public bool SolveSudokoHelper(int[,] sudoko, int index, bool[,] row, bool[,] col, bool[,] box)
    {
        //如果是81说明最后一个说明都填好了
        if (index == 81)
            return true;
        int i = index / 9;
        int j = index % 9;
        //如果第[i,j]个有数字，就去试填下一个
        if (sudoko[i, j] != 0)
            return SolveSudokoHelper(sudoko, index + 1, row, col, box);
        int k = (i / 3) * 3 + j / 3;
        //开始尝试填写数字
        for (int num = 0; num < 9; num++)
        {
            //如果所在的行列小九宫里有这个数字，就跳过，如果九次都没有，就把这一个设置成0，return false
            if (row[i, num] || col[j, num] || box[k, num]) continue;
            //如果没有，填入数字，把所在的行列小九宫表示该数字的位置设为true
            sudoko[i,j] = num+1;
            row[i, num] = col[j, num] = box[k, num] = true;
            //用更新好是数据进行下一个位置的填写
            if (SolveSudokoHelper(sudoko, index + 1, row, col, box)) return true;
            //如果运行到这里，说明这个位置的填写是错误的，要进行回撤，就把在行列小九宫里的这个位置的锁定解除，进行下一个循环的修改
            row[i, num] = col[j, num] = box[k, num] = false;
        }
        sudoko[i, j] = 0;
        return false;
    }

    /// <summary>
    /// 检查数独数组里是否有0
    /// </summary>
    /// <returns></returns>
    private bool CheckSudokoCompliance()
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (_answerSudoko[i, j] == 0) return false;
            }
        }
        return true;
    }

    private bool CheckSudokoEqual(int[,] sudoko_1,int[,] sudoko_2)
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (sudoko_1[i,j] != sudoko_2[i,j])
                {
                    return false;
                }
            }
        }
        return true;
    }

    public bool CheckPointValid(int num,int row,int col)
    {
        return _answerSudoko[row, col] == num;
    }

    public int GetRightAnser(int row,int col)
    {
        return _answerSudoko[row, col];
    }
}

