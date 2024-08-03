using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InfiniteScrollList : MonoBehaviour
{
    // 滚动内容的 RectTransform
    public RectTransform content;
    // 列表项的预制件
    public GameObject listItemPrefab;
    // 总项数
    public int totalItems = 1000;
    // 可见项数
    public int visibleItems = 10;

    // 存储当前可见的列表项
    private List<GameObject> items = new List<GameObject>();
    // 列表项的高度
    private float itemHeight;
    // 当前顶部项的索引
    private int topIndex = 0;
    // 当前底部项的索引
    private int bottomIndex;

    void Start()
    {
        // 获取列表项的高度
        itemHeight = listItemPrefab.GetComponent<RectTransform>().rect.height;
        // 初始化底部索引
        bottomIndex = visibleItems - 1;

        // 创建初始可见的列表项
        for (int i = 0; i < visibleItems; i++)
        {
            GameObject item = Instantiate(listItemPrefab, content);
            item.GetComponentInChildren<Text>().text = "Item " + i;
            items.Add(item);
        }

        // 设置内容的高度
        content.sizeDelta = new Vector2(content.sizeDelta.x, totalItems * itemHeight);
    }

    void Update()
    {
        // 如果没有可见的列表项，则返回
        if (items.Count == 0) return;

        // 获取当前滚动位置
        float scrollPos = content.anchoredPosition.y;
        // 计算新的顶部索引
        int newTopIndex = Mathf.FloorToInt(scrollPos / itemHeight);
        // 计算新的底部索引
        int newBottomIndex = newTopIndex + visibleItems - 1;

        // 如果顶部索引发生变化
        if (newTopIndex != topIndex)
        {
            // 如果新的顶部索引大于当前顶部索引（向下滚动）
            if (newTopIndex > topIndex)
            {
                // 更新列表项
                for (int i = topIndex; i < newTopIndex; i++)
                {
                    // 更新列表项的文本
                    items[0].GetComponentInChildren<Text>().text = "Item " + (bottomIndex + 1);
                    // 将列表项移动到末尾
                    items[0].transform.SetAsLastSibling();
                    // 将列表项添加到末尾
                    items.Add(items[0]);
                    // 从开头移除列表项
                    items.RemoveAt(0);
                    // 更新底部索引
                    bottomIndex++;
                }
            }
            // 如果新的顶部索引小于当前顶部索引（向上滚动）
            else
            {
                // 更新列表项
                for (int i = newTopIndex; i < topIndex; i++)
                {
                    // 更新列表项的文本
                    items[items.Count - 1].GetComponentInChildren<Text>().text = "Item " + (newTopIndex - 1);
                    // 将列表项移动到开头
                    items[items.Count - 1].transform.SetAsFirstSibling();
                    // 将列表项添加到开头
                    items.Insert(0, items[items.Count - 1]);
                    // 从末尾移除列表项
                    items.RemoveAt(items.Count - 1);
                    // 更新新的顶部索引
                    newTopIndex--;
                }
            }

            // 更新顶部和底部索引
            topIndex = newTopIndex;
            bottomIndex = newBottomIndex;
        }
    }
}