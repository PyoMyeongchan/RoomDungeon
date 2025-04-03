using UnityEngine;

public class PlayerTrigger : MonoBehaviour
{
    public GameObject keyUI;
    public GameObject moveTutorial;
    public GameObject JumpTutorial;
    public GameObject AttackTutorial;
    public GameObject RunTutorial;

    private Vector2 startPlayerPos;


    private void Start()
    {
        startPlayerPos = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Key"))
        {
            keyUI.SetActive(true);
            GameManager.instance.AddKey(1);
            Destroy(collision.gameObject);

        }
        else if (collision.CompareTag("DeadZone"))
        { 
            gameObject.transform.position = startPlayerPos;
        }

        if (collision.CompareTag("Tutorial1"))
        {
            moveTutorial.SetActive(true);
        }

        if (collision.CompareTag("Tutorial2"))
        {
            JumpTutorial.SetActive(true);
        }

        if (collision.CompareTag("Tutorial3"))
        {
            AttackTutorial.SetActive(true);

        }

        if (collision.CompareTag("Tutorial4"))
        {
            RunTutorial.SetActive(true);

        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Tutorial1"))
        {
            moveTutorial.SetActive(false);
        }

        if (collision.CompareTag("Tutorial2"))
        {
            JumpTutorial.SetActive(false);

        }

        if (collision.CompareTag("Tutorial3"))
        {
            AttackTutorial.SetActive(false);

        }

        if (collision.CompareTag("Tutorial4"))
        {
            RunTutorial.SetActive(false);

        }
    }

}
