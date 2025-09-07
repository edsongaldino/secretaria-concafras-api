﻿namespace SecretariaConcafras.Domain.Enums
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
        Aprovado = 3,
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

    public enum PublicoCurso
    {
        Crianca = 1,
        Jovem = 2,
        Adulto = 3
    }
    public enum PagamentoStatus : short
    {
        Pendente = 1,
        Aguardando = 2, // aguardando confirmação do provedor
        Pago = 3,
        Cancelado = 4,
        Expirado = 5,
        Falhou = 6
    }

    public enum MetodoPagamento : short
    {
        Checkout = 1, // Mercado Pago Checkout Pro
        Pix = 2  // (se um dia quiser ativar PIX direto)
    }

}
