// SalomDunyoEndpoint.cs
public static class SalomDunyoEndpoint
{
    public static void MapSalomDunyo(this WebApplication app)
    {
        app.MapGet("/salom-dunyo", () => Results.Ok(new { message = "Salom, Dunyo!" }));
    }
}