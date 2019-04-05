using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This properties component contains all of the publicly availible properties specific to non-animal type entities, such as coral.   
/// These top level components will serve as an interface which allows the various entities in the reef to know how to interact with one another.   
/// This base class shouldn't be used directly, please use PropertiesPlant, PropertiesCoral, or PropertiesOther.
/// </summary>
public class PropertiesNonAnimal : PropertiesBase
{
    /// <summary>
    /// The physical resistance which this entity represents to animals attempting to pass through it.
    /// </summary>
    [Tooltip("The physical resistance which this entity presents to animals attempting to pass through it.")]
    public Tangibility tangibility;
}
