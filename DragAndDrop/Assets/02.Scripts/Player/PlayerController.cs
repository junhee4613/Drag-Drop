using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : Unit
{
    
    public Rigidbody2D rb;
    public CircleCollider2D cc;
    public float speed;
    public int player_life = 3;
    public float speed_down;
    #region Ŭ���� �ȿ��� �ذ��Ұ͵�
    Vector2 mouse_pos;
    Vector2 player_pos; 
    Vector2 mouse_button_up_pos;
    public float player_move_pos;
    public Vector2 player_move_dir;
    bool player_sence = false;
    RaycastHit2D ray_hit;
    Player_statu player_statu = Player_statu.IDLE;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       if (Input.GetMouseButton(0)) //���� 2�� �̰� ���� ���߿� ��ư ������ �� ����Ǵ� �Լ��� ������ ��
        {
            mouse_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Input.GetMouseButtonDown(0))    //���� 1��
            {
                Mouse_button_down();
            }
            
        }
        else if (Input.GetMouseButtonUp(0) && player_sence)     //���� 3��
        {
            Mouse_button_up();
        }

        Player_Statu();
    }
    public void Mouse_button_down()
    {
        ray_hit = Physics2D.Raycast(mouse_pos, Vector2.zero, 0, 1 << 6);
        if (ray_hit)
        {
            player_statu = Player_statu.DRAG;
            player_pos = ray_hit.collider.gameObject.transform.position;
            player_sence = true;
        }
        else
        {
            player_sence = false;
        }
    }
    public void Mouse_button_up()
    {
        Debug.Log("��ư ��");
        player_move_pos = Mathf.Abs(player_pos.magnitude - mouse_pos.magnitude); // - ���콺�� �� ��ġ�� ũ�⸦ �����ͼ� �׸�ŭ ����
        player_move_dir = new Vector2(player_pos.x - mouse_pos.x, player_pos.y - mouse_pos.y).normalized;
        rb.AddForce(player_move_dir * player_move_pos * speed, ForceMode2D.Impulse);
        player_statu = Player_statu.RUN;

    }

    public override void Hit(float damage)
    {
        if(player_statu != Player_statu.HIT)
        {
            player_life -= (int)damage;
            player_statu = Player_statu.HIT;
        }
        
        //���⿡ ���ʵ��� �ǰ� ���� ���·� ����� �׿� ���� ó���� �ϸ� ��
    }
    public void Player_Statu()
    {
        switch (player_statu)
        {
            case Player_statu.IDLE:
                break;
            case Player_statu.DRAG:
                break;
            case Player_statu.RUN:
                Run();
                break;
            case Player_statu.HIT:
                break;
            case Player_statu.SKILLS:
                break;
            default:
                break;
        }
    }

    public void Run()
    {
        rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x - Time.deltaTime * speed_down, 0, rb.velocity.x), Mathf.Clamp(rb.velocity.y - Time.deltaTime * speed_down, 0, rb.velocity.y));
    }
}
//�÷��̾ ���ư� ũ��� ���� ���ϱ� nomalize�� magnitude�� ���
//��ǥ ���� �������� �� Ŭ���� ��ǥ�� ������ ���� �÷��̾� ��ǥ�� ������ ���� ����
//�÷��̾� �巡�� ǥ�ø� ��� ���� ����
//�巡�� ��¡�� �󸶳� �ƴ��� ǥ���� ���
//�׿� ȿ�������� ��� ���� ����
//skwnddp wpwkr
