using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    Transform player;
    [SerializeField] LayerMask playerMask;
    [SerializeField] Transform spawnBullet;
    [SerializeField] GameObject bullet;
    [SerializeField] float fireRate;
    public float maxSpreadAngle;
    public float timeTillMaxSpread;
    public float cooldownSpeed;
    private int health;
    [SerializeField] Transform playerPos;
    [Header("HealthBar")]
    public HealthBar healthBar;
    [Header("Range")]
    public int attackRange;
    public int chaseRange;
    private Animator anim;

    [Header("Astar")]
    private float aStarTimer = 2;
    [SerializeField] float chaseSpeed = 1;
    [SerializeField] float aStarDistance = 3;
    List<Vector2> aStarPath;
    public GameObject win;
    void Start()
    {
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        health = 2000;
    }
    public GameObject otherMenu;
    void Update()
    {
        Vector3 loc = new Vector3(player.position.x, this.transform.position.y, player.position.z);
        //healthBar.transform.LookAt(loc);

        if (Physics.CheckSphere(transform.position, attackRange, playerMask))
        {
            anim.SetFloat("Blend", 0);
            lookPlayer();

            cooldownSpeed += Time.deltaTime * 60f;
            if (cooldownSpeed >= fireRate)
            {
                Shoot();
                cooldownSpeed = 0;
            }
        }
        else if (Physics.CheckSphere(transform.position, chaseRange, playerMask))
        {
            lookPlayer();
            Chase();
        }

        if (health <= 0)
        {
            Destroy(this.gameObject);
            otherMenu.SetActive(false);
            CursorHide.Lock();
            win.SetActive(true);
        }
    }

    public void Chase()
    {
        anim.SetFloat("Blend", 1);

        if (aStarTimer == 2)
        {
            aStarPath = SearchAStar(transform, player);
        }
        GoTo(aStarPath);

        aStarTimer -= 1 * Time.deltaTime;
        if (aStarTimer <= 0)
            aStarTimer = 2;
    }

    private void GoTo(List<Vector2> aStarPath)
    {
        Vector3 nextPoint;
        if (aStarPath.Count != 0)
        {
            float y = Terrain.activeTerrain.SampleHeight(transform.position);
            nextPoint = new Vector3(aStarPath[0].x, y, aStarPath[0].y);
            var lookTo = nextPoint - transform.position;
            lookTo.y = 0;
            var rotation = Quaternion.LookRotation(lookTo);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 10);


            transform.position = Vector3.Lerp(transform.position, nextPoint, chaseSpeed * Time.deltaTime);
            if (aStarPath[1] != null)
            {
                if (GetDistance(this.transform.position.x, this.transform.position.z, aStarPath[1].x, aStarPath[1].y) < aStarDistance)
                {
                    aStarPath.RemoveAt(0);
                }
            }
        }
    }

    public void Shoot()
    {
        spawnBullet.LookAt(player);
        GameObject tempBullet = Instantiate(bullet, spawnBullet.transform.position, spawnBullet.rotation);
        tempBullet.GetComponent<EnemyBullet>().Push();
    }

    public void Hit()
    {
        health -= 35;
        healthBar.SetHealth(health);
    }

    public void pistolHit()
    {
        health -= 70;
        healthBar.SetHealth(health);
    }

    public void lookPlayer()
    {
        Vector3 loc = new Vector3(player.position.x, this.transform.position.y, player.position.z);
        transform.LookAt(loc);
    }


    static float gridSize = 1f;
    int[] tempX = { 1, 0, -1, 0, 1, 1, -1, -1 };
    int[] tempZ = { 1, 0, -1, 0, 1, 1, -1, -1 };

    int lastX;
    int lastZ;
    public Transform target1, target2;
    Node[,] nodeList;
    public bool isVillageSoldier;

    public List<Vector2> SearchAStar(Transform target1, Transform target2)
    {
        this.target1 = target1;
        this.target2 = target2;

        List<Vector2> pathFinded = Pathfind((int)target1.position.x, (int)target1.position.z, (int)target2.position.x, (int)target2.position.z);
        pathFinded.Reverse();
        return pathFinded;
    }

    public float GetHeuristic(float x, float z)
    {
        float targetX = target2.position.x;
        float targetZ = target2.position.z;

        x += (gridSize / 2);
        z += (gridSize / 2);

        float value = Mathf.Sqrt(Mathf.Pow((x - targetX), 2) + Mathf.Pow((z - targetZ), 2));
        return value;
    }
    public class Node
    {
        public int nextX;
        public int nextZ;
        public int prevX;
        public int prevZ;
        public int x;
        public int z;
        public bool visited;
        public float hx;
        Transform target;
        public Node(int x, int z, Transform target)
        {
            this.target = target;
            this.x = x;
            this.z = z;
            this.visited = false;
            this.hx = GetHeuristic(x, z);
            this.prevX = -99;
            this.prevZ = -99;

        }
        public float GetHeuristic(float x, float z)
        {
            float targetX = target.position.x;
            float targetZ = target.position.z;

            x += (gridSize / 2);
            z += (gridSize / 2);

            float value = Mathf.Sqrt(Mathf.Pow((x - targetX), 2) + Mathf.Pow((z - targetZ), 2));
            return value;
        }
    }

    private Node GetNode(int x, int z)
    {
        if (nodeList[x, z] == null)
        {
            nodeList[x, z] = new Node(x, z, target2);
            return nodeList[x, z];
        }
        else
            return nodeList[x, z];
    }

    public bool IsValid(int x, int z, int prevX, int prevZ)
    {
        bool isAreaValid = CalculateAreaScript.validatedArea[x, z];
        bool isNotVisited = GetNode(x, z).visited == false;
        bool isNotExceedMap = x >= 0 && z >= 0 && x <= 1000 & z <= 1000;

        if (isAreaValid && isNotVisited && isNotExceedMap)
            return true;
        else
            return false;
    }

    public float GetDistance(float x, float z, float targetX, float targetZ)
    {
        //Euclidean Distance
        float distance = Mathf.Sqrt((Mathf.Pow((x - targetX), 2f) + Mathf.Pow((z - targetZ), 2f)));
        return distance;
    }

    public bool CheckDiagonal(int i, int j)
    {
        if (i != 0 && j != 0)
            return true;
        else
            return false;
    }
    public List<Vector2> Pathfind(int x, int z, int targetX, int targetZ)
    {
        nodeList = new Node[900, 900];
        List<Vector2> pathList = new List<Vector2>();
        bool foundTarget = false;

        GetNode(x, z).visited = true;
        List<Node> nodeQueue = new List<Node>();
        nodeQueue.Add(new Node(x, z, target2));

        while (nodeQueue.Count > 0 && !foundTarget)
        {
            float searchHeuristic = Mathf.Infinity;
            int idxLowestHeuristic = -1;
            for (int i = 0; i < nodeQueue.Count; i++)
            {
                if (nodeQueue[i].hx < searchHeuristic)
                {
                    searchHeuristic = nodeQueue[i].hx;
                    idxLowestHeuristic = i;
                }
            }

            Node nodeCheck = nodeQueue[idxLowestHeuristic];
            nodeQueue.RemoveAt(idxLowestHeuristic);

            if (GetDistance(nodeCheck.x, nodeCheck.z, targetX, targetZ) <= 1)
            {
                foundTarget = true;
                lastX = nodeCheck.x;
                lastZ = nodeCheck.z;
                break;
            }

            foreach (int i in tempX)
            {
                foreach (int j in tempZ)
                {
                    if (i == 0 && j == 0)
                        continue;
                    int nextX = nodeCheck.x + i;
                    int nextZ = nodeCheck.z + j;

                    if (IsValid(nextX, nextZ, nodeCheck.x, nodeCheck.z))
                    {
                        if (CheckDiagonal(i, j))
                        {
                            if (!IsValid(nextX - i, nextZ, nodeCheck.x, nodeCheck.z) && IsValid(nextX, nextZ - j, nodeCheck.x, nodeCheck.z))
                                continue;
                        }
                        GetNode(nextX, nextZ).visited = true;
                        GetNode(nextX, nextZ).prevX = nodeCheck.x;
                        GetNode(nextX, nextZ).prevZ = nodeCheck.z;
                        Node nextNode = new Node(nextX, nextZ, target2);
                        nodeQueue.Add(nextNode);
                    }
                    else
                    {
                    }
                }
            }
        }

        if (foundTarget)
        {
            int xCurr = lastX;
            int zCurr = lastZ;

            while (GetNode(xCurr, zCurr).prevX > 0)
            {
                int tempX = xCurr;
                xCurr = GetNode(xCurr, zCurr).prevX;
                zCurr = GetNode(tempX, zCurr).prevZ;

                Vector2 curr = new Vector2(xCurr, zCurr);
                pathList.Add(curr);
            }
        }

        return pathList;
    }
}
