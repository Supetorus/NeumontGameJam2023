using UnityEngine;

public class Title : MonoBehaviour
{
    void Update()
    {
		float sin0 = Mathf.Sin(Time.time * 2.0f);
		float sin1 = Mathf.Sin(Time.time * 4.0f);

        transform.rotation = Quaternion.Euler(0, 0, sin0 * 3.0f);
		transform.localScale = Vector3.one * (sin1 + 3.0f) / 3.0f;
    }
}
