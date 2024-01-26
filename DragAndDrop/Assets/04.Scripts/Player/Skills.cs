using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skills : MonoBehaviour
{
    [Header("��ų ����")]
    public float slow_skill_range;
    [Header("���ο� ��ų ������ ��� ���̾�")]
    public LayerMask slow_skill_targets;
    public bool q_down;
    public Collider2D[] targets;
    public GameObject Test2;
    public Button init_button;
    public HashSet<Collider2D> slow_Obstacle = new HashSet<Collider2D>();
    // Start is called before the first frame update
    private void Awake()
    {
        init_button.onClick.AddListener(() => {
            slow_Obstacle.Clear();
        });
    }
    void Start()
    {
        
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
    public void Skill()
    {
        Key_Press();
        if (q_down)
        {
            targets = Physics2D.OverlapCircleAll(transform.position, slow_skill_range, slow_skill_targets);
            foreach (var item in targets)
            {
                Debug.Log("����");
                if (!slow_Obstacle.Contains(item))
                {
                    Debug.Log("����");
                    if (item.TryGetComponent<slow_eligibility>(out slow_eligibility target))
                    {
                        Debug.Log("����");
                        target.Slow_apply();
                    }
                    slow_Obstacle.Add(item);
                    item.gameObject.layer = 11;
                }
            }
            //�̰� ��ֹ� ��ü�� ������Ʈ�� ������ �ƴϸ� �÷��̾�� ������ ������ε� 
            //�÷��̾�� ���ѰŸ� Getcomponent�������� �ٸ� ��� ��� ����Ʈ�� �־ �� ����Ʈ �ȿ� �ִ� �͵鿡�Ը� �ӵ� ���� ��Ű�°�?
            //��ֹ����� ������ Physics2D�� �ʹ� ���������� �ִٴ°� ����ؾߵ�
        }
        /*else if
        {

        }*/
    }
    public void Key_Press()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Test2.SetActive(true);
            q_down = true;
        }
        else if (Input.GetKeyUp(KeyCode.Q))
        {
            Test2.SetActive(false);
            q_down = false;
        }
    }
}
