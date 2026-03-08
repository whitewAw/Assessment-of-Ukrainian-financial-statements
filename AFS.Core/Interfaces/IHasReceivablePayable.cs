namespace AFS.Core.Interfaces;

/// <summary>
/// Interface for receivable/payable assessment data
/// </summary>
public interface IHasReceivablePayable
{
    double ReceivableBase { get; }
    double ReceivableCurrent { get; }
    double PayableBase { get; }
    double PayableCurrent { get; }
    double ExceedingReceivableBase { get; }
    double ExceedingReceivableCurrent { get; }
    double ExceedingPayableBase { get; }
    double ExceedingPayableCurrent { get; }
}
