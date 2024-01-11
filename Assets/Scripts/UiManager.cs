using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    float timer = 10;
    private int chance = 5;
    private int score = 0;
    public float actualAnswer;
    [SerializeField] private TMP_Text _timerText;
    [SerializeField] private TMP_Text _num1Text;
    [SerializeField] private TMP_Text _num2Text;
    [SerializeField] private TMP_Text _signText;
    [SerializeField] private TMP_Text _resultText;
    [SerializeField] private TMP_Text _chanceLeftText;
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _popupText;
    [SerializeField] private Button _option1;
    [SerializeField] private Button _option2;
    [SerializeField] private Button _option3;
    [SerializeField] private Button _option4;
    private List<float> optionList = new List<float>();
    

    public static UiManager uiManager;

    private void Awake()
    {
        if(uiManager == null)
        {
            uiManager = this;
            Debug.Log("awake");
        }
    }

    private void Start()
    {
        _scoreText.text = "Scores :" + score.ToString();
        _chanceLeftText.text = "Chance Left :" + chance.ToString();
    }

    private void Update()
    {
        if(timer > 0)
        {
            timer -= Time.deltaTime;
            _timerText.text = timer.ToString("0");
        }
        else
        {
            Quiz._quizInitializer?.Invoke();
        }
    }

    //public void SetUiValues(float num1, float num2, string sign, float res)
    //{
    //    optionList.Clear();
    //    timer = 10;
    //    _num1Text.text = num1.ToString();
    //    _num2Text.text = num2.ToString();
    //    _signText.text = sign;
    //    actualAnswer = res;

    //    optionList.Add(res);

    //    for (float i = 1; i < 4; i++)
    //    {
    //        optionList.Add(Random.Range(0, 20));
    //    }

    //    optionList.Shuffle();
    //    SetOptions();

    //}
    public void SetUiValues(float num1, float num2, string sign, float res)
    {
        optionList.Clear();
        timer = 10;
        _num1Text.text = num1.ToString();
        _num2Text.text = num2.ToString();
        _signText.text = sign;
        actualAnswer = res;

        optionList.Add(res);

        // Calculate a dynamic range based on the magnitude of the correct result
        float magnitude = Mathf.Max(1f, Mathf.Abs(res));
        float dynamicRange = Mathf.Clamp(magnitude / 2f, 0.1f, 10f);

        for (int i = 1; i < 4; i++)
        {
            // Generate random options close to the result value within the dynamic range
            float randomOffset = Random.Range(-dynamicRange, dynamicRange);
            float randomOption = res + randomOffset;

            // Ensure that the random option is not the same as the correct result
            while (Mathf.Approximately(randomOption, res))
            {
                randomOffset = Random.Range(-dynamicRange, dynamicRange);
                randomOption = res + randomOffset;
            }

            optionList.Add(randomOption);
        }

        optionList.Shuffle();
        SetOptions();
    }

    private void SetOptions()
    {
        _option1.GetComponentInChildren<TMP_Text>().text = optionList[0].ToString();
        _option2.GetComponentInChildren<TMP_Text>().text = optionList[1].ToString();
        _option3.GetComponentInChildren<TMP_Text>().text = optionList[2].ToString();
        _option4.GetComponentInChildren<TMP_Text>().text = optionList[3].ToString();
    }

    private void OnEnable()
    {
        _option1.onClick.AddListener(OnOption1Selected);
        _option2.onClick.AddListener(OnOption2Selected);
        _option3.onClick.AddListener(OnOption3Selected);
        _option4.onClick.AddListener(OnOption4Selected);
    }
    void OnOption1Selected()
    {
        CompareResult(optionList[0]);
    }
    void OnOption2Selected()
    {
        CompareResult(optionList[1]);
    }
    void OnOption3Selected()
    {
        CompareResult(optionList[2]);
    }
    void OnOption4Selected()
    {
        CompareResult(optionList[3]);
    }

    void CompareResult(float ans)
    {
        if (actualAnswer == ans)
        {
            //Debug.Log("Correct answer");
            _popupText.text = "Correct!";
            score += 1;
            _scoreText.text = "Scores :" + score.ToString();
            _resultText.text = ans.ToString("0.00");
            Invoke("ResetPopup", 2f);
            StartCoroutine(nameof(Timer));
        }
        else
        {
            chance -= 1;

            if (chance > 0)
            {
                // Debug.Log("Wrong answer");
                _popupText.text = "Wrong answer";
                _chanceLeftText.text = "Chance Left :" + chance.ToString();
                Invoke("ResetPopup", 1f);
            }
            else
            {
                //StartCoroutine(nameof(Timer));
                Debug.Log("GameOver");
            }
        }
    }

    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(1f);
        _resultText.text = "";
        Quiz._quizInitializer?.Invoke();

    }

    private void ResetPopup()
    {
        _popupText.text = "";
    }
}
