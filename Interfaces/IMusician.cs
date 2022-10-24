using SystemTypes;

namespace Interfaces;

public interface IMusician
{
    int SeatsRequired(SectionType section);
    void Play();
}