namespace CollectApp.Helpers;

public static class LogStatusValue
{
    public static string LogStatusValueSpaceBetween(string logValue)
    {
        if (logValue == "PendenteAprovar")
        {
            return "Pendente aprovar";
        }
        else
        {
            return "Pendente coletar";
        }
    }
}