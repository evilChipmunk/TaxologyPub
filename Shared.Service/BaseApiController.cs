using Microsoft.AspNetCore.Mvc;
using System; 
using System.Threading.Tasks; 
using Microsoft.Extensions.Logging;
using Shared.DTO;

namespace Shared.Service
{ 
    public abstract class BaseApiController : Controller
    {
        private ILogger logger;

        protected ILogger Logger
        {
            get
            {
                if (logger == null)
                {
                    logger = CreateLogger();
                }

                return logger;
            }
        }

        protected BaseApiController(ILoggerFactory loggerFactory)
        {
            this.loggerFactory = loggerFactory; 
        }

        protected readonly ILoggerFactory loggerFactory;
        protected abstract ILogger CreateLogger();

        protected async Task<IActionResult> HandleAsync (Func<Task<BaseResponse>> process) 
        {
            try
            {
                var response = await process();
                switch (response.ResponseAction)
                {
                    case ResponseAction.Created:
                        return Created(response.RequestUri, response);

                    case ResponseAction.Updated:
                        return Ok(response);

                    case ResponseAction.Deleted:
                        return Ok(response); //really should just be an OK but caller is expecting a result

                    case ResponseAction.Found:
                        return Ok(response);

                    case ResponseAction.NotFound:
                        return NotFound(response);

                    case ResponseAction.Error:
                        return BadRequest(response);

                    case ResponseAction.Exception:
                        return BadRequest(response);

                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                 logger.LogError(ex, "Error processing request");
                return BadRequest("Unable to process request");
            }
        }
    }
}
