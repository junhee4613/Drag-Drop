using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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
    public float speed_down;
    public int player_life = 3;
    public LayerMask collision_target;
    public GameObject shoot_dir_image;
    public Transform arrow_rotation_base;
    public float invincibility_time;
    #region Ŭ���� �ȿ��� �ذ��Ұ͵�
    Vector2 mouse_pos;
    Vector2 player_pos;
    Vector2 player_move_dir;
    bool wall_collider;
    public bool hit_statu = false;
    float time;
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
        if (Managers.GameManager.player_die)
        {
            return;
        }
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
        if(rb.drag != 0)
        {
            rb.drag = 0;
            rb.velocity = drag_before_speed * speed; //�̰� �ӵ� �����ϰ� �ٲٱ�
        }
        shoot_dir_image.SetActive(true);
        drag_dis = Mathf.Abs(player_pos.magnitude - mouse_pos.magnitude); 
        player_move_dir = new Vector3(player_pos.x - mouse_pos.x, player_pos.y - mouse_pos.y, 0).normalized;
        player_rotation_z = Mathf.Atan2(player_move_dir.y, player_move_dir.x) * Mathf.Rad2Deg;
        arrow_rotation_base.rotation = Quaternion.Euler(0, 0, player_rotation_z - 90);
        if (Input.GetMouseButtonUp(0))     //���� 3��
        {
            shoot_dir_image.SetActive(false);
            transform.rotation = arrow_rotation_base.rotation;
            rb.velocity = Vector2.zero;
            rb.drag = speed_down;
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
                    Obstacle();
                    //��ֹ��� ���� ó��
                    break;
                case "Special_zone":
                    Special_zone();
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
        }
    }
    public void Obstacle()
    {
        if(!hit_statu)
        {
            Hit(1);
            Debug.Log("���ݴ���");
            //��Ʈ ���ص� ȿ���� �ް� �ϱ�
        }
    }
    public override void Hit(float damage)
    {
        hit_statu = true;
        player_life -= (int)damage;
        if (player_life <= 0)
        {
            Managers.GameManager.player_die = true;
            Managers.GameManager.gameover();
            return;
        }
        StartCoroutine(invincibility());
    }
    IEnumerator invincibility()
    {
        if(player_life != 0)
        {
            transform.DOPunchScale(Vector2.one * 1.5f, 2f, 3, 0.5f);
        }

        while (time < invincibility_time)
        {
            time += Time.deltaTime;
            yield return null;
        }
        hit_statu = false;
        time = 0;
    }
    public void Special_zone()
    {
        //���⿣ Ư�� �������� ���� �� �ִ� �ɷµ� �ֱ�
    }
}
