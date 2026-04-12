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
    None,
    AI,
    Animal,
    Surreal,
    Ironic,
    Classic,
    References,
    Massive,
    Movies,
    TVShow,
    Celebrities,
    Tiktok,
    InRealLife,
    Iconic
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
