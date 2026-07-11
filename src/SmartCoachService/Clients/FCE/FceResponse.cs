namespace SmartCoachService.Clients.FCE
{
    public sealed record FceResponse(decimal Weight,
                                     decimal Height,
                                     DateOnly BirthDate,
                                     string Gender,
                                     string Goal,
                                     string ActivityLevel);
}
 