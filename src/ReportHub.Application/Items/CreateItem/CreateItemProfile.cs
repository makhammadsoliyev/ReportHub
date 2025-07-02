using AutoMapper;
using ReportHub.Domain;

namespace ReportHub.Application.Items.CreateItem;

public class CreateItemProfile : Profile
{
	public CreateItemProfile()
	{
		CreateMap<CreateItemRequest, Item>();
	}
}
