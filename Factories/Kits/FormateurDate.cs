using System.Globalization;

namespace InscriptionApp.Api.Factories.Kits;

public class FormateurDate : IFormateurDate
{
    private readonly CultureInfo _culture;

    public FormateurDate(CultureInfo culture) => _culture = culture;

    public string Formater(DateTime date) => date.ToString("D", _culture);   // date en toutes lettres
}