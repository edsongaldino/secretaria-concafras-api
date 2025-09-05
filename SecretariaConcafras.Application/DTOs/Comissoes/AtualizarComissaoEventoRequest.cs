using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretariaConcafras.Application.DTOs.Comissoes;

public class AtualizarComissaoEventoRequest
{
	public string? Observacoes { get; set; }
	public Guid? CoordenadorUsuarioId { get; set; }
	public object Nome { get; set; }
	public string? Slug { get; set; }
	public bool Ativa { get; set; }
}
