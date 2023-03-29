using System.Collections.Generic;
using UnityEngine;

namespace ComicLogic
{
    class ComicManager : MonoBehaviour
    {
        [SerializeField]
        List<ComicClip> comicClips = new List<ComicClip>();
        
        public void NextClip(int index)
        {
            comicClips[index].gameObject.SetActive(true);
        }
    }
}