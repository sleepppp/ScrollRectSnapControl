using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollRectSnap : MonoBehaviour
{
    public RectTransform ScrollView;        //Buttons���� �θ� Transform : ����� ��ũ�� �� �� �ش� Transform�� �̵��Ѵ�. 
    public Button[] Buttons;                //Button��
    public RectTransform Center;            //ȭ���� Center��ǥ�� ��ġ�� Transform 
    public int StartButtonIndex = 1;        //������ �� ȭ���� �߽ɿ� ǥ�õ� ButtonIndex

    float[] _distances;                     //�߽������� �� ��ư���� ��ǥ������ �Ÿ�(���밪 O)
    float[] _distanceRepositions;           //�߽������� �� ��ư���� ��ǥ������ �Ÿ�(���밪 X)
    bool _isDragging;                       //���� �巡�� ������ ����
    int _buttonDistance;                    //�� ��ư ������ �Ÿ�
    int _minButtonIndex;                    //�߽������� ���� ���� ����� ��ư�ε���

    private void Start()
    {
        //�߽������κ��� ��ư������ �Ÿ��� ���� �������̹Ƿ� Button ����ŭ �Ҵ�
        _distances = new float[Buttons.Length];                 
        _distanceRepositions = new float[Buttons.Length];
        //�� ��ư ������ �Ÿ�
        _buttonDistance = (int)Mathf.Abs(Buttons[0].GetComponent<RectTransform>().anchoredPosition.x -
            Buttons[1].GetComponent<RectTransform>().anchoredPosition.x);
        //���� ��ġ ����
        ScrollView.anchoredPosition = new Vector2((StartButtonIndex) * -300, 0f);
    }

    private void Update()
    {
        //�Ÿ� ���~
        for(int i =0; i <_distances.Length; ++i)
        {
            _distanceRepositions[i] = Center.position.x - Buttons[i].transform.position.x;
            _distances[i] = Mathf.Abs(_distanceRepositions[i]);

            //�������� ���� ������ ����ٸ� ���� �����ʿ� ��ġ�ϰ� ó��
            if(_distanceRepositions[i] > 1200)
            {
                float currentX = Buttons[i].GetComponent<RectTransform>().anchoredPosition.x;
                float currentY = Buttons[i].GetComponent<RectTransform>().anchoredPosition.y;

                Vector2 newPosition = new Vector2(currentX + (Buttons.Length * _buttonDistance), currentY);
                Buttons[i].GetComponent<RectTransform>().anchoredPosition = newPosition;
            }
            //���������� ���� ������ ����ٸ� ���� ���ʿ� ��ġ�ϰ� ó��
            else if (_distanceRepositions[i] < -1200)
            {
                float currentX = Buttons[i].GetComponent<RectTransform>().anchoredPosition.x;
                float currentY = Buttons[i].GetComponent<RectTransform>().anchoredPosition.y;

                Vector2 newPosition = new Vector2(currentX -(Buttons.Length * _buttonDistance), currentY);
                Buttons[i].GetComponent<RectTransform>().anchoredPosition = newPosition;
            }
        }
        // {{ ��ü ��ư�� ���� �߽����� ����� ��ư �ε��� ã��~
        float minDistance = Mathf.Min(_distances);

        for(int i =0; i < Buttons.Length; ++i)
        {
            if(minDistance == _distances[i])
            {
                _minButtonIndex = i;
            }
        }
        // }}
        
        //�巡�� ���� �ƴ϶�� ���� �߽ɰ� ����� ��ư�� �߽����� õõ�� ���� ó��
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
