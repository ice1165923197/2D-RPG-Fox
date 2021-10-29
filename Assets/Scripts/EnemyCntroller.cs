using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCntroller : Enemies
{
    GameObject player;
    Transform thistf, playertf;
    StateManager stateManager;

    Vector3 leftValue, rightValue;
    public Transform leftpoint, rightpoint;
    public bool isFaceRight = false;
    float speed;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        thistf = GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        playertf = player.GetComponent<Transform>();

        stateManager = new StateManager();
        stateManager.RegionState("Idle");
        stateManager.RegionState("Chase");
        stateManager.SetDefaultState("Idle");

        leftValue = leftpoint.position;
        rightValue = rightpoint.position;

        speed = 3f;
    }

    // Update is called once per frame
    void Update()
    {
        //Chase();
        if ((player.transform.position - transform.position).magnitude < 4)
        {
            stateManager.ChangeState("Chase");
        }
        else
        {
            stateManager.ChangeState("Idle");
        }
        stateManager.KeepState(this);
        transform.localScale = new Vector3(rb.velocity.x > 0 ? -1 : 1, 1, 1);
    }

    public void Chase()
    {
        rb.velocity = new Vector2((playertf.position.x>thistf.position.x?1:-1) * speed, rb.velocity.y);
    }

    public void Idle()
    {
        if (isFaceRight)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            //target = this.transform.position.x < point1.x;
            if(transform.position.x > rightValue.x)
            {
                isFaceRight = false;
            }
        }
        else
        {
            rb.velocity = new Vector2(-1 * speed, rb.velocity.y);
            //target = this.transform.position.x <= point2.x;
            if (transform.position.x < leftValue.x)
            {
                isFaceRight = true;
            }
        }
    }
}



//abstract class State
public abstract class State
{
    public string stateType;
    public bool stateOn;

    public State()
    {
        stateOn = false;
    }
    public abstract void EnterState();
    public abstract void UpdateState(EnemyCntroller enemy);
    public abstract void ExitState();
}

//巡逻状态
public class IdleState : State
{
    public IdleState() : base()
    {
        stateType = "Idle";
    }

    public override void EnterState()
    {
        stateOn = true;
    }

    public override void UpdateState(EnemyCntroller enemy)
    {
        enemy.Idle();
    }

    public override void ExitState()
    {
        stateOn = false;
    }

}

public class ChaseState : State
{
    public ChaseState() : base()
    {
        stateType = "Chase";
    }

    public override void EnterState()
    {
        stateOn = true;
    }

    public override void UpdateState(EnemyCntroller enemy)
    {
        enemy.Chase();
    }

    public override void ExitState()
    {
        stateOn = false;
    }
}


//创建状态实例的建造类
public class StateCreator
{
    public State CreateState(string type)
    {
        switch (type)
        {
            case "Idle":
                return new IdleState();
            case "Chase":
                return new ChaseState();
        }
        return null;
    }
}

public class StateManager
{
    State currentState;
    State defaultState;
    Dictionary<string, State> regioned;//存放所有注册的状态实例

    public StateManager()
    {
        regioned = new Dictionary<string, State>();
        currentState = null;
        defaultState = null;
    }

    //选择一个状态作为默认状态
    public void SetDefaultState(string stateName)
    {
        if (!regioned.ContainsKey(stateName))
        {
            return;
        }
        defaultState = regioned[stateName];
        currentState = defaultState;
    }

    //添加需要的状态
    public void RegionState(string stateName)
    {
        if (regioned.ContainsKey(stateName))
        {
            return;
        }
        regioned.Add(stateName, new StateCreator().CreateState(stateName));
    }

    //状态切换
    public void ChangeState(string stateName)
    {
        if (!regioned.ContainsKey(stateName))
        {
            return;
        }
        currentState.ExitState();
        currentState = regioned[stateName];
        currentState.EnterState();
    }

    //状态进行
    public void KeepState(EnemyCntroller enemy)
    {
        if (currentState != null)
        {
            currentState.UpdateState(enemy);
        }
    }
}
