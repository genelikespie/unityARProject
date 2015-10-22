using UnityEngine;

public class EnemyNavigation : MonoBehaviour {

	public Transform target;
	NavMeshAgent agent;

	void Start()
	{
		agent = GetComponent<NavMeshAgent> ();

        //if target not supplied in editor, try to get it from
        //Game Control
        if (GameControl.instance != null && GameControl.instance.tower != null)
            target = GameControl.instance.tower.transform;
    }

	void Update()
	{
		if (target && !agent.hasPath)
		{
			agent.SetDestination(target.position);
		}
	}
}
