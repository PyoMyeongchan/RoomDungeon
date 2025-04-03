using UnityEngine;


public enum GroundType
{ 
    UpGround,
    RightGround,
    LeftGround

}

public class GroundManager : MonoBehaviour
{
    public float speed = 2.0f;
    public float maxDistance = 3.0f;
    private Vector2 startPos;
    private int direction = 1;

    public GroundType currentGroundType;

    void Start()
    {
        startPos = transform.position;


        if (currentGroundType == GroundType.LeftGround)
        {
            direction = -1;
        }

    }

    void Update()
    {
        if (currentGroundType == GroundType.RightGround)
        {
            if (transform.position.x > startPos.x + maxDistance)
            {
                direction = -1;
            }
            else if (transform.position.x < startPos.x - maxDistance)
            {
                direction = 1;
            }
            transform.position += new Vector3(speed * direction * Time.deltaTime, 0, 0);

        }
        else if (currentGroundType == GroundType.LeftGround)
        {
            if (transform.position.x > startPos.x + maxDistance)
            {
                direction = -1;
            }
            else if (transform.position.x < startPos.x - maxDistance)
            {
                direction = 1;
            }
            transform.position += new Vector3(speed * direction * Time.deltaTime, 0, 0);
        }

        else if (currentGroundType == GroundType.UpGround)
        {
            if (transform.position.y > startPos.y + maxDistance)
            {
                direction = -1;
            }
            else if (transform.position.y < startPos.y - maxDistance)
            {
                direction = 1;
            }

            transform.position += new Vector3(0, speed * direction * Time.deltaTime, 0);
        }
                

    }


    // 플레이어가 발판의 자식이됨으로써 플레이어가 발판과 위치를 같이 이동할 수 있게 해준다!
    // 발판 위에서 프로젝트를 끄면 오류라고하니 크게 걱정말고 발판에 내려와서 꺼라
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.transform.SetParent(gameObject.transform);
        }

        if (collision.tag == "Monster")
        {
            collision.transform.SetParent(gameObject.transform);
        }

    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player" && collision.transform.parent.gameObject.activeInHierarchy)
        {
            collision.transform.SetParent(null);
        }

        if (collision.tag == "Monster" && collision.transform.parent.gameObject.activeInHierarchy)
        {
            collision.transform.SetParent(null);
        }
    }

}
