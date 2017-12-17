using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    private Transform myCameraTransform;
    private Transform[] myLayers;
    private float viewZone = 3;
    private int leftIndex;
    private int rightIndex;
    private float timeStamp;
    private float gameWidth;

    public float Cooldown;
    public Vector2 Direction;
    public ObjectPool MyObjectPool;
    public Sprite[] Planets;
    public float BackgroundSize;

    void Start()
    {
        gameWidth = Camera.main.orthographicSize;
        myCameraTransform = Camera.main.transform;
        myLayers = new Transform[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
            myLayers[i] = transform.GetChild(i);

        leftIndex = 0;
        rightIndex = myLayers.Length - 1;
    }

    private void Update()
    {
        if (myCameraTransform.position.y > myLayers[leftIndex].transform.position.y + viewZone)
            ScrollUp();

        if (timeStamp < Time.time)
        {
            CreatePlanet();
            timeStamp = Time.time + Random.Range(Cooldown / 2, Cooldown);
        }

        foreach (var myLayer in myLayers)
        {
            myLayer.transform.Translate(Direction * Time.deltaTime);
        }
    }

    private void CreatePlanet()
    {
        MovingObject so = MyObjectPool.GetPooledObject("planet");
        
        so.transform.position = new Vector3(Random.Range(-gameWidth / 2, gameWidth / 2), 3);
        float scale = Random.Range(0.2f, 0.4f);
        so.transform.localScale = new Vector2(scale, scale);

        so.GetComponent<SpriteRenderer>().sprite = Planets[Random.Range(0, Planets.Length)];
        so.GetComponent<ScrollingObject>().Direction = new Vector2(0, Random.Range(-1f, -0.6f));

        so.gameObject.SetActive(true);
    }   

    private void ScrollUp()
    {
        myLayers[leftIndex].localPosition = Vector3.up * (myLayers[rightIndex].localPosition.y + BackgroundSize);
        rightIndex = leftIndex;
        leftIndex++;

        if (leftIndex == myLayers.Length)
            leftIndex = 0;
    }
}
