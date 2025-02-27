using UnityEngine;
using UnityEngine.UI; // Make sure to include this if you're using UI elements

public class CoinController : MonoBehaviour
{
    public float spinSpeed = 100f;
    public Text scoreText;
    private int score = 0;

    private GameManager gameManager;

    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
    }

    void Update()
    {
        transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.IncrementScore();
            Destroy(gameObject);
        }
    }
}
