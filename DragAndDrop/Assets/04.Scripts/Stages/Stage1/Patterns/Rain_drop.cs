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
    public float test_num;

    private void Start()
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

    private void FixedUpdate()
    {
        transform.position = new Vector3(transform.position.x + Mathf.Sin(transform.rotation.eulerAngles.z * Mathf.Deg2Rad) * Time.fixedDeltaTime * shoot_power, transform.position.y - (test_num += Time.fixedDeltaTime) * Time.fixedDeltaTime);
        if (positive_num)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, Mathf.Clamp(transform.rotation.eulerAngles.z - rotation_zero_speed * Time.fixedDeltaTime, 0, 90));
        }
        else
        {
            transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, Mathf.Clamp(transform.rotation.eulerAngles.z + rotation_zero_speed * Time.fixedDeltaTime, 0, 359.99f));
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
