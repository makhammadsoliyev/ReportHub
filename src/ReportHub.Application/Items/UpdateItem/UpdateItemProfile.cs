using AutoMapper;
using ReportHub.Domain;

namespace ReportHub.Application.Items.UpdateItem;

public class UpdateItemProfile : Profile
{
	public UpdateItemProfile()
	{
		CreateMap<UpdateItemRequest, Item>();
	}
}
