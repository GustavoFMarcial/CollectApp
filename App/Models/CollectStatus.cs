using System.ComponentModel.DataAnnotations;

namespace CollectApp.Models;

public enum CollectStatus
{
    [Display(Name = "Pendente aprovar")]
    PendenteAprovar,

    [Display(Name = "Pendente coletar")]
    PendenteColetar,

    Coletado,

    Cancelado,
}