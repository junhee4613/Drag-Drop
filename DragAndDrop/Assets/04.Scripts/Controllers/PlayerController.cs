using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class PlayerController : playerData
{
    //���� ���鿡 ������ �ٸ� ��
    //���� �ӵ��� �÷��̾ �巡�� ������ ���� �ӵ��� ������ ��� �ְ� �ӵ��� 
    //���ϱ� addForce�� velocity�� ���� �������༭ ����Ǵ� ������ �ٸ� �� ���� velrocity�� �����ϸ� �ٿ ���׸��� ���� ä�� ���� ����� �� ƨ�������� �ʰ� �ݴ�� addForce�� ���׸����� �ȳְ� �������� ó���ϸ� �۵��� �ȵ�
    //Drag_statu_walls_collider(); �� �Լ��� ������ ������� �ȵ�
    // ��ų�� �����ؾߵǴµ� �̰� ��� �����͸� �������� ����� ��ų���� ������ json���� ����
    public Rigidbody2D rb;
    public CapsuleCollider2D cc;
    public GameObject shoot_dir_image;
    public Transform arrow_rotation_base;
    public float obj_size;
    public AudioClip bounce_sound;
    public GameObject player;
    public Transform character;
    [Header("�÷��̾� ������")]
    public float player_size;
    [Header("�÷��̾� ������ ����(1�� �⺻ ��)")]
    public float player_size_magnification;
    [Header("�巡�� �� ���� �ӵ� �� ũ�� ����(1�� �⺻��)")]
    public float drag_dis_magnification = 1;
    public Animator animator;
    #region Ŭ���� �ȿ��� �ذ��Ұ͵�
    sbyte break_num = 0;
    public Vector2 mouse_current_pos;
    public Vector2 mouse_click_pos;
    Vector2 drag_dis;
    Vector2 drag_min_shoot_dir;
    public bool hit_statu = false;
    float time;
    public float player_rotation_z;
    float shoot_power_range;
    Vector2 spacebar_dir;
    float spacebar_mag;
    
    //string wall_name;
    //public Collider2D[] walls_sence;
    Managers Managers => Managers.instance;                 //���� �巡�� ������ �� �߻簡 �ȵǴ� ���� ����
    [SerializeField]
    public Player_statu player_statu = Player_statu.IDLE;
    public Collider2D[] interation_obj;
    public ParticleSystem move_fragments;
    public ParticleSystem move_wave;
    public ParticleSystem wavelength;
    public Particle_figure move_fragments_figurel;
    public Particle_figure move_wave_figurel;
    #endregion
    #region �׽�Ʈ��
    //[Header("�׽�Ʈ��")]
    #endregion
    // Start is called before the first frame update
    private void Awake()
    {
        Managers.GameManager.gameover += Player_die_setActive;
        //move_fragments_figurel.module = new ParticleSystem.Burst(0, 60, 60, 1, 0.010f);
        move_fragments_figurel.module = move_fragments.emission;
    }
    public void Player_slow_skill_down()
    {
        //rb.velocity
    }
    public void Player_slow_skill_up()
    {

    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //Drag_statu_walls_collider();
        if (!Managers.GameManager.ui_on)
        {
            Interaction_obj();
            Key_operate();
        }
        #region ������� �����ڿ� 
        /*if (Managers.developer_mode)
        {
            
            if (Input.GetKeyDown(KeyCode.D))
            {
                Time.timeScale = 0.2f;
            }
            else if (Input.GetKeyUp(KeyCode.D))
            {
                Time.timeScale = 1;
            }
            if (Input.GetKeyDown(KeyCode.H))
            {
                Hit(1);
            }
        }*/

        #endregion
    }
    public void Mouse_button_down()
    {
        //wall_name = string.Empty;
        player_statu = Player_statu.DRAG;
    }
    public void Player_Statu()
    {
        switch (player_statu)
        {
            case Player_statu.IDLE:
                break;
            case Player_statu.DRAG:
                mouse_current_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                //wavelength.gameObject.SetActive(true);
                Drag();

                break;
            case Player_statu.RUN:
                wavelength.gameObject.SetActive(false);
                break;
            case Player_statu.HIT:
                break;
            default:
                break;
        }
    }
    private void FixedUpdate()      //������ ���⼭ ó���ؾ� ���� ���������� ���������� �� ����
    {
        if (hit_statu)
        {
            time += Time.fixedDeltaTime;
            if(time > invincibility_time)
            {
                hit_statu = false;
                time = 0;
            }
        }
        Run();
    }
    public void Key_operate()
    {
        Player_Statu();
        
        if (Input.GetMouseButtonDown(0))    //���� 1��
        {
            shoot_dir_image.SetActive(true);
            mouse_click_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //rb.velocity = Vector3.zero;
            animator.Play("Drag_statu");
            Mouse_button_down();
        }
        if (Input.GetKeyDown(KeyCode.Space) && break_num == 1)
        {
            break_num = 0;
            rb.velocity = Vector2.zero;
            //spacebar_dir = rb.velocity.normalized;
            //spacebar_mag = rb.velocity.magnitude;
            //rb.velocity = spacebar_dir * Mathf.Clamp(spacebar_mag - speed_break, 0, shoot_speed);
            
        }
        if (rb.velocity == Vector2.zero && player_statu == Player_statu.RUN)
        {
            animator.Play("Idle");
            player_statu = Player_statu.IDLE;
        }
        if(player_statu != Player_statu.RUN)
        {
            animator.speed = 1;
        }
    }
    public void Drag()
    {
        drag_dis = new Vector3(mouse_click_pos.x - mouse_current_pos.x, mouse_click_pos.y - mouse_current_pos.y, 0) * drag_dis_magnification;
        if(drag_dis != Vector2.zero)
        {
            player_rotation_z = Mathf.Atan2(drag_dis.normalized.y, drag_dis.normalized.x) * Mathf.Rad2Deg;
        }
        else
        {
            drag_min_shoot_dir = new Vector2(mouse_click_pos.x - transform.position.x, mouse_click_pos.y - transform.position.y).normalized;
            player_rotation_z = Mathf.Atan2(drag_min_shoot_dir.y, drag_min_shoot_dir.x) * Mathf.Rad2Deg;
        }
        transform.rotation = Quaternion.Euler(0, 0, player_rotation_z - 90);
        animator.SetBool("Drag", true);
        //character.transform.localScale = new Vector3(character.transform.localScale.x, Mathf.Clamp(player_size + (drag_dis.magnitude / player_size_magnification), player_size, player_size + (shoot_speed / player_size_magnification)));
        if (Input.GetMouseButtonUp(0))
        {
            animator.SetTrigger("Shoot");
            animator.SetBool("Drag", false);
            if (break_num == 0)
            {
                break_num = 1;
            }
            shoot_dir_image.SetActive(false);
            shoot_power_range = Mathf.Clamp(drag_dis.magnitude, Mathf.Abs(slow_speed), Mathf.Abs(shoot_speed));
            animator.speed = 5f / shoot_power_range;
            transform.rotation = arrow_rotation_base.rotation;
            rb.velocity = Vector2.zero;
            move_fragments.Play();
            //move_fragments_figurel.module.SetBurst(0, new ParticleSystem.Burst(0, 60 * (int)(shoot_power_range / Mathf.Abs(shoot_speed))));
            move_fragments_figurel.module.SetBurst(0, new ParticleSystem.Burst(0, 60 * shoot_power_range / Mathf.Abs(shoot_speed)));
            //Debug.Log(60 * (int)(shoot_power_range / Mathf.Abs(shoot_speed)));
            //Debug.Log((shoot_power_range / Mathf.Abs(shoot_speed)));
            //move_fragments_figurel.module.count = 60 * (int)(shoot_power_range / Mathf.Abs(shoot_speed));
            //move_fragments.transform.localScale = Vector2.one * (shoot_power_range / Mathf.Abs(shoot_speed));
            move_wave.Play();
            //move_wave.transform.localScale = Vector2.one * (shoot_power_range / Mathf.Abs(shoot_speed));
            Drag_shoot();
            /*if (drag_dis.magnitude == 0)
            {
                animator.SetBool("Drag", false);
                player_statu = Player_statu.RUN;
                shoot_dir_image.SetActive(false);
            }
            else
            {
                animator.SetTrigger("Shoot");
                animator.SetBool("Drag", false);
                if (break_num == 0)
                {
                    break_num = 1;
                }
                shoot_dir_image.SetActive(false);
                shoot_power_range = Mathf.Clamp(drag_dis.magnitude, Mathf.Abs(slow_speed), Mathf.Abs(shoot_speed));
                animator.speed =  5f / shoot_power_range;
                transform.rotation = arrow_rotation_base.rotation;
                rb.velocity = Vector2.zero;
                test_particle.Play();
                test_particle2.Play();
                Drag_shoot();
            }*/
            
        }
    }
    public void Drag_shoot()
    {
        rb.velocity = transform.up * shoot_power_range;
        player_statu = Player_statu.RUN;
    }
    public void Run()
    {
        rb.velocity = rb.velocity.normalized * (Mathf.Clamp(rb.velocity.magnitude - Time.fixedDeltaTime * gradually_down_speed, 0, shoot_speed));
    }
    
    public void Hit()
    {
        if (!hit_statu)
        {
            hit_statu = true;
            if (!Managers.invincibility)
            {
                player_life -= 1;
                Managers.UI_jun.player_hp[player_life].SetActive(false);
            }

            if (player_life <= 0)
            {
                Managers.GameManager.player_die = true;
                gameObject.SetActive(false);
                Managers.GameManager.gameover();
                return;
            }
            //StartCoroutine(invincibility());
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
            time += Time.fixedDeltaTime;
            yield return null;
        }
        hit_statu = false;
        time = 0;
    }
    public void Player_die_setActive()
    {
        //�÷��̾� ���� �� ó�� �ִϸ��̼��� �۵��Ѵٵ��� ���
    }
    public void Interaction_obj()
    {
        if (!hit_statu)
        {
            interation_obj = Physics2D.OverlapCircleAll(transform.position, obj_size, 1 << 8);

            foreach (var item in interation_obj)
            {
                IInteraction_obj obj = item.GetComponent<IInteraction_obj>();
                if (obj != null)
                {
                    obj.practice();
                }
                else
                {
                    Hit();
                }
            }
        }
    }
    [System.Serializable]
    public class Particle_figure 
    {
        public ParticleSystem.EmissionModule module;
        public float origin_min_speed;
        public float origin_max_speed;
        public float origin_start_size;
        public float origin_life_time;
    }
}
