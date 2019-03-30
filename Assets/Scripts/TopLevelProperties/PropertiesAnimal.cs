using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This properties component contains all of the publicly availible properties specific to animal type entities, such as fish.   
/// These top level components will serve as an interface which allows the various entities in the reef to know how to interact with one another.   
/// Even though some entities may technically be considered animals by real science, such as coral or tube worms, please apply the coral component to them instead.
/// </summary>
public class PropertiesAnimal : PropertiesBase
{
    /// <summary>
    /// The diet that will indicate how this animal attempts to feed itself.
    /// </summary>
    [Tooltip("The diet that will indicate how this animal attempts to feed itself.")]
    public Diet diet;
    /// <summary>
    /// The activity which the AI of this animal is currently engaged in.
    /// </summary>
    [Tooltip("The activity which the AI of this animal is currently engaged in.")]
    public Activity activity;
    /// <summary>
    /// The relative ability of this animal to kill a similarly sized animal in a conflict. 
    /// </summary>
    [Tooltip("The relative ability of this animal to kill a similarly sized animal in a conflict.")]
    public Strength strength;
    /// <summary>
    /// The relative ability of this animal to out-run a similarly sized animal.  Not nessicarily a literal repersentation if it's actual meters/second speed.
    /// </summary>
    [Tooltip("The relative ability of this animal to out-run a similarly sized animal.")]
    public Speed speed;
}
