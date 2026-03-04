Anleitung von Gemini
https://share.google/aimode/lLNbLVVCYf6iDvIw4

Dokumentation: OpenAPI zu C# mit Microsoft Kiota
1. Tool-Empfehlungen
Für die Generierung von C#-Code aus OpenAPI-Spezifikationen sind folgende Tools führend:
NSwag: Beste Integration in .NET/Visual Studio, bietet eine GUI (NSwagStudio).
Microsoft Kiota: Modern, generiert schlanken Fluent-Code, ideal für große APIs.
OpenAPI Generator: Plattformübergreifend, extrem viele Konfigurationsmöglichkeiten.
Refitter: Erzeugt Interfaces für die Library "Refit".
2. Verwendung von Microsoft Kiota
Installation:
dotnet tool install --global Microsoft.OpenApi.Kiota
Generierung:
kiota generate -l CSharp -o ./GeneratedClient -n MyApp.Client -d [URL/DATEI]
Nutzung im Code:
Der Client nutzt einen RequestAdapter und einen AuthenticationProvider. Der Aufruf erfolgt über eine Fluent-API: await client.Path.To.Resource.GetAsync();
3. Fehlerbehebung: Datentyp-Konflikte (int32)
Problem: Kiota gibt Warnungen aus, dass int32 nicht unterstützt wird und auf string zurückgefallen wird. Dies liegt meist an mehrdeutigen Typ-Definitionen (z. B. ["integer", "null"]) in der von ASP.NET Core generierten JSON.
Lösung in ASP.NET Core (.NET 9+):
Da Properties in der OpenAPI-Implementierung oft readonly sind, muss ein SchemaTransformer verwendet werden, um die Typen global zu korrigieren:
csharp
builder.Services.AddOpenApi(options =>
{
    options.AddSchemaTransformer((schema, context, cancellationToken) =>
    {
        // Korrektur für int und int?
        if (context.JsonTypeInfo.Type == typeof(int) || context.JsonTypeInfo.Type == typeof(int?))
        {
            schema.Type = "integer";
            schema.Format = "int32";
            
            // Optional: Nullable-Status explizit setzen, um Kiota-Warnungen zu vermeiden
            if (context.JsonTypeInfo.Type == typeof(int))
            {
                schema.Nullable = false;
            }
        }
        return Task.CompletedTask;
    });
});
Verwende Code mit Vorsicht.

Warum dieser Weg?
Der SchemaTransformer modifiziert die Werte innerhalb des bestehenden Schema-Objekts, anstatt zu versuchen, das Objekt selbst zu ersetzen (was aufgrund von readonly-Constraints scheitern würde).
Soll ich dir noch die spezifischen NuGet-Pakete auflisten, die du für den Kiota-Client in deinem Projekt installieren musst?
KI-Antworten können Fehler enthalten. Weitere Informationen