﻿namespace ReportHub.Application.Plans.CreatePlan;

public class CreatePlanRequest
{
	public string Title { get; set; }

	public DateTime StartDate { get; set; }

	public DateTime EndDate { get; set; }
}
