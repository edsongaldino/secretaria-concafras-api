namespace SecretariaConcafras.Domain.Enums
{
    public enum PerfilUsuario
    {
        Participante = 1,
        Gestor = 2,
        Coordenador = 3,
        Financeiro = 4,
        Secretaria = 5
    }

    public enum BlocoCurso
    {
        TemaAtual = 1,
        TemaEspecifico = 2
    }

    public enum TipoInscricao
    {
        Normal = 1,
        Trabalhador = 2
    }

    public enum StatusPagamento
    {
        Pendente = 1,
        AguardandoConfirmacao = 2,
        Pago = 3,
        Cancelado = 4,
        Expirado = 5,
        Reembolsado = 6
    }

    public enum TipoCheckin
    {
        Evento = 1,
        Curso = 2
    }

    public enum PerfilComissao
    {
        Coordenacao = 1, // acesso ao sistema
        Voluntario = 2   // apenas inscrito, sem acesso ao sistema
    }

    public enum RoleSistema
    {
        VisualizarRelatorios = 1,
        GerenciarFinanceiro = 2,
        GerenciarInscricoes = 3,
        GerenciarCursos = 4,
        GerenciarEvento = 5
    }

    public enum NivelTrabalhador
    {
        Voluntario = 1,
        Coordenacao = 2
    }
}
