using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSelected : MonoBehaviour
{
	private Image image;
	private RectTransform rectTransform;
	private void Awake()
	{
		image= gameObject.GetComponent<Image>();
		image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
		rectTransform= gameObject.GetComponent<RectTransform>();
	}

	public void SetParentButton(Transform target)
	{
		image.color = new Color(image.color.r, image.color.g, image.color.b, 1);

		transform.position = target.position;


	}
}
