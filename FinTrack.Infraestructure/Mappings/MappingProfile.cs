using AutoMapper;
using FinTrack.Core.DTOs;
using FinTrack.Core.Entities;
using System.Globalization;

namespace FinTrack.Infrastructure.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<Role, RoleDto>().ReverseMap();

            CreateMap<Transaction, TransactionDto>()
                .ForMember(dest => dest.Date,
                    opt => opt.ConvertUsing<DateTimeToStringConverter, DateTime>());

            CreateMap<TransactionDto, Transaction>()
                .ForMember(dest => dest.Date,
                    opt => opt.ConvertUsing<StringToDateTimeConverter, string>());
        }
    }

    public class DateTimeToStringConverter : IValueConverter<DateTime, string>
    {
        public string Convert(DateTime source, ResolutionContext context)
        {
            return source.ToString("dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
        }
    }

    public class StringToDateTimeConverter : IValueConverter<string, DateTime>
    {
        public DateTime Convert(string source, ResolutionContext context)
        {
            if (string.IsNullOrWhiteSpace(source))
                throw new ArgumentException("La fecha no puede estar vacía");

            source = source.Trim();

            source = source.Replace("a. m.", "AM")
                           .Replace("p. m.", "PM")
                           .Replace("a.m.", "AM")
                           .Replace("p.m.", "PM")
                           .Replace("am", "AM")
                           .Replace("pm", "PM");

            string[] formats = new[]
            {
                "dd-MM-yyyy", 
                "dd-MM-yyyy HH:mm:ss", 
                "dd-MM-yyyy hh:mm:ss tt",
                "dd/MM/yyyy",
                "dd/MM/yyyy HH:mm:ss", 
                "dd/MM/yyyy hh:mm:ss tt",
                "yyyy-MM-dd", 
                "yyyy-MM-dd HH:mm:ss"
            };

            if (DateTime.TryParseExact(source, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
            {
                return result;
            }

            if (DateTime.TryParse(source, CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
            {
                return result;
            }

            throw new FormatException($"No se pudo convertir la fecha '{source}' a DateTime. Formatos soportados: fecha, fecha y hora, fecha con AM/PM.");
        }
    }
}
