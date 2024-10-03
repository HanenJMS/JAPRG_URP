using UnityEngine;


namespace GameLab.WorldSpaceComponents
{
    public class PlayAudioOnDestroy : MonoBehaviour
    {
        [SerializeField] AudioClip audioClip;

        private void OnDestroy()
        {
            if (audioClip != null)
            {
                GameObject obj = new();
                GameObject obj1 = Instantiate(obj, this.transform.position, Quaternion.identity);
                obj1.AddComponent<AudioSource>().clip = audioClip;
            }
        }
    }
}


