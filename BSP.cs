using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BSP : MonoBehaviour
{
    [SerializeField] GameObject material;
    [SerializeField] GameObject spawn;
    bool[,] Maze;
    public class Node{
        public int x1;
        public int x2;
        public int z1;
        public int z2;
        public Node left;
        public Node right;

        public Node(int x1,int x2,int z1,int z2)
        {
            this.x1 = x1;
            this.x2 = x2;
            this.z1 = z1;
            this.z2 = z2;
            this.left = null;
            this.right = null;
        }

        public Node()
        {
            this.left = null;
            this.right = null;
        }
    }
    Node root;

    public void MakeMaze(Node current, int iteration)
    {
        current.left = new Node();
        current.right = new Node();
        float rand= Random.Range(30,70)/100f;
        if (iteration % 2 == 1)
        {
            current.left.x2 = current.x1+Mathf.RoundToInt((current.x2-current.x1)*rand);
            current.left.x1 = current.x1;
            current.left.z1 = current.z1;
            current.left.z2 = current.z2;

            current.right.x1 = current.left.x2+1;
            current.right.x2 = current.x2;
            current.right.z1 = current.z1;
            current.right.z2 = current.z2;
            
        }
        else if(iteration % 2 == 0)
        {
            current.left.z2 = current.z1 + Mathf.RoundToInt((current.z2 - current.z1) * rand);
            current.left.z1 = current.z1;
            current.left.x1 = current.x1;
            current.left.x2 = current.x2;


            current.right.z1 = current.left.z2 + 1;
            current.right.z2 = current.z2;
            current.right.x1 = current.x1;
            current.right.x2 = current.x2;        
        }

        if (iteration > 0)
        {
            MakeMaze(current.right,iteration-1);
            MakeMaze(current.left,iteration-1);
        }
    }
    
    public void MakePath(Node curr)
    {
        if(curr.left == null|| curr.right == null)
        {
            return;
        }
        Vector3 leftMid = getMid(curr.left);
        Vector3 rightMid = getMid(curr.right);
        for(int x = Mathf.RoundToInt(leftMid.x); x <= rightMid.x; x++)
        {
           for(int z = Mathf.RoundToInt(leftMid.z); z <= rightMid.z; z++)
            {
                Maze[x,z] = false;
            }
        }
        MakePath(curr.right);
        MakePath(curr.left);
    }

    public void MakeExit(Node curr)
    {
        Node rightMost = RightMost(curr);

        for (int j = (int)getMid(rightMost).x; j < 50; j++)
        {
            Maze[j, (int)getMid(rightMost).z] = false;
        }
    }

    public Node RightMost(Node curr)
    {
        while(curr.right != null)
            curr = curr.right;
        return curr;
    }

    public Node LeftMost(Node curr)
    {
        while (curr.left != null)
            curr = curr.left;
        return curr;
    }

    public void Build()
    {
        for(int x = 0; x < 50; x++)
        {
            for(int z = 0; z < 50 ; z++)
            {
                if (Maze[x, z]==true)
                {
                    Vector3 coordinate = new Vector3(x*4 + 50, 0, z*4 + 300);
                    Instantiate(material, coordinate, Quaternion.identity);
                }
            }
        }
    }

    public Vector3 getMid(Node curr)
    {
        Vector3 mid = new Vector3(curr.x1+((curr.x2 - curr.x1) / 2), 0, curr.z1+ ((curr.z2 - curr.z1) / 2));
        return mid;
    }

    public void MakeSpawn(Node curr)
    {

        Node leftMost = LeftMost(curr);
        spawn.transform.position = new Vector3(getMid(leftMost).x * 4 + 50, 0 , getMid(leftMost).z * 4 + 300);
    }

    void Start()
    {
        Maze = new bool[50,50];
        for(int i = 0;i < 50;i++)
        {
            for(int j = 0; j < 50; j++)
            {
                Maze[i,j] = true;
            }
        }
        root = new Node(1,49,1,49);
        MakeMaze(root,4);
        MakePath(root);
        MakeExit(root);
        MakeSpawn(root);
        Build();
    }
}
