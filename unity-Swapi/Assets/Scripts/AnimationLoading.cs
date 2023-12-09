using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationLoading : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Sprite[] frames;
    [SerializeField] private float timeBetweenFrames=0.1f;
    [SerializeField] private int currentFrame;

	private void OnEnable()
	{
        StopAllCoroutines();
        StartCoroutine(NextFrame());
	}

	IEnumerator NextFrame()
    {
        while (true)
        {
            image.sprite = frames[currentFrame];
            currentFrame = (currentFrame + 1) % frames.Length;
            yield return new WaitForSeconds(timeBetweenFrames);
        }
    }
}
