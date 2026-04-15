using UnityEngine;
using System.Collections.Generic;

public class BrainrotDisplayUI : MonoBehaviour
{
    public Brainrot mybrain;
    public BrainrotData mybraindebug;
    public BrainrotNames mybrainrotnames;
    public List<bool> mycategories;
    public List<GameObject> mybrainlayers;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (mybraindebug is not null) {
            Brainrot newbrainrot = new Brainrot(mybraindebug, "Chuck Testa");
            mybrain = newbrainrot;
            Begin(mybrain);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Begin(Brainrot brainrot) {
        mybrain = brainrot;
        UpdateRot();
    }

    void UpdateRot() {
        mycategories = mybrainrotnames.MyCategoriesHas(mybrainrotnames.MyCategories(mybrain), mybrainrotnames.prioritylist);
        for (int i = 0; i < mycategories.Count; i++) {
            mybrainlayers[i].active = mycategories[i];
        }
    }
}
