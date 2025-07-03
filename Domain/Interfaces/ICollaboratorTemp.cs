using Domain.Models;

namespace Domain.Interfaces;

public interface ICollaboratorTemp
{
    public Guid Id { get; }
    public string Names { get; }
    public string Surnames { get; }
    public string Email { get; }
    public DateTime FinalDate { get; }
    public PeriodDateTime PeriodDateTime { get; }
    public Guid? UserId { get; set; }
    public bool ContractContainsDates(PeriodDateTime periodDateTime);
    public void UpdatePeriod(PeriodDateTime period);
}
