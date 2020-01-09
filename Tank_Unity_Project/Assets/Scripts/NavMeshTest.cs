using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavMeshTest : MonoBehaviour
{
    private UnityEngine.AI.NavMeshAgent man;
    public Transform target;
    // Start is called before the first frame update
    void Start()
    {
        man = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
        Debug.Log(man);
    }

    // Update is called once per frame
    void Update()
    {
        man.SetDestination(target.position);
    }
}
