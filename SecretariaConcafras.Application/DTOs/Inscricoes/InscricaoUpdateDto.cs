namespace SecretariaConcafras.Application.DTOs.Inscricoes
{
    public class InscricaoUpdateDto
    {
        public Guid Id { get; set; }

        // Atualização de cursos
        public List<Guid> CursosIds { get; set; } = new();

        // Atualização da comissão (se for trabalhador)
        public Guid? ComissaoTrabalhoId { get; set; }
    }
}
