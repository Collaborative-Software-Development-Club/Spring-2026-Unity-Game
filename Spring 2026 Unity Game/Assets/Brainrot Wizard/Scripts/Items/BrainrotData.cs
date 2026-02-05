using System;
using System.Collections.Generic;
using UnityEngine;


public enum Category 
{
    None,
    Italian,
    Throwback,
    Massive,
    RageComic,
    PopCulture,
    Challenge
}

public enum Attribute
{
    AI,
    Animal,
    Surreal,
    Ironic,
    Classic,
    References,
    DamageMultiplier,
    Movies,
    TVShow,
    Celebrities,
    Tiktok,
    InRealLife
}

[CreateAssetMenu(fileName = "New Brainrot", menuName = "Game/BrainrotMixer/Item/Brainrot")]
public class BrainrotData : ItemData 
{
    public Category category;
    public List<AttributeQuantity> attributes;

    private void OnEnable()
    {
        type = ItemType.Brainrot;
    }
}
