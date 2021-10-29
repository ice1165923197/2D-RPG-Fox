using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCntroller : Enemies
{
    GameObject player;
    Transform thistf, playertf;
    Rigidbody2D thisrb;

    StateManager statemanager;
    Vector3 point1, point2;//��point1��point2֮������Ѳ��
    public Transform leftpoint, rightpoint;
    public bool target;//true��ʾ��2ΪĿ�꣬false��ʾ��1
    float moveSpeed;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        thistf = this.GetComponent<Transform>();
        thisrb = this.GetComponent<Rigidbody2D>();
        playertf = player.GetComponent<Transform>();

        statemanager = new StateManager();
        statemanager.RegionState("Idle");
        statemanager.RegionState("Chase");
        statemanager.SetDefaultState("Idle");

        point1 = leftpoint.position;
        point2 = rightpoint.position;

        moveSpeed = 6f;
    }

    // Update is called once per frame
    void Update()
    {
        //Chase();
        if ((player.transform.position - transform.position).magnitude < 3)
        {
            statemanager.ChangeState("Chase");
        }
        else
        {
            statemanager.ChangeState("Idle");
        }
        statemanager.KeepState(this);
    }

    public void Chase()
    {
        thisrb.velocity = Vector3.Normalize(playertf.position - thistf.position) * 2f;
    }

    public void Idle()
    {
        //�������ԭ��·�������ȷ��أ�0��0��0��
        /*if (Mathf.Abs(this.transform.position.y) >= 0.05)
        {
            this.transform.position = Vector3.Lerp(this.transform.position, new Vector3(0, 0, 0),0.05f);
            //this.GetComponent<Rigidbody2D>().velocity = Vector3.Normalize(-1 * this.transform.position) * moveSpeed;
            return;
        }*/

        if (target)
        {
            this.GetComponent<Rigidbody2D>().velocity = new Vector2(moveSpeed, 0);
            //target = this.transform.position.x < point1.x;
            if(transform.position.x > point2.x)
            {
                target = false;
            }
        }
        else
        {
            this.GetComponent<Rigidbody2D>().velocity = new Vector2(-1 * moveSpeed, 0);
            //target = this.transform.position.x <= point2.x;
            if (transform.position.x < point1.x)
            {
                target = true;
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

//Ѳ��״̬
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


//����״̬ʵ���Ľ�����
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
    Dictionary<string, State> regioned;//�������ע���״̬ʵ��

    public StateManager()
    {
        regioned = new Dictionary<string, State>();
        currentState = null;
        defaultState = null;
    }

    //ѡ��һ��״̬��ΪĬ��״̬
    public void SetDefaultState(string stateName)
    {
        if (!regioned.ContainsKey(stateName))
        {
            return;
        }
        defaultState = regioned[stateName];
        currentState = defaultState;
    }

    //�����Ҫ��״̬
    public void RegionState(string stateName)
    {
        if (regioned.ContainsKey(stateName))
        {
            return;
        }
        regioned.Add(stateName, new StateCreator().CreateState(stateName));
    }

    //״̬�л�
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

    //״̬����
    public void KeepState(EnemyCntroller enemy)
    {
        if (currentState != null)
        {
            currentState.UpdateState(enemy);
        }
    }
}
