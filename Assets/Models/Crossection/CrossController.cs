using UnityEngine;

public class CrossController : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;
    [SerializeField] private int blendShapeIndex = 0;
    [SerializeField] private float maxMuscleValue = 0.23f;
    [SerializeField] private float maxTissueValue = 1f;
    private float MuscleFloat = 0f;
    private float TissueFloat = 0f;
    private Material material;
    
    
    //Timing variables
    public SharedTimingData sharedTimingData;
    
    /*
     * Stages of animation:
     *  Stage1: The muscle layer contracts
     *  Stage2: Some cells starts to show up.
     *  Stage3: The tissue gets irritated and contracts, slime starts showing up
     *
     * Stage 4: Pause for explanation or similar mid-animation
     *
     * Stage 5: Medicine is administered
     * Stage 6: Quick acting medicine makes the muscle layer expand/return to original size.
     * Stage 7: Cells go away over time and tissue layer calms down.
     */
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        material = skinnedMeshRenderer.material;
        skinnedMeshRenderer.SetBlendShapeWeight(blendShapeIndex, 0);
        material.SetFloat("_MainSlider1", 0);

    }

    // Update is called once per frame
    void Update()
    {
        //Stages of animation
        switch (sharedTimingData.Stage)
        {
            case 1:
                sharedTimingData.Time += (Time.deltaTime * sharedTimingData.Speed); //Updates passed time for both cells and crossection
                if (MuscleFloat < maxMuscleValue)
                {
                    material.SetFloat("_MainSlider2", MuscleFloat);
                    MuscleFloat += (maxMuscleValue / 15f) * (Time.deltaTime * sharedTimingData.Speed);
                    if (TissueFloat < (maxTissueValue * 0.2f))
                    {
                        skinnedMeshRenderer.SetBlendShapeWeight(blendShapeIndex, TissueFloat * 100);
                        material.SetFloat("_MainSlider1", TissueFloat);
                        TissueFloat += ((maxTissueValue * 0.2f) / 15f) * (Time.deltaTime * sharedTimingData.Speed);
                        sharedTimingData.Contraction = TissueFloat;
                    }
                }
                else
                {
                    sharedTimingData.Stage = 2;
                }
                break;
            case 3:
                if (TissueFloat < maxTissueValue)
                {
                    skinnedMeshRenderer.SetBlendShapeWeight(blendShapeIndex, TissueFloat * 100);
                    material.SetFloat("_MainSlider1", TissueFloat);
                    TissueFloat += (maxTissueValue / 10f) * (Time.deltaTime * sharedTimingData.Speed);
                    sharedTimingData.Contraction = TissueFloat;
                }
                break;
            case 6:
                sharedTimingData.Time += (Time.deltaTime * sharedTimingData.Speed); //Updates passed time for both cells and crossection
                if (MuscleFloat > 0)
                {
                    material.SetFloat("_MainSlider2", MuscleFloat);
                    MuscleFloat -= (1f / 100f) * (Time.deltaTime * sharedTimingData.Speed);
                }
                else
                {
                    sharedTimingData.Stage = 7;
                }

                break;
            case 7:
                if (TissueFloat > 0)
                {
                    skinnedMeshRenderer.SetBlendShapeWeight(blendShapeIndex, TissueFloat * 100);
                    material.SetFloat("_MainSlider1", TissueFloat);
                    TissueFloat -= (1f / 100f) * (Time.deltaTime * sharedTimingData.Speed);
                    sharedTimingData.Contraction = TissueFloat;
                }
                break;
                
        }
        
    }
}
