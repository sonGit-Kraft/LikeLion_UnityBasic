using UnityEngine;

public class Player : MonoBehaviour
{
    public DynamicJoystick joystick;
    public GameObject missile;
    public bool fire = false;

    void Start()
    {

    }

    void Update()
    {
        float x = joystick.Horizontal;
        float y = joystick.Vertical;

        Vector3 dir = new Vector3(x, y, 0);
        transform.Translate(dir * 3 * Time.deltaTime);

        if(fire)
            Instantiate(missile, transform.position, Quaternion.identity);
    }

    // 버튼에 연결
    public void StartMissile()
    {
        Instantiate(missile, transform.position, Quaternion.identity);
    }

    public void FireEnter()
    {
        fire = true;
    }

    public void FireExit()
    {
        fire = false;
    }
}
