using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Runtime.CompilerServices;

public class BrainrotNames : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Dictionary<Category, Attribute[]> names = new Dictionary<Category, Attribute[]>();
    public Category[] prioritylist = new Category[] {Category.Massive, Category.Italian, Category.Throwback, Category.RageComic, Category.PopCulture, Category.Challenge};
    public Dictionary<Category, string> name = new Dictionary<Category, string>();

    private Attribute[][] setupPriorities = new Attribute[][] { new Attribute[]{Attribute.Massive}, new Attribute[]{Attribute.AI, Attribute.Surreal}, new Attribute[]{Attribute.Classic, Attribute.References}, new Attribute[]{Attribute.Classic, Attribute.Ironic}, new Attribute[]{Attribute.Tiktok, Attribute.TVShow, Attribute.Celebrities, Attribute.Movies}, new Attribute[]{Attribute.InRealLife, Attribute.Classic}};

    private void Start() {
        for (int i = 0; i < prioritylist.Length; i++) {
            
        }
    }
    
    public string Name(Brainrot brainrot) {
        List<Category> myCategories = new List<Category>();
        myCategories = hasAllOfCategories(brainrot.GetAttributes());
        return "Placeholder";
    }

    public List<Attribute> AttributesFromAttributesGiven(List<AttributeQuantity> input) {
        List<Attribute> output = new List<Attribute>();
        for (int i = 0; i < input.Count(); i++) {
            output.Add(input[i].attribute);
        }
        return output;
    }

    public List<Category> hasAllOfCategories(List<AttributeQuantity> listGiven) {
        List<Category> export = new List<Category>();
        var listAttributes = AttributesFromAttributesGiven(listGiven);
        print(listAttributes);
        foreach (Category category in prioritylist) {
            if (hasAllOfCategory(category, listAttributes)) {
                export.Add(category);
            }
        }
        return export;
    }

    public bool hasAllOfCategory(Category category, List<Attribute> listGiven) {
        foreach (Attribute attribute in names[category]) {
            if (!listGiven.Contains(attribute)) {
                return false;
            }
        }
        return true;
    }
}
