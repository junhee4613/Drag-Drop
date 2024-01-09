using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : Unit
{
    //���ϱ� addForce�� velocity�� ���� �������༭ ����Ǵ� ������ �ٸ� �� ���� velrocity�� �����ϸ� �ٿ ���׸��� ���� ä�� ���� ����� �� ƨ�������� �ʰ� �ݴ�� addForce�� ���׸����� �ȳְ� �������� ó���ϸ� �۵��� �ȵ�
    //Drag_statu_walls_collider(); �� �Լ��� ������ ������� �ȵ�
    // ��ų�� �����ؾߵǴµ� �̰� ��� �����͸� �������� ����� ��ų���� ������ json���� ����
    public Rigidbody2D rb;
    public CircleCollider2D cc;
    [Header("���� ���� �ʱ� �ӵ�")]
    public float shoot_speed;
    [Header("�巡�� �� ���� �ӵ�")]
    public float slow_speed;
    [Header("���� �ð�")]
    public float invincibility_time;
    public int player_life = 3;
    public GameObject shoot_dir_image;
    public Transform arrow_rotation_base;
    #region Ŭ���� �ȿ��� �ذ��Ұ͵�
    Vector2 mouse_pos;
    Vector2 player_pos;
    Vector2 player_move_dir;
    public bool hit_statu = false;
    float time;
    float speed;
    float drag_dis;
    float player_rotation_z;
    public Vector2 drag_before_dir;
    public Collider2D[] targets;
    Managers Managers => Managers.instance;
    RaycastHit2D ray_hit;
    public Player_statu player_statu = Player_statu.IDLE;
    #endregion
    // Start is called before the first frame update
    private void Awake()
    {
        Managers.GameManager.player = gameObject.GetComponent<PlayerController>();
        Managers.GameManager.gameover += Player_die_setActive;
    }
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
        if (Input.GetMouseButton(0)) //���� 2�� �̰� ���� ���߿� ��ư ������ �� ����Ǵ� �Լ��� ������ ��
        {
            mouse_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Input.GetMouseButtonDown(0))    //���� 1��
            {
                Mouse_button_down();
            }
        }
        if (Managers.developer_mode)
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                Hit(1);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                Time.timeScale = 0.2f;
            }
            else if (Input.GetKeyUp(KeyCode.D))
            {
                Time.timeScale = 1;
            }
        }
        
    }
    public void Mouse_button_down()
    {
        ray_hit = Physics2D.Raycast(mouse_pos, Vector2.zero, 0, 1 << 6);
        if (ray_hit)
        {
            player_statu = Player_statu.DRAG;
            drag_before_dir = rb.velocity.normalized;
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
                Skills();   //��ų�� Ű ���� ���� �־ ���� �� ���¸� �ȳ־ �� �� ����
                break;
            default:
                break;
        }
    }
    public void Drag()
    {
        Drag_statu_walls_collider();
        if (speed != slow_speed)
        {
            speed = slow_speed;
            rb.velocity = drag_before_dir * speed;
        }
        shoot_dir_image.SetActive(true);
        player_pos = ray_hit.collider.gameObject.transform.position;
        drag_dis = Mathf.Abs(player_pos.magnitude - mouse_pos.magnitude); 
        player_move_dir = new Vector3(player_pos.x - mouse_pos.x, player_pos.y - mouse_pos.y, 0).normalized;
        player_rotation_z = Mathf.Atan2(player_move_dir.y, player_move_dir.x) * Mathf.Rad2Deg;
        arrow_rotation_base.rotation = Quaternion.Euler(0, 0, player_rotation_z - 90);
        if (Input.GetMouseButtonUp(0))
        {
            shoot_dir_image.SetActive(false);
            transform.rotation = arrow_rotation_base.rotation;
            rb.velocity = Vector2.zero;
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
        //�� ���� ���� �ɰͰ���
    }
    public void Skills()
    {

    }
    
    public override void Hit(float damage)
    {
        if (!hit_statu && !Managers.developer_mode)
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
    public void Player_die_setActive()
    {
        gameObject.SetActive(false);
        Managers.GameManager.gameover -= Player_die_setActive;
    }

    public void Drag_statu_walls_collider()
    {
        targets = Physics2D.OverlapCircleAll(transform.position, cc.radius, 1 << 9);
        for (int i = 0; i < targets.Length; i++)
        {
            switch (targets[i].tag)
            {
                case "Horizontal":
                    rb.velocity = new Vector2(rb.velocity.x * -1, rb.velocity.y);
                        break;
                case "Virtical":
                    rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * -1);
                    break;
                default:
                    break;
            }
        }
        
    }
    
}
