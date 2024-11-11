using AutoMapper;
using FinalProject.Models;
using FinalProject.DTOs;

namespace FinalProject.Profiles
{
    /// <summary>
    /// This class defines the AutoMapper profile for mapping between DTOs and models.
    /// It is used to configure the mapping of data from one object type to another.
    /// </summary>
    public class MappingProfiles : Profile
    {
        /// <summary>
        /// Constructor for the MappingProfiles class where the mappings are defined.
        /// </summary>
        public MappingProfiles()
        {
            CreateMap<SignupDto, User>();
        }
    }
}
