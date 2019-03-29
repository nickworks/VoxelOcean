using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The root base for the properties components which contains all public properties which all entities share.  
/// These top level components will serve as an interface which allows the various entities in the reef to know how to interact with one another.   
/// This base class shouldn't be used directly, please use PropertiesAnimal, PropertiesPlant, PropertiesCoral, or PropertiesOther.
/// </summary>
public class PropertiesBase : MonoBehaviour
{
    /// <summary>
    /// A shared, static integer which ensures that every generated entity has a unique id value.
    /// </summary>
    protected static int runningID;
    /// <summary>
    /// The privately stored core id value of this entity.  Accessed via id.
    /// </summary>
    protected string _id;
    /// <summary>
    /// The publicly exposed access to this entity's _id value.  Upon first being called, it will incriment runningID to ensure a unique id value;
    /// </summary>
    public string id {
        get{
            if (_id == null) _id = "id:" + runningID++;

            return _id;
        }
    }
    /// <summary>
    /// The name of the student who initially created this entity.
    /// </summary>
    public string creator;

    /// <summary>
    /// The general classification of this entity, primarly intended for use by schooling entities to find one another.  
    /// For fish and coral the label "species" should be used literally, for something like a rock just set the species to "volcanicRock" or something.
    /// </summary>
    public string species;
    /// <summary>
    /// The mobility capabilities of this entity.  For most fish it will probably be swimming, and for most other entities it will probably be immobile.
    /// </summary>
    public Mobility mobility;
    /// <summary>
    /// A list of all the special attributes this entity has.  For example, an octopus may have both camo and poisonous, depending on the species.
    /// Most entities will likely have no special attributes.
    /// </summary>
    public List<Special> special;
    /// <summary>
    /// A float representing the physical durability of this entity relative to its size.  
    /// For example a turtule or snail would have a very high value, while a squid would have a low one.  
    /// Recommended to keep in the range of 1 - 10, however going above or below is permitted if an extreme case demands it.
    /// </summary>
    public float defense;
    /// <summary>
    /// A float representing the aproximate size of this entity.  
    /// This is not intended to be a literal representation of the size of this entity by any actual unity, just about how large it is relative to other entities.
    /// </summary>
    public float size;

    /// <summary>
    /// A publicly accessable reference to all entities within the game.
    /// </summary>
    public static Dictionary<string, PropertiesBase> all = new Dictionary<string, PropertiesBase>();
    /// <summary>
    /// A publicly accessable reference to all animals within the game.
    /// </summary>
    public static Dictionary<string, PropertiesAnimal> animals = new Dictionary<string, PropertiesAnimal>();
    /// <summary>
    /// A publicly accessable reference to all plants within the game.
    /// </summary>
    public static Dictionary<string, PropertiesPlant> plants = new Dictionary<string, PropertiesPlant>();
    /// <summary>
    /// A publicly accessable reference to all coral within the game.
    /// </summary>
    public static Dictionary<string, PropertiesCoral> coral = new Dictionary<string, PropertiesCoral>();
    /// <summary>
    /// A publicly accessable reference to all entities within the game that do not fit into the other catagories, such as rocks.
    /// </summary>
    public static Dictionary<string, PropertiesOther> others = new Dictionary<string, PropertiesOther>();

    /// <summary>
    /// This OnEnable logic is used to add this entity to all dictionaries which accurately describe it and do not already contain it.
    /// </summary>
    private void OnEnable()
    {
        // add yourself to the all collection
        if (!all.ContainsKey(id)) all.Add(id, this);

        //  add yourself to the animals collection
        if (this is PropertiesAnimal && !animals.ContainsKey(id))
        {
            animals.Add(id, this as PropertiesAnimal);
        }// add yourself to the plants collection
        else if (this is PropertiesPlant && !plants.ContainsKey(id))
        {
            plants.Add(id, this as PropertiesPlant);
        }// add yourself to the coral collection
        else if (this is PropertiesCoral && !coral.ContainsKey(id))
        {
            coral.Add(id, this as PropertiesCoral);
        }// add yourself to the others collection
        else if (this is PropertiesOther && !others.ContainsKey(id))
        {
            others.Add(id, this as PropertiesOther);
        }
    }
    /// <summary>
    /// This OnDisable logic is used to remove this entity from all dictionaries which accurately describe it and contain it.
    /// </summary>
    private void OnDisable()
    {
        RemoveFromDicts();
    }
    /// <summary>
    /// This OnDestroy logic is used to remove this entity from all dictionaries which accurately describe it and contain it.
    /// </summary>
    private void OnDestroy()
    {
        RemoveFromDicts();
    }
    /// <summary>
    /// This logic is used by OnDisable and OnDestroy to remove this entity from all dictionaries which accurately describe it and contain it.
    /// </summary>
    private void RemoveFromDicts()
    {
        // remove yourself to the all collection
        if (all.ContainsKey(id)) all.Remove(id);

        //  remove yourself to the animals collection
        if (this is PropertiesAnimal && animals.ContainsKey(id))
        {
            animals.Remove(id);
        }// remove yourself to the plants collection
        else if (this is PropertiesPlant && plants.ContainsKey(id))
        {
            plants.Remove(id);
        }// remove yourself to the coral collection
        else if (this is PropertiesCoral && coral.ContainsKey(id))
        {
            coral.Remove(id);
        }// remove yourself to the others collection
        else if (this is PropertiesOther && others.ContainsKey(id))
        {
            others.Remove(id);
        }
    }
}

