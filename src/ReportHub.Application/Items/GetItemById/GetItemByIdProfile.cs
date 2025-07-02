using AutoMapper;
using ReportHub.Domain;

namespace ReportHub.Application.Items.GetItemById;

public class GetItemByIdProfile : Profile
{
	public GetItemByIdProfile()
	{
		CreateMap<Item, GetItemByIdDto>();
	}
}
