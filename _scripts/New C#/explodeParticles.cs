using UnityEngine;

public class explodeParticles : MonoBehaviour
{
	public float baseForce = 30;
	public float expSize = 0.3f;
	public int myLayer;
	public readonly int particleNum = 30;
	public GameObject theParticle;

	private void Start()
	{
		myLayer = gameObject.layer;
	}

	private void ExplodeParticle()
	{
		for (var i = 0; i < particleNum; i++)
		{
			GameObject newParticle = Instantiate(theParticle);
			newParticle.transform.position =
				new Vector2(Random.Range(transform.position.x - expSize, transform.position.x + expSize),
					Random.Range(transform.position.y - expSize, transform.position.y + expSize));
			var newForce = new Vector2(Random.Range(-baseForce, baseForce), Random.Range(baseForce/2, baseForce));
			newParticle.GetComponent<Rigidbody2D>().AddForceAtPosition(newForce, transform.position);
		}

		Physics2D.IgnoreLayerCollision(myLayer, myLayer, true);
	}
}