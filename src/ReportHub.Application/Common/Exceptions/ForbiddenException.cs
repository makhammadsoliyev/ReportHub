﻿namespace ReportHub.Application.Common.Exceptions;

public class ForbiddenException(string message) : Exception(message)
{
}
