using UnityEngine;

public class WaterSeed : MonoBehaviour
{
    public int waterCost = 1;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryWaterPlant();
        }
    }

    void TryWaterPlant()
    {
        if (PlayerInv.water >= waterCost)
        {
            PlayerInv.water -= waterCost;
            Debug.Log("Plant watered! Water left: " + PlayerInv.water);

        }
        else
        {
            Debug.Log("Not enough water!");
        }
    }
}
