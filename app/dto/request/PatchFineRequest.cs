using sharing_bikes.net.Enum;

namespace sharing_bikes.net.dto.request;

public record PatchFineRequest
{
    public Decimal? Amount { get; init; }
    public FineStatus? FineStatus { get; init; }

    public PatchFineRequest(Decimal? amount, FineStatus? fineStatus)
    {
        Amount = amount;
        FineStatus = fineStatus;
    }
}