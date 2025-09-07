using AutoMapper;
using SecretariaConcafras.Application.DTOs.Enderecos;
using SecretariaConcafras.Application.DTOs.Participantes;

public class CommonProfile : Profile
{
    public CommonProfile()
    {
        CreateMap<EnderecoCreateDto, Endereco>();

        CreateMap<ParticipanteCreateDto, Participante>()
            .ForMember(d => d.CPF, o => o.MapFrom(s => OnlyDigits(s.Cpf)))
            .ForMember(d => d.InstituicaoId, o => o.Ignore())
            .ForMember(d => d.Instituicao, o => o.Ignore())
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.Endereco, o => o.Ignore())
            .AfterMap((src, dest, ctx) =>
            {
                if (src.Endereco != null)
                {
                    dest.Endereco ??= new Endereco();
                    ctx.Mapper.Map(src.Endereco, dest.Endereco);
                }
            })
            // 🔽 Em AutoMapper v12, use a assinatura de 5 parâmetros:
            .ForAllMembers(opt => opt.Condition(
                (src, dest, srcMember, destMember, ctx) => srcMember != null
            ));
    }

    static string OnlyDigits(string v) =>
        new string((v ?? "").Where(char.IsDigit).ToArray());
}
