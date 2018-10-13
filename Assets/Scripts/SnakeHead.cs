using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Collections;
using UnityEngine.SceneManagement;

public class SnakeHead : MonoBehaviour {
    public List<Transform> bodyList = new List<Transform>() ;
    private  float x;             //左右移动的距离
    private  float y;             //上下移动的距离
    private float MoveSpeed=0.35f; //蛇头移动速度
    private Vector3 HeadPos;      //头部所在的位置
    public GameObject bodyPrefabs;//蛇身预制品
    public Sprite[] bodySprite=new Sprite[2];
    private Transform Cancas;      //获取蛇身的父亲组件
    public    int  Score;            //记录分数
    public Text ScoreText;         //分数UI
    public    int  Length;           //蛇的长度
    public Text LengthText;      //长度UI；
                                 //  private int Step;            //表示阶段
    public Text PhaseText;      // 阶段ui
    public Image Bgimage;       //获取背景图片
    private Color PhaseColor;   //换的颜色
    public GameObject DieText; //死亡特效
    private bool IsDie=false ;        //判断是否死亡
    public Image PauseImage;    //暂停图片
    private bool IsPause;       //判断是否暂停
    public Sprite[] PauseSprite;  //暂停播放图片组
    private int Bestlength;       //最好长度
    private int Bestscore;        //最好分数
    public AudioClip Eatclip;    //吃到东西音效
    public AudioClip Dieclip;    //死亡音效
    public AudioSource Bg;    //背景音

    void Start()
    {
        InvokeRepeating("Move", 0, MoveSpeed);
        x = 20; transform.localRotation = Quaternion.Euler(0, 0, 270);
        Cancas = GameObject.Find("GameCanvas").transform;
        gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>(PlayerPrefs.GetString("sh","sh02"));
        bodySprite [0]= Resources.Load<Sprite>(PlayerPrefs.GetString("sb01", "sb0201"));
        bodySprite[1] = Resources.Load<Sprite>(PlayerPrefs.GetString("sb02", "sb0202"));

    }
	
