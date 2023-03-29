using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace ComicLogic
{
    [RequireComponent(typeof(CanvasRenderer), typeof(Image))]
    public class ComicClip : MonoBehaviour
    {
        [Tooltip("Speed of the comic clip")]
        [SerializeField]
        private float speed;
        
        [SerializeField]
        private RectTransform targetTransform;
        
        private void OnEnable()
        {
            StartCoroutine(MoveToPosition());
        }

        IEnumerator MoveToPosition()
        {
            while (transform.position != targetTransform.position)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetTransform.position, speed * Time.deltaTime);
                yield return null;
            }
        }
    }
}
