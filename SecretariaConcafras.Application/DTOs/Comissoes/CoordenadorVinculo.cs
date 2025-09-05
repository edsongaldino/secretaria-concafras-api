using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretariaConcafras.Application.DTOs.Comissoes;

public record CoordenadorVinculo(Guid ComissaoId, Guid UsuarioId);

public record AtualizarCoordenadorRequest(Guid? UsuarioId);
