using AutoMapper;
using ReportHub.Domain;

namespace ReportHub.Application.Items.GetItemsList;

public class GetItemsListProfile : Profile
{
	public GetItemsListProfile()
	{
		CreateMap<Item, GetItemsListDto>();
	}
}
