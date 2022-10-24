using Orchestra;
using NUnit.Framework;
using SystemTypes;

namespace OrchestraTest;

public class OrchestraTests
{
    private OrchestraManager _orchestra;
    
    [SetUp]
    public void Setup()
    {
        _orchestra = new OrchestraManager(2,3,3);
    }

    private void CleanUp()
    {
        foreach (var violinist in _orchestra.GetMusicianList<Violinist>())
            _orchestra.LeaveOrchestra(violinist);

        foreach (var violinist in _orchestra.GetMusicianList<Cellist>())
            _orchestra.LeaveOrchestra(violinist);

        foreach (var violinist in _orchestra.GetMusicianList<Bassist>())
            _orchestra.LeaveOrchestra(violinist);
    }
    
    [Test]
    public void JoinTheOrchestra()
    {
       //join a valid section 
       Assert.True(_orchestra.JoinOrchestra(new Bassist(), SectionType.Bass));
       //join invalid section
       Assert.False(_orchestra.JoinOrchestra(new Bassist(), SectionType.Violin));
       //join a section then join another section
       var cellist = new Cellist();
       Assert.True(_orchestra.JoinOrchestra(cellist, SectionType.Cello));
       Assert.True(_orchestra.JoinOrchestra(cellist, SectionType.Bass));
       Assert.AreEqual(_orchestra.GetMusiciansBySection(SectionType.Cello).Count, 0);
       Assert.AreEqual(_orchestra.GetMusiciansBySection(SectionType.Bass).Count, 2);
       
       //Try to join a full section
       _orchestra.JoinOrchestra(new Bassist(), SectionType.Bass);
       Assert.False(_orchestra.JoinOrchestra(new Bassist(), SectionType.Bass));

       CleanUp();
    }

    [Test]
    public void LeaveTheOrchestra()
    {
        var bassist1 = new Bassist();
        var bassist2 = new Bassist();
        var cellist1 = new Cellist();
        var cellist2 = new Cellist();

        _orchestra.JoinOrchestra(bassist1, SectionType.Bass);
        _orchestra.JoinOrchestra(bassist2, SectionType.Bass);
        _orchestra.JoinOrchestra(cellist1, SectionType.Cello);
        _orchestra.JoinOrchestra(cellist2, SectionType.Cello);
        _orchestra.JoinOrchestra(new Violinist(), SectionType.Violin);
        _orchestra.JoinOrchestra(new Violinist(), SectionType.Cello);
        _orchestra.JoinOrchestra(new Violinist(), SectionType.Bass);
       
        //verify the setup above worked as expected
        Assert.AreEqual(_orchestra.GetMusiciansBySection(SectionType.Bass).Count, 3);
        Assert.AreEqual(_orchestra.GetMusiciansBySection(SectionType.Cello).Count, 3);
        Assert.AreEqual(_orchestra.GetMusiciansBySection(SectionType.Violin).Count, 1);
        
        //verify removing specific items
        Assert.True(_orchestra.LeaveOrchestra(bassist1)); 
        Assert.AreEqual(_orchestra.GetMusiciansBySection(SectionType.Bass).Count, 2);
        Assert.True(_orchestra.LeaveOrchestra(bassist2));
        Assert.AreEqual(_orchestra.GetMusiciansBySection(SectionType.Bass).Count, 1);
        Assert.True(_orchestra.LeaveOrchestra(cellist1));
        Assert.AreEqual(_orchestra.GetMusiciansBySection(SectionType.Cello).Count, 2);
        Assert.True(_orchestra.LeaveOrchestra(cellist2)); 
        Assert.AreEqual(_orchestra.GetMusiciansBySection(SectionType.Cello).Count, 1);
        //This one was already removed. verify it acts accordingly 
        Assert.False(_orchestra.LeaveOrchestra(cellist2));
        //remove all of a given type
        foreach (var violinist in _orchestra.GetMusicianList<Violinist>())
            _orchestra.LeaveOrchestra(violinist);
        //verify final counts
        Assert.AreEqual(_orchestra.GetMusiciansBySection(SectionType.Violin).Count, 0);
        Assert.AreEqual(_orchestra.GetMusiciansBySection(SectionType.Cello).Count, 0);
        Assert.AreEqual(_orchestra.GetMusiciansBySection(SectionType.Bass).Count, 0);
        Assert.True(_orchestra.IsEmpty());

    }

    [Test]
    public void CountSeats()
    {
        //verify totals from setup
        Assert.AreEqual(_orchestra.CountTotalSeats(), 8);
        Assert.AreEqual(_orchestra.CountEmptySeats(), 8);
        
        //add some test data
        _orchestra.JoinOrchestra(new Violinist(), SectionType.Violin);
        _orchestra.JoinOrchestra(new Violinist(), SectionType.Cello);
        _orchestra.JoinOrchestra(new Violinist(), SectionType.Bass);
        //verify totals again        
        Assert.AreEqual(_orchestra.CountEmptySeats(), 5);
        Assert.AreEqual(_orchestra.CountMusicians<Violinist>(), 3);
        
        CleanUp();
    }

    [Test]
    public void FillThingsUp()
    {
        //verify totals from setup
        Assert.AreEqual(_orchestra.CountTotalSeats(), 8);
        Assert.AreEqual(_orchestra.CountEmptySeats(), 8);
        //verify orchestra is empty
        Assert.True(_orchestra.IsEmpty());
        //verify orchestra is not full
        Assert.False(_orchestra.IsFull());
        
        //add some test data
        _orchestra.JoinOrchestra(new Violinist(), SectionType.Violin);
        _orchestra.JoinOrchestra(new Violinist(), SectionType.Violin);
        //verify violin is full
        Assert.True(_orchestra.IsSectionFull(SectionType.Violin));
        
        //add some more test data
        _orchestra.JoinOrchestra(new Violinist(), SectionType.Cello);
        //verify cello is not full
        Assert.False(_orchestra.IsSectionFull(SectionType.Cello));
        
        //verify orchestra is not full
        Assert.False(_orchestra.IsFull());
        //add some more test data
        _orchestra.JoinOrchestra(new Violinist(), SectionType.Cello);
        _orchestra.JoinOrchestra(new Violinist(), SectionType.Cello);
        _orchestra.JoinOrchestra(new Violinist(), SectionType.Bass);
        _orchestra.JoinOrchestra(new Violinist(), SectionType.Bass);
        _orchestra.JoinOrchestra(new Violinist(), SectionType.Bass);
        //verify orchestra is full
        Assert.True(_orchestra.IsFull());
        
        CleanUp();
    }

    [Test]
    public void GetTheMusicians()
    {
        //add some test data
        _orchestra.JoinOrchestra(new Violinist(), SectionType.Violin);
        _orchestra.JoinOrchestra(new Violinist(), SectionType.Violin);
        _orchestra.JoinOrchestra(new Violinist(), SectionType.Cello);
        _orchestra.JoinOrchestra(new Violinist(), SectionType.Cello);
        _orchestra.JoinOrchestra(new Violinist(), SectionType.Bass);
        _orchestra.JoinOrchestra(new Violinist(), SectionType.Bass);
        _orchestra.JoinOrchestra(new Violinist(), SectionType.Bass);

        //verify the counts
        Assert.AreEqual(_orchestra.GetMusicianList<Violinist>().Count, 7);
        Assert.AreEqual(_orchestra.GetMusiciansBySection(SectionType.Bass).Count, 3);
        Assert.AreEqual(_orchestra.CountMusicians<Violinist>(), 7);
        
        CleanUp();
    }
}