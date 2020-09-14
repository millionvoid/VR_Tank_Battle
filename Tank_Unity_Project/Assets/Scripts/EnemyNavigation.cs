using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyNavigation : MonoBehaviour
{
    private UnityEngine.AI.NavMeshAgent man;
    public Transform target;
    public Rigidbody m_Shell;                   // Prefab of the shell.
    public Transform m_FireTransform;           // A child of the tank where the shells are spawned.
    private float last_fire_time = 0f;
    public float fire_internal = 1f;
    private System.Random ran;
    // Start is called before the first frame update
    void Start()
    {
        ran = new System.Random();
        last_fire_time = Time.time;
        man = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
        Debug.Log(man);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - last_fire_time > fire_internal)
        {
            Fire();
            last_fire_time = Time.time;
        }
        man.SetDestination(target.position);
    }

    private void Fire()
    {
        Rigidbody shellInstance =
            Instantiate(m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;

        shellInstance.velocity = ran.Next(15,30) * m_FireTransform.forward;
    }
}
