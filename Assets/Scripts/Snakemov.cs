using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snakemov : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Food")
        {
            GrowSn = true;
            Vector3 partpos = new Vector3(other.transform.position.x, .5f, other.transform.position.z);
            Instantiate(particle, partpos,Quaternion.identity);
            Destroy(other.gameObject);
            
        }
        if(other.tag == "End")
        {
            Utils.ReloadLvl();
        }
    }

    public GameObject particle;
    public GameObject SnakeEnd;  //  used to expand snake 
    public List<GameObject> Foods; // use to get hold of all types of food
    Rigidbody Headrgbd;   //  save rigidbody of head 
    Rigidbody Snake;      //  get rigidbody for snake
    List<Rigidbody> SnakePart; // to get all Rigidbody component of child saved
    List<Vector3> Pos;         // to remember position to move
    Transform SnkTr;
    int dir;
    public float SnakeApart = .2f;// how much distance keep the snake parts apart 
    public float Movamt = .5f;  // To move snake by amt
    float MoveTym = .2f,Counter = 0;        // To move snake after time 
    bool MovSnak = false,GrowSn = false;
    // Start is called before the first frame update
    void Awake()
    {
        SnkTr = transform;
        Snake = GetComponent<Rigidbody>();
        InitSnakeRgbd();
        SetSnake();
        InstantFood();
        Pos = new List<Vector3>()
        {
            new Vector3(-Movamt,0,0), //left 0
            new Vector3(Movamt,0,0),// right 1
            new Vector3(0,0,Movamt), // up
            new Vector3(0,0,-Movamt) // down
        };

    }

    // Update is called once per frame
    void Update()
    {
        CallMovFrequency();
        PlayerInpt();
    }
    private void FixedUpdate()
    {
        if (MovSnak)
        {
            MovSnak = false;
            SnakeMov();
            
        }
        //PlayerInpt();
    }


    #region *** init snake rgbd ***
    // get rigidbody for all snake parts
    void InitSnakeRgbd()
    {
        SnakePart = new List<Rigidbody>();
        //SnakePart.Add(Snake.GetComponentInChildren<Rigidbody>());
        //alternatively have 3 public variables/List and drag rigidbody component to it
        
        SnakePart.Add(SnkTr.GetChild(0).GetComponent<Rigidbody>()); // get rigidbody of head
        SnakePart.Add(SnkTr.GetChild(1).GetComponent<Rigidbody>()); // mid
        SnakePart.Add(SnkTr.GetChild(2).GetComponent<Rigidbody>()); //end
        Headrgbd = SnakePart[0];
        //Debug.Log(SnakePart[0] +""+ SnakePart[1]+""+ SnakePart[2]);
    }
    #endregion

    #region *** Set snake ***
    // Set the snake at an orientation left , right , up , left
    void SetSnake()
    {
        SetDir();
        if(dir == 0)
        {
            //left snake orientation
            SnakePart[1].position = SnakePart[0].position + new Vector3(SnakeApart,0,0);//left pos so place the mid part behind head so headpos + snakeapart at x
            SnakePart[2].position = SnakePart[1].position + new Vector3(SnakeApart, 0, 0);
            //Debug.Log("Left");
        }
        if (dir == 1)
        {
            //right snake orientation
            SnakePart[1].position = SnakePart[0].position - new Vector3(SnakeApart, 0, 0);//right pos so place the mid part behind head so headpos - snakeapart at x
            SnakePart[2].position = SnakePart[1].position - new Vector3(SnakeApart, 0, 0);
            //Debug.Log("right");
        }
        if (dir == 2)
        {
            //up snake orientation
            SnakePart[1].position = SnakePart[0].position - new Vector3(0, 0, SnakeApart);//left pos so place the mid part behind head so headpos + snakeapart at z axis
            SnakePart[2].position = SnakePart[1].position - new Vector3(0, 0, SnakeApart);
            //Debug.Log("up");
        }
        if (dir == 3)
        {
            //down snake orientation
            SnakePart[1].position = SnakePart[0].position + new Vector3(0, 0, SnakeApart);//left pos so place the mid part behind head so headpos - snakeapart at z axis
            SnakePart[2].position = SnakePart[1].position + new Vector3(0, 0, SnakeApart);
            //Debug.Log("down");
        }
    }

    #endregion

    #region *** Snake mov ***
    void SnakeMov()
    {
        Vector3 Mover = Pos[dir]; // as dir defines where to move the snake
        Vector3 HeadPos = Headrgbd.position;
        Vector3 PrevPos;
        Headrgbd.position = Headrgbd.position + Mover;
        Snake.position = Snake.position + Mover; // to move the collider on head with snake 


        for(int i = 1;i<SnakePart.Count;i++)
        {
            PrevPos = SnakePart[i].position;
            SnakePart[i].position= HeadPos;
            HeadPos = PrevPos;
            
        }

        if(GrowSn == true)
        {           
            GrowSn = false;
            GameObject Ending = Instantiate(SnakeEnd, SnakePart[SnakePart.Count - 1].position, Quaternion.identity);
            Ending.transform.SetParent(transform);
            SnakePart.Add(Ending.GetComponent<Rigidbody>());
            // instantiate new food 
            InstantFood();
        }
    }
    #endregion

    #region ***player input***
    void PlayerInpt()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (dir != 1 && dir != 0)
            {// dir 1 is right and dir 0 is left so cant move right if moving left
                dir = 0;
                MoveNow();
            }

        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (dir != 0 && dir != 1)
            {
                dir = 1;
                MoveNow();
            }

        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (dir != 3 && dir != 2)
            {
                dir = 2;
                MoveNow();
            }

        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (dir != 2 && dir != 3)
            {
                dir = 3;
                MoveNow();
            }
        }
    }
    #endregion

    void CallMovFrequency()
    {
        Counter += Time.deltaTime;
        if(Counter>=MoveTym)
        {
            MovSnak = true;
            Counter = 0;
        }
    }

    void SetDir()
    {
        dir = Random.Range(0, 4);
    }

    void MoveNow()
    {
        Counter = 0;
        MovSnak = false;
        SnakeMov();
    }

    void InstantFood()
    {
        float px = Random.Range(-5.5f, 5.5f);
        float pz = Random.Range(-5.5f, 5.5f);
        int randfood = Random.Range(0, Foods.Count);
        Instantiate(Foods[randfood], new Vector3(px, .33f, pz), Quaternion.identity);
    }
}
