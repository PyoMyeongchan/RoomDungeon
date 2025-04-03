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


    // �÷��̾ ������ �ڽ��̵����ν� �÷��̾ ���ǰ� ��ġ�� ���� �̵��� �� �ְ� ���ش�!
    // ���� ������ ������Ʈ�� ���� ��������ϴ� ũ�� �������� ���ǿ� �����ͼ� ����
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
