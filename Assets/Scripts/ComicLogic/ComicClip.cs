using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace ComicLogic
{

    enum ScreenPosition 
    {
        CenterMiddle,
        CenterLeft,
        CenterRight,
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight
    }
    
    public class ComicClip : MonoBehaviour
    {
        [Tooltip("Local position of the comic clip when it starts")]
        [SerializeField]
        public Vector3 startPosition;
        
        [Tooltip("Speed of the comic clip")]
        [SerializeField]
        private float speed;
        
        [SerializeField]
        private ScreenPosition screenPosition;
        
        [Tooltip("Size of the renderTexture")]
        [SerializeField]
        private Vector2 renderTextureSize;

        [SerializeField]
        private VideoClip videoClip;
        
        //To be instantiated at Start
        private VideoPlayer _videoPlayer;
        private RenderTexture _renderTexture;
        private RawImage _rawImage;

        private void Start()
        {
            transform.position = startPosition;
            
            //Adds a RawImage to the GameObject
            _rawImage = gameObject.AddComponent<RawImage>();
            _rawImage.rectTransform.sizeDelta = renderTextureSize;
            
            _renderTexture = new RenderTexture((int)videoClip.width, (int)videoClip.height, 24);
            _rawImage.texture = _renderTexture;
            
            //assigns the VideoClip to the renderTexture
            _videoPlayer = gameObject.AddComponent<VideoPlayer>();
            _videoPlayer.targetTexture = _renderTexture;
            _videoPlayer.clip = videoClip;
            _videoPlayer.Play();

            StartCoroutine(MoveToPosition());
        }

        IEnumerator MoveToPosition()
        {
            Vector3 endPosition = CalculateEndPosition();
            
            while (transform.position != endPosition)
            {
                transform.position = Vector3.MoveTowards(transform.position, endPosition, speed * Time.deltaTime);
                yield return null;
            }
        }

        //Calculates the end position of the comic clip based on the screenPosition
        // TODO: Refine Positions
        Vector3 CalculateEndPosition()
        {
            Vector3 result = Vector3.zero;
            
            switch (screenPosition)
            {
                case ScreenPosition.CenterMiddle:
                    result= CalculateCenterOfScreen();
                    break;
                case ScreenPosition.CenterLeft:
                    result = CalculateCenterOfScreen() + new Vector3(-renderTextureSize.x, 0, 0);
                    break;
                case ScreenPosition.CenterRight:
                    result= CalculateCenterOfScreen() + new Vector3(renderTextureSize.x, 0, 0);
                    break;
                case ScreenPosition.TopLeft:
                    result = CalculateCenterOfScreen() + new Vector3(-renderTextureSize.x, renderTextureSize.y, 0);
                    break;
                case ScreenPosition.TopRight:
                    result = CalculateCenterOfScreen() + new Vector3(renderTextureSize.x, renderTextureSize.y, 0);
                    break;
                case ScreenPosition.BottomLeft:
                    result = CalculateCenterOfScreen() + new Vector3(-renderTextureSize.x, -renderTextureSize.y, 0);
                    break;
                case ScreenPosition.BottomRight:
                    result = CalculateCenterOfScreen() + new Vector3(renderTextureSize.x, -renderTextureSize.y, 0);
                    break;
            }
            return result;
        }
        
        Vector3 CalculateCenterOfScreen()
        {   
            return new Vector3(Screen.width / 2, Screen.height / 2, 0);
        }
    }
}
