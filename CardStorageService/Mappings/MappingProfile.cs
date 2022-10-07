using AutoMapper;
using CardStorageService.Data;
using CardStorageService.Models;
using CardStorageService.Models.Requests;

namespace CardStorageService.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Card, CardDto>();
        CreateMap<CreateCardRequest, Card>();
        CreateMap<Client, ClientDto>();
        CreateMap<CreateClientRequest, Client>();
    }
}