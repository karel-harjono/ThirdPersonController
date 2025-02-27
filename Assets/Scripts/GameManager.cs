using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float score = 0;
    [SerializeField] private TextMeshProUGUI scoreText;

    // [SerializeField] private BallController ball;
    // [SerializeField] private GameObject pinCollection;
    // [SerializeField] private Transform pinAnchor;
    [SerializeField] private InputManager inputManager;


    private void Start()
    {
        inputManager.OnResetPressed.AddListener(HandleReset);
    }

    private void HandleReset()
    {
    }

    public void IncrementScore()
    {
        score++;
        scoreText.text = $"Score: {score}";
    }
}
