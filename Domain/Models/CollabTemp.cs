using Domain.Interfaces;

namespace Domain.Models;

public class CollaboratorTemp : ICollaboratorTemp
{
    public Guid Id { get; }
    public string Names { get; }
    public string Surnames { get; }
    public string Email { get; }
    public DateTime FinalDate { get; set; }
    public PeriodDateTime PeriodDateTime { get; private set; }

    public CollaboratorTemp(string names, string surnames, string email, DateTime finalDate, PeriodDateTime periodDateTime)
    {
        Id = Guid.NewGuid();
        Names = names;
        Surnames = surnames;
        Email = email;
        FinalDate = finalDate;
        PeriodDateTime = periodDateTime;
    }

    public CollaboratorTemp(Guid id, string names, string surnames, string email, DateTime finalDate, PeriodDateTime periodDateTime)
    {
        Id = id;
        Names = names;
        Surnames = surnames;
        Email = email;
        FinalDate = finalDate;
        PeriodDateTime = periodDateTime;
    }

    public bool ContractContainsDates(PeriodDateTime periodDateTime)
    {
        return PeriodDateTime.Contains(periodDateTime);
    }

    public void UpdatePeriod(PeriodDateTime period)
    {
        this.PeriodDateTime = period;
    }
}
