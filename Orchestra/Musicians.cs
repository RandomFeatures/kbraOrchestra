using Interfaces;
using SystemTypes;

namespace Orchestra;


/// <summary>
/// base class for musician
/// </summary>
public abstract class Musician : IMusician
{
    /// <summary>
    /// business rules to track what sections a musician can sit in and how many seats they require
    /// </summary>
    protected readonly Dictionary<SectionType, int> SectionSeats;

    /// <summary>
    /// This constructor forces inherited classes to init sectionSeats
    /// </summary>
    /// <param name="sectionSeats">Dict defining the seating requirements</param>
    protected Musician(Dictionary<SectionType, int> sectionSeats)
    {
        SectionSeats = sectionSeats;
    }
    /// <summary>
    /// validate this musician can occupy a section and if so how many seats required
    /// </summary>
    /// <param name="section">the section type to verify</param>
    /// <returns>how many/if any seats are required</returns>
    public int SeatsRequired(SectionType section)
    {
        //return 0 if the musician is not allowed in the section
        return SectionSeats.ContainsKey(section) ? SectionSeats[section] : 0;
    }
    /// <summary>
    /// play the instrument
    /// </summary>
    public abstract void Play();
}

/// <summary>
/// implementation of the violinist 
/// </summary>
public class Violinist : Musician
{
    public Violinist() : base
    (
        new Dictionary<SectionType, int>()
        {
            { SectionType.Violin, 1 },
            { SectionType.Cello, 1 },
            { SectionType.Bass, 1 }
        }
    )
    {
    }

    public override void Play()
    {
        return;
    }
}

/// <summary>
/// implementation of the cellist 
/// </summary>
public class Cellist : Musician
{
    public Cellist() : base
    (
        new Dictionary<SectionType, int>()
        {
            { SectionType.Cello, 1 },
            { SectionType.Bass, 1 }
        }
    )
    {
    }
    
    public override void Play()
    {
        return;
    }
}

/// <summary>
/// implementation of the bassist 
/// </summary>
public class Bassist : Musician
{
    public Bassist() : base
    (
        new Dictionary<SectionType, int>()
        {
            { SectionType.Cello, 2 },
            { SectionType.Bass, 1 }
        }
    )
    {
    }
    
    public override void Play()
    {
        return;
    }
}