using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Quiz : MonoBehaviour
{
    public static Action _quizInitializer;
    private string[] _sign = { "+", "-", "*", "/" };
    private float _num1, _num2, _res;
    private string _currentSign;

    private void OnEnable()
    {
        _quizInitializer += InitializeQuiz;
    }

    void Start()
    {
        InitializeQuiz();
    }

    private void OnDisable()
    {
        _quizInitializer -= InitializeQuiz;
    }
    // Start is called before the first frame update

    [ContextMenu("Test")]
    public void InitializeQuiz()
    {
        _num1 = Random.Range(1, 10);
        _num2 = Random.Range(1, 10);

        _currentSign = _sign[Random.Range(0, _sign.Length)];

        switch(_currentSign)
        {
            case "+": _res = _num1 + _num2;
                      break;
            case "*":
                _res = _num1 * _num2;
                break;
            case "/":
                _res = _num1 / _num2;
                break;
            case "-":
                _res = _num1 - _num2;
                break;
        }

        Debug.Log($"{_num1} {_currentSign} {_num2} = {_res}");

        UiManager.uiManager.SetUiValues(_num1, _num2, _currentSign, _res);
    }

}
