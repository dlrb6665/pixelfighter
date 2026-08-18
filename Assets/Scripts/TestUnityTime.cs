using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 三种不同的延时方法：
 *                      1. 协程
 *                      2. Invoke
 *                      3. Time.DeltaTime
 */

public class TestUnityTime : MonoBehaviour
{
    public GameObject[] objs;
    public float coldTime;

    private int j = 0;
    private float time;

    // Start is called before the first frame update
    void Start()
    {
        //DeletByCoroutine();
        //DeletByInvoke();
        time = coldTime;
    }

    // Update is called once per frame
    void Update()
    {
        DeletByDeltaTime();
    }

    //1. 协程
    void DeletByCoroutine()
    {
        for (int i = 0; i < objs.Length; i++)
        {
            StartCoroutine(DeleteObj(i, i + 1));
        }
    }

    IEnumerator DeleteObj(int i,float t)
    {
        yield return new WaitForSeconds(t);
        Destroy(objs[i]);
    }

    //2. Invoke
    void DeletByInvoke()
    {
        for (int i = 0; i < objs.Length; i++)
        {
            Invoke("DeletObj2", i + 1);
        }
    }

    void DeletObj2()
    {
        Destroy(objs[j]);
        j++;
    }

    //3. Time.DeltaTime
    void DeletByDeltaTime()
    {
        if (j < 10)
        {
            if (coldTime > 0)
            {
                coldTime -= Time.deltaTime;
            }
            else
            {
                Destroy(objs[j++]);
                coldTime = time;
            }
        }       
    }
}
