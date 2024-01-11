public class StoryBlock
{
    public string storyText;
    public string option1text;
    public string option2text;

    public StoryBlock option1block;
    public StoryBlock option2block;


    public StoryBlock(string storyText, string option1text, string option2text, StoryBlock option1block = null , StoryBlock option2block = null)
    {
        this.storyText = storyText;
        this.option1text = option1text;
        this.option2text = option2text;
        this.option1block = option1block;
        this.option2block = option2block;

    }
}