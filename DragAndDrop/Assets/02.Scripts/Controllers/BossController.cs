using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public abstract class BossController : Stage_base_controller        //time	action_num	duration
{
    protected Dictionary<GameObject, Animator> anim_ongoing_obj = new Dictionary<GameObject, Animator>();
    protected Dictionary<GameObject, SpriteRenderer> general_warning_box_sr = new Dictionary<GameObject, SpriteRenderer>();
    protected HashSet<GameObject> anim_end_push_objs = new HashSet<GameObject>();
    public Color[] color = new Color[2];
    protected override void Awake()
    {
        base.Awake();
    }
    protected void Update()
    {
        if (Managers.GameManager.game_start)
        {
            Anim_end_push();
            Pattern_processing();
            if(Managers.Sound.bgSound.clip != null && Managers.Sound.bgSound)
            {
                if (Managers.Sound.bgSound.loop == false && Managers.Sound.bgSound.time >= Managers.Sound.bgSound.clip.length - 0.2f)
                {
                    Game_clear();
                }
            }
            
        }
    }
    protected virtual void FixedUpdate()
    {
        
    }
    public void Pattern_function(ref List<Pattern_json_date> pattern_json_data, ref bool pattern_ending, ref float pattern_duration_time, ref sbyte pattern_count, 
         Action not_duration_pattern, bool pattern_duration_obj_enable = false, float pattern_time = 0f, float time = 0f, Action duration_pattern = null)
    {
        if (!pattern_ending)
        {
            if ((pattern_json_data[pattern_count].time <= Managers.Sound.bgSound.time || pattern_duration_time != 0))
            {
                if (pattern_duration_time == 0)
                {
                    pattern_duration_time = pattern_json_data[pattern_count].duration;
                }
                
                if (pattern_duration_obj_enable)
                {
                    pattern_time -= Time.fixedDeltaTime;
                    if (pattern_time <= 0)
                    {
                        pattern_time += time;
                        duration_pattern();
                    }
                }
                not_duration_pattern();
                pattern_duration_time = Mathf.Clamp(pattern_duration_time - Time.fixedDeltaTime, 0, pattern_json_data[pattern_count].duration);
                if (pattern_duration_time == 0)
                {
                    pattern_count++;
                    if (pattern_json_data.Count == pattern_count)
                    {
                        pattern_ending = true;
                    }
                }
            }
        }
    }
    public void Warning_box_fade(Vector3 size, Vector3 pos, bool fade_option, sbyte count = 0, float minute = 0, Action dotween_end_function = null)
    {
        GameObject warning_box = Managers.Pool.Pop(Managers.Resource.Load<GameObject>("Warning_box"));
        warning_box.transform.position = pos;
        warning_box.transform.localScale = size;
        if (fade_option)
        {
            warning_box.GetComponent<SpriteRenderer>().DOFade(1, 0);
            warning_box.GetComponent<SpriteRenderer>().DOFade(0, minute).SetLoops(count, LoopType.Yoyo).OnComplete(() =>
            {
                if(dotween_end_function != null)
                {
                    dotween_end_function();
                }
                Managers.Pool.Push(warning_box);
            });
        }
    }
    public void Warning_box_scale(Vector3 init_size, Vector3 pos, float dotween_dration, Vector3 end_value, bool only_vertical = false, bool only_horizontal = false, Action dotween_end_function = null)
    {
        GameObject warning_box = Managers.Pool.Pop(Managers.Resource.Load<GameObject>("Warning_box"));
        warning_box.transform.position = pos;
        warning_box.transform.localScale = init_size;
        if (only_horizontal)
        {
            warning_box.transform.DOScaleX(end_value.x, dotween_dration).OnComplete(() =>
            {
                dotween_end_function();
                Managers.Pool.Push(warning_box);
            });
        }
        else if (only_vertical)
        {
            warning_box.transform.DOScaleY(end_value.y, dotween_dration).OnComplete(() =>
            {
                dotween_end_function();
                Managers.Pool.Push(warning_box);
            });
        }
        else
        {
            warning_box.transform.DOScale(end_value, dotween_dration).OnComplete(() =>
            {
                dotween_end_function();
                Managers.Pool.Push(warning_box);
            });
        }
    }
    public GameObject Warning_box_punch_scale(Vector3 pos, Vector3 init_size, Vector3 first_dotween, float first_dotween_duration, Vector3 end_size, float seconde_dotween_duration
        ,bool auto_disable, bool only_vertical = false, bool only_horizontal = false, Action dotween_end_function = null)
    {
        GameObject warning_box = Managers.Pool.Pop(Managers.Resource.Load<GameObject>("Warning_box"));
        warning_box.transform.position = pos;
        warning_box.transform.localScale = init_size;
        if (only_horizontal)
        {
            warning_box.transform.DOScaleX(first_dotween.x, first_dotween_duration).OnComplete(() => 
            {
                warning_box.transform.DOScaleX(end_size.x, seconde_dotween_duration).OnComplete(() => 
                { 
                    if(dotween_end_function != null)
                    {
                        dotween_end_function();
                    }
                    if (auto_disable)
                    {
                        Managers.Pool.Push(warning_box);
                    }
                }); 
            });
        }
        else if (only_vertical)
        {
            warning_box.transform.DOScaleY(first_dotween.y, first_dotween_duration).OnComplete(() =>
            {
                warning_box.transform.DOScaleY(end_size.y, seconde_dotween_duration).OnComplete(() =>
                {
                    if (dotween_end_function != null)
                    {
                        dotween_end_function();
                    }
                    if (auto_disable)
                    {
                        Managers.Pool.Push(warning_box);
                    }
                });
            });
        }
        else
        {
            warning_box.transform.DOScale(first_dotween, first_dotween_duration).OnComplete(() =>
            {
                warning_box.transform.DOScale(end_size, seconde_dotween_duration).OnComplete(() =>
                {
                    if (dotween_end_function != null)
                    {
                        dotween_end_function();
                    }
                    if (auto_disable)
                    {
                        Managers.Pool.Push(warning_box);
                    }
                });
            });
        }
        return warning_box;
    }
    public GameObject General_warning_box(Vector3 init_size, Vector3 pos, Color color)
    {
        GameObject warning_box = Managers.Resource.Load<GameObject>("Warning_box");
        warning_box.transform.position = pos;
        warning_box.transform.localScale = init_size;
        if (!general_warning_box_sr.ContainsValue(warning_box.GetComponent<SpriteRenderer>()))
        {
            general_warning_box_sr.Add(warning_box, warning_box.GetComponent<SpriteRenderer>());
        }
        general_warning_box_sr[warning_box].color = color;
        return Managers.Pool.Pop(warning_box);
    }
    /// <summary>
    /// 0.5 단위로 피봇값 설정 양의 Y축이면 0.5로 설정
    /// </summary>
    /// <param name="init_size"></param>
    /// <param name="box1_pos"></param>
    /// <param name="box2_pos"></param>
    /// <param name="pivot"></param> 
    /// <param name="color"></param>
    /// <param name="color2"></param>
    /// <param name="action_time"></param>
    public void Charging_doscale_y_warning_box(Vector3 init_size, Vector3 box1_pos,Vector3 box2_pos, Warning_box_pivots pivot, Color[] color, float action_time)
    {
        GameObject warning_box = Managers.Pool.Pop(Managers.Resource.Load<GameObject>("Warning_box"));
        warning_box.transform.position = box1_pos;
        warning_box.transform.localScale = init_size;
        if (!general_warning_box_sr.ContainsValue(warning_box.GetComponent<SpriteRenderer>()))
        {
            general_warning_box_sr.Add(warning_box, warning_box.GetComponent<SpriteRenderer>());
        }
        general_warning_box_sr[warning_box].color = color[0];
        GameObject warning_box2 = Managers.Pool.Pop(Managers.Resource.Load<GameObject>("Warning_box2"));
        warning_box2.transform.localScale = new Vector3(init_size.x, 0, init_size.z);
        switch (pivot)
        {
            case Warning_box_pivots.UP:
                warning_box2.transform.GetChild(0).transform.localPosition = new Vector3(0, -0.5f, 0);
                break;
            case Warning_box_pivots.DOWN:
                warning_box2.transform.GetChild(0).transform.localPosition = new Vector3(0, 0.5f, 0);
                break;
        }
        warning_box2.transform.position = box2_pos;
        if (!general_warning_box_sr.ContainsValue(warning_box2.transform.GetChild(0).GetComponent<SpriteRenderer>()))
        {
            general_warning_box_sr.Add(warning_box2, warning_box2.transform.GetChild(0).GetComponent<SpriteRenderer>());
        }
        general_warning_box_sr[warning_box2].color = color[1];
        warning_box2.transform.DOScaleY(init_size.y, action_time).OnComplete(() =>
        {
            Managers.Pool.Push(warning_box2);
            Managers.Pool.Push(warning_box);
        });
    }
    public void Charging_doscale_x_warning_box(Vector3 init_size, Vector3 box1_pos, Vector3 box2_pos, Warning_box_pivots pivot, Color[] color, float action_time)
    {
        GameObject warning_box = Managers.Pool.Pop(Managers.Resource.Load<GameObject>("Warning_box"));
        warning_box.transform.position = box1_pos;
        warning_box.transform.localScale = init_size;
        if (!general_warning_box_sr.ContainsValue(warning_box.GetComponent<SpriteRenderer>()))
        {
            general_warning_box_sr.Add(warning_box, warning_box.GetComponent<SpriteRenderer>());
        }
        general_warning_box_sr[warning_box].color = color[0];
        GameObject warning_box2 = Managers.Pool.Pop(Managers.Resource.Load<GameObject>("Warning_box2"));
        warning_box2.transform.localScale = new Vector3(0, init_size.y, init_size.z);
        switch (pivot)
        {
            case Warning_box_pivots.LEFT:
                warning_box2.transform.GetChild(0).transform.localPosition = new Vector3(-0.5f, 0,  0);
                break;
            case Warning_box_pivots.RIGHT:
                warning_box2.transform.GetChild(0).transform.localPosition = new Vector3(0.5f, 0,  0);
                break;
        }
        warning_box2.transform.position = box2_pos;
        if (!general_warning_box_sr.ContainsValue(warning_box2.transform.GetChild(0).GetComponent<SpriteRenderer>()))
        {
            general_warning_box_sr.Add(warning_box2, warning_box2.transform.GetChild(0).GetComponent<SpriteRenderer>());
        }
        general_warning_box_sr[warning_box2].color = color[1];
        warning_box2.transform.DOScaleX(init_size.y, action_time).OnComplete(() =>
        {
            Managers.Pool.Push(warning_box2);
            Managers.Pool.Push(warning_box);
        });
    }
    public void Charging_doscale_center_warning_box(Vector3 init_size, Vector3 box1_pos, Vector3 box2_pos, Color[] color, float action_time)
    {
        GameObject warning_box = Managers.Pool.Pop(Managers.Resource.Load<GameObject>("Warning_box"));
        warning_box.transform.position = box1_pos;
        warning_box.transform.localScale = init_size;
        if (!general_warning_box_sr.ContainsValue(warning_box.GetComponent<SpriteRenderer>()))
        {
            general_warning_box_sr.Add(warning_box, warning_box.GetComponent<SpriteRenderer>());
        }
        general_warning_box_sr[warning_box].color = color[0];
        GameObject warning_box2 = Managers.Pool.Pop(Managers.Resource.Load<GameObject>("Warning_box2"));
        warning_box2.transform.localScale = new Vector3(0, 0, init_size.z);
        warning_box2.transform.GetChild(0).transform.localPosition = new Vector3(0, 0, 0);
        warning_box2.transform.position = box2_pos;
        if (!general_warning_box_sr.ContainsValue(warning_box2.transform.GetChild(0).GetComponent<SpriteRenderer>()))
        {
            general_warning_box_sr.Add(warning_box2, warning_box2.transform.GetChild(0).GetComponent<SpriteRenderer>());
        }
        general_warning_box_sr[warning_box2].color = color[1];
        warning_box2.transform.DOScale(init_size, action_time).OnComplete(() =>
        {
            Managers.Pool.Push(warning_box2);
            Managers.Pool.Push(warning_box);
        });
    }
    void Game_clear()
    {
        Managers.GameManager.game_start = false;
        if (!string.IsNullOrEmpty(Managers.GameManager.scene_name) && !Managers.GameManager.stage_clear[Managers.GameManager.scene_name])          //전에 있던 스테이지가 어떤 스테이지인지 알기 위해 scene_name이 변수에 할당 하기 전에 먼저 실행
        {
            if (Managers.GameManager.stage_clear.TryGetValue(Managers.GameManager.scene_name, out bool is_clear))
            {
                Managers.GameManager.stage_clear[Managers.GameManager.scene_name] = !is_clear;
            }
        }
        if(Managers.GameManager.stage_clear.TryGetValue(Managers.instance.last_stage, out bool is_init))
        {
            if (is_init == true)
            {
                Managers.UI_jun.Fade_out_next_in("Black", 0, 1, "Lobby_screen", 1, stage_clear_init);
            }
            else
            {
                Managers.UI_jun.Fade_out_next_in("Black", 0, 1, "Main_screen", 1);
            }
        }
        
    }
    void stage_clear_init()
    {
        foreach (var item in Managers.GameManager.stage_clear)
        {
            Managers.GameManager.stage_clear[item.Key] = false;
        }
        
    }
    public abstract void Pattern_processing();
    public void Anim_end_push()
    {
        if (anim_ongoing_obj.Count != 0)
        {
            foreach (var item in anim_ongoing_obj)
            {
                if (item.Value.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
                {
                    Managers.Pool.Push(item.Value.gameObject);
                    anim_end_push_objs.Add(item.Key);
                }
            }
            foreach (var item in anim_end_push_objs)
            {
                anim_ongoing_obj.Remove(item);
            }
            anim_end_push_objs.Clear();
        }
    }
    public enum Warning_box_pivots
    {
        UP,
        DOWN,
        LEFT,
        RIGHT,
    }
}
