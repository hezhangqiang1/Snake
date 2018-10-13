using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodMake : MonoBehaviour {

    public static FoodMake _interface;
    public static FoodMake Interface
    {
        get
        {
            return _interface;
        }
    }
    public GameObject foodPrefab;            //食物预制体
    public Transform up;
    public Transform down;
    public Transform right;
    public Transform left;
    private  Transform GameCanvas;           //获取父亲组件
    public Sprite[] foodSprite;            // 图片组
   

    void Start () {
        GameCanvas = GameObject.Find("GameCanvas").transform;
        Makefood();
	}

      void Awake()
    {
        
        _interface = this;
    }
    void Update () {
		
	}
    public void Makefood()
    {
        int index = Random.Range(0, foodSprite.Length);
        int x = (int)Random.Range(left.position.x+20, right.position.x-20);
        int y = (int)Random.Range(down.position.y+20, up.position.y-20);
        GameObject food = Instantiate(foodPrefab);
        food.GetComponent<Image>().sprite = foodSprite[index];
        food.transform.SetParent(GameCanvas, false);                   //设置父亲节点
        food.transform.position = new Vector3(x, y, 0);
       
    }
}
