using SecretariaConcafras.Application.DTOs.Comissoes;
using SecretariaConcafras.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretariaConcafras.Application.DTOs.Inscricoes;

public class InscricaoTrabalhadorDto
{
	Guid Id;
	Guid InscricaoId;
	Guid ComissaoEventoId;
	int Nivel; // se você serializa enum como string, troque para string
	InscricaoSlimDto Inscricao;

	public InscricaoTrabalhadorDto(Guid id, Guid inscricaoId, Guid comissaoEventoId, NivelTrabalhador nivel, InscricaoSlimDto inscricaoSlimDto)
	{
		Id = id;
		InscricaoId = inscricaoId;
		ComissaoEventoId = comissaoEventoId;
	}
}
