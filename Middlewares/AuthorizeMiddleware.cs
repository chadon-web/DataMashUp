
using System;
using System.Net;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Humanizer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;

namespace DataMashUp.Middlewares;
public class AuthorizeMiddleware
{
	private readonly RequestDelegate _next;

	public AuthorizeMiddleware(RequestDelegate next)
	{
		_next = next;
	}

	public async Task Invoke(HttpContext context)
	{
		// Check if the request has an [Authorize] attribute
		var actionDescriptor = context.GetEndpoint()?.Metadata.GetMetadata<ControllerActionDescriptor>();

		if (IsActionAuthorized(actionDescriptor))
		{
			var sss = context.User.Identity.IsAuthenticated;
			// Check if the user is authenticated
			if (!context.User.Identity.IsAuthenticated)
			{
				// Redirect to the login page
				var actionName = actionDescriptor.ActionName;
				var url = "/Auth/Login?returnUrl=" + actionName;
				context.Response.Redirect(url);
				return;
			}
		}

		// Call the next middleware in the pipeline
		await _next(context);
	}

	private bool IsActionAuthorized(ControllerActionDescriptor actionDescriptor)
	{
		// Get the action descriptor for the current endpoint
		if (actionDescriptor != null)
		{
			// Check if the action method has the [Authorize] attribute
			var hasAuthorizeAttribute = actionDescriptor.MethodInfo.GetCustomAttributes(typeof(AuthorizeAttribute), inherit: true).Any();

			if (hasAuthorizeAttribute)
			{
				// You can also get the action name if needed
				var actionName = actionDescriptor.ActionName;

				// You may want to perform additional authorization logic here if needed
				// For example, check the user's roles or claims

				return true; // Authorized
			}
		}
		return false; // Not authorized
	}


}

