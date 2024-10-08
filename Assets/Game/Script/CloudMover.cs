using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMover : MonoBehaviour
{
    // Start is called before the first frame update
    float speed = 0.02f; // 云彩移动速度
    Vector3 startPoint; // 云彩初始位置
    Vector3 endPoint;   // 云彩目标位置
    void Start()
    {
        startPoint = new Vector3(-1.25f, transform.localPosition.y, transform.localPosition.z); // 设置云彩初始位置
        endPoint = new Vector3(0.5f, transform.localPosition.y, transform.localPosition.z); // 设置云彩目标位置
    }

    // Update is called once per frame
    void Update()
    {
        // 移动云彩
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, endPoint, speed * Time.deltaTime);

        // 如果云彩到达目标位置，重置为初始位置
        if (transform.localPosition == endPoint)
        {
            transform.localPosition = startPoint;
        }
    }
}
