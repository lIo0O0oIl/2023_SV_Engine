using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource bye, pew, end;

    public void SoundByePlay()
    {
        bye.Play();
    }

    public void SoundPewPlay()
    {
        pew.Play();
    }

    public void SoundEndPlay()
    {
        end.Play();
    }
}
