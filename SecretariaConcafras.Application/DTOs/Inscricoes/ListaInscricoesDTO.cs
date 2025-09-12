using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretariaConcafras.Application.DTOs.Inscricoes;

public class ListaInscricoesDTO
{
    public Guid InscricaoId { get; set; }
    public Guid EventoId { get; set; }
    public string EventoTitulo { get; set; } = default!;
    public DateTime DataInscricao { get; set; }

    public Guid ParticipanteId { get; set; }
    public string ParticipanteNome { get; set; } = default!;
	public DateOnly ParticipanteDataNascimento { get; set; }
	public int ParticipanteIdade { get; set; }

	public string TemaAtual { get; set; } = default!;
    public string TemaEspecifico { get; set; } = default!;

    public bool Trabalhador { get; set; }
    public string PagamentoStatus { get; set; } = "Pendente";
    public decimal ValorInscricao { get; set; } // se houver precificação
    public bool PodePagar => PagamentoStatus != "Pago";
}
