using UnityEngine;

public class Fillbar : MonoBehaviour
{
	[SerializeField] private float length;
	[SerializeField] private RectTransform fill;

	private RectTransform rectTransform;

    void Start()
    {
		rectTransform = GetComponent<RectTransform>();
		rectTransform.sizeDelta = new Vector2(length, 30.0f);
		fill.sizeDelta = new Vector2(length, 30.0f);
	}

	public void SetPercentage(float percent)
	{
		fill.sizeDelta = new Vector2(length * percent, 30.0f);
	}
}
