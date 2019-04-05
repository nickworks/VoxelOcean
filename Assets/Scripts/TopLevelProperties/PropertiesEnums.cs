
/// <summary>
/// Enumerator to track the movement capabilities of any given entity.
/// Typically coral and plants will be "immobile" and fish will be "swimming", however there are exceptions such as a free drifting piece of kelp.
/// </summary>
public enum Mobility
{
    Swimming,
    Crawling,
    Drifting,
    Immobile
};

/// <summary>
/// Enumerator to track the dietary habits of any given animal entity.
/// </summary>
public enum Diet
{
    Carnivore,
    Herbivore,
    Omnivore
};

/// <summary>
/// Enumerator to track any non-conventional special attributes of any given entity.
/// This section will be expanded as needed as new fringe case animals are introduced.
/// </summary>
public enum Special
{
    Poisonous,
    Spikey,
    BioLuminescent,
    Camo
};

/// <summary>
/// Enumerator to track the current activity that any given animal entity is currently performing.
/// </summary>
public enum Activity
{
    Idle,
    Resting,
    Fleeing,
    Hunting,
    Wandering,
    Scavanging
};

/// <summary>
/// Enumerator to track how easy it is to pass through any given non-animal entity.
/// Rocks, for example, would be solid, while a flimsy plant like kelp would likely be LowDense.
/// </summary>
public enum Tangibility
{
    Solid,
    HighDense,
    MidDense,
    LowDense,
    PassThrough
};

/// <summary>
/// Enumerator to track how durable any given entity is.
/// </summary>
public enum Defense
{
    Invincible,
    HighDef,
    MidDef,
    LowDef,
    Frail
}

/// <summary>
/// Enumerator to track aproximately how large any given entity is.
/// </summary>
public enum Size
{
    Huge,
    Large,
    Mid,
    Small,
    Tiny
}

/// <summary>
/// Enumerator to track how fast any given animal is relatie to its size.
/// </summary>
public enum Speed
{
    LightSpeed,
    HighSpeed,
    MidSpeed,
    LowSpeed,
    Slow
}

/// <summary>
/// Enumerator to track how offensively powerful any given animal is relative to its size.
/// </summary>
public enum Strength
{
    OnePunch,
    HighStrength,
    MidStrength,
    LowStrength,
    Weak
}
