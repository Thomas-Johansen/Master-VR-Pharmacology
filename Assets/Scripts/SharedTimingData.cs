using UnityEngine;

[CreateAssetMenu(fileName = "SharedTimingData", menuName = "Scriptable Objects/SharedTimingData")]
public class SharedTimingData : ScriptableObject
{
    /// <summary>
    /// Speed at which The morphing of crossection happens,
    /// and interval between cells "spawning".
    /// Might end up affecting cell speed
    /// </summary>
    public float speed = 1.0f; 
    /// <summary>
    /// The elapsed time used for timing mechanisms.
    /// </summary>
    public float time; 
    /// <summary>
    /// Which stage of the animation we are at.
    /// </summary>
    public int stage;
}
