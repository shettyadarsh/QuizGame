using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _storyText;
    [SerializeField] private Button _button1;
    [SerializeField] private Button _button2;

    StoryBlock currentBlock;
    //GameManager gm = new GameManager();

    //static StoryBlock block6 = new StoryBlock("10+1 = _", "11", "3");
    //static StoryBlock block5 = new StoryBlock("12+1 = _", "2", "13");
    //static StoryBlock block4 = new StoryBlock("5+1 = _", "6", "3");
    //static StoryBlock block3 = new StoryBlock("3+1 = _", "4", "3", block5, block6);
    //static StoryBlock block2 = new StoryBlock("2+1 = _", "3", "4", block3, block4);
    //StoryBlock block1 = new StoryBlock("1+1 = _", "2", "3", block2, block3);

    public StoryBlock block1;
    StoryBlock block2;
    StoryBlock block3;
    StoryBlock block4;
    StoryBlock block5;
    StoryBlock block6;

    public GameManager()
    {
        block6 = new StoryBlock("10+1 = _", "11", "3");
        block5 = new StoryBlock("12+1 = _", "2", "13");
        block4 = new StoryBlock("5+1 = _", "6", "3");
        block3 = new StoryBlock("3+1 = _", "4", "3", block5, block6);
        block2 = new StoryBlock("2+1 = _", "3", "4", block3, block4);
        block1 = new StoryBlock("1+1 = _", "2", "3", block2, block3);
    }

    // Start is called before the first frame update
    void Start()
    {
        displayBlock(block1);
    }

    void displayBlock(StoryBlock block)
    {
        _storyText.text = block.storyText;

        _button1.GetComponentInChildren<TextMeshProUGUI>().text = block.option1text;
        _button2.GetComponentInChildren<TextMeshProUGUI>().text = block.option2text;

        currentBlock = block;
    }

    public void Button1Clicked()
    {
        displayBlock(currentBlock.option1block);
    }

    public void Button2Clicked()
    {
        displayBlock(currentBlock.option2block);
    }

}
