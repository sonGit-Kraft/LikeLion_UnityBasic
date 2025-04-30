using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Node
{
    public Node(bool _isWall, int _x, int _y) { isWall = _isWall; x = _x; y = _y; }

    public bool isWall;
    public Node ParentNode;

    // G : 시작으로부터 이동했던 거리, H : |가로|+|세로| 장애물 무시하여 목표까지의 거리, F : G + H
    public int x, y, G, H;
    public int F { get { return G + H; } }
}

public class Bir : MonoBehaviour
{
    bool damaged = false;
    SpriteRenderer mst;
    public float flashSpeed = 1f;
    Color origin;

    //public GameObject fire;
    public Transform player;
    float delayTime = 0;

    Animator monAny;
    float angle;
    Vector3 dir;


    //체력
    public Image Monhp;

    float CurrentHp = 50;
    float MonMaxHp = 50;

    //Astar
    public Vector2Int bottomLeft, topRight, startPos, targetPos;
    public List<Node> FinalNodeList;
    public bool allowDiagonal, dontCrossCorner;
     Transform PlayerTR;

    int sizeX, sizeY;
    Node[,] NodeArray;
    Node StartNode, TargetNode, CurNode;
    List<Node> OpenList, ClosedList;

   


    public void PathFinding()
    {
        // NodeArray의 크기 정해주고, isWall, x, y 대입
        sizeX = topRight.x - bottomLeft.x + 1;
        sizeY = topRight.y - bottomLeft.y + 1;
        NodeArray = new Node[sizeX, sizeY];

        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                bool isWall = false;
                foreach (Collider2D col in Physics2D.OverlapCircleAll(new Vector2(i + bottomLeft.x, j + bottomLeft.y), 0.4f))
                    if (col.gameObject.layer == LayerMask.NameToLayer("Wall")) isWall = true;

                NodeArray[i, j] = new Node(isWall, i + bottomLeft.x, j + bottomLeft.y);
            }
        }


        // 시작과 끝 노드, 열린리스트와 닫힌리스트, 마지막리스트 초기화
        StartNode = NodeArray[startPos.x - bottomLeft.x, startPos.y - bottomLeft.y];
        TargetNode = NodeArray[targetPos.x - bottomLeft.x, targetPos.y - bottomLeft.y];

        OpenList = new List<Node>() { StartNode };
        ClosedList = new List<Node>();
        FinalNodeList = new List<Node>();


        while (OpenList.Count > 0)
        {
            // 열린리스트 중 가장 F가 작고 F가 같다면 H가 작은 걸 현재노드로 하고 열린리스트에서 닫힌리스트로 옮기기
            CurNode = OpenList[0];
            for (int i = 1; i < OpenList.Count; i++)
                if (OpenList[i].F <= CurNode.F && OpenList[i].H < CurNode.H) CurNode = OpenList[i];

            OpenList.Remove(CurNode);
            ClosedList.Add(CurNode);


            // 마지막
            if (CurNode == TargetNode)
            {
                Node TargetCurNode = TargetNode;
                while (TargetCurNode != StartNode)
                {
                    FinalNodeList.Add(TargetCurNode);
                    TargetCurNode = TargetCurNode.ParentNode;
                }
                FinalNodeList.Add(StartNode);
                FinalNodeList.Reverse();

                return;
            }


            // ↗↖↙↘
            if (allowDiagonal)
            {
                OpenListAdd(CurNode.x + 1, CurNode.y + 1);
                OpenListAdd(CurNode.x - 1, CurNode.y + 1);
                OpenListAdd(CurNode.x - 1, CurNode.y - 1);
                OpenListAdd(CurNode.x + 1, CurNode.y - 1);


            }

            // ↑ → ↓ ←
            OpenListAdd(CurNode.x, CurNode.y + 1);
            OpenListAdd(CurNode.x + 1, CurNode.y);
            OpenListAdd(CurNode.x, CurNode.y - 1);
            OpenListAdd(CurNode.x - 1, CurNode.y);
        }
    }

    void OpenListAdd(int checkX, int checkY)
    {
        // 상하좌우 범위를 벗어나지 않고, 벽이 아니면서, 닫힌리스트에 없다면
        if (checkX >= bottomLeft.x && checkX < topRight.x + 1 && checkY >= bottomLeft.x && checkY < topRight.y + 1 && !NodeArray[checkX - bottomLeft.x, checkY - bottomLeft.y].isWall && !ClosedList.Contains(NodeArray[checkX - bottomLeft.x, checkY - bottomLeft.y]))
        {
            // 대각선 허용시, 벽 사이로 통과 안됨
            if (allowDiagonal) if (NodeArray[CurNode.x - bottomLeft.x, checkY - bottomLeft.y].isWall && NodeArray[checkX - bottomLeft.x, CurNode.y - bottomLeft.y].isWall) return;

            // 코너를 가로질러 가지 않을시, 이동 중에 수직수평 장애물이 있으면 안됨
            if (dontCrossCorner) if (NodeArray[CurNode.x - bottomLeft.x, checkY - bottomLeft.y].isWall || NodeArray[checkX - bottomLeft.x, CurNode.y - bottomLeft.y].isWall) return;


            // 이웃노드에 넣고, 직선은 10, 대각선은 14비용
            Node NeighborNode = NodeArray[checkX - bottomLeft.x, checkY - bottomLeft.y];
            int MoveCost = CurNode.G + (CurNode.x - checkX == 0 || CurNode.y - checkY == 0 ? 10 : 14);


            // 이동비용이 이웃노드G보다 작거나 또는 열린리스트에 이웃노드가 없다면 G, H, ParentNode를 설정 후 열린리스트에 추가
            if (MoveCost < NeighborNode.G || !OpenList.Contains(NeighborNode))
            {
                NeighborNode.G = MoveCost;
                NeighborNode.H = (Mathf.Abs(NeighborNode.x - TargetNode.x) + Mathf.Abs(NeighborNode.y - TargetNode.y)) * 10;
                NeighborNode.ParentNode = CurNode;

                OpenList.Add(NeighborNode);
            }
        }
    }

    void OnDrawGizmos()
    {
        if (FinalNodeList.Count != 0) for (int i = 0; i < FinalNodeList.Count - 1; i++)
                Gizmos.DrawLine(new Vector2(FinalNodeList[i].x, FinalNodeList[i].y), new Vector2(FinalNodeList[i + 1].x, FinalNodeList[i + 1].y));
    }


    private void Start()
    {
        monAny = GetComponent<Animator>();
        mst = GetComponent<SpriteRenderer>();
        origin = mst.color;
        delayTime = Time.deltaTime;
        PlayerTR = player;
    }
    private void Update()
    {
        if (damaged)
        {
            mst.color = Color.red;
        }
        else
        {
            mst.color = Color.Lerp(mst.color, origin
                , flashSpeed * Time.deltaTime);
        }
        damaged = false;

        float dis = Vector3.Distance(player.position, transform.position);

        //거리 5이상 벗어나면 파이어 안쏨
        if (dis < 7f)
        {
            if (Time.time >= delayTime+1 )
            {
                delayTime = Time.time;
                //GameObject gFire = Instantiate(fire, transform.position, Quaternion.identity);
                //gFire.GetComponent<fire>().target = player;

                //  bottomLeft, topRight, startPos, targetPos 정해준 다음 길찾기
                startPos = Vector2Int.RoundToInt(transform.position);
                targetPos = Vector2Int.RoundToInt(player.position);

                bottomLeft = new Vector2Int(Mathf.Min(startPos.x, targetPos.x) - 5, Mathf.Min(startPos.y, targetPos.y) - 5);
                topRight = new Vector2Int(Mathf.Max(startPos.x, targetPos.x) + 5, Mathf.Max(startPos.y, targetPos.y) + 5);

                PathFinding();
            }
           
        }

        AngleAnimation();


    

        if (FinalNodeList.Count != 0)
        {
            //길찾기 성공시
            if (FinalNodeList.Count == 1) return;

            Vector2 FinalNodePos = new Vector2(FinalNodeList[1].x, FinalNodeList[1].y);
            transform.position = Vector2.MoveTowards(transform.position, FinalNodePos, 0.03f);

            if ((Vector2)transform.position == FinalNodePos) FinalNodeList.RemoveAt(0);
        }


    }

    void AngleAnimation()
    {
        //각도에따른 애니메이션 구현
        //바라보는 방향벡터구하기
        dir = player.position - transform.position;

        //바라보는 각도구하기
        angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;


        //Debug.Log("각도 : " + angle);

        // 45     135     -45    -135
        if (angle > -45 && angle <= 45)
        {
            //x
            monAny.SetFloat("x", 1);
            monAny.SetFloat("y", 0);
        }
        else if (angle > 45 && angle <= 135)
        {
            //y
            monAny.SetFloat("x", 0);
            monAny.SetFloat("y", 1);
        }
        else if (angle > 135 && angle <= 180 || angle <= -135)
        {
            //-x
            monAny.SetFloat("x", -1);
            monAny.SetFloat("y", 0);
        }
        else if (angle > -135 && angle <= -45)
        {
            //-y
            monAny.SetFloat("x", 0);
            monAny.SetFloat("y", -1);
        }
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {

            damaged = true;
            //SpriteRenderer msr = collision.gameObject.GetComponent<SpriteRenderer>();
            //msr.color = Color.red;
        }







    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("트리거충돌");
        if (collision.tag == "PlayerSkill")
        {
            damaged = true;
            float Damage = 10;
            if (CurrentHp > 0)
            {
                CurrentHp -= Damage;
                Monhp.fillAmount = CurrentHp / MonMaxHp;
            }
            else
            {
                ItemDatabase.instance.ItemDrop(transform.position);
                Destroy(gameObject);
            }


           // Destroy(collision.gameObject);
        }
    }

}
