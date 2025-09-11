using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretariaConcafras.Domain.Helpers;

public class DataHelper
{
	public static int CalcularIdadePelaData(DateOnly nascimento, DateTime naData)
	{
		var refDate = DateOnly.FromDateTime(naData); // converte DateTime -> DateOnly

		if (nascimento > refDate)
			throw new ArgumentOutOfRangeException(nameof(nascimento), "Nascimento no futuro.");

		var idade = refDate.Year - nascimento.Year;
		// AddYears em DateOnly já lida com 29/02 (vira 28/02 em ano não bissexto)
		if (refDate < nascimento.AddYears(idade)) idade--;
		return idade;
	}
}
