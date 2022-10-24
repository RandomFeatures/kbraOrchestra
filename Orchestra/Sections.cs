using Interfaces;
using SystemTypes;

namespace Orchestra;

/// <summary>
/// Manage each section in the orchestra
/// </summary>
internal class Section
{
    private readonly SectionType _section;
    public int MaxSeats { get; }
    public List<IMusician> Musicians { get; } = new();
    public Section(int maxSeats, SectionType section)
    {
        MaxSeats = maxSeats;
        _section = section;
    }
    /// <summary>
    /// Attempt to put the musician into the section
    /// </summary>
    /// <param name="musician">Instance of the musician that is joining the section</param>
    /// <returns>The result of success or identifying why the musician was rejected </returns>
    /// <exception cref="ArgumentNullException"></exception>
    public OccupySeatRspType OccupySeats(IMusician musician)
    {
        if (musician == null) throw new ArgumentNullException(nameof(musician));
        //Is the section full?
        if (CountSeatsRemaining() == 0)
            return OccupySeatRspType.SectionFull;
        //Is the musician allowed in this section?
        if (musician.SeatsRequired(_section) == 0)
            return OccupySeatRspType.NotAllowed;
        //Is there enough room for the musician in this section?
        if (CountSeatsRemaining() < musician.SeatsRequired(_section))
            return OccupySeatRspType.NotEnoughSeats;

        Musicians.Add(musician);
       
        return OccupySeatRspType.Accepted;
    }
    /// <summary>
    ///  Remove a given musician from the seciton
    /// </summary>
    /// <param name="musician">Instance of the musician that is leaving the section</param>
    /// <returns>true of the musician was found and removed</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public bool VacateSeats(IMusician  musician)
    {
        if (musician == null) throw new ArgumentNullException(nameof(musician));
        return Musicians.Remove(musician);
    }
    /// <summary>
    /// Count the number of seats being occupied in the section.
    /// Note: this is not necessarily the same as the number of musicians
    /// </summary>
    /// <returns>the number of occupied seats</returns>
    public int CountSeatsOccupied()
    {
       return Musicians.Sum(x => x.SeatsRequired(_section));
    }
    /// <summary>
    /// Count the number of seats remaining open in the section
    /// </summary>
    /// <returns>the number of unoccupied seats</returns>
    public int CountSeatsRemaining()
    {
        return MaxSeats - CountSeatsOccupied();
    }
    /// <summary>
    /// Counts the number of a given type of musicians in the section
    /// </summary>
    /// <typeparam name="T">Class type of the musician to count</typeparam>
    /// <returns>number of musicians</returns>
    public int CountMusicianByType<T>() where T : Musician
    {
        return Musicians.OfType<T>().Count();
    }
    /// <summary>
    /// Get a list of a given type of musicians in the section
    /// </summary>
    /// <typeparam name="T">Class type of the musician to return</typeparam>
    /// <returns>List of musicians</returns>
    public List<T> GetMusiciansByType<T>() where T : Musician
    {
        return Musicians.OfType<T>().ToList();
    }

}