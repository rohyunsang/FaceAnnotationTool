using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleConnect : MonoBehaviour
{
    public RectTransform[] circles;
    public GameObject linePrefab;
    public Transform spawnPoint;

    public void SetLine()
    {
        for (int i = 0; i < circles.Length; i++)
        {
            RectTransform startCircle = circles[i];
            RectTransform endCircle = circles[(i + 1) % circles.Length];  // 원의 마지막과 첫 번째 원을 연결하기 위해 % 연산자 사용

            // 원과 원 사이의 중간 위치 계산
            Vector2 middlePosition = (startCircle.anchoredPosition + endCircle.anchoredPosition) / 2;

            // 선의 길이 계산
            float distance = Vector3.Distance(startCircle.position, endCircle.position);

            // 선의 방향 계산
            Vector3 direction = (endCircle.position - startCircle.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // 선 인스턴스 생성 및 설정
            GameObject lineInstance = Instantiate(linePrefab, middlePosition, Quaternion.Euler(0, 0, angle), spawnPoint.transform);
            RectTransform lineRect = lineInstance.GetComponent<RectTransform>();
            if (lineRect)
            {
                lineRect.sizeDelta = new Vector2(distance, lineRect.sizeDelta.y);
            }
        }
    }
}


