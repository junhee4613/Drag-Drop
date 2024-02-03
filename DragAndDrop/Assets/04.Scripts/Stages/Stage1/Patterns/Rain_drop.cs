using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rain_drop : MonoBehaviour
{
    [Header("ó���� ���ư��� �ִ� ����")]
    public float rotation_z;
    [Header("�ʱ⿡ ���ư��� ��")]
    public float shoot_power;
    float rotation_init;
    [Header("z���� 0���� ���ƿ��� �ӵ�(�������� ���� 0���� �ǵ��ư�)")]
    public float rotation_zero_speed;
    bool positive_num;
    [Header("�߷� ��")]
    public float temp_gravity;
    float gravity;
    [Header("������� �ð�")]
    public float push_time;
    float time;
    private void Start()
    {
        gravity = temp_gravity;
        rotation_init = Random.Range(-rotation_z, rotation_z);
        transform.rotation = Quaternion.Euler(0, 0, rotation_init);
        if (transform.rotation.eulerAngles.z < 90)
        {
            positive_num = true;
        }
        else if (transform.rotation.eulerAngles.z > 270)
        {
            positive_num = false;
        }
    }

    private void FixedUpdate()
    {
        if(push_time <= time)
        {
            time = 0;
            Managers.Pool.Push(this.gameObject);
            gravity = temp_gravity;
        }
        time += Time.fixedDeltaTime;
        transform.position = new Vector3(transform.position.x + Mathf.Sin(transform.rotation.eulerAngles.z * Mathf.Deg2Rad) * Time.fixedDeltaTime * shoot_power, transform.position.y - (gravity += Time.fixedDeltaTime) * Time.fixedDeltaTime);
        if (positive_num)
        {
            if(transform.rotation.eulerAngles.z != 0)
            {
                transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, Mathf.Clamp(transform.rotation.eulerAngles.z - rotation_zero_speed * Time.fixedDeltaTime, 0, 90));
            }
        }
        else
        {
            if (transform.rotation.eulerAngles.z != 359.99f)
            {
                transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, Mathf.Clamp(transform.rotation.eulerAngles.z + rotation_zero_speed * Time.fixedDeltaTime, 0, 359.99f));

            }
        }
    }
    private void OnEnable()
    {

        rotation_init = Random.Range(-rotation_z, rotation_z);
        transform.rotation = Quaternion.Euler(0, 0, rotation_init);
        if (transform.rotation.eulerAngles.z < 90)
        {
            positive_num = true;
        }
        else if (transform.rotation.eulerAngles.z > 270)
        {
            positive_num = false;
        }
    }
}
