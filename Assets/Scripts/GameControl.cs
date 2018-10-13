using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour {
    private int LastScore;    //上次分数
    private int LastLength;   //上次长度
    private int BestScore;    //最好分数
    private int BestLength;   //最好长度
    public Text LastScoreText; //上次分数UI
    public Text BestScoreText; //最好分数UI
	
	void Start () {
        LastLength = PlayerPrefs.GetInt("lastlength");
        LastScore = PlayerPrefs.GetInt("lastscore");
        BestLength = PlayerPrefs.GetInt("bestlength");
        BestScore = PlayerPrefs.GetInt("bestscore");
        LastScoreText.text = "上次 长度:" + LastLength + " 分数:" + LastScore;
        BestScoreText.text = "最好 长度:" + BestLength + " 分数:" + BestScore;
    }

	void Update () {
      
	}
    public void StartGame()
    {
        SceneManager.LoadScene("Play");
    }
    public void UpdateScore()
    {
        //if (SnakeHead.Score >= LastScore)
        //{
        //    BestScore = SnakeHead.Score;
        //}
        //else
        //{
        //    BestScore = LastScore;
        //}
        //BestScore = (SnakeHead.Score >= LastScore) ? SnakeHead.Score : LastScore;
        //BestLength = (SnakeHead.Length >= LastLength) ? SnakeHead.Score : LastLength;
        //LastScore = SnakeHead.Score;
        //LastLength = SnakeHead.Length;
        
        
    }
    public void Bule(bool Ison)
    {
        if (Ison)
        {
            PlayerPrefs.SetString("sh","sh01");
            PlayerPrefs.SetString("sb01", "sb0101");
            PlayerPrefs.SetString("sb02", "sb0102");
        }
    }
    public void Yellow(bool Ison)
    {
        if (Ison)
        {
            PlayerPrefs.SetString("sh", "sh02");
            PlayerPrefs.SetString("sb01", "sb0201");
            PlayerPrefs.SetString("sb02", "sb0202");
        }
    }
    public void Border(bool Ison)
    {
        if (Ison)
        {
            PlayerPrefs.SetInt("border", 0);  //0的时候有边框
        }
    }
    public void Noborder(bool Ison)
    {
        if (Ison)
        {
            PlayerPrefs.SetInt("border", 1); //1的时候无边框
        }
    }
}
