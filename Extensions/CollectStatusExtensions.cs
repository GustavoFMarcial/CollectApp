using CollectApp.Models;

namespace CollectApp.Extensions;

public static class CollectPermisions
{
    public static bool CanChangeStatus(this CollectStatus status)
    {
        return status != CollectStatus.Coletado && status != CollectStatus.Cancelado;
    }

    public static bool CanEdit(this CollectStatus status)
    {
        return status == CollectStatus.PendenteAprovar;
    }

    public static bool CanDelete(this CollectStatus status)
    {
        return status == CollectStatus.PendenteAprovar;
    }

    public static bool CanOpen(this CollectStatus status)
    {
        return status == CollectStatus.PendenteColetar;
    }
}