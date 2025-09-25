namespace TestManager.Enum
{
    public enum EntityType
    {
        Appointments = 3,
        Product = 40,
        Andrologist = 11,
        AppointmentRules = 1020,
        DicommModality = 1021
    }

    public enum AccuroObservationResultStatus : byte
    {
        ReviewedResults,
        UploadedResults,
        Delayed48Hours,
        DoNotUpload
    }
}
