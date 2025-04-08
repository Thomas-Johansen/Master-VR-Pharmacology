using UnityEngine;
/// <summary>
/// This Scriptable object acts as a data sharing point between the Cell and Crossection shader controllers. \n
/// It Contains variables for: \n
///  - Animation speed.
///  - Time elaspsed in the animation.
///  - Stage of the animation
/// </summary>
[CreateAssetMenu(fileName = "SharedTimingData", menuName = "Scriptable Objects/SharedTimingData")]
public class SharedTimingData : ScriptableObject
{
    /// <summary>
    /// Speed at which The morphing of crossection happens,
    /// and interval between cells "spawning".
    /// Might end up affecting cell speed
    /// </summary>
    public float Speed = 1.0f; 
    /// <summary>
    /// The elapsed time used for timing mechanisms.
    /// </summary>
    public float Time = 0; 
    /// <summary>
    /// Which stage of the animation we are at.
    /// </summary>
    public int Stage = 1;
    /*
     * Stages of animation:
     *  Stage1: The muscle layer contracts
     *  Stage2: Some cells starts to show up.
     *  Stage3: The tissue gets irritated and contracts, slime starts showing up
     *
     * Stage 4: Pause for explanation or similar mid animation
     *
     * Stage 5: Medicine is administered
     * Stage 6: Quick acting medicine makes the muscle layer expand/return to original size.
     * Stage 7: Cells go away over time and tissue layer calms down.
     */
    
    /// <summary>
    /// Float from 0 to 1 that represents how contracted the shader and model for crossection is
    /// </summary>
    public float Contraction = 0;
    

    void OnEnable()
    {
        Speed = 1.0f;
        Time = 0.0f;
        Stage = 0; //Set to 0 here so we can start the animation at a spesific point from somewhere else
        Contraction = 0;
    }
}
