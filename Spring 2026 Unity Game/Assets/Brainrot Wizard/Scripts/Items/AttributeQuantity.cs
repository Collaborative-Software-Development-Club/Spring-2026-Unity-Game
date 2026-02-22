using System;
using UnityEngine;

[Serializable]
public class AttributeQuantity 
{
   public AttributeQuantity(Attribute attribute, int quantity)
   {
      this.attribute = attribute;
      this.quantity = quantity;
   }

   public AttributeQuantity()
   {
      attribute = Attribute.None; 
      quantity = 0;
   }
   
   public Attribute attribute;
   public int quantity;
}
