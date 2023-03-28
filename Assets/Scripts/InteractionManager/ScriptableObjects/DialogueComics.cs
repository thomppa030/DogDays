using ComicLogic;
using UnityEngine;

[CreateAssetMenu(fileName = "data", menuName = "new DialogueComicData", order = 3)]
public class DialogueComics : ScriptableObject
{
    public ComicClip[] comicClips;
}