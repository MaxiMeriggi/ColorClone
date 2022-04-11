using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private float verticalForce = 400f;
    private float restartDelay = 5f;

    [SerializeField] private ParticleSystem playerParticles;


    [SerializeField] private Color yellowColor;
    [SerializeField] private Color purpleColor;
    [SerializeField] private Color cyanColor;
    [SerializeField] private Color pinkColor;
    private string currentColor;

    Rigidbody2D playerRigidBody;
    SpriteRenderer playerSpriteRender;

    // Start is called before the first frame update
    void Start()
    {
        playerRigidBody = GetComponent<Rigidbody2D>();
        playerSpriteRender = GetComponent<SpriteRenderer>();
        ChangeColor();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayerJump();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.tag);
        Debug.Log(currentColor);

        if (collision.gameObject.CompareTag("ColorChanger"))
        {
            ChangeColorIngame(collision);
            return;
        }

        if (collision.gameObject.CompareTag("FinishLine"))
        {
            NewLevel();
            return;
        }

        if (!collision.gameObject.CompareTag(currentColor))
        {
            GameOver();
        }
    }

    private void NewLevel()
    {
        DisablePlayer();
        InvokeNewSceneAfterDelay();
        InstantiateParticlesOnPlayer();
    }
    private void GameOver()
    {
        DisablePlayer();
        InstantiateParticlesOnPlayer();
        InvokeRestartSceneAfterDelay();
    }
    private void ChangeColorIngame(Collider2D collision)
    {
        ChangeColor();
        Destroy(collision.gameObject);
    }
    private void PlayerJump()
    {
        playerRigidBody.velocity = Vector2.zero;
        playerRigidBody.AddForce(new Vector2(0, verticalForce));
    }
    private void ChangeColor()
    {
        int randomNumber =  Random.Range(0, 4);
        switch (randomNumber)
        {
            case 0:
                playerSpriteRender.color = yellowColor;
                SetCurrentColor("Yellow");
                break;
            case 1:
                playerSpriteRender.color = purpleColor;
                SetCurrentColor("Purple");
                break;
            case 2:
                playerSpriteRender.color = cyanColor;
                SetCurrentColor("Cyan");
                break;
            case 3:
                playerSpriteRender.color = pinkColor;
                SetCurrentColor("Pink");
                break;
        }
    }
    private void SetCurrentColor(string color)
    {
        AvoidSameColor(color);
        currentColor = color;
    }
    private void AvoidSameColor(string newColor)
    {
        if(newColor == currentColor)
        {
            ChangeColor();
        }
    }
    private void RestartScene()
    {

        int activeSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(activeSceneIndex);
    }
    private void NextScene()
    {
        int activeSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(activeSceneIndex + 1);
    }
    private void DisablePlayer()
    {
            gameObject.SetActive(false);
    }
    private void InstantiateParticlesOnPlayer()
    {
        Instantiate(playerParticles, transform.position, Quaternion.identity);
    }
    private void InvokeRestartSceneAfterDelay()
    {
        Invoke(nameof(RestartScene), restartDelay);
    }
    private void InvokeNewSceneAfterDelay()
    {
        Invoke(nameof(NextScene), restartDelay);
    }
}
