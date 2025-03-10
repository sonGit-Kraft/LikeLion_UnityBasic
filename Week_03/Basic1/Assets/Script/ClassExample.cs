using UnityEngine;

public class Player
{
    public string name;
    public int score;

    public Player(string name, int score)
    {
        this.name = name;
        this.score = score;
    }

    public void ShowInfo()
    {
        Debug.Log("Player: " + name + ", Score: " + score);
    }

}
public class ClassExample : MonoBehaviour
{
    void Start()
    {
        Player player = new Player("Hero", 10);
        player.ShowInfo();
    }

    void Update()
    {
        
    }
}