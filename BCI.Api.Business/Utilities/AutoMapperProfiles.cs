using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BCI.Api.DTOs;
using BCI.Api.Models;

namespace BCI.Api.Business.Utilities
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<ProcessLogDTO, ProcessLog>();
            CreateMap<CreationProductDTO, Product>();
            CreateMap<CreactionCompanyDTO, Company>();
            CreateMap<CreationClientDTO, Client>();
            CreateMap<CreactionPollDTO, Poll>();
            CreateMap<CreationRequestDTO, ActivationRequest>()
                .ForMember(dest => dest.Client, opt => opt.MapFrom(src => src.Client))
                .ForMember(dest => dest.Company, opt => opt.MapFrom(src => src.Company));

            CreateMap<CreationTraceDTO, Trace>();
            CreateMap<Product, ProductDTO>()
                .ForMember(dest => dest.Codigo, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Name));

            CreateMap<SalesAmount, SalesAmountDTO>()
                .ForMember(dest => dest.Codigo, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Name));

        }
    }
}
