using UnityEngine;

public class FadingAnim : MonoBehaviour
{
    GameStarter start;

    void Start()
    {
        start = GameObject.Find("EventSystem").GetComponent<GameStarter>();
    }

    public void Starting()
    {
        start.FinStart();
    }
}
