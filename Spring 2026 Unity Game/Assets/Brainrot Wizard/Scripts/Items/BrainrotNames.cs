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
    public Dictionary<Category, string> brainrotName = new Dictionary<Category, string>();

    public bool PrintDebug;

    public String[] SetupNames = new String[] {"Massive", "Italian", "Classical", "Trolly", "Iconic", "Honorable"};

    public BrainrotData brainrottest;

    private Attribute[][] setupPriorities = new Attribute[][] { new Attribute[]{Attribute.Massive}, new Attribute[]{Attribute.AI, Attribute.Surreal}, new Attribute[]{Attribute.Classic, Attribute.References}, new Attribute[]{Attribute.Classic, Attribute.Ironic}, new Attribute[]{Attribute.Tiktok, Attribute.TVShow, Attribute.Celebrities, Attribute.Movies}, new Attribute[]{Attribute.InRealLife, Attribute.Classic}};

    private void Start() {
        for (int i = 0; i < setupPriorities.Length; i++) {
            names[prioritylist[i]] = setupPriorities[i];
            brainrotName[prioritylist[i]] = SetupNames[i];
        }

        if (brainrottest != null && PrintDebug) {
            print(Name(brainrottest));
        }
    }

    public List<Category> MyCategories(BrainrotData brainrot) {
        List<Category> myCategories = hasAllOfCategories(brainrot.attributes);
        return myCategories;
    }

    public string Name(BrainrotData brainrot) {
        List<Category> myCategories = new List<Category>();
        myCategories = hasAllOfCategories(brainrot.attributes);
        string named = "Brainrot";
        if (myCategories.Count() == 0) {
            return "Disappointing Brainrot";
        }
        for (int i = myCategories.Count() - 1; i > -1; i--) {
            named = brainrotName[myCategories[i]] + " " + named;
        }
        return named;
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
        foreach (Category category in prioritylist) {
            if (hasAllOfCategory(category, listAttributes)) {
                export.Add(category);
            }
        }
        return export;
    }

    public bool hasAllOfCategory(Category category, List<Attribute> listGiven) {
        bool bufferbool = true;
        foreach (Attribute attribute in names[category]) {
            if (!listGiven.Contains(attribute)) {
                bufferbool = false;
            }
        }
        if (category == Category.PopCulture) {
            foreach (Attribute attribute in names[category]) {
            if (listGiven.Contains(attribute)) {
                bufferbool = true;
            }
        }
        }
        return bufferbool;
    }
}
