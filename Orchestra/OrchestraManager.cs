using Interfaces;
using SystemTypes;
namespace Orchestra;
/// <summary>
/// Manage all aspects of setting up the orchestra
/// </summary>
public class OrchestraManager
{
    private readonly Section _violin;
    private readonly Section _cello;
    private readonly Section _bass;

    public OrchestraManager(int violinMax = 16, int celloMax = 12, int bassMax = 8)
    {
        _violin = new Section(violinMax, SectionType.Violin);
        _cello = new Section(celloMax, SectionType.Cello);
        _bass = new Section(bassMax, SectionType.Bass);
    }
    
    /// <summary>
    ///  Allows musicians to join the orchestra and decide where they will sit
    /// </summary>
    /// <param name="musician">Instance of the musician that is joining the orchestra</param>
    /// <param name="section">The section that the new musician should sit in</param>
    /// <returns>true if the musician was allowed in the section and there was room</returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public bool JoinOrchestra(IMusician musician, SectionType section)
    {
        try
        {
            if (musician == null) throw new ArgumentNullException(nameof(musician));
            var rtn = false;
            //verify that this musician is not already in the Orchestra
            //if they are, boot them out, then put them back in
            LeaveOrchestra(musician);

            var rps = section switch
            {
                SectionType.Violin => _violin.OccupySeats(musician),
                SectionType.Cello => _cello.OccupySeats(musician),
                SectionType.Bass => _bass.OccupySeats(musician),
                _ => throw new ArgumentOutOfRangeException(nameof(section), section, null)
            };

            switch (rps)
            {
                case OccupySeatRspType.NotAllowed:
                    Logger.Log($"{musician.GetType().Name} is not allowed in {section.ToString()}");
                    break;
                case OccupySeatRspType.NotEnoughSeats:
                    Logger.Log($"{section.ToString()} does not have enough free seats for a {nameof(musician)}");
                    break;
                case OccupySeatRspType.SectionFull:
                    Logger.Log($"{section.ToString()} is full.");
                    break;
                case OccupySeatRspType.Accepted:
                    Logger.Log($"{musician.GetType().Name} was able to join {section.ToString()}");
                    rtn = true;
                    break;
            }

            //return true if the musician was allowed in the section and there was room
            return rtn;
        }
        catch (ArgumentNullException e)
        {
            Logger.Log($"parameter: {e.ParamName} can not be null");
            return false;
        }
        catch (ArgumentOutOfRangeException e)
        {
            Logger.Log($"parameter: '{e.ParamName}' value: '{e.ActualValue}' is not a valid section");
            return false;
        }
    }

    /// <summary>
    ///  Find the musician and remove them from the orchestra
    /// </summary>
    /// <param name="musician"></param>
    /// <returns>true or false indicating if the musician was found in the orchestra</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public bool LeaveOrchestra(IMusician musician)
    {
        try
        {
            if (musician == null) throw new ArgumentNullException(nameof(musician));

            var rtn = _violin.VacateSeats(musician);
            if (!rtn) rtn = _cello.VacateSeats(musician);
            if (!rtn) rtn = _bass.VacateSeats(musician);
            //return false if the musician was found in the orchestra
            return rtn;
        }
        catch (ArgumentNullException e)
        {
            Logger.Log($"parameter: {e.ParamName} can not be null");
            return false;
        }
    }
    /// <summary>
    ///  Does a quick count of all the sections max number of seats
    /// </summary>
    /// <returns>the max seat count</returns>
    public int CountTotalSeats()
    {
        return _violin.MaxSeats + _cello.MaxSeats + _bass.MaxSeats;
    }
    /// <summary>
    /// Adds up the unoccupied seats from all the sections
    /// </summary>
    /// <returns>total empty seats</returns>
    public int CountEmptySeats()
    {
        return _violin.CountSeatsRemaining() + _cello.CountSeatsRemaining() + _bass.CountSeatsRemaining();
    }
    /// <summary>
    /// Quickly determine if the orchestra is full
    /// </summary>
    /// <returns>full or not</returns>
    public bool IsFull() => CountEmptySeats() == 0;
    /// <summary>
    /// Quickly determine if the orchestra is empty
    /// </summary>
    /// <returns>empty or not</returns>
    public bool IsEmpty() => CountEmptySeats() == CountTotalSeats();
    /// <summary>
    /// Determine if a given section is full
    /// </summary>
    /// <param name="section"></param>
    /// <returns>the given section is full or not</returns>
    public bool IsSectionFull(SectionType section)
    {
        return section switch
        {
            SectionType.Violin => _violin.CountSeatsRemaining() == 0,
            SectionType.Cello => _cello.CountSeatsRemaining() == 0,
            SectionType.Bass => _bass.CountSeatsRemaining() == 0,
            _ => false
        };
    }
    /// <summary>
    /// Get a count of all the Bassist, Cellist, or Violinist in the orchestra
    /// </summary>
    /// <typeparam name="T">Class type of the musician to count</typeparam>
    /// <returns>total number of musicians</returns>
    public int CountMusicians<T>() where T : Musician
    {
        return _violin.CountMusicianByType<T>() + _cello.CountMusicianByType<T>() + _bass.CountMusicianByType<T>();
    }
    /// <summary>
    /// Get a list of all the Bassist, Cellist, or Violinist in the orchestra
    /// </summary>
    /// <typeparam name="T">Class type of the musician to return</typeparam>
    /// <returns>A list of musicians</returns>
    public List<T> GetMusicianList<T>() where T : Musician
    {
        var rtn = _violin.GetMusiciansByType<T>();
        rtn.AddRange(_cello.GetMusiciansByType<T>());
        rtn.AddRange(_bass.GetMusiciansByType<T>());
        return rtn;
    }
    /// <summary>
    /// Get a list of all the musicians in a given section
    /// </summary>
    /// <param name="section">The section to search</param>
    /// <returns>A list of musicians in the section</returns>
    public List<IMusician> GetMusiciansBySection(SectionType section)
    {
        return section switch
        {
            SectionType.Violin => _violin.Musicians,
            SectionType.Cello => _cello.Musicians,
            SectionType.Bass => _bass.Musicians,
        };
    }
}