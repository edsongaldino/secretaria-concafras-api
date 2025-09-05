// Application/DTOs/Shared/UsuariosEInscricoesSlim.cs
namespace SecretariaConcafras.Application.DTOs.Comissoes
{
	public class UsuarioSlimDto
	{
		public Guid Id { get; set; }
		public string Nome { get; set; } = default!;
		public string Email { get; set; } = default!;
	}

	public class UsuarioRoleSlimDto
    {
        public Guid Id { get; set; }
        public Guid UsuarioId { get; set; }
        public int Role { get; set; }           // troque para string se serializar enums como string
        public Guid? EventoId { get; set; }
        public Guid? ComissaoEventoId { get; set; }
        public UsuarioSlimDto Usuario { get; set; } = default!;
    }

    public class InscricaoSlimDto
    {

		public Guid Id { get; set; }
        public Guid ParticipanteId { get; set; }
        public string ParticipanteNome { get; set; } = default!;
    }

    public class InscricaoTrabalhadorSlimDto
    {
        public Guid Id { get; set; }
        public Guid InscricaoId { get; set; }
        public Guid ComissaoEventoId { get; set; }
        public int Nivel { get; set; }          // troque para string se serializar enums como string
        public InscricaoSlimDto Inscricao { get; set; } = default!;
    }
}
