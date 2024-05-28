using Application.Dtos;
using AutoMapper;
using Domain;

namespace Application.Extensions;

public class MappingExtensions : Profile
{
    public MappingExtensions()
    {
        CreateMap<Product, ProductListDto>().ReverseMap();
    }
}