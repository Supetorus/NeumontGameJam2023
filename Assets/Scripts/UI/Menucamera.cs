using UnityEngine;

public class MenuCamera : MonoBehaviour
{
	[SerializeField] private float speed;
	[SerializeField] private float rotSpeed;

	private float direction;

    void Start()
    {
		direction = Random.Range(0.0f, Mathf.PI * 2.0f);
	}

    void Update()
    {
		float dir = Mathf.PerlinNoise1D(Time.time) * 2.0f * Mathf.PI - Mathf.PI;
		direction += dir * rotSpeed * Time.deltaTime;

		float sin = Mathf.Sin(direction);
		float cos = Mathf.Cos(direction);

		transform.position += new Vector3(cos, sin, 0.0f) * speed * Time.deltaTime;
    }
}
