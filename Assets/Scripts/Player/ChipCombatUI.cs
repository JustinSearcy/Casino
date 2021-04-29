using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChipCombatUI : MonoBehaviour
{
    [SerializeField] TextMeshPro chipCountText = null;
    [SerializeField] GameObject chipLossParticles = null;

    public void UpdateChipText(int currentChipCount)
    {
        chipCountText.text = "x" + currentChipCount;
    }

    public void UpdateAndLoseChips(int currentChipCount)
    {
        chipLossParticles.GetComponent<ParticleSystem>().Play();
        chipCountText.text = "x" + currentChipCount;
    }
}
