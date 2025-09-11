using SecretariaConcafras.Domain.Entities;
using SecretariaConcafras.Domain.Exceptions;
using System;
using System.Net;

namespace SecretariaConcafras.Domain.Helpers;

public static class InscricaoHelper
{
	public static decimal DefinirValorInscricao(Evento evento, Participante participante)
	{
		if (evento is null) throw new ArgumentNullException(nameof(evento));

		var idadeMaxInfantil = 12;
		var dataRef = GetDataReferencia(evento); // escolha a implementação abaixo

		var idade = DataHelper.CalcularIdadePelaData(participante.DataNascimento, dataRef);
		var ehCrianca = idade <= idadeMaxInfantil;

		// valores são decimal (não-nulos) → sem ??
		var valor = ehCrianca ? evento.ValorInscricaoCrianca : evento.ValorInscricaoAdulto;

		if (valor <= 0)
			throw new InscricaoException(HttpStatusCode.PreconditionFailed, "Valor de inscrição do evento inválido.");

		return valor;
	}

	private static DateTime GetDataReferencia(Evento evento) =>
	evento.DataInicio == default
		? DateTime.UtcNow
		: DateTime.SpecifyKind(evento.DataInicio, DateTimeKind.Utc);
}
