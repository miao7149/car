using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class WheelOfFortune : MonoBehaviour
{
    public Image wheelImage; // 转盘的 Image 组件
    public float spinDuration = 5f; // 旋转持续时间
    public int numberOfSectors = 8; // 扇区数量

    private float anglePerSector; // 每个扇区的角度
    private bool isSpinning = false;

    void Start()
    {
        anglePerSector = 360f / numberOfSectors;
    }

    public void StartSpin()
    {
        if (!isSpinning)
        {
            StartCoroutine(SpinWheel());
        }
    }

    private IEnumerator SpinWheel()
    {
        isSpinning = true;

        // 随机选择一个目标角度，增加某些区域的权重
        float targetAngle = GetWeightedRandomAngle();
        float totalAngle = 360f - targetAngle + 360 * 3; // 旋转多圈
        wheelImage.rectTransform.DORotate(new Vector3(0, 0, -totalAngle), spinDuration, RotateMode.FastBeyond360)
            .SetEase(Ease.OutCubic)
            .OnComplete(() =>
            {
                // 确保最终停在目标角度
                wheelImage.rectTransform.rotation = Quaternion.Euler(0, 0, targetAngle);

                // 计算中奖扇区
                int selectedSector = Mathf.FloorToInt((targetAngle + 30) / anglePerSector);
                Debug.Log("Selected Sector: " + (selectedSector + 1));
                isSpinning = false;
            });

        yield return new WaitForSeconds(spinDuration);
    }

    // 获取带权重的随机角度
    private float GetWeightedRandomAngle()
    {
        // 定义每个扇区的权重
        float[] sectorWeights = new float[numberOfSectors];
        for (int i = 0; i < numberOfSectors; i++)
        {
            sectorWeights[i] = 1f; // 默认权重为1
        }

        // 增加特定扇区的权重，例如第一个扇区
        sectorWeights[0] = 15f; // 增加第一个扇区的权重

        // 计算总权重
        float totalWeight = 0f;
        foreach (float weight in sectorWeights)
        {
            totalWeight += weight;
        }

        // 随机选择一个加权扇区
        float randomValue = Random.Range(0, totalWeight);
        float cumulativeWeight = 0f;
        int selectedSector = 0;
        for (int i = 0; i < numberOfSectors; i++)
        {
            cumulativeWeight += sectorWeights[i];
            if (randomValue < cumulativeWeight)
            {
                selectedSector = i;
                break;
            }
        }

        // 返回对应扇区的随机角度
        float sectorStartAngle = selectedSector * anglePerSector - 30f;
        float sectorEndAngle = sectorStartAngle + anglePerSector;
        Debug.Log("Sector " + (selectedSector + 1) + " Start Angle: " + sectorStartAngle + ", End Angle: " + sectorEndAngle);
        return Random.Range(sectorStartAngle, sectorEndAngle);
    }

    // 点击按钮
    public void OnClickSpin()
    {
        StartSpin();
    }
    // 缓动函数：EaseOutCubic
    private float EaseOutCubic(float t)
    {
        return 1f - Mathf.Pow(1f - t, 3f);
    }
}