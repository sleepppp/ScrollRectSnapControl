using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollRectSnap : MonoBehaviour
{
    public RectTransform ScrollView;        //Buttons들의 부모 Transform : 실재로 스크롤 할 때 해당 Transform이 이동한다. 
    public Button[] Buttons;                //Button들
    public RectTransform Center;            //화면의 Center좌표에 위치한 Transform 
    public int StartButtonIndex = 1;        //시작할 때 화면의 중심에 표시될 ButtonIndex

    float[] _distances;                     //중심점부터 각 버튼들의 좌표까지의 거리(절대값 O)
    float[] _distanceRepositions;           //중심점부터 각 버튼들의 좌표까지의 거리(절대값 X)
    bool _isDragging;                       //현재 드래그 중인지 여부
    int _buttonDistance;                    //각 버튼 사이의 거리
    int _minButtonIndex;                    //중심점으로 부터 가장 가까운 버튼인덱스

    private void Start()
    {
        //중심점으로부터 버튼가지의 거리를 담을 변수들이므로 Button 수만큼 할당
        _distances = new float[Buttons.Length];                 
        _distanceRepositions = new float[Buttons.Length];
        //각 버튼 사이의 거리
        _buttonDistance = (int)Mathf.Abs(Buttons[0].GetComponent<RectTransform>().anchoredPosition.x -
            Buttons[1].GetComponent<RectTransform>().anchoredPosition.x);
        //시작 위치 세팅
        ScrollView.anchoredPosition = new Vector2((StartButtonIndex) * -300, 0f);
    }

    private void Update()
    {
        //거리 계산~
        for(int i =0; i <_distances.Length; ++i)
        {
            _distanceRepositions[i] = Center.position.x - Buttons[i].transform.position.x;
            _distances[i] = Mathf.Abs(_distanceRepositions[i]);

            //왼쪽으로 일정 범위를 벗어났다면 가장 오른쪽에 위치하게 처리
            if(_distanceRepositions[i] > 1200)
            {
                float currentX = Buttons[i].GetComponent<RectTransform>().anchoredPosition.x;
                float currentY = Buttons[i].GetComponent<RectTransform>().anchoredPosition.y;

                Vector2 newPosition = new Vector2(currentX + (Buttons.Length * _buttonDistance), currentY);
                Buttons[i].GetComponent<RectTransform>().anchoredPosition = newPosition;
            }
            //오른쪽으로 일정 범위를 벗어났다면 가장 왼쪽에 위치하게 처리
            else if (_distanceRepositions[i] < -1200)
            {
                float currentX = Buttons[i].GetComponent<RectTransform>().anchoredPosition.x;
                float currentY = Buttons[i].GetComponent<RectTransform>().anchoredPosition.y;

                Vector2 newPosition = new Vector2(currentX -(Buttons.Length * _buttonDistance), currentY);
                Buttons[i].GetComponent<RectTransform>().anchoredPosition = newPosition;
            }
        }
        // {{ 전체 버튼중 가장 중심점과 가까운 버튼 인덱스 찾기~
        float minDistance = Mathf.Min(_distances);

        for(int i =0; i < Buttons.Length; ++i)
        {
            if(minDistance == _distances[i])
            {
                _minButtonIndex = i;
            }
        }
        // }}
        
        //드래깅 중이 아니라면 가장 중심과 가까운 버튼을 중심으로 천천히 보간 처리
        if(_isDragging == false)
        {
            LerpToButton(-Buttons[_minButtonIndex].GetComponent<RectTransform>().anchoredPosition.x);
        }
    }

    void LerpToButton(float position)
    {
        float newX = Mathf.Lerp(ScrollView.anchoredPosition.x, position, Time.deltaTime * 10f);
        Vector2 newPosition = new Vector2(newX, ScrollView.anchoredPosition.y);

        ScrollView.anchoredPosition = newPosition;
    }

    public void StartDrag()
    {
        _isDragging = true;
    }

    public void EndDrag()
    {
        _isDragging = false;
    }
}
