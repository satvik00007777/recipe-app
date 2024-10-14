using AutoMapper;
using FinalProject.Models;
using FinalProject.DTOs;

namespace FinalProject.Profiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<SignupDto, User>();
        }
    }
}
