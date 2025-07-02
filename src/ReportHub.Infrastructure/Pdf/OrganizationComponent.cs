using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using ReportHub.Domain;

namespace ReportHub.Infrastructure.Pdf;

public class OrganizationComponent(string title, Organization organization) : IComponent
{
	public void Compose(IContainer container)
	{
		container.ShowEntire().Column(column =>
		{
			column.Spacing(2);

			column.Item().Text(title).SemiBold();
			column.Item().PaddingBottom(5).LineHorizontal(1);

			column.Item().Text(organization.Name);
			column.Item().Text(organization.CountryCode.ToUpper());
		});
	}
}