	void Update () {
      //  Debug.Log(PlayerPrefs.GetInt("lastscore"));
		if(Input .GetKeyDown (KeyCode.W)&&y!=-20 && IsDie == false)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
            y = 20;x = 0;
        }
        if (Input.GetKeyDown(KeyCode.S)&&y!=20 && IsDie == false)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 180);
            y = -20; x = 0;
        }
        if (Input.GetKeyDown(KeyCode.A)&&x!=20 && IsDie == false)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 90);
            y = 0; x = -20;
        }
        if (Input.GetKeyDown(KeyCode.D)&&x!=-20 && IsDie == false)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 270);
            y = 0; x = 20;
        }
        if(Input.GetKeyDown (KeyCode .Space) && IsDie == false&&IsPause ==false )
        {
            CancelInvoke();
            InvokeRepeating("Move", 0, MoveSpeed-0.3f);

        }
        if(Input .GetKeyUp (KeyCode .Space) && IsDie == false && IsPause == false)
        {
            CancelInvoke();
            InvokeRepeating("Move", 0, MoveSpeed );
        }
      switch (Score / 50) //阶段
        {
            case 1:
                PhaseText.text = "阶段:\n" + 2;
                ColorUtility.TryParseHtmlString("#CCEEFFFF", out PhaseColor);
                Bgimage.color = PhaseColor;
                break;
            case 3:
                PhaseText.text = "阶段:\n" + 3;
                ColorUtility.TryParseHtmlString("#CCFFDBFF", out PhaseColor);
                Bgimage.color = PhaseColor;
                break;
            case 5:
                PhaseText.text = "阶段:\n" + 4;
                ColorUtility.TryParseHtmlString("#EBFFCCFF", out PhaseColor);
                Bgimage.color = PhaseColor;
                break;
            case 7:
                PhaseText.text = "阶段:\n" + 5;
                ColorUtility.TryParseHtmlString("#FFF3CCFF", out PhaseColor);
                Bgimage.color = PhaseColor;
                break;
            case 90:
                PhaseText.text = "阶段:\n " +"无尽";
                ColorUtility.TryParseHtmlString("#FFDACCFF", out PhaseColor);
                Bgimage.color = PhaseColor;
                break;
        }
    }
    public void Move()
    {
        HeadPos = transform.localPosition;                                              //记录头部位置
        transform.localPosition = new Vector3(HeadPos.x + x, HeadPos.y + y, HeadPos .z);//控制移动
        if(bodyList .Count > 0)
        {
            for (int i=bodyList .Count - 2; i >=0; i--)
            {
               bodyList[i + 1].localPosition = bodyList[i].localPosition;
           }
            bodyList[0].localPosition = HeadPos;
        }
    }
     void Grow()          //实例化蛇身
    {
        AudioSource.PlayClipAtPoint(Eatclip, Vector3.zero);
       int index = (bodyList.Count % 2 == 0 )? 0 : 1;
      // Debug.Log(index);
        GameObject body = Instantiate(bodyPrefabs,new Vector3 (2000,2000,0),Quaternion .identity );
        body.GetComponent<Image>().sprite = bodySprite[index];
        body.transform.SetParent(Cancas, false);
        bodyList.Add(body.transform);
    }
    void Die()
    {
        Bg.Stop();
        AudioSource.PlayClipAtPoint(Dieclip, new  Vector3(0,0,-10));
        CancelInvoke();           //暂停游戏
        IsDie = true;
        DieText.SetActive(true);
        PlayerPrefs.SetInt("lastscore",Score );
        PlayerPrefs.SetInt("lastlength", Length);
        if(PlayerPrefs .GetInt("bestlength",0)<Length)
        {
            PlayerPrefs.SetInt("bestscore", Score);
            PlayerPrefs.SetInt("bestlength", Length );
        }
       
       // Bestlength = (PlayerPrefs.GetInt("bestlength",0) >= Bestlength) ? PlayerPrefs.GetInt("lastlength") : Bestlength;
      //  Bestscore = (PlayerPrefs.GetInt("bestscore",0) >= Bestscore) ? PlayerPrefs.GetInt("lastscore") : Bestscore;
      
        StartCoroutine(Gameover(2));

    }
    IEnumerator Gameover(float t)
    {
        yield return new WaitForSeconds(t);
        SceneManager.LoadScene("Play");
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision .tag == "food")
        {
            Destroy(collision.gameObject);
            Score += 5;
            ScoreText.text = "分数:\n" + Score;
            Length += 1;
            LengthText.text = "长度:\n" + Length;
          //  Debug.Log(Score);
            Grow();
            FoodMake._interface.Makefood();
           
        }
        else if (collision .tag =="body")
        {
           
            Die();
        }
        else
        {
            if (PlayerPrefs .GetInt ("border")==0)
            {
                Die();
            }
            else
            {
                switch (collision.name)
                {
                    case "Up":
                        transform.localPosition = new Vector3(transform.localPosition.x, -transform.localPosition.y + 30, transform.localPosition.z);
                        break;
                    case "Down":
                        transform.localPosition = new Vector3(transform.localPosition.x, -transform.localPosition.y - 30, transform.localPosition.z);
                        break;
                    case "Left":
                        transform.localPosition = new Vector3(-transform.localPosition.x + 220, transform.localPosition.y, transform.localPosition.z);
                        break;
                    case "Right":
                        transform.localPosition = new Vector3(-transform.localPosition.x + 280, transform.localPosition.y, transform.localPosition.z);
                        break;
                }
            }
           
           
        }
    }
    public void Pause()             //暂停游戏方法
    {
        IsPause = !IsPause;
        if (IsPause)
        {
            Time.timeScale = 0;     //暂停游戏
            Bg.Pause();
            PauseImage.GetComponent<Image>().sprite = PauseSprite[1];
        }
        else
        {
            Time.timeScale = 1;     //开始游戏
            Bg.Play();
            PauseImage.GetComponent<Image>().sprite = PauseSprite[0];
        }
    }
    public void Home()
    {
             
            SceneManager.LoadScene("Game");
            
    }
}
