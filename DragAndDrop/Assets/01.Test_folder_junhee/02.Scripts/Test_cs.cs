using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
public class Test_cs : MonoBehaviour
{
    public Transform test_target;
    public float test_float = 0f;
    public Vector3 target_diretion;
    /*float Test_float
    {
        get { return test_float; }
        set 
        {
            if(test_float > 2)
            {
                value -= 2;
                //���⼱ -1�� ������
            }
            test_float = value;
            //��� �ڵ尡 ����ž� ���� �����

        }
    }*/
    private void Awake()
    {
        

    }
    public void FixedUpdate()
    {
        
        
    }
    public void Update()
    {
        test_float += Time.deltaTime;
        //transform.position = new Vector3()
        //transform.position = new Vector3(test_target.position - transform.position).normalized
        //if(target_diretion)
        //transform.Translate(new Vector3(test_target.position.x - transform.position.x, test_target.position.y - transform.position.y).normalized * Time.deltaTime);
        //transform.Translate(new Vector3(test_target.position.x - transform.position.x, test_target.position.y - transform.position.y).normalized * 2f * Time.deltaTime);

        
        //transform.position = Target_diraction(transform.position, test_target.position);

    }
    /*public Vector3 Target_diraction(Vector3 target, Vector3 follow_target)
    {
        target = new Vector3(Mathf.Clamp(target.x + follow_target.normalized.x * Time.deltaTime, -Mathf.Abs(follow_target.x), Mathf.Abs(follow_target.x))
            , Mathf.Clamp(target.y + (follow_target.normalized.y + Mathf.Sin(test_float)) * Time.deltaTime, follow_target.y - 4, follow_target.y + 4), target.z);
        return target;
    }*/

}


