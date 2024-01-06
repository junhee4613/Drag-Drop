using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : Unit
{
    //���� ������ ���׸���� �ϴϱ� ƨ������ ������ �̻���
    //�߻� ������ y������ �س��� ���� �ε����� ���ƿ� ������ ���� �׿� �´� �������� ���ư��� �ؾ�(�Ի簢�� �ݻ簢�� �����ؾߵ�) �̷��� �ؼ� velocity�� ���̸� ���� ������
    public Rigidbody2D rb;
    public CircleCollider2D cc;
    [Header("���� ���� �ʱ� �ӵ�")]
    public float shoot_speed;
    [Header("�巡�� �� ���� �ӵ�")]
    public float slow_speed;
    public int player_life = 3;
    public float speed_down;
    public LayerMask collision_target;
    public GameObject shoot_dir_image;
    public Transform arrow_rotation_base;
    #region Ŭ���� �ȿ��� �ذ��Ұ͵�
    Vector2 mouse_pos;
    Vector2 player_pos;
    Vector2 player_move_dir;
    bool wall_collider;
    float speed;
    float drag_dis;
    float player_rotation_z;
    public Vector2 drag_before_speed;
    public Collider2D[] targets;
    RaycastHit2D ray_hit;
    public Player_statu player_statu = Player_statu.IDLE;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Player_Statu();
        targets = Physics2D.OverlapCircleAll(transform.position, cc.radius, collision_target);
        Collider_target();
       if (Input.GetMouseButton(0)) //���� 2�� �̰� ���� ���߿� ��ư ������ �� ����Ǵ� �Լ��� ������ ��
        {
            mouse_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Input.GetMouseButtonDown(0))    //���� 1��
            {
                Mouse_button_down();
            }
        }
    }
    public void Mouse_button_down()
    {
        ray_hit = Physics2D.Raycast(mouse_pos, Vector2.zero, 0, 1 << 6);
        if (ray_hit)
        {
            player_statu = Player_statu.DRAG;
            player_pos = ray_hit.collider.gameObject.transform.position;
            drag_before_speed = rb.velocity;
        }
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
                Drag();
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
    public void Drag()
    {
        speed = slow_speed;
        rb.drag = 0;
        if(rb.velocity != Vector2.zero)
        {
            rb.velocity = drag_before_speed * speed;
            //���� �� ���¿��� ���� �ε����� �� ���⿡ �Ի簢�� �ݻ簢�� ���� �ִ� ������ �ؼ� ó��
        }
        shoot_dir_image.SetActive(true);
        drag_dis = Mathf.Abs(player_pos.magnitude - mouse_pos.magnitude); // - ���콺�� �� ��ġ�� ũ�⸦ �����ͼ� �׸�ŭ ����
        player_move_dir = new Vector3(player_pos.x - mouse_pos.x, player_pos.y - mouse_pos.y, 0).normalized;
        player_rotation_z = Mathf.Atan2(player_move_dir.y, player_move_dir.x) * Mathf.Rad2Deg;
        arrow_rotation_base.rotation = Quaternion.Euler(0, 0, player_rotation_z - 90);
        if (Input.GetMouseButtonUp(0))     //���� 3��
        {
            transform.rotation = arrow_rotation_base.rotation;
            rb.velocity = Vector2.zero;
            rb.drag = 1;
            speed = shoot_speed;
            Drag_shoot();
        }
    }
    public void Drag_shoot()
    {
        rb.AddForce(transform.up * drag_dis * speed, ForceMode2D.Impulse);
        player_statu = Player_statu.RUN;
    }
    public void Run()
    {
        /*if(Mathf.Abs(transform.rotation.eulerAngles.z) >= 90)
        {
            player_velocity_dir_y = -1;
            min_speed_y = -drag_dis * shoot_speed;
            max_speed_y = 0;
        }
        else
        {
            player_velocity_dir_y = 1;
            min_speed_y = 0;
            max_speed_y = drag_dis * shoot_speed;
        }

        if (transform.rotation.eulerAngles.z >= 0)
        {
            player_velocity_dir_x = -1;
            min_speed_x = -drag_dis * shoot_speed;
            max_speed_x = 0;
        }
        else
        {
            player_velocity_dir_x = 1;
            min_speed_x = 0;
            max_speed_x = drag_dis * shoot_speed;
        }
        shoot_dir_image.SetActive(false);
        if (rb.velocity.y <= 0)
        {
            Debug.Log(rb.velocity.y);
        }
        else if (rb.velocity.x <= 0)
        {
            Debug.Log(rb.velocity.x);
        }
        rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x - Time.deltaTime * speed_down * player_velocity_dir_x, -min_speed_x, max_speed_x), 
            Mathf.Clamp(rb.velocity.y - Time.deltaTime * speed_down * player_velocity_dir_y, -max_speed_x, min_speed_x));*/
        //if(�ӵ��� 0�� �ȴٸ� idle ���·� �ٲ��ִ� ����)
    }
    public void Collider_target()
    {
        for (int i = 0; i < targets.Length; i++)
        {
            switch (targets[i].gameObject.tag)
            {
                case "Wall":
                    Wall();
                    //���� ���� ó��
                    break;
                case "Obstacle":
                    //��ֹ��� ���� ó��
                    break;
                case "Special_zone":
                    //��Ʈ������ �ƴ� ���� ���� �� ����
                    break;
                default:
                    break;
            }
        }
        wall_collider = false;
        
    }
    public void Wall()
    {

        if (!wall_collider)
        {
            wall_collider = true;
            //�Ի簢�� �ݻ簢�� ���� �� ���� �̿��Ͽ� ȸ���� �ش�
        }
        //���� �������ٸ�
        //�ӷ� * -1�� ���ֱ� �Ǵ� ȸ���� �༭ �ݴ� �������� ���ư��� �ϸ鼭 y�� �������� ������

    }
}
//�÷��̾ ���ư� ũ��� ���� ���ϱ� nomalize�� magnitude�� ���
//��ǥ ���� �������� �� Ŭ���� ��ǥ�� ������ ���� �÷��̾� ��ǥ�� ������ ���� ����
//�÷��̾� �巡�� ǥ�ø� ��� ���� ����
//�巡�� ��¡�� �󸶳� �ƴ��� ǥ���� ���
//�׿� ȿ�������� ��� ���� ����
//skwnddp wpwkr
