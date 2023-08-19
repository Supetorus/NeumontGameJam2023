using UnityEngine;

public class Fillbar : MonoBehaviour
{
	[SerializeField] private float length;
	[SerializeField] private float height;
	[SerializeField] private RectTransform fill;

	private RectTransform rectTransform;

    void Start()
    {
		rectTransform = GetComponent<RectTransform>();
		rectTransform.sizeDelta = new Vector2(length, height);
		fill.sizeDelta = new Vector2(length, height);
	}

	public void SetPercentage(float percent)
	{
		fill.sizeDelta = new Vector2(length * percent, height);
	}
}
